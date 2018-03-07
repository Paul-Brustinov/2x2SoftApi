using _2x2SoftApi.Area.Common.Store;
using A2v10.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2x2SoftApi.Area.Crm
{
    public class WorkTimeDetailStore : Store<WorkTimeDetail>
    {
        public WorkTimeDetailStore(IDbContext dbContext, string name, string schema) : base(dbContext, name, schema)
        {
        }

        public async Task WorkTimeDetailDeleteExcess(Int64 workTimeId, int no)
        {
            await _dbContext.LoadAsync<Person>(null, _entitySchema + "." + _entityName + "DeleteExcess", new { WorkTimeId = workTimeId, maxNo = no });
        }
    }
}
