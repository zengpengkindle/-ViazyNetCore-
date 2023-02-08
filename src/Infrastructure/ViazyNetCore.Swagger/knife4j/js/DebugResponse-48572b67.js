import{_ as e,d as s,a as t,b as n,c as a,f as o,u as i,o as r,x as d,w as l,q as p,l as u,k as c,t as g,p as b,j as h,v as f,a3 as m,M as y,r as C,A as k,a4 as w,s as E,T as _}from"./doc-381f7261.js";import{C as v}from"./clipboard-bcacd816.js";import{C as j}from"./CopyOutlined-d85fc4be.js";const x={props:{api:{type:Object,required:!0},swaggerInstance:{type:Object,required:!0},debugSend:{type:Boolean,default:!1},responseHeaders:{type:Array},responseRawText:{type:String,default:""},responseCurlText:{type:String,default:""},responseStatus:{type:Object},responseContent:{type:Object},responseFieldDescriptionChecked:{type:Boolean,default:!0}},components:{CopyOutlined:j,EditorDebugShow:s((()=>t((()=>import("./EditorDebugShow-7fc97397.js")),["./EditorDebugShow-7fc97397.js","./doc-381f7261.js","..\\css\\doc-f14538b3.css","./ace-d54cf035.js","./ext-language_tools-bf1decbd.js"],import.meta.url)))},setup(){const e=n(),s=a((()=>e.language)),{messages:t}=o();return{language:s,messages:t}},data:()=>({pagination:!1,i18n:null,base64Image:!1,debugResponse:!0,responseHeaderColumn:[]}),watch:{language:function(e,s){this.initI18n()}},computed:{responseSizeText(){var e="0 B",s=this.responseStatus;if(null!=s&&null!=s){var t=s.size,n=(t/1024).toFixed(2),a=(t/1024/1024).toFixed(2);e=n>1?n+" KB":a>1?a+" MB":t+" B"}return e}},created(){this.initI18n(),this.copyRawText(),this.copyCurlText()},methods:{getCurrentI18nInstance(){return this.messages[this.language]},base64Init(){var e=i.getValue(this.responseContent,"base64","",!0);i.strNotBlank(e)&&(this.base64Image=!0)},initI18n(){this.i18n=this.getCurrentI18nInstance(),this.responseHeaderColumn=this.i18n.table.debugResponseHeaderColumns},copyRawText(){var e=this,s="btnDebugCopyRaw"+this.api.id,t=new v("#"+s,{text:()=>e.responseRawText}),n=this.i18n.message.copy.raw.success,a=this.i18n.message.copy.raw.fail;t.on("success",(function(s){e.$message.info(n)})),t.on("error",(function(s){e.$message.info(a)}))},copyCurlText(){var e=this,s="btnDebugCopyCurl"+this.api.id,t=new v("#"+s,{text:()=>e.responseCurlText}),n=this.i18n.message.copy.curl.success,a=this.i18n.message.copy.curl.fail;t.on("success",(function(s){e.$message.info(n)})),t.on("error",(function(s){e.$message.info(a)}))},resetResponseContent(){if(null!=this.responseContent&&"json"==this.responseContent.mode){const e=this.responseContent.text;this.responseContent.text=i.json5stringify(i.json5parse(e))}},showFieldDesChange(e){var s=e.target.checked;this.$emit("debugShowFieldDescriptionChange",s),this.toggleFieldDescription(s)},debugEditorChange(e){this.$emit("debugEditorChange",e)},toggleFieldDescription(e){var s="responseEditorContent"+this.api.id,t=document.getElementById(s).getElementsByClassName("knife4j-debug-editor-field-description");i.arrNotEmpty(t)?t.forEach((function(s){s.style.display=e?"block":"none"})):this.showEditorFieldAnyWay()},showEditorFieldDescription(e){var s=this;i.checkUndefined(e)&&parseInt(e)<=200&&setTimeout((()=>{s.showEditorFieldWait()}),100)},showEditorFieldWait(){this.debugSend&&this.responseFieldDescriptionChecked&&"json"==this.responseContent.mode&&this.showEditorFieldAnyWay()},showEditorFieldAnyWay(){var e=this.swaggerInstance,s=this.api.getHttpSuccessCodeObject(),t="responseEditorContent"+this.api.id,n=document.getElementById(t),a=[],o=n.getElementsByClassName("ace_text-layer"),r=0,d=n.querySelector(".ace_print-margin");if(i.checkUndefined(d)&&i.checkUndefined(d.style)&&(r=d.style.left),o.length>0)for(var l=o[0].getElementsByClassName("ace_line"),p=0;p<l.length;p++){var u=l[p],c=u.getElementsByClassName("ace_variable"),g=null;if(i.arrNotEmpty(c)){g=i.toString(c[0].innerHTML,"").replace(/^"(.*)"$/g,"$1");var b=u.getElementsByClassName("knife4j-debug-editor-field-description");if(!i.arrNotEmpty(b)){var h=document.createElement("span");h.className="knife4j-debug-editor-field-description",h.innerHTML=s.responseDescriptionFind(a,g,e),h.style.left=r,u.appendChild(h)}}var f=u.getElementsByClassName("ace_paren");if(i.arrNotEmpty(f)){for(var m=[],y=0;y<f.length;y++)m.push(f[y].innerHTML);switch(m.join("")){case"[":case"{":a.push(g||0);break;case"}":case"]":a.pop()}}}}}},D={slot:"tabBarExtraContent"},S={style:{color:"#919191"}},F={class:"key"},R={class:"value"},B={class:"key"},I={class:"value"},T={class:"key"},$={class:"value"},N={key:0},H=["src"],O={key:1},A=["id"],z={class:"knife4j-debug-response-curl"},M=["src"];const U=e(x,[["render",function(e,s,t,n,a,o){const i=m,v=p,j=y,x=C("editor-debug-show"),U=k,W=C("CopyOutlined"),q=w,L=E,K=_;return r(),d(v,{class:"knife4j-debug-response"},{default:l((()=>[t.debugSend?(r(),d(v,{key:0},{default:l((()=>[u(K,{defaultActiveKey:"debugResponse"},{default:l((()=>[c("template",D,[t.responseStatus?(r(),d(v,{key:0,class:"knife4j-debug-status"},{default:l((()=>[c("span",null,[u(i,{defaultChecked:t.responseFieldDescriptionChecked,onChange:o.showFieldDesChange},{default:l((()=>[c("span",S,g(e.$t("debug.response.showDes")),1)])),_:1},8,["defaultChecked","onChange"])]),c("span",F,g(e.$t("debug.response.code")),1),c("span",R,g(t.responseStatus.code),1),c("span",B,g(e.$t("debug.response.cost")),1),c("span",I,g(t.responseStatus.cost),1),c("span",T,g(e.$t("debug.response.size")),1),c("span",$,g(o.responseSizeText),1)])),_:1})):b("",!0)]),u(U,{tab:a.i18n.debug.response.content,key:"debugResponse"},{default:l((()=>[t.responseContent?(r(),d(v,{key:0},{default:l((()=>[t.responseContent.blobFlag?(r(),d(v,{key:0},{default:l((()=>[t.responseContent.imageFlag?(r(),h("div",N,[c("img",{src:t.responseContent.blobUrl},null,8,H)])):(r(),h("div",O,[u(j,{type:"link",href:t.responseContent.blobUrl,download:t.responseContent.blobFileName},{default:l((()=>[f(g(e.$t("debug.response.download")),1)])),_:1},8,["href","download"])]))])),_:1})):(r(),h("div",{key:1,id:"responseEditorContent"+t.api.id},[u(x,{onShowDescription:o.showEditorFieldDescription,onDebugEditorChange:o.debugEditorChange,debugResponse:a.debugResponse,value:t.responseContent.text,mode:t.responseContent.mode},null,8,["onShowDescription","onDebugEditorChange","debugResponse","value","mode"])],8,A))])),_:1})):b("",!0)])),_:1},8,["tab"]),u(U,{tab:"Raw",key:"debugRaw",forceRender:""},{default:l((()=>[u(v,{class:"knife4j-debug-response-mt"},{default:l((()=>[u(j,{id:"btnDebugCopyRaw"+t.api.id,type:"primary"},{default:l((()=>[u(W),f(),c("span",null,g(e.$t("debug.response.copy")),1)])),_:1},8,["id"])])),_:1}),u(v,{class:"knife4j-debug-response-mt"},{default:l((()=>[u(q,{rows:10,value:t.responseRawText},null,8,["value"])])),_:1})])),_:1}),u(U,{tab:"Headers",key:"debugHeaders"},{default:l((()=>[u(v,{class:"knife4j-debug-response-mt"},{default:l((()=>[u(L,{bordered:"",size:"small",columns:a.responseHeaderColumn,pagination:a.pagination,dataSource:t.responseHeaders,rowKey:"id"},null,8,["columns","pagination","dataSource"])])),_:1})])),_:1}),u(U,{tab:"Curl",key:"debugCurl"},{default:l((()=>[u(v,{class:"knife4j-debug-response-mt"},{default:l((()=>[u(j,{id:"btnDebugCopyCurl"+t.api.id,type:"primary"},{default:l((()=>[u(W),f(),c("span",null,g(e.$t("debug.response.copy")),1)])),_:1},8,["id"])])),_:1}),u(v,{class:"knife4j-debug-response-mt"},{default:l((()=>[c("pre",z,g(t.responseCurlText),1)])),_:1})])),_:1}),null!=t.responseContent&&null!=t.responseContent.base64&&""!=t.responseContent.base64?(r(),d(U,{tab:"Base64Img",key:"debugBase64Img"},{default:l((()=>[u(v,{class:"knife4j-debug-response-mt"},{default:l((()=>[c("img",{src:t.responseContent.base64},null,8,M)])),_:1})])),_:1})):b("",!0)])),_:1})])),_:1})):(r(),d(v,{key:1}))])),_:1})}]]);export{U as default};
