using _2x2SoftApi.Area.Common.Store;
using A2v10.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2x2SoftApi.Area.Crm
{
    public class WorkTimeStore : Store<WorkTime>
    {
        public WorkTimeStore(IDbContext dbContext, string name, string schema) : base(dbContext, name, schema)
        {
        }

        public Task<IList<WorkTime>> WorkTimesGetBySupp(Int64 suppId)
        {
            return _dbContext.LoadListAsync<WorkTime>(null, _entitySchema + "." + _entityName + "GetBySupp", new { SuppId = suppId });
        }

        public Task<IList<WorkTimeDetail>> GetWorkTimeDetails(Int64 workTimeId)
        {
            return _dbContext.LoadListAsync<WorkTimeDetail>(null, _entitySchema + "." + _entityName + "GetWorkTimeDetails", new { Id = workTimeId });
        }
    }
}
