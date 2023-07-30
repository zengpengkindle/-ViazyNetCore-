﻿using System;
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
        Texteara = 2,
        Radio = 3
    }
}
