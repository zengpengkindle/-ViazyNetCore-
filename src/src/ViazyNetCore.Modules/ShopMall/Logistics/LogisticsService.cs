using Microsoft.Extensions.Logging;

namespace ViazyNetCore.Modules.ShopMall
{
    public class LogisticsService
    {
        private readonly IFreeSql _engine;
        private readonly ILogger<LogisticsService> _logger;
        private readonly StockService _stockService;

        public LogisticsService(IFreeSql engine, ILogger<LogisticsService> logger, StockService stockService)
        {
            this._engine = engine;
            this._logger = logger;
            this._stockService = stockService;
        }

        public async Task Delivery(string tradeId, DeliveryModel deliveryModel)
        {
            var logistics = new ProductLogistics
            {
                Id = Snowflake<ProductLogistics>.NextIdString(),
                TradeId = tradeId,
                ExpressNo = deliveryModel.LogisticsCode,
                ReceiverProvince = deliveryModel.Address.Province,
                ReceiverCity = deliveryModel.Address.City,
                ReceiverDistrict = deliveryModel.Address.County,
                ReceiverDetail = deliveryModel.Address.AddressDetail,
                ReceiverName = deliveryModel.Address.Name,
                ReceiverMobile = deliveryModel.Address.Tel,
                LogisticsId = deliveryModel.LogisticsId,
                LogisticsCompany = deliveryModel.LogisticsCompany,
                LogisticsFee = deliveryModel.LogisticsFee,
                LogisticsCreateTime = DateTime.Now,
                LogisticsUpdateTime = DateTime.Now,
                LogisticsStatus = 0,
                DeliveryAmount = deliveryModel.LogisticsFee

            };
            await this._engine.Insert<ProductLogistics>().AppendData(logistics).ExecuteAffrowsAsync();
        }
    }
}
