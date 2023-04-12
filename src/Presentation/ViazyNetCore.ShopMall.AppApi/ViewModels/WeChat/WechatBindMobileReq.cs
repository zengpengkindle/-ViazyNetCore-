using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ViazyNetCore.ShopMall.AppApi.ViewModels
{
    public class WechatBindMobileReq
    {
        public string AuthCode { get; set; }

        public string Mobile { get; set; }

        public string SmsCode { get; set; }

        /// <summary>
        /// 唤起 wx.getUserProfile 后获得
        /// </summary>
        [JsonProperty(PropertyName = "encryptedData")]
        public string EncryptedData { get; set; }

        /// <summary>
        /// 唤起 wx.getUserProfile 后获得
        /// </summary>
        [JsonProperty(PropertyName = "iv")]
        public string Iv { get; set; }
    }
}
