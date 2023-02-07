using System;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Manage.WebApi.ViewModel;

namespace ViazyNetCore.Manage.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// ��ȡget
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// ��״̬�뷵�ز���
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        [HttpPost, Route("TestPost")]
        [SwaggerStatusCodeResponse(10021, "����״̬����Ӧ")]
        public TestModel PostModel(TestModel test)
        {
            return test;
        }

        /// <summary>
        /// ����ֵǶ�ײ���
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost, Route("GetInnerModel")]
        public InnerTestModel GetInnerTestModel(TestModel model)
        {
            return new InnerTestModel
            {
                Enum1 = model.Value1,
                WeatherForecast = new WeatherForecast
                {
                    Date = DateTime.Now,
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }
            };
        }
    }
}