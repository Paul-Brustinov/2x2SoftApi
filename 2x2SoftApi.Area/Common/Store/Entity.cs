using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2x2SoftApi.Area.Common.Store
{
    public class Entity : IEntity
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
    }
}
