import codeRender from './tpl/codeRender'
import lodash from 'lodash'

var defaultOptions = {
    SwaggerUrl: '', // 接口文档地址
    ApiBase: '', // 接口根节点（必填）
    ApiName: '', // 接口名称（必填）
    Mock: false, // 是否启用模拟数据 （默认：false）
    FormatMock: defaultFormatMock, // 接管模拟数据格式化
    FormatControllerName: formatControllerName, // 格式化模块名称（默认：接口名称+Api）
    FormatMethodName: formatMethodName, // 格式化接口名称（默认：小驼峰命名）
    FormatModelName: formatModelName // 格式化dto对象、枚举名称（默认：只会去除特殊字符）
}
/**
   * 格式化模块名称（默认：接口名称+Api）
   * @param name 名称
   */
function formatControllerName(name) {
    return name.indexOf('Api') !== -1 ? name : name + 'Api'
}
/**
 * 格式化接口名称（默认：小驼峰命名）
 * @param name 名称
 */
function formatMethodName(name) {
    if (name === '/' || name === '') {
        return ''
    }
    const fnName = name.substring(name.lastIndexOf('/'))
    return lodash.camelCase(fnName)
}
/**
 * 格式化dto对象、枚举名称（默认：只会去除特殊字符）
 * @param name 名称
 */
function formatModelName(name) {
    return name.replace(/[.,\[\]]/g, '')
}

/**
* 格式化模拟值
* @param v 默认格式化后的值
* @param p 对应的属性
*/
function defaultFormatMock(v, p) {
    return v
}
/**
 * 去掉换行
 * @param str 字符串
 */
function removeLineBreak(str) {
    return str ? str.replace(/[\r\n]/g, '') : '';
}
/**
 * 格式化模拟数据
 * @param properties 当前需要模拟的对象
 * @param models 接口全部对象
 * @param fm 格式化模拟数据函数
 * @param deep 递归层级，防止对象父子嵌套导致死循环 默认递归5级
 */
function formatMock(properties, models, fm, deep) {
    if (deep === void 0) { deep = 1; }
    var model = lodash_1.default.find(models, { Name: properties.Ref });
    if (model) {
        var mock_1 = {};
        model.Properties.forEach(function (p) {
            // 'string' | 'number' | 'boolean' | 'file' | 'array' | 'enum' | 'schema'
            var v = '';
            switch (p.Type.TypeOf) {
                case 'string':
                    v = '@ctitle(10, 20)';
                    break;
                case 'number':
                    v = '@integer(0, 100)';
                    break;
                case 'boolean':
                    v = '@boolean';
                    break;
                case 'file':
                    v = '';
                    break;
                case 'array':
                    v = deep > 5 ? [] : [formatMock(p.Type, models, fm, deep + 1)];
                    break;
                case 'enum':
                    v = p.Type.TsType[0];
                    break;
                case 'schema':
                    v = deep > 5 ? null : formatMock(p.Type, models, fm, deep + 1);
                    break;
            }
            mock_1 = fm(v, p, mock_1);
        });
        return mock_1;
    }
    else {
        return '';
    }
}
/**
 * 参数名称处理
 * @param {*} oldName
 */
function getParameterName(oldName) {
    var newName = oldName;
    // 关键词处理
    if (oldName === 'number') {
        newName = 'num';
    }
    if (oldName === 'string') {
        newName = 'str';
    }
    newName = lodash.camelCase(oldName);
    return newName;
}
/**
 * 处理重名问题
 * @param name 当前名称
 * @param list 列表，对象必须有Name属性才行
 */
function reName(name, list) {
    // 方法名称-重名处理
    if (lodash.findIndex(list, { Name: name }) !== -1) {
      let i = 1
      while (true) {
        if (lodash.findIndex(list, { Name: name + '_' + i }) !== -1) {
          i++
        } else {
          name = name + '_' + i
          break
        }
      }
    }
    return name
  }
/**
 * 格式化属性
 * @param properties 属性
 * @param options 配置
 */
