using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ticket.Application.Infrastructure.AutofacModules
{
    public class ApplicationModule: Module
    {

        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;
        }

        protected override void Load(ContainerBuilder builder)
        {

        }
    }
}
