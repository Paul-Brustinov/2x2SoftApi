using A2v10.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2x2SoftApi.Mvc.Configuration
{
    public class DataLocalizer : IDataLocalizer
    {
        public string Localize(string content)
        {
            return "En";
        }
    }
}