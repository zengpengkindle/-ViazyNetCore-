using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ViazyNetCore.Modules.ShopMall.Repositories;

namespace ViazyNetCore.Modules.ShopMall
{
    [Injection]
    public class ProductCatService
    {
        private readonly IProductCatRepository _productCatRepository;
        private readonly IMapper _mapper;

        public ProductCatService(IProductCatRepository productCatRepository, IMapper mapper)
        {
            this._productCatRepository = productCatRepository;
            this._mapper = mapper;
        }

        public async Task<List<ProductCatDto>> GetProductCats(ComStatus? status = ComStatus.Enabled)
        {
            var list = await this._productCatRepository.WhereIf(status.HasValue, t => t.Status == status).ToListAsync();
            return this._mapper.Map<List<ProductCat>, List<ProductCatDto>>(list);
        }

        public async Task<PageData<ProductCatDto>> FindAll(Pagination args)
        {
            var (total, list) = await this._productCatRepository.FindAllAsync(args);
            return new PageData<ProductCatDto>()
            {
                Total = total,
                Rows = this._mapper.Map<List<ProductCat>, List<ProductCatDto>>(list)
            };
        }

        public async Task AddCat(ProductCatAddDto addDto)
        {
            var enity = this._mapper.Map<ProductCatAddDto, ProductCat>(addDto);
            enity.CreateTime = DateTime.Now;
            enity.Id = Snowflake<ProductCat>.NextIdString();
            await this._productCatRepository.InsertAsync(enity);
        }

        public async Task UpdateCat(ProductCatUpdateDto updateDto)
        {
            var enity = this._mapper.Map<ProductCatUpdateDto, ProductCat>(updateDto);
            await this._productCatRepository.UpdateAsync(enity);
        }

        public async Task<ProductCatDto> Get(string id)
        {
            var enity = await this._productCatRepository.GetAsync(id);
            return this._mapper.Map<ProductCat, ProductCatDto>(enity);
        }
    }
}
