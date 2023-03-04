using System.Collections.Generic;
using Essensoft.AspNetCore.Payment.Alipay.Response;
using Essensoft.AspNetCore.Payment.Alipay.Utility;

namespace Essensoft.AspNetCore.Payment.Alipay.Request
{
    /// <summary>
    /// AOP API: alipay.open.agent.mini.create
    /// </summary>
    public class AlipayOpenAgentMiniCreateRequest : IAlipayUploadRequest<AlipayOpenAgentMiniCreateResponse>
    {
        /// <summary>
        /// 小程序应用类目，参数格式：一级类目_二级类目。  应用类目参考文档：https://docs.alipay.com/isv/10325
        /// </summary>
        public string AppCategoryIds { get; set; }

        /// <summary>
        /// 商家小程序描述信息，简要描述小程序主要功能（20-200个字），例：xx小程序提供了xx功能，主要解决了XX问题。
        /// </summary>
        public string AppDesc { get; set; }

        /// <summary>
        /// 小程序英文名称，长度3~20个字符
        /// </summary>
        public string AppEnglishName { get; set; }

        /// <summary>
        /// 商家小程序应用图标，最大256KB，LOGO不允许涉及政治敏感与色情；图片格式必须为：png、jpeg、jpg，建议上传像素为180*180，LOGO核心图形建议在白色160PX范围内
        /// </summary>
        public FileItem AppLogo { get; set; }

        /// <summary>
        /// 代商家创建的小程序应用名称。名称可以由中文、数字、英文及下划线组成，长度在3-20个字符之间，一个中文字等于2个字符，更多名称规则见：https://docs.alipay.com/mini/operation/name
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// 代商家创建的小程序的简介，请用一句话简要描述小程序提供的服务；应用上架后一个自然月可修改5次（10~32个字符）
        /// </summary>
        public string AppSlogan { get; set; }

        /// <summary>
        /// ISV 代商家操作事务编号，通过事务开启接口alipay.open.agent.create调用返回。
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 商家小程序客服邮箱  商家小程序客服电话和邮箱，可以二选一填写，但不能同时为空
        /// </summary>
        public string ServiceEmail { get; set; }

        /// <summary>
        /// 商家小程序的客服电话，推荐填写  商家小程序客服电话和邮箱，可以二选一填写，但不能同时为空
        /// </summary>
        public string ServicePhone { get; set; }

        #region IAlipayUploadRequest Members

        public IDictionary<string, FileItem> GetFileParameters()
        {
            IDictionary<string, FileItem> parameters = new Dictionary<string, FileItem>
            {
                { "app_logo", AppLogo }
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
            return "alipay.open.agent.mini.create";
        }

        public IDictionary<string, string> GetParameters()
        {
            var parameters = new AlipayDictionary
            {
                { "app_category_ids", AppCategoryIds },
                { "app_desc", AppDesc },
                { "app_english_name", AppEnglishName },
                { "app_name", AppName },
                { "app_slogan", AppSlogan },
                { "batch_no", BatchNo },
                { "service_email", ServiceEmail },
                { "service_phone", ServicePhone }
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
