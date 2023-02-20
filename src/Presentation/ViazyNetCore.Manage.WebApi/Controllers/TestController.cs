using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Manage.WebApi.ViewModel;

namespace ViazyNetCore.Manage.WebApi.Controllers
{
    /// <summary>
    /// 测试Controller
    /// </summary>
    [Route("test")]
    [ApiController]
    [Authorize]
    [Area("test")]
    public class TestController : ControllerBase
    {
        /// <summary>
        /// Post数组返回类型
        /// </summary>
        /// <param name="enum">枚举描述</param>
        /// <returns></returns>
        [Route("getTestModel"), HttpPost]
        public List<TestModel> GetTestModels(TestEnum @enum)
        {
            return new List<TestModel>(); ;
        }

        /// <summary>
        /// Post数组返回类型
        /// </summary>
        /// <param name="test">测试</param>
        /// <returns></returns>
        [Route("getTestModelByQuery"), HttpPost]
        public List<TestModel> GetTestModelsByQurey([FromQuery][Required]string test,TestModel test1)
        {
            return new List<TestModel>(); ;
        }


        /// <summary>
        /// Post数组返回类型
        /// </summary>
        /// <param name="test">测试</param>
        /// <returns></returns>
        [Route("getTestModel/ByQuery"), HttpPost]
        public List<TestModel> GetTestModelsByQureyMuitl([FromQuery][Required] string test, TestModel test1)
        {
            return new List<TestModel>(); ;
        }
    }
}
