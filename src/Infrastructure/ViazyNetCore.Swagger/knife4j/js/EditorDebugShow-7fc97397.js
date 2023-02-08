import{_ as e,V as t,a1 as n,a2 as o,o as i,j as r,l as a,I as s,r as l}from"./doc-381f7261.js";import"./ace-d54cf035.js";import"./ext-language_tools-bf1decbd.js";var u;u={exports:{}},ace.define("ace/mode/json_highlight_rules",["require","exports","module","ace/lib/oop","ace/mode/text_highlight_rules"],(function(e,t,n){var o=e("../lib/oop"),i=e("./text_highlight_rules").TextHighlightRules,r=function(){this.$rules={start:[{token:"variable",regex:'["](?:(?:\\\\.)|(?:[^"\\\\]))*?["]\\s*(?=:)'},{token:"string",regex:'"',next:"string"},{token:"constant.numeric",regex:"0[xX][0-9a-fA-F]+\\b"},{token:"constant.numeric",regex:"[+-]?\\d+(?:(?:\\.\\d*)?(?:[eE][+-]?\\d+)?)?\\b"},{token:"constant.language.boolean",regex:"(?:true|false)\\b"},{token:"text",regex:"['](?:(?:\\\\.)|(?:[^'\\\\]))*?[']"},{token:"comment",regex:"\\/\\/.*$"},{token:"comment.start",regex:"\\/\\*",next:"comment"},{token:"paren.lparen",regex:"[[({]"},{token:"paren.rparen",regex:"[\\])}]"},{token:"punctuation.operator",regex:/[,]/},{token:"text",regex:"\\s+"}],string:[{token:"constant.language.escape",regex:/\\(?:x[0-9a-fA-F]{2}|u[0-9a-fA-F]{4}|["\\\/bfnrt])/},{token:"string",regex:'"|$',next:"start"},{defaultToken:"string"}],comment:[{token:"comment.end",regex:"\\*\\/",next:"start"},{defaultToken:"comment"}]}};o.inherits(r,i),t.JsonHighlightRules=r})),ace.define("ace/mode/matching_brace_outdent",["require","exports","module","ace/range"],(function(e,t,n){var o=e("../range").Range,i=function(){};(function(){this.checkOutdent=function(e,t){return!!/^\s+$/.test(e)&&/^\s*\}/.test(t)},this.autoOutdent=function(e,t){var n=e.getLine(t).match(/^(\s*\})/);if(!n)return 0;var i=n[1].length,r=e.findMatchingBracket({row:t,column:i});if(!r||r.row==t)return 0;var a=this.$getIndent(e.getLine(r.row));e.replace(new o(t,0,t,i-1),a)},this.$getIndent=function(e){return e.match(/^\s*/)[0]}}).call(i.prototype),t.MatchingBraceOutdent=i})),ace.define("ace/mode/folding/cstyle",["require","exports","module","ace/lib/oop","ace/range","ace/mode/folding/fold_mode"],(function(e,t,n){var o=e("../../lib/oop"),i=e("../../range").Range,r=e("./fold_mode").FoldMode,a=t.FoldMode=function(e){e&&(this.foldingStartMarker=new RegExp(this.foldingStartMarker.source.replace(/\|[^|]*?$/,"|"+e.start)),this.foldingStopMarker=new RegExp(this.foldingStopMarker.source.replace(/\|[^|]*?$/,"|"+e.end)))};o.inherits(a,r),function(){this.foldingStartMarker=/([\{\[\(])[^\}\]\)]*$|^\s*(\/\*)/,this.foldingStopMarker=/^[^\[\{\(]*([\}\]\)])|^[\s\*]*(\*\/)/,this.singleLineBlockCommentRe=/^\s*(\/\*).*\*\/\s*$/,this.tripleStarBlockCommentRe=/^\s*(\/\*\*\*).*\*\/\s*$/,this.startRegionRe=/^\s*(\/\*|\/\/)#?region\b/,this._getFoldWidgetBase=this.getFoldWidget,this.getFoldWidget=function(e,t,n){var o=e.getLine(n);if(this.singleLineBlockCommentRe.test(o)&&!this.startRegionRe.test(o)&&!this.tripleStarBlockCommentRe.test(o))return"";var i=this._getFoldWidgetBase(e,t,n);return!i&&this.startRegionRe.test(o)?"start":i},this.getFoldWidgetRange=function(e,t,n,o){var i,r=e.getLine(n);if(this.startRegionRe.test(r))return this.getCommentRegionBlock(e,r,n);if(i=r.match(this.foldingStartMarker)){var a=i.index;if(i[1])return this.openingBracketBlock(e,i[1],n,a);var s=e.getCommentFoldRange(n,a+i[0].length,1);return s&&!s.isMultiLine()&&(o?s=this.getSectionRange(e,n):"all"!=t&&(s=null)),s}return"markbegin"!==t&&(i=r.match(this.foldingStopMarker))?(a=i.index+i[0].length,i[1]?this.closingBracketBlock(e,i[1],n,a):e.getCommentFoldRange(n,a,-1)):void 0},this.getSectionRange=function(e,t){for(var n=e.getLine(t),o=n.search(/\S/),r=t,a=n.length,s=t+=1,l=e.getLength();++t<l;){var u=(n=e.getLine(t)).search(/\S/);if(-1!==u){if(o>u)break;var g=this.getFoldWidgetRange(e,"all",t);if(g){if(g.start.row<=r)break;if(g.isMultiLine())t=g.end.row;else if(o==u)break}s=t}}return new i(r,a,s,e.getLine(s).length)},this.getCommentRegionBlock=function(e,t,n){for(var o=t.search(/\s*$/),r=e.getLength(),a=n,s=/^\s*(?:\/\*|\/\/|--)#?(end)?region\b/,l=1;++n<r;){t=e.getLine(n);var u=s.exec(t);if(u&&(u[1]?l--:l++,!l))break}if(n>a)return new i(a,o,n,t.length)}}.call(a.prototype)})),ace.define("ace/mode/json",["require","exports","module","ace/lib/oop","ace/mode/text","ace/mode/json_highlight_rules","ace/mode/matching_brace_outdent","ace/mode/behaviour/cstyle","ace/mode/folding/cstyle","ace/worker/worker_client"],(function(e,t,n){var o=e("../lib/oop"),i=e("./text").Mode,r=e("./json_highlight_rules").JsonHighlightRules,a=e("./matching_brace_outdent").MatchingBraceOutdent,s=e("./behaviour/cstyle").CstyleBehaviour,l=e("./folding/cstyle").FoldMode,u=e("../worker/worker_client").WorkerClient,g=function(){this.HighlightRules=r,this.$outdent=new a,this.$behaviour=new s,this.foldingRules=new l};o.inherits(g,i),function(){this.lineCommentStart="//",this.blockComment={start:"/*",end:"*/"},this.getNextLineIndent=function(e,t,n){var o=this.$getIndent(t);return"start"==e&&t.match(/^.*[\{\(\[]\s*$/)&&(o+=n),o},this.checkOutdent=function(e,t,n){return this.$outdent.checkOutdent(t,n)},this.autoOutdent=function(e,t,n){this.$outdent.autoOutdent(t,n)},this.createWorker=function(e){var t=new u(["ace"],"ace/mode/json_worker","JsonWorker");return t.attachToDocument(e.getDocument()),t.on("annotate",(function(t){e.setAnnotations(t.data)})),t.on("terminate",(function(){e.clearAnnotations()})),t},this.$id="ace/mode/json"}.call(g.prototype),t.Mode=g})),ace.require(["ace/mode/json"],(function(e){u&&(u.exports=e)}));!function(e,t){ace.define("ace/mode/xml_highlight_rules",["require","exports","module","ace/lib/oop","ace/mode/text_highlight_rules"],(function(e,t,n){var o=e("../lib/oop"),i=e("./text_highlight_rules").TextHighlightRules,r=function(e){var t="[_:a-zA-ZÀ-￿][-_:.a-zA-Z0-9À-￿]*";this.$rules={start:[{token:"string.cdata.xml",regex:"<\\!\\[CDATA\\[",next:"cdata"},{token:["punctuation.instruction.xml","keyword.instruction.xml"],regex:"(<\\?)("+t+")",next:"processing_instruction"},{token:"comment.start.xml",regex:"<\\!--",next:"comment"},{token:["xml-pe.doctype.xml","xml-pe.doctype.xml"],regex:"(<\\!)(DOCTYPE)(?=[\\s])",next:"doctype",caseInsensitive:!0},{include:"tag"},{token:"text.end-tag-open.xml",regex:"</"},{token:"text.tag-open.xml",regex:"<"},{include:"reference"},{defaultToken:"text.xml"}],processing_instruction:[{token:"entity.other.attribute-name.decl-attribute-name.xml",regex:t},{token:"keyword.operator.decl-attribute-equals.xml",regex:"="},{include:"whitespace"},{include:"string"},{token:"punctuation.xml-decl.xml",regex:"\\?>",next:"start"}],doctype:[{include:"whitespace"},{include:"string"},{token:"xml-pe.doctype.xml",regex:">",next:"start"},{token:"xml-pe.xml",regex:"[-_a-zA-Z0-9:]+"},{token:"punctuation.int-subset",regex:"\\[",push:"int_subset"}],int_subset:[{token:"text.xml",regex:"\\s+"},{token:"punctuation.int-subset.xml",regex:"]",next:"pop"},{token:["punctuation.markup-decl.xml","keyword.markup-decl.xml"],regex:"(<\\!)("+t+")",push:[{token:"text",regex:"\\s+"},{token:"punctuation.markup-decl.xml",regex:">",next:"pop"},{include:"string"}]}],cdata:[{token:"string.cdata.xml",regex:"\\]\\]>",next:"start"},{token:"text.xml",regex:"\\s+"},{token:"text.xml",regex:"(?:[^\\]]|\\](?!\\]>))+"}],comment:[{token:"comment.end.xml",regex:"--\x3e",next:"start"},{defaultToken:"comment.xml"}],reference:[{token:"constant.language.escape.reference.xml",regex:"(?:&#[0-9]+;)|(?:&#x[0-9a-fA-F]+;)|(?:&[a-zA-Z0-9_:\\.-]+;)"}],attr_reference:[{token:"constant.language.escape.reference.attribute-value.xml",regex:"(?:&#[0-9]+;)|(?:&#x[0-9a-fA-F]+;)|(?:&[a-zA-Z0-9_:\\.-]+;)"}],tag:[{token:["meta.tag.punctuation.tag-open.xml","meta.tag.punctuation.end-tag-open.xml","meta.tag.tag-name.xml"],regex:"(?:(<)|(</))((?:"+t+":)?"+t+")",next:[{include:"attributes"},{token:"meta.tag.punctuation.tag-close.xml",regex:"/?>",next:"start"}]}],tag_whitespace:[{token:"text.tag-whitespace.xml",regex:"\\s+"}],whitespace:[{token:"text.whitespace.xml",regex:"\\s+"}],string:[{token:"string.xml",regex:"'",push:[{token:"string.xml",regex:"'",next:"pop"},{defaultToken:"string.xml"}]},{token:"string.xml",regex:'"',push:[{token:"string.xml",regex:'"',next:"pop"},{defaultToken:"string.xml"}]}],attributes:[{token:"entity.other.attribute-name.xml",regex:t},{token:"keyword.operator.attribute-equals.xml",regex:"="},{include:"tag_whitespace"},{include:"attribute_value"}],attribute_value:[{token:"string.attribute-value.xml",regex:"'",push:[{token:"string.attribute-value.xml",regex:"'",next:"pop"},{include:"attr_reference"},{defaultToken:"string.attribute-value.xml"}]},{token:"string.attribute-value.xml",regex:'"',push:[{token:"string.attribute-value.xml",regex:'"',next:"pop"},{include:"attr_reference"},{defaultToken:"string.attribute-value.xml"}]}]},this.constructor===r&&this.normalizeRules()};(function(){this.embedTagRules=function(e,t,n){this.$rules.tag.unshift({token:["meta.tag.punctuation.tag-open.xml","meta.tag."+n+".tag-name.xml"],regex:"(<)("+n+"(?=\\s|>|$))",next:[{include:"attributes"},{token:"meta.tag.punctuation.tag-close.xml",regex:"/?>",next:t+"start"}]}),this.$rules[n+"-end"]=[{include:"attributes"},{token:"meta.tag.punctuation.tag-close.xml",regex:"/?>",next:"start",onMatch:function(e,t,n){return n.splice(0),this.token}}],this.embedRules(e,t,[{token:["meta.tag.punctuation.end-tag-open.xml","meta.tag."+n+".tag-name.xml"],regex:"(</)("+n+"(?=\\s|>|$))",next:n+"-end"},{token:"string.cdata.xml",regex:"<\\!\\[CDATA\\["},{token:"string.cdata.xml",regex:"\\]\\]>"}])}}).call(i.prototype),o.inherits(r,i),t.XmlHighlightRules=r})),ace.define("ace/mode/behaviour/xml",["require","exports","module","ace/lib/oop","ace/mode/behaviour","ace/token_iterator","ace/lib/lang"],(function(e,t,n){var o=e("../../lib/oop"),i=e("../behaviour").Behaviour,r=e("../../token_iterator").TokenIterator;function a(e,t){return e&&e.type.lastIndexOf(t+".xml")>-1}e("../../lib/lang");var s=function(){this.add("string_dquotes","insertion",(function(e,t,n,o,i){if('"'==i||"'"==i){var s=i,l=o.doc.getTextRange(n.getSelectionRange());if(""!==l&&"'"!==l&&'"'!=l&&n.getWrapBehavioursEnabled())return{text:s+l+s,selection:!1};var u=n.getCursorPosition(),g=o.doc.getLine(u.row).substring(u.column,u.column+1),c=new r(o,u.row,u.column),d=c.getCurrentToken();if(g==s&&(a(d,"attribute-value")||a(d,"string")))return{text:"",selection:[1,1]};if(d||(d=c.stepBackward()),!d)return;for(;a(d,"tag-whitespace")||a(d,"whitespace");)d=c.stepBackward();var m=!g||g.match(/\s/);if(a(d,"attribute-equals")&&(m||">"==g)||a(d,"decl-attribute-equals")&&(m||"?"==g))return{text:s+s,selection:[1,1]}}})),this.add("string_dquotes","deletion",(function(e,t,n,o,i){var r=o.doc.getTextRange(i);if(!i.isMultiLine()&&('"'==r||"'"==r)&&o.doc.getLine(i.start.row).substring(i.start.column+1,i.start.column+2)==r)return i.end.column++,i})),this.add("autoclosing","insertion",(function(e,t,n,o,i){if(">"==i){var s=n.getSelectionRange().start,l=new r(o,s.row,s.column),u=l.getCurrentToken()||l.stepBackward();if(!u||!(a(u,"tag-name")||a(u,"tag-whitespace")||a(u,"attribute-name")||a(u,"attribute-equals")||a(u,"attribute-value")))return;if(a(u,"reference.attribute-value"))return;if(a(u,"attribute-value")){var g=l.getCurrentTokenColumn()+u.value.length;if(s.column<g)return;if(s.column==g){var c=l.stepForward();if(c&&a(c,"attribute-value"))return;l.stepBackward()}}if(/^\s*>/.test(o.getLine(s.row).slice(s.column)))return;for(;!a(u,"tag-name");)if("<"==(u=l.stepBackward()).value){u=l.stepForward();break}var d=l.getCurrentTokenRow(),m=l.getCurrentTokenColumn();if(a(l.stepBackward(),"end-tag-open"))return;var h=u.value;if(d==s.row&&(h=h.substring(0,s.column-m)),this.voidElements.hasOwnProperty(h.toLowerCase()))return;return{text:"></"+h+">",selection:[1,1]}}})),this.add("autoindent","insertion",(function(e,t,n,o,i){if("\n"==i){var a=n.getCursorPosition(),s=o.getLine(a.row),l=new r(o,a.row,a.column),u=l.getCurrentToken();if(u&&-1!==u.type.indexOf("tag-close")){if("/>"==u.value)return;for(;u&&-1===u.type.indexOf("tag-name");)u=l.stepBackward();if(!u)return;var g=u.value,c=l.getCurrentTokenRow();if(!(u=l.stepBackward())||-1!==u.type.indexOf("end-tag"))return;if(this.voidElements&&!this.voidElements[g]){var d=o.getTokenAt(a.row,a.column+1),m=(s=o.getLine(c),this.$getIndent(s)),h=m+o.getTabString();return d&&"</"===d.value?{text:"\n"+h+"\n"+m,selection:[1,h.length,1,h.length]}:{text:"\n"+h}}}}}))};o.inherits(s,i),t.XmlBehaviour=s})),ace.define("ace/mode/folding/xml",["require","exports","module","ace/lib/oop","ace/range","ace/mode/folding/fold_mode"],(function(e,t,n){var o=e("../../lib/oop"),i=e("../../range").Range,r=e("./fold_mode").FoldMode,a=t.FoldMode=function(e,t){r.call(this),this.voidElements=e||{},this.optionalEndTags=o.mixin({},this.voidElements),t&&o.mixin(this.optionalEndTags,t)};o.inherits(a,r);var s=function(){this.tagName="",this.closing=!1,this.selfClosing=!1,this.start={row:0,column:0},this.end={row:0,column:0}};function l(e,t){return e.type.lastIndexOf(t+".xml")>-1}(function(){this.getFoldWidget=function(e,t,n){var o=this._getFirstTagInLine(e,n);return o?o.closing||!o.tagName&&o.selfClosing?"markbeginend"===t?"end":"":!o.tagName||o.selfClosing||this.voidElements.hasOwnProperty(o.tagName.toLowerCase())||this._findEndTagInLine(e,n,o.tagName,o.end.column)?"":"start":this.getCommentFoldWidget(e,n)},this.getCommentFoldWidget=function(e,t){return/comment/.test(e.getState(t))&&/<!-/.test(e.getLine(t))?"start":""},this._getFirstTagInLine=function(e,t){for(var n=e.getTokens(t),o=new s,i=0;i<n.length;i++){var r=n[i];if(l(r,"tag-open")){if(o.end.column=o.start.column+r.value.length,o.closing=l(r,"end-tag-open"),!(r=n[++i]))return null;for(o.tagName=r.value,o.end.column+=r.value.length,i++;i<n.length;i++)if(r=n[i],o.end.column+=r.value.length,l(r,"tag-close")){o.selfClosing="/>"==r.value;break}return o}if(l(r,"tag-close"))return o.selfClosing="/>"==r.value,o;o.start.column+=r.value.length}return null},this._findEndTagInLine=function(e,t,n,o){for(var i=e.getTokens(t),r=0,a=0;a<i.length;a++){var s=i[a];if(!((r+=s.value.length)<o)&&l(s,"end-tag-open")&&(s=i[a+1])&&s.value==n)return!0}return!1},this.getFoldWidgetRange=function(e,t,n){var o=e.getMatchingTags({row:n,column:0});return o?new i(o.openTag.end.row,o.openTag.end.column,o.closeTag.start.row,o.closeTag.start.column):this.getCommentFoldWidget(e,n)&&e.getCommentFoldRange(n,e.getLine(n).length)}}).call(a.prototype)})),ace.define("ace/mode/xml",["require","exports","module","ace/lib/oop","ace/lib/lang","ace/mode/text","ace/mode/xml_highlight_rules","ace/mode/behaviour/xml","ace/mode/folding/xml","ace/worker/worker_client"],(function(e,t,n){var o=e("../lib/oop"),i=e("../lib/lang"),r=e("./text").Mode,a=e("./xml_highlight_rules").XmlHighlightRules,s=e("./behaviour/xml").XmlBehaviour,l=e("./folding/xml").FoldMode,u=e("../worker/worker_client").WorkerClient,g=function(){this.HighlightRules=a,this.$behaviour=new s,this.foldingRules=new l};o.inherits(g,r),function(){this.voidElements=i.arrayToMap([]),this.blockComment={start:"\x3c!--",end:"--\x3e"},this.createWorker=function(e){var t=new u(["ace"],"ace/mode/xml_worker","Worker");return t.attachToDocument(e.getDocument()),t.on("error",(function(t){e.setAnnotations(t.data)})),t.on("terminate",(function(){e.clearAnnotations()})),t},this.$id="ace/mode/xml"}.call(g.prototype),t.Mode=g})),ace.require(["ace/mode/xml"],(function(t){e&&(e.exports=t)}))}({exports:{}});!function(e,t){ace.require(["ace/mode/text"],(function(t){e&&(e.exports=t)}))}({exports:{}});const g={name:"EditorShow",components:{editor:t},props:{value:{type:String,required:!0,default:""},mode:{type:String,required:!0,default:"json"},debugResponse:{type:Boolean,default:!1}},emits:["update:value","debugEditorChange","showDescription"],setup(e){const t=n(e.value);return o((()=>e.value),(()=>{t.value=e.value})),{valueText:t}},data:()=>({editor:null,editorHeight:200,debugOptions:{readOnly:!1,autoScrollEditorIntoView:!0,displayIndentGuides:!1,fixedWidthGutter:!0},commonOptions:{readOnly:!1}}),methods:{resetEditorHeight(){var e=this;setTimeout((()=>{var t=e.editor.session.getLength();1==t&&(t=15),t<15&&(t=e.debugResponse?30:15),t>20&&(e.debugResponse||(t=20));var n=16*t;n>2e3&&(n=2e3),e.editorHeight=n}),10)},change(){this.$emit("update:value",this.valueText),this.debugResponse||this.resetEditorHeight()},editorInit(e){var t=this;this.editor=e,this.debugResponse?(this.editor.getSession().setUseWrapMode(!0),this.editor.setOptions(this.debugOptions),"text"==this.mode&&this.editor.getSession().setUseWrapMode(!0)):this.editor.setOptions(this.commonOptions),this.resetEditorHeight(),this.editor.renderer.on("afterRender",(function(){var e=t.editor.session.getLength();t.$emit("showDescription",e)}))}}},c={key:0},d={key:1};const m=e(g,[["render",function(e,t,n,o,u,g){const m=l("editor");return i(),r("div",null,[n.debugResponse?(i(),r("div",c,[a(m,{class:"knife4j-debug-ace-editor",onInput:g.change,options:u.debugOptions,value:o.valueText,"onUpdate:value":t[0]||(t[0]=e=>o.valueText=e),onInit:g.editorInit,lang:n.mode,theme:"eclipse",width:"100%",style:s({height:u.editorHeight+"px"})},null,8,["onInput","options","value","onInit","lang","style"])])):(i(),r("div",d,[a(m,{value:o.valueText,"onUpdate:value":t[1]||(t[1]=e=>o.valueText=e),onInit:g.editorInit,onInput:g.change,lang:n.mode,theme:"eclipse",width:"100%",style:s({height:u.editorHeight+"px"})},null,8,["value","onInit","onInput","lang","style"])]))])}]]);export{m as default};
