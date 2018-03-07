using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2x2SoftApi.Mvc.Models.IdentityModels
{
    public class ApplicationRole : IRole<Int64>
    {
        #region IRole<Int64>
        public Int64 Id { get; set; }
        public string Name { get; set; }
        #endregion
    }
}