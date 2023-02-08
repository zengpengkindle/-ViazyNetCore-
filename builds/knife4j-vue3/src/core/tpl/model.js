export let modelTpl=
`<% apiData.Models.forEach( m => { %>
/**
* <%= m.Description || m.Name %>
*/
export interface <%= m.Name + (m.BaseModel?' extends '+m.BaseModel:'') %> {<% m.Properties.forEach( p => { %>
  <% if(p.Description){ %>/** <%=p.Description %> */ <% } %>
  <%= p.Name %>: <%- p.Type.TsType %><%=m.IsParameter?' | null':''%><% }) %>
}
<% }) %>
<% apiData.Enums.forEach( m => { %>// <%= m.Description || '' %><%
  let items = m.Items.map(p=>{
    return p.Name +' = '+ p.Value
  });
%>
export enum <%= m.Name %> { <% -%>
<% items.map(p=>{ %> 
  <%= p+',' -%>
<%}) -%>

}
<% }) %>`;

export let methodTpl=`
<% if(controller.ImportModels && controller.ImportModels.length>0){%>import { <% controller.ImportModels.forEach( (item,index) => {%><%= item+(index<controller.ImportModels.length-1?', ':'') %><% }) %> } from './model'<% } %>
<% if(controller.Description){ %> /**
 * <%=controller.Description || '' %>
 */ <% } %>
export class <%=controller.Name%> extends Base {<% controller.Methods.forEach( m => {
  // 判断是否为导出函数
  let isDownload=m.Responses.TsType=='any'
  
  // 方法参数-对象（排除请求头中参数）
  let ps = m.Parameters;
  
  // 方法参数-输出（排除请求头中参数）
  let pss = ps.map(p=>{
    return p.CamelCaseName+(p.Required?'':'?')+': '+(p.Type.TsType + (isDownload?' | any':''))
  }).join(', ');

  // 参数说明-输出
  let psd =[];
   psd.map(p=>{
    return '* @param '+ p.CamelCaseName + ' ' + p.Type.TsType  + ' ' +  p.In + ' ' + (p.Description || '无') + ' '+ (p.Required?'必填':'')
  });

  // body formData 参数
  let pdt = [].concat(m.ParametersBody).concat(m.ParametersFormData);
  let dt = pdt.length>0?pdt[0].CamelCaseName:null

  // Query 参数
  let qr = m.ParametersQuery.map(p=>{
    return p.Name == p.CamelCaseName ? p.Name : (p.Name+': ' + p.CamelCaseName)
  }).join(', ');

  // Path 参数
  let url = m.Url
  m.ParametersPath.map(p=>{
    url = url.replace('{'+p.CamelCaseName+'}',"' + "+p.CamelCaseName+" + '")
  })
%>
  /**
   * <%=m.Description || '无' -%>
<% psd.map(p=>{ %>
<%='   '+ p -%>
<% }) %>
   */
  public <%=m.Name %>(<%-pss%>): <%-isDownload?'void':'Promise<'+m.Responses.TsType+'>' %> {
    return this.<%=isDownload?'download':'request'%>({
      url: '<%-url%>',
      method: '<%=m.RequestName%>'<% if(dt) { %><%- ','%>
<%- '      data: '+dt -%>
<% } %><% if(qr) { %><%- ',' %>
<%- '      params: { '+qr+' }' -%>
<% } %>
    })
  }<%})%>
}
export default new <%=controller.Name%>()
`;