function convertType(properties, options) {
    var type = {
        TypeOf: 'string',
        TsType: 'void',
        Ref: ''
    };
    if (!properties) {
        return type;
    }
    if (properties.hasOwnProperty('oneOf')) {
        return convertType(properties.oneOf[0], options);
    }
    if (properties.hasOwnProperty('allOf')) {
        return convertType(properties.allOf[0], options);
    }
    if (properties.hasOwnProperty('schema')) {
        return convertType(properties.schema, options);
    }
    if (properties.hasOwnProperty('$ref')) {
        var t = options.FormatModelName(properties.$ref.substring(properties.$ref.lastIndexOf('/') + 1));
        type = {
            TypeOf: 'schema',
            TsType: t,
            Ref: t
        };
    }
    else if (properties.hasOwnProperty('enum')) {
        type = {
            TypeOf: 'enum',
            TsType: properties.enum
                .map(function (item) {
                    return JSON.stringify(item);
                })
                .join(' | '),
            Ref: ''
        };
    }
    else if (properties.type === 'array') {
        var iType = convertType(properties.items, options);
        type = {
            TypeOf: 'array',
            TsType: 'Array<' + iType.TsType + '>',
            Ref: iType.Ref
        };
    }
    else {
        type = {
            TypeOf: properties.type,
            TsType: '',
            Ref: ''
        };
        switch (properties.type) {
            case 'string':
                type.TypeOf = 'string';
                type.TsType = 'string';
                break;
            case 'number':
            case 'integer':
                type.TypeOf = 'number';
                type.TsType = 'number';
                break;
            case 'boolean':
                type.TypeOf = 'boolean';
                type.TsType = 'boolean';
                break;
            case 'file':
                type.TypeOf = 'file';
                type.TsType = 'string | Blob';
                break;
            default:
                type.TsType = 'any';
                break;
        }
    }
    return type;
}

/**
 * swagger 文档格式化
 * @param swagger
 * @param options
 */
