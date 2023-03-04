using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Essensoft.AspNetCore.Payment.Alipay.Domain
{
    /// <summary>
    /// AlipayEcoRenthouseCommunityBaseinfoSyncModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayEcoRenthouseCommunityBaseinfoSyncModel : AlipayObject
    {
        /// <summary>
        /// 商圈编码
        /// </summary>
        [JsonProperty("bus_code")]
        [XmlElement("bus_code")]
        public string BusCode { get; set; }

        /// <summary>
        /// 商圈所在纬度
        /// </summary>
        [JsonProperty("bus_lat")]
        [XmlElement("bus_lat")]
        public string BusLat { get; set; }

        /// <summary>
        /// 商圈所在经度
        /// </summary>
        [JsonProperty("bus_lng")]
        [XmlElement("bus_lng")]
        public string BusLng { get; set; }

        /// <summary>
        /// 商圈名称
        /// </summary>
        [JsonProperty("bus_name")]
        [XmlElement("bus_name")]
        public string BusName { get; set; }

        /// <summary>
        /// 商圈覆盖半径(单位:米)
        /// </summary>
        [JsonProperty("bus_radius")]
        [XmlElement("bus_radius")]
        public long BusRadius { get; set; }

        /// <summary>
        /// 城市编码
        /// </summary>
        [JsonProperty("city_code")]
        [XmlElement("city_code")]
        public string CityCode { get; set; }

        /// <summary>
        /// 城市所在纬度
        /// </summary>
        [JsonProperty("city_lat")]
        [XmlElement("city_lat")]
        public string CityLat { get; set; }

        /// <summary>
        /// 城市所在经度
        /// </summary>
        [JsonProperty("city_lng")]
        [XmlElement("city_lng")]
        public string CityLng { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        [JsonProperty("city_name")]
        [XmlElement("city_name")]
        public string CityName { get; set; }

        /// <summary>
        /// 小区/大楼编码
        /// </summary>
        [JsonProperty("community_code")]
        [XmlElement("community_code")]
        public string CommunityCode { get; set; }

        /// <summary>
        /// 小区/大楼所在纬度
        /// </summary>
        [JsonProperty("community_lat")]
        [XmlElement("community_lat")]
        public string CommunityLat { get; set; }

        /// <summary>
        /// 小区/大楼所在经度
        /// </summary>
        [JsonProperty("community_lng")]
        [XmlElement("community_lng")]
        public string CommunityLng { get; set; }

        /// <summary>
        /// 小区/大楼名称
        /// </summary>
        [JsonProperty("community_name")]
        [XmlElement("community_name")]
        public string CommunityName { get; set; }

        /// <summary>
        /// 小区/大楼弄号
        /// </summary>
        [JsonProperty("community_nong")]
        [XmlElement("community_nong")]
        public string CommunityNong { get; set; }

        /// <summary>
        /// 小区/大楼街道
        /// </summary>
        [JsonProperty("community_street")]
        [XmlElement("community_street")]
        public string CommunityStreet { get; set; }

        /// <summary>
        /// 小区/大楼标识类型 1：小区  2:大楼
        /// </summary>
        [JsonProperty("community_tag")]
        [XmlElement("community_tag")]
        public string CommunityTag { get; set; }

        /// <summary>
        /// 行政区域编码
        /// </summary>
        [JsonProperty("district_code")]
        [XmlElement("district_code")]
        public string DistrictCode { get; set; }

        /// <summary>
        /// 行政区域所在纬度
        /// </summary>
        [JsonProperty("district_lat")]
        [XmlElement("district_lat")]
        public string DistrictLat { get; set; }

        /// <summary>
        /// 行政区域所在经度
        /// </summary>
        [JsonProperty("district_lng")]
        [XmlElement("district_lng")]
        public string DistrictLng { get; set; }

        /// <summary>
        /// 行政区域名称
        /// </summary>
        [JsonProperty("district_name")]
        [XmlElement("district_name")]
        public string DistrictName { get; set; }

        /// <summary>
        /// 地铁线地铁站关系
        /// </summary>
        [JsonProperty("subway_stations")]
        [XmlArray("subway_stations")]
        [XmlArrayItem("string")]
        public List<string> SubwayStations { get; set; }
    }
}
