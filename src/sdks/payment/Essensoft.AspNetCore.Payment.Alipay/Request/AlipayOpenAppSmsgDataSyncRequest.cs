using System.Collections.Generic;
using Essensoft.AspNetCore.Payment.Alipay.Response;

namespace Essensoft.AspNetCore.Payment.Alipay.Request
{
    /// <summary>
    /// AOP API: alipay.open.app.smsg.data.sync
    /// </summary>
    public class AlipayOpenAppSmsgDataSyncRequest : IAlipayRequest<AlipayOpenAppSmsgDataSyncResponse>
    {
        /// <summary>
        /// To蚂蚁消息测试数据同步
        /// </summary>
        public string BizContent { get; set; }

        #region IAlipayRequest Members

        private bool  needEncrypt;
        private string apiVersion = "1.0";
        private string terminalType;
        private string terminalInfo;
        private string prodCode;
        private string notifyUrl;
        private string returnUrl;
        private AlipayObject bizModel;

        public void SetNeedEncrypt(bool needEncrypt){
             this.needEncrypt=needEncrypt;
        }

        public bool GetNeedEncrypt(){

            return needEncrypt;
        }

        public void SetNotifyUrl(string notifyUrl){
            this.notifyUrl = notifyUrl;
        }

        public string GetNotifyUrl(){
            return notifyUrl;
        }

        public void SetReturnUrl(string returnUrl){
            this.returnUrl = returnUrl;
        }

        public string GetReturnUrl(){
            return returnUrl;
        }

        public void SetTerminalType(string terminalType){
			this.terminalType=terminalType;
		}

        public string GetTerminalType(){
    		return terminalType;
    	}

        public void SetTerminalInfo(string terminalInfo){
    		this.terminalInfo=terminalInfo;
    	}

        public string GetTerminalInfo(){
    		return terminalInfo;
    	}

        public void SetProdCode(string prodCode){
            this.prodCode=prodCode;
        }

        public string GetProdCode(){
            return prodCode;
        }

        public string GetApiName()
        {
            return "alipay.open.app.smsg.data.sync";
        }

        public void SetApiVersion(string apiVersion){
            this.apiVersion=apiVersion;
        }

        public string GetApiVersion(){
            return apiVersion;
        }

        public IDictionary<string, string> GetParameters()
        {
            var parameters = new AlipayDictionary
            {
                { "biz_content", BizContent }
            };
            return parameters;
        }

        public AlipayObject GetBizModel()
        {
            return bizModel;
        }

        public void SetBizModel(AlipayObject bizModel)
        {
            this.bizModel = bizModel;
        }

        #endregion
    }
}
