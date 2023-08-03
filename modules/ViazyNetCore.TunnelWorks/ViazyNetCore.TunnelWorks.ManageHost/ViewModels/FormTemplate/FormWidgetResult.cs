using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ViazyNetCore.TunnelWorks.ManageHost.ViewModels
{
    public class FormWidgetResult
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public string? Icon { get; set; }
        public bool FormItemFlag { get; set; }
        public FormWidgetOptionResult Options { get; set; }
    }

    public class FormWidgetOptionResult
    {
        public string Name { get; set; }
        //"label": "input",
        public string Label { get; set; }
        //"labelAlign": "",
        public string LabelAlign { get; set; }
        //"type": "text",
        public string? Type { get; set; }
        //"defaultValue": "",
        public JToken? DefaultValue { get; set; }
        //"placeholder": "",
        public string? Placeholder { get; set; }
        //"columnWidth": "200px",
        public string? ColumnWidth { get; set; }
        //"size": "",
        public string? Size { get; set; }
        //"labelWidth": null,
        public string? LabelWidth { get; set; }
        //"labelHidden": false,
        public bool LabelHidden { get; set; }
        //"readonly": false,
        public bool Readonly { get; set; }
        //"disabled": false,
        public bool Disabled { get; set; }
        //"hidden": false,
        public bool Hidden { get; set; }
        //"clearable": true,
        public bool Clearable { get; set; }
        //"showPassword": false,
        public bool ShowPassword { get; set; }
        //"required": false,
        public bool Required { get; set; }
        //"requiredHint": "",
        public string? RequiredHint { get; set; }
        //"validation": "",
        public string? Validation { get; set; }
        //"validationHint": "",
        public string? ValidationHint { get; set; }
        //"customClass": "",
        //"labelIconClass": null,
        //"labelIconPosition": "rear",
        //"labelTooltip": null,
        //"minLength": null,
        //"maxLength": null,
        //"showWordLimit": false,
        public bool ShowWordLimit { get; set; }
        //"prefixIcon": "",
        //"suffixIcon": "",
        //"appendButton": false,
        public bool AppendButton { get; set; }
        //"appendButtonDisabled": false,
        public bool AppendButtonDisabled { get; set; }
        public JToken? OptionItems { get; set; }
        //"buttonIcon": "custom-search",
        //"onCreated": "",
        //"onMounted": "",
        //"onInput": "",
        //"onChange": "",
        //"onFocus": "",
        //"onBlur": "",
        //"onValidate": "",
        //"onAppendButtonClick": ""
    }
}
