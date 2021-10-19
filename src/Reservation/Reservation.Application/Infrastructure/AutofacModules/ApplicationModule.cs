using Autofac;
using Microsoft.AspNetCore.Http;
using Reservation.Application.Queries;
using Reservation.Application.Queries.Contract;
using Reservation.Domain.IRepositories;
using Reservation.Infrastructure.Context;
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
            builder.RegisterType<OfficeRepository>().As<IOfficeRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RoomReservationRepository>().As<IRoomReservationRepository>().InstancePerLifetimeScope(); 
            builder.RegisterType<ReservationSeedData>().As<IReservationSeedData>().InstancePerLifetimeScope();
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().InstancePerLifetimeScope();
            builder.RegisterType<RoomReservationQuery>().As<IRoomReservationQuery>().InstancePerLifetimeScope();
            builder.RegisterType<ResourceReservationQuery>().As<IResourceReservationQuery>().InstancePerLifetimeScope();
            builder.RegisterType<ReservationDbContextReadOnly>().InstancePerLifetimeScope();

        }
    }
}
