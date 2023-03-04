using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Modules.ShopMall.Models;

namespace ViazyNetCore.Modules.ShopMall
{
    public class StockPageArgments
    {
        public string ProductId { get; set; }

        public string SkuId { get; set; }
    }

    public class StockModel
    {
        /// <summary>
        /// 设置或获取一个值，表示主键。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品编号。（与商品Sku编号二选一）
        /// </summary>
        public string ProductId { get; set; }

        public string Title { get; set; }

        public string ImgUrl { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示在库数量。
        /// </summary>
        public int InStock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示锁定数量（待付款商品）。
        /// </summary>
        public int Lock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示出库数。
        /// </summary>
        public int OutStock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示退货数。
        /// </summary>
        public int Refund { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示销量。
        /// </summary>
        public int SellNum { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示换货数。
        /// </summary>
        public int Exchange { get; set; }

        public List<StockSkuModel> Skus { get; set; } = new List<StockSkuModel>();

    }

    public class StockSkuModel
    {
        /// <summary>
        /// 设置或获取一个值，表示主键。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品Sku编号。（与商品编号二选一）
        /// </summary>
        public string ProductSkuId { get; set; }

        public string SkuText { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示在库数量。
        /// </summary>
        public int InStock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示锁定数量（待付款商品）。
        /// </summary>
        public int Lock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示出库数。
        /// </summary>
        public int OutStock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示销量。
        /// </summary>
        public int SellNum { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示退货数。
        /// </summary>
        public int Refund { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示换货数。
        /// </summary>
        public int Exchange { get; set; }

    }

    public class StockLogModel
    {
        public string StockId { get; set; }

        public string ProductId { get; set; }

        public string SkuId { get; set; }

        public string Title { get; set; }

        public string SkuText { get; set; }

        public string ImgUrl { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示在库数量。
        /// </summary>
        public int InStock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示锁定数量（待付款商品）。
        /// </summary>
        public int Lock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示出库数。
        /// </summary>
        public int OutStock { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示退货数。
        /// </summary>
        public int Refund { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示销量。
        /// </summary>
        public int SellNum { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示换货数。
        /// </summary>
        public int Exchange { get; set; }

        public List<StockLogModel> SkuStockLogModel { get; set; } = new List<StockLogModel>();

        public List<StockLogItem> Logs { get;internal set; } = new List<StockLogItem>();
    }

    public class StockLogItem
    {
        /// <summary>
        /// 在库库存旧数值
        /// </summary>
        public int OldInStock { get; set; }

        /// <summary>
        /// 在库库存新数值
        /// </summary>
        public int NewInStock { get; set; }

        /// <summary>
        /// 在库库存变动数值
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 记录产生时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
