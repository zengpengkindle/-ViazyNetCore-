using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViazyNetCore.ShopMall.AppApi.ViewModels;

public class GoodsPageItem
{
    public string Title { get; set; }
    public string Type { get; set; }
    public string LookMore { get; set; }
    public string ClassifyId { get; set; }

    public string BrandId { get; set; }

    public string Display { get; set; }

    public int Limit { get; set; }

    public int Column { get; set; }

    public List<GoodsPageProductItem> List { get; set; }
}
public class GoodsPageProductItem
{
    public string Id { get; set; }

    /// <summary>
    /// 设置或获取一个值，表示品牌编号。
    /// </summary>
    public string BrandId { get; set; }

    /// <summary>
    /// 设置或获取一个值，表示品牌名称。
    /// </summary>
    public string BrandName { get; set; }

    /// <summary>
    /// 设置或获取一个值，表示类目编号。
    /// </summary>
    public string CatId { get; set; }

    /// <summary>
    /// 设置或获取一个值，表示类目名称。
    /// </summary>
    public string CatName { get; set; }

    /// <summary>
    /// 设置或获取一个值，表示标题。
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 设置或获取一个值，表示副标题。
    /// </summary>
    public string SubTitle { get; set; }

    /// <summary>
    /// 设置或获取一个值，表示关键词。
    /// </summary>
    public string Keywords { get; set; }

    /// <summary>
    /// 设置或获取一个值，表示描述。
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 设置或获取一个值，表示最低售价。
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 设置或获取一个值，表示是否开启属性。
    /// </summary>
    public bool OpenSpec { get; set; }

    /// <summary>
    /// 设置或获取一个值，表示主图。
    /// </summary>
    public string Image { get; set; }
}

public class NavBarPageItem
{
    public int Limit { get; set; }
    public List<NavBarItem> List { get; set; }
}

public class NavBarItem
{
    public string Image { get; set; }
    public string Text { get; set; }
    public string LinkType { get; set; }
    public string LinkValue { get; set; }
    public string Url { get; set; }
}