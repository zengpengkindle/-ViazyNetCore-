using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.Manage.WebApi.Tasks;

namespace ViazyNetCore.Manage.WebApi.Controllers
{
    [Route("api/eventbus")]
    [ApiController]
    [Area("test")]
    [AllowAnonymous]
    public class EventBusController : ControllerBase
    {
        private readonly IEventBus _eventBus;

        public EventBusController(IEventBus eventBus)
        {
            this._eventBus = eventBus;
        }

        [HttpGet]
        [Route("sendTest")]
        public async Task SendMqTestModel()
        {
            await this._eventBus.PublishAsync(new MqTestModel
            {
                EventTime = DateTime.Now,
                Id = Guid.NewGuid(),
            });
        }
    }
}
