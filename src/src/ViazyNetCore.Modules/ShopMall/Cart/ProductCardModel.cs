using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.Modules.ShopMall
{
    //carts: [
    //                {
    //                    shopId: '0123', shopName: '兴宇家居', check: false,
    //                    items: [
    //                        {
    //                            check: false, pId: 1, imgUrl: 'https://mp.tea-l.com/upload/0/OUFF24WA4DE7/image/20190111/6368282766718735545765206.jpg',
    //                            pn: '飘窗阳台垫现代简约北欧坐垫榻榻米坐垫落地窗', price: 15.8,
    //                            sku: { name: '颜色:红色', ks: "123" }, num: 1
    //                        },
    //                        {
    //                            check: false, pId: 2, imgUrl: 'https://mp.tea-l.com/upload/0/OUFF24WA4DE7/image/20190111/6368282766718735545765206.jpg',
    //                            pn: '北欧简约日式全实木餐桌，白橡木伸缩饭桌小户型', price: 2422.8, num: 1
    //                        }]
    //                },
    //                {
    //                    shopId: '223', shopName: 'PONY官方旗舰店', check: false,
    //                    items: [
    //                        {
    //                            check: false, pId: 1, imgUrl: 'https://mp.tea-l.com/upload/0/OUFF24WA4DE7/image/20190111/6368282766718735545765206.jpg',
    //                            pn: 'PONY时尚运动鞋C123-123 黑白', price: 15.8,
    //                            sku: { name: '颜色:黑色 尺码:23', ks: "123" }, num: 1
    //     }]
    // }]
    /// <summary>
    /// 购物车
    /// </summary>
    public class ShoppingCart
    {
        /// <summary>
        /// 内含商品种类数目 上限100
        /// </summary>
        public int Num { get; set; } = 0;

        public string PropertyKeys { get; set; }

        public string PropertyValues { get; set; }

        public List<ShoppingCartPackage> Packages { get; set; } = new List<ShoppingCartPackage>();
    }

    /// <summary>
    /// 将同一个商店商品打包一块
    /// </summary>
    public class ShoppingCartPackage
    {
        public string ShopId { get; set; }

        public string ShopName { get; set; }

        public bool Check { get; set; } = false;

        /// <summary>
        /// 包含的商品
        /// </summary>
        public List<ShoppingCartProduct> Items { get; set; } = new List<ShoppingCartProduct>();
    }

    public class ShoppingCartProduct
    {
        /// <summary>
        /// 设置或获取一个值，表示主键。
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品Id。
        /// </summary>
        public string PId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示SkuId。
        /// </summary>
        public string SkuId { get; set; }

        /// <summary>
        /// 商品所属商店
        /// </summary>
        public string ShopId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品名称。
        /// </summary>
        public string Pn { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示规格描述。
        /// </summary>
        public string? SkuText { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示图片。
        /// </summary>
        public string? ImgUrl { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示商品售价。
        /// </summary>
        public decimal Price { get; set; }

        public bool Check { get; set; } = false;

        /// <summary>
        /// 设置或获取一个值，表示购买数。
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示购物车商品是否有效。
        /// </summary>
        public ComStatus Status { get; set; }
    }

    public class ShoppingCartEditDto
    {
        /// <summary>
        /// 设置或获取一个值，表示商品Id。
        /// </summary>
        public string PId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示SkuId。
        /// </summary>
        public string? SkuId { get; set; }

        /// <summary>
        /// 设置或获取一个值，表示购买数。
        /// </summary>
        public int Num { get; set; }
    }
}
