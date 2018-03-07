using _2x2SoftApi.Area.Common.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2x2SoftApi.Area.Crm
{
    public class WorkTimeDetail : IEntity
    {
        public Int64 Id { get; set; }

        public Int64 IssueId { get; set; }

        public int LnNo { get; set; }

        public string Description { get; set; }

        public decimal Hours { get; set; }

        public WorkTimeDetail()
        {
            Id = 0;
            Hours = 0;
            LnNo = 0;
        }
    }
}
