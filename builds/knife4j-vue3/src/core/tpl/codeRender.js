import { modelTpl, methodTpl } from './model';
import ejs from 'ejs'
/**
 * 生成代码
 * @param modelType 模板类型 1.model 2,method
 * @param data 数据 
 */
function codeRender(modelType, data) {
    console.log(data)
    switch (modelType) {
        case 1:
            return ejs.render(modelTpl, data)
        case 2:
            return ejs.render(methodTpl, data)
    }
}
export default codeRender;