using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ViazyNetCore.AttachmentProvider;

namespace ViazyNetCore.ShopMall.Manage.Application.Controllers
{
    [Route("api/common")]
    [ApiController]
    [Area("shopmall")]
    [Authorize]
    public class CommonController : ControllerBase
    {
        /// <summary>
        /// 上传图片(图片大小不得超过5mb)
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("upload/image")]
        public async Task<string> UploadImage([FromServices] IStoreProvider storeProvider)
        {
            var file = this.HttpContext.Request.Form.Files[0];
            var attachment = await storeProvider.SaveAsync(file);
            return attachment.RelativeUrl;
        }
    }
}
