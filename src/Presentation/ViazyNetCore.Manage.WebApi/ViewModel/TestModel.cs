using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Manage.WebApi.ViewModel
{
    public class TestModel
    {
        public TestEnum Value1 { get; set; }
    }

    public enum TestEnum
    {
        /// <summary>
        /// 默认
        /// </summary>
        [Description("默认")]
        Default = 0,
        /// <summary>
        /// 测试
        /// </summary>
        [Description("测试")]
        Test = 1,
    }

    public class InnerTestModel
    {
        /// <summary>
        /// 枚举类型
        /// </summary>
        public TestEnum Enum1 { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 数字类型
        /// </summary>
        public int Number { get; set; }
    }
}
