using _2x2SoftApi.Infrastructure;
using _2x2SoftApi.Mvc.Configuration;
using A2v10.Data;
using A2v10.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _2x2SoftApi.Mvc
{
    public partial class Startup
    {
        public void StartServices()
        {
            // DI ready
            IServiceLocator locator = ServiceLocator.Current;

            IDataProfiler profiler = new DataProfiler();
            IDataConfiguration dataConfiguration = new DataConfiguration();
            IDataLocalizer dataLocalizer = new DataLocalizer();
            IApplicationHost host = new WebApplicationHost(profiler);
            IDbContext dbContext = new SqlDbContext(profiler, dataConfiguration, dataLocalizer);
            //IRenderer renderer = new XamlRenderer();
            //IWorkflowEngine workflowEngine = new WorkflowEngine(host, dbContext);


            locator.RegisterService<IDataProfiler>(profiler);
            locator.RegisterService<IDataConfiguration>(dataConfiguration);
            locator.RegisterService<IDataLocalizer>(dataLocalizer);
            locator.RegisterService<IApplicationHost>(host);
            locator.RegisterService<IDbContext>(dbContext);

            //locator.RegisterService<IRenderer>(renderer);
            //locator.RegisterService<IWorkflowEngine>(workflowEngine);

        }
    }
}