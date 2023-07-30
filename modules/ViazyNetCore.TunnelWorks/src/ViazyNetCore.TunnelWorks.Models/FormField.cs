using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeSql.DataAnnotations;

namespace ViazyNetCore.TunnelWorks.Models
{
    public class FormField : EntityUpdate
    {
        public string FieldName { get; set; }
        public string Name { get; set; }
        public long FormId { get; set; }
        public WeightType Type { get; set; }
        public string Lable { get; set; }
        public string Remark { get; set; }
        public string InputTips { get; set; }
        public string MaxLength { get; set; }
        public string DefaultValue { get; set; }
        public int IsUnique { get; set; }
        public bool IsNull { get; set; }
        public int Sorting { get; set; }
        [Column(StringLength = -1)]
        public string Options { get; set; }
        public int Operating { get; set; }
        public int ExamineCategoryId { get; set; }
        public int FieldType { get; set; }
        public int Relevant { get; set; }
    }

    public enum WeightType
    {
        Input = 1,
        Textarea = 2,
        Number = 3,
        Radio = 4,
        Checkbox=5,
        Select=6,
        Time=7,
        Time_Range=8,
        Switch=9,
        Rate=10,
        Color=11,
        Slider=12,
        Static_Text=13,
        Html_Text=14,
        Button=15,
        Divider=16,
        Picture_Upload=17,
        File_Upload=18,
        Rich_Editor=19,
        /// <summary>
        /// 级联选择
        /// </summary>
        Cascader=20
    }
}