function formatData(swagger, options) {
    // 文档模式 是否openapi 模式 还是 默认 swagger模式
    const isOpenApi = swagger.hasOwnProperty('openapi')
  
    let apiData = {
      BaseInfo: {
        Title: swagger.info.title, // 接口标题
        Description: swagger.info.description, // 接口说明
        Version: swagger.info.version // 接口版本号
      },
      Controllers: [],
      Models: [],
      Enums: []
    }
  
    // 格式化属性方法
    function fmProperties(properties, model) {
      lodash.forEach(properties, function (propertie, name) {
        const newp = {
          Name: name,
          Description: removeLineBreak(propertie.description),
          Type: convertType(propertie, options)
        }
        model.Properties.push(newp)
      })
    }
  
    // dto对象 / enum对象
    lodash.forEach(isOpenApi ? swagger.components.schemas : swagger.definitions, function (definition, name) {
      if (definition.hasOwnProperty('enum')) {
        const e = {
          Name: options.FormatModelName(name),
          Description: removeLineBreak(definition.description),
          Items: []
        }
        let enums = lodash.zipObject(definition['x-enumNames'], definition.enum)
        // console.log('x-enumNames',enums,definition.enum);
        enums= enums.length>0?enums:definition.enum;
        lodash.forEach(enums, function (enumSource) {
          const enumItem= enumSource.split(',');
          const hasDes= enumItem.length>1;
          const enumValue=Number(enumItem[0]);
          const item = {
            Name: hasDes?enumItem[1]:('_'+enumValue),
            Value:Number(enumItem[0])
          }
          e.Items.push(item)
        })
  
        apiData.Enums.push(e)
      } else {
        const m = {
          Name: options.FormatModelName(name),
          Description: removeLineBreak(definition.description),
          IsParameter: false,
          BaseModel: '',
          Properties: []
        }
  
        // 格式化属性
        if (definition.hasOwnProperty('allOf')) {
          lodash.forEach(definition.allOf, function (propertie) {
            if (propertie.hasOwnProperty('$ref')) {
              m.BaseModel = options.FormatModelName(propertie.$ref.substring(propertie.$ref.lastIndexOf('/') + 1))
            } else {
              if (propertie.hasOwnProperty('properties')) {
                fmProperties(propertie.properties, m)
              }
            }
          })
        } else {
          fmProperties(definition.properties, m)
        }
  
        apiData.Models.push(m)
      }
    })
  
    // 模块
    lodash.mapKeys(swagger.ControllerDesc, function (value, key) {
      apiData.Controllers.push({
        Name: options.FormatControllerName(key),
        Description: removeLineBreak(value) || '',
        Methods: [],
        ImportModels: []
      })
      return key
    })
  
    // 方法
    lodash.forEach(swagger.paths, function (api, url) {
      lodash.forEach(api, function (md, requestName) {
        // 模块名称
        const cName = options.FormatControllerName(md.tags[0])
        // 当前模块
        let currController = lodash.find(apiData.Controllers, { Name: cName })
        if (!currController) {
          // 没有就新加一个模块
          const newController = { Name: cName, Description: '', Methods: [], ImportModels: [] }
          currController = newController
          apiData.Controllers.push(currController)
        }
        // 方法名称
        let mName = options.FormatMethodName(url)
        mName = reName(mName, currController.Methods)
  
        // 添加方法
        const method = {
          Name: mName,
          Url: url,
          Description: removeLineBreak(md.summary) || '',
          RequestName: requestName,
          Parameters: [],
          ParametersQuery: [],
          ParametersBody: [],
          ParametersFormData: [],
          ParametersHeader: [],
          ParametersPath: [],
          Responses: convertType(md.responses['200'] ? (isOpenApi ? md.responses['200'].content['application/json'].schema : md.responses['200'].schema) : null, options),
          MockData: null
        }
        // 方法参数处理
        // 兼容openapi 模式 requestBody 参数
        if (isOpenApi && md.requestBody) {
          md.parameters = md.parameters || []
          md.parameters.push(
            Object.assign(
              {
                name: md.requestBody['x-name'],
                required: md.requestBody.required,
                in: 'body',
                description: ''
              },
              md.requestBody.content['application/json']
            )
          )
        }
        let pindex=1;
        lodash.forEach(md.parameters, (parameter) => {
          let pName=  parameter.name||('param'+pindex++);
          let pa = {
            Name: pName,
            CamelCaseName: reName(getParameterName(pName), method.Parameters),
            Description: removeLineBreak(parameter.description),
            In: parameter.in,
            Required: parameter.required,
            Default: '',
            Type: convertType(parameter, options)
          }
          if (pa.In === 'query') {
            method.ParametersQuery.push(pa)
            method.Parameters.push(pa)
          }
          if (pa.In === 'body') {
            method.ParametersBody.push(pa)
            method.Parameters.push(pa)
          }
          if (pa.In === 'formData') {
            method.ParametersFormData.push(pa)
            method.Parameters.push(pa)
          }
          if (pa.In === 'header') {
            method.ParametersHeader.push(pa)
          }
          if (pa.In === 'path') {
            method.ParametersPath.push(pa)
            method.Parameters.push(pa)
          }
  
          // 接口参数：存在引用型参数&没有没添加到引用列表的则添加到引用列表
          if (pa.Type.Ref && currController && currController.ImportModels.indexOf(pa.Type.Ref) == -1) {
            currController.ImportModels.push(pa.Type.Ref)
            // 标记为输入参数对象
            const d = lodash.find(apiData.Models, { Name: pa.Type.Ref })
            if (d) {
              d.IsParameter = true
            }
          }
        })
        // 排序一下参数，把非必填参数排后面
        method.Parameters = lodash.orderBy(method.Parameters, ['Required'], ['asc'])
  
        // 返回值：存在引用型参数&没有没添加到引用列表的则添加到引用列表
        method.Responses.Ref && currController && currController.ImportModels.indexOf(method.Responses.Ref) == -1 && currController.ImportModels.push(method.Responses.Ref)
        // 返回值模拟
        if (options.Mock) {
          method.MockData = formatMock(method.Responses, apiData.Models, options.FormatMock)
        }
        // 添加方法
        currController.Methods.push(method)
      })
    })
  
    // 调整方法顺序，因为mock时 有可能匹配错误的mock拦截
    apiData.Controllers.map(c => {
      c.Methods = lodash.orderBy(c.Methods, ['Name'], ['desc'])
      return c
    })
  
    // 清理无方法空模块
    lodash.remove(apiData.Controllers, (c) => {
      return c.Methods.length <= 0
    })
  
    return apiData
  }

/**
 * 获取Swagger的JSON数据
 * @param {*} swaggerUrl
 */
function getSwaggerData(swaggerData) {
    return new Promise(function (resolve, reject) {
        if (typeof swaggerData == 'string') {
            var obj = eval('(' + swaggerData + ')');
            resolve(obj);
        }
        else {
            resolve(swaggerData);
        }
    });
}

/**
 * 格式化成TS统一模板格式数据-数据源
 * @param swaggerData
 * @param options
 */
function getApiData(swaggerData, options) {
    return new Promise(function (resolve, reject) {
        getSwaggerData(swaggerData)
            .then(function (r) {
                var apiData = formatData(r, options);
                resolve(apiData);
            })
            .catch(function (e) {
                reject(e);
            });
    });
}

function codeBuild(apiData, options) {
    // 生成-dto对象
    let code='';
    code = codeRender(1, { apiData: apiData, options: options });
    // 按模块生成接口
    apiData.Controllers.forEach(function (controller) {
        code+= codeRender(2, { controller: controller, options: options });
    })
    return code;
}

function swaggerBuild(options) {
    var _options = Object.assign(defaultOptions, options);
   return getApiData(options.SwaggerUrl, _options).then(r=>{
       return codeBuild(r,_options);
    })
} 

export default swaggerBuild;