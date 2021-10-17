using Autofac;
using Microsoft.AspNetCore.Http;
using Reservation.Domain.IRepositories;
using Reservation.Infrastructure.Repositories;
using Reservation.Infrastructure.SeedData;
using Reservation.Infrastructure.SeedData.Contract;

namespace Ticket.Application.Infrastructure.AutofacModules
{
    public class ApplicationModule: Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ResourceRepository>().As<IResourceRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RoomRepository>().As<IRoomRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RoomReservationRepository>().As<IRoomReservationRepository>().InstancePerLifetimeScope(); 
            builder.RegisterType<ReservationSeedData>().As<IReservationSeedData>().InstancePerLifetimeScope();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();

        }
    }
}
