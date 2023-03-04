using System.Collections.Generic;
using Essensoft.AspNetCore.Payment.Alipay.Response;
using Essensoft.AspNetCore.Payment.Alipay.Utility;

namespace Essensoft.AspNetCore.Payment.Alipay.Request
{
    /// <summary>
    /// AOP API: alipay.security.prod.signature.file.upload
    /// </summary>
    public class AlipaySecurityProdSignatureFileUploadRequest : IAlipayUploadRequest<AlipaySecurityProdSignatureFileUploadResponse>
    {
        /// <summary>
        /// 业务唯一标识，由支付宝统一分配，无法自助获取
        /// </summary>
        public string BizProduct { get; set; }

        /// <summary>
        /// 传入上传的文件流
        /// </summary>
        public FileItem FileContent { get; set; }

        #region IAlipayUploadRequest Members

        public IDictionary<string, FileItem> GetFileParameters()
        {
            IDictionary<string, FileItem> parameters = new Dictionary<string, FileItem>
            {
                { "file_content", FileContent }
            };
            return parameters;
        }

        #endregion

        #region IAlipayRequest Members

        private bool needEncrypt;
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

        public void SetApiVersion(string apiVersion){
            this.apiVersion=apiVersion;
        }

        public string GetApiVersion(){
            return apiVersion;
        }

        public string GetApiName()
        {
            return "alipay.security.prod.signature.file.upload";
        }

        public IDictionary<string, string> GetParameters()
        {
            var parameters = new AlipayDictionary
            {
                { "biz_product", BizProduct }
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
