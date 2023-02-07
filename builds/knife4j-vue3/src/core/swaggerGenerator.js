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
    newName = lodash_1.default.camelCase(oldName);
    return newName;
}
/**
 * 处理重名问题
 * @param name 当前名称
 * @param list 列表，对象必须有Name属性才行
 */
function reName(name, list) {
    // 方法名称-重名处理
    if (lodash_1.default.findIndex(list, { Name: name }) !== -1) {
        var i = 1;
        while (true) {
            if (lodash_1.default.findIndex(list, { Name: name + '_' + i }) !== -1) {
                i++;
            }
            else {
                name = name + '_' + i;
                break;
            }
        }
    }
    return name;
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
    var isOpenApi = swagger.hasOwnProperty('openapi');
    var apiData = {
        BaseInfo: {
            Title: swagger.info.title,
            Description: swagger.info.description,
            Version: swagger.info.version // 接口版本号
        },
        Controllers: [],
        Models: [],
        Enums: []
    };
    // 格式化属性方法
    function fmProperties(properties, model) {
        lodash_1.default.forEach(properties, function (propertie, name) {
            var newp = {
                Name: name,
                Description: removeLineBreak(propertie.description),
                Type: convertType(propertie, options)
            };
            model.Properties.push(newp);
        });
    }
    // dto对象 / enum对象
    lodash_1.default.forEach(isOpenApi ? swagger.components.schemas : swagger.definitions, function (definition, name) {
        if (definition.hasOwnProperty('enum')) {
            var e_1 = {
                Name: options.FormatModelName(name),
                Description: removeLineBreak(definition.description),
                Items: []
            };
            var enums = lodash_1.default.zipObject(definition['x-enumNames'], definition.enum);
            lodash_1.default.forEach(enums, function (enumValue, enumName) {
                var item = {
                    Name: enumName,
                    Value: Number(enumValue)
                };
                e_1.Items.push(item);
            });
            apiData.Enums.push(e_1);
        }
        else {
            var m_1 = {
                Name: options.FormatModelName(name),
                Description: removeLineBreak(definition.description),
                IsParameter: false,
                BaseModel: '',
                Properties: []
            };
            // 格式化属性
            if (definition.hasOwnProperty('allOf')) {
                lodash_1.default.forEach(definition.allOf, function (propertie) {
                    if (propertie.hasOwnProperty('$ref')) {
                        m_1.BaseModel = options.FormatModelName(propertie.$ref.substring(propertie.$ref.lastIndexOf('/') + 1));
                    }
                    else {
                        if (propertie.hasOwnProperty('properties')) {
                            fmProperties(propertie.properties, m_1);
                        }
                    }
                });
            }
            else {
                fmProperties(definition.properties, m_1);
            }
            apiData.Models.push(m_1);
        }
    });
    // 模块
    lodash_1.default.mapKeys(swagger.ControllerDesc, function (value, key) {
        apiData.Controllers.push({
            Name: options.FormatControllerName(key),
            Description: removeLineBreak(value) || '接口太懒没写注释',
            Methods: [],
            ImportModels: []
        });
        return key;
    });
    // 方法
    lodash_1.default.forEach(swagger.paths, function (api, url) {
        lodash_1.default.forEach(api, function (md, requestName) {
            // 模块名称
            var cName = options.FormatControllerName(md.tags[0]);
            // 当前模块
            var currController = lodash_1.default.find(apiData.Controllers, { Name: cName });
            if (!currController) {
                // 没有就新加一个模块
                var newController = { Name: cName, Description: '接口太懒没写注释', Methods: [], ImportModels: [] };
                currController = newController;
                apiData.Controllers.push(currController);
            }
            // 方法名称
            var mName = options.FormatMethodName(url);
            mName = reName(mName, currController.Methods);
            // 添加方法
            var method = {
                Name: mName,
                Url: url,
                Description: removeLineBreak(md.summary) || '接口太懒没写注释',
                RequestName: requestName,
                Parameters: [],
                ParametersQuery: [],
                ParametersBody: [],
                ParametersFormData: [],
                ParametersHeader: [],
                ParametersPath: [],
                Responses: convertType(md.responses['200'] ? (isOpenApi ? md.responses['200'].content['application/json'].schema : md.responses['200'].schema) : null, options),
                MockData: null
            };
            // 方法参数处理
            // 兼容openapi 模式 requestBody 参数
            if (isOpenApi && md.requestBody) {
                md.parameters = md.parameters || [];
                md.parameters.push(Object.assign({
                    name: md.requestBody['x-name'],
                    required: md.requestBody.required,
                    in: 'body',
                    description: ''
                }, md.requestBody.content['application/json']));
            }
            lodash_1.default.forEach(md.parameters, function (parameter) {
                var pa = {
                    Name: parameter.name,
                    CamelCaseName: reName(getParameterName(parameter.name), method.Parameters),
                    Description: removeLineBreak(parameter.description),
                    In: parameter.in,
                    Required: parameter.required,
                    Default: '',
                    Type: convertType(parameter, options)
                };
                if (pa.In === 'query') {
                    method.ParametersQuery.push(pa);
                    method.Parameters.push(pa);
                }
                if (pa.In === 'body') {
                    method.ParametersBody.push(pa);
                    method.Parameters.push(pa);
                }
                if (pa.In === 'formData') {
                    method.ParametersFormData.push(pa);
                    method.Parameters.push(pa);
                }
                if (pa.In === 'header') {
                    method.ParametersHeader.push(pa);
                }
                if (pa.In === 'path') {
                    method.ParametersPath.push(pa);
                    method.Parameters.push(pa);
                }
                // 接口参数：存在引用型参数&没有没添加到引用列表的则添加到引用列表
                if (pa.Type.Ref && currController && currController.ImportModels.indexOf(pa.Type.Ref) == -1) {
                    currController.ImportModels.push(pa.Type.Ref);
                    // 标记为输入参数对象
                    var d = lodash_1.default.find(apiData.Models, { Name: pa.Type.Ref });
                    if (d) {
                        d.IsParameter = true;
                    }
                }
            });
            // 排序一下参数，把非必填参数排后面
            method.Parameters = lodash_1.default.orderBy(method.Parameters, ['Required'], ['asc']);
            // 返回值：存在引用型参数&没有没添加到引用列表的则添加到引用列表
            method.Responses.Ref && currController && currController.ImportModels.indexOf(method.Responses.Ref) == -1 && currController.ImportModels.push(method.Responses.Ref);
            // 返回值模拟
            if (options.Mock) {
                method.MockData = formatMock(method.Responses, apiData.Models, options.FormatMock);
            }
            // 添加方法
            currController.Methods.push(method);
        });
    });
    // 调整方法顺序，因为mock时 有可能匹配错误的mock拦截
    apiData.Controllers.map(function (c) {
        c.Methods = lodash_1.default.orderBy(c.Methods, ['Name'], ['desc']);
        return c;
    });
    // 清理无方法空模块
    lodash_1.default.remove(apiData.Controllers, function (c) {
        return c.Methods.length <= 0;
    });
    return apiData;
}
/**
 * 格式化成TS统一模板格式数据-数据源
 * @param swaggerUrl
 * @param options
 */
function getApiData(swaggerUrl, options) {
    return new Promise(function (resolve, reject) {
        getSwaggerData(swaggerUrl)
            .then(function (r) {
            var apiData = formatData(r, options);
            resolve(apiData);
        })
            .catch(function (e) {
            reject(e);
        });
    });
}