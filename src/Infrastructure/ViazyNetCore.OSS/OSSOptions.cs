﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.AttachmentProvider;

namespace ViazyNetCore.OSS
{
    public class OSSOptions
    {
        /// <summary>
        /// 枚举，OOS提供商
        /// </summary>
        public OSSProvider Provider { get; set; }

        public string DefaultBucketName { get; set; }

        /// <summary>
        /// 节点
        /// </summary>
        /// <remarks>
        /// 腾讯云中表示AppId
        /// </remarks>
        public string Endpoint { get; set; }

        /// <summary>
        /// AccessKey
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// SecretKey
        /// </summary>
        public string SecretKey { get; set; }

        private string _region = "cn-east-1";

        /// <summary>
        /// 地域
        /// </summary>
        public string Region
        {
            get
            {
                return _region;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _region = "cn-east-1";
                }
                else
                {
                    _region = value;
                }
            }
        }

        /// <summary>
        /// 是否启用HTTPS
        /// </summary>
        public bool IsEnableHttps { get; set; } = true;

        /// <summary>
        /// 是否启用缓存，默认缓存在MemeryCache中（可使用自行实现的缓存替代默认缓存）
        /// 在使用之前请评估当前应用的缓存能力能否顶住当前请求
        /// </summary>
        public bool IsEnableCache { get; set; } = false;

        public string SessionToken { get; set; }
        public List<MediaType> MediaTypes { get; set; } = new List<MediaType> { MediaType.Image };
    }
    public enum OSSProvider
    {
        /// <summary>
        /// 无效
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// Minio自建对象储存
        /// </summary>
        Minio = 1,

        /// <summary>
        /// 阿里云OSS
        /// </summary>
        Aliyun = 2,

        /// <summary>
        /// 腾讯云OSS
        /// </summary>
        QCloud = 3,

        /// <summary>
        /// 七牛云 OSS
        /// </summary>
        Qiniu = 4,

        /// <summary>
        /// 华为云 OBS
        /// </summary>
        HuaweiCloud = 5,

        /// <summary>
        /// 百度云 BOS
        /// </summary>
        BaiduCloud = 6,
        /// <summary>
        /// 天翼云 OOS
        /// </summary>
        Ctyun = 7
    }
}
