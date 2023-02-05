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
        [Description("默认")]
        Default=0,
        [Description("测试")]
        Test = 1,
    }
}
