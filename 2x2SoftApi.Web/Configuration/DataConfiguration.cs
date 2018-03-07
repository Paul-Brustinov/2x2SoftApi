using A2v10.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace _2x2SoftApi.Mvc.Configuration
{
    public class DataConfiguration : IDataConfiguration
    {
        IDictionary<String, String> _cnnStrings = new Dictionary<String, String>();

        public string ConnectionString(string source)
        {
            if (String.IsNullOrEmpty(source))
                source = "Default";

            String cnnStr = null;
            if (_cnnStrings.TryGetValue(source, out cnnStr))
                return cnnStr;
            var strSet = ConfigurationManager.ConnectionStrings[source];
            if (strSet == null)
                throw new ConfigurationErrorsException($"Connection string '{source}' not found");
            cnnStr = strSet.ConnectionString;
            _cnnStrings.Add(source, strSet.ConnectionString);
            return cnnStr;
        }
    }
}