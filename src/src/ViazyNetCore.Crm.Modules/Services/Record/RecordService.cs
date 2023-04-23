using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViazyNetCore.Model.Crm;
using ViazyNetCore.Modules.Repository;

namespace ViazyNetCore.Modules
{
    public class RecordService
    {
        private readonly IActionRecordRepository _actionRecordRepository;

        public RecordService(IActionRecordRepository actionRecordRepository)
        {
            this._actionRecordRepository = actionRecordRepository;
        }

        public Task UpdateRecord(Dictionary<string, object> array, string batchId)
        {
            throw new NotImplementedException();
        }

        public async Task AddRecord(long actionId, CrmType crmType)
        {
            var record = new CrmActionRecord
            {
                ActionId = actionId,
                Type = crmType,
                Content = $"新建了{crmType.GetDescription()}"
            };
            await this._actionRecordRepository.InsertAsync(record);
        }

        private void SearchChange(List<string> remarks, List<KeyValuePair<string, object>> oldEnities, List<KeyValuePair<string, object>> newEnities, CrmType crmType)
        {
            oldEnities.ForEach(x =>
            {
                var oldValue = x.Value?.ToString();
                if (oldValue == null || oldValue.IsNull())
                {
                    oldValue = "空";
                }

                newEnities.ForEach(y =>
                {
                    var newValue = y.Value?.ToString();
                    if (newValue == null || newValue.IsNull())
                    {
                        newValue = "空";
                    }
                    if (x.Key == y.Key && oldValue != newValue)
                    {
                        if (x.Key != "updateTime" && x.Key != "createUserId")
                        {
                            remarks.Add("将" + crmType.GetDescription() + " 由" + oldValue + "修改为" + newValue + "。");
                        }
                    }
                });
            });
        }
    }
}
