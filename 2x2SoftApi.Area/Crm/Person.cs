using _2x2SoftApi.Area.Common.Store;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2x2SoftApi.Area.Crm
{
    public class Person : IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public int Type { get; set; }

        [JsonIgnore]
        public List<Phone> Phones { get; set; }

        [JsonIgnore]
        public List<Email> Emails { get; set; }
        //public List<BusinessAddress> Addresses { get; set; }

        public string Memo { get; set; }

        public Person()
        {
            Phones = new List<Phone>();
            Emails = new List<Email>();
            //Addresses = new List<BusinessAddress>();
        }

    }
}
