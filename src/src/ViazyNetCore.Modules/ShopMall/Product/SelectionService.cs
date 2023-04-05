using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class SelectionService
    {
        private readonly IFreeSql _engine;
        private readonly IMapper _mapper;

        public SelectionService(IFreeSql freeSql, IMapper mapper)
        {
            this._engine = freeSql;
            this._mapper = mapper;
        }

        public async Task<(long total, List<SelectionFeedListDto>)> FindProductAll(FindAllArguments args)
        {
            //if (args.ShopId.IsNull())
            //    throw new ApiException("商店编号不能为空");
            var table = this._engine.Select<Product>().Where(t => t.Status != ProductStatus.Delete);

            //if (args.ShopId != "111111")//假定官方自营店Id为"111111".官方可查所有店铺商品
            //    table = table.Where(s => s.ShopId == args.ShopId);

            if (args.TitleLike.IsNotNull())
                table = table.Where(t => t.Title.Contains(args.TitleLike));
            if (args.CatId.IsNotNull())
                table = table.Where(t => t.CatId == args.CatId);

            if (args.CatName.IsNotNull())
                table = table.Where(t => t.CatName == args.CatName);
            if (args.IsHidden.HasValue)
                table = table.Where(t => t.IsHidden == args.IsHidden);
            if (args.Status.HasValue)
                table = table.Where(t => t.Status == args.Status);


            if (args.CreateTimes?.Length == 2) table = table.Where(t => t.CreateTime >= args.CreateTimes[0] && t.CreateTime < args.CreateTimes[1].ToEndTime());

            var query = from t in table
                        orderby t.CreateTime descending
                        select t;
            var value = await query.ToPageAsync(args);

            return (value.Total, this._mapper.Map<List<Product>, List<SelectionFeedListDto>>(value.Rows));
        }
    }
}
