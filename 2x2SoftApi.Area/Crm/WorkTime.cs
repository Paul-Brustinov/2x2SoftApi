using _2x2SoftApi.Area.Common.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2x2SoftApi.Area.Crm
{
    public class WorkTime : IEntity
    {
        public Int64 Id { get; set; }
        public DateTime OpDate { get; set; }
        public Int64 ClientId { get; set; }
        public Int64 SuppId { get; set; }
        public IList<WorkTimeDetail> WorkTimeDetails { get; set; }

        public WorkTime()
        {
            Id = 0;
            ClientId = 0;
            WorkTimeDetails = new List<WorkTimeDetail> { new WorkTimeDetail() };
            OpDate = DateTime.Today;
        }

        public Person Client { get; set; }

        public Person Supporter { get; set; }

        public string ClientName => Client == null ? "not selected" : Client.Name;
        public string SupporterName => Supporter == null ? "not selected" : Supporter.Name;

        public string Date => OpDate.ToString("dd.MM.yyyy");

        public string Name { get; set; }
    }
}
