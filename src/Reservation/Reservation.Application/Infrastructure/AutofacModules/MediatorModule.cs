using Autofac;
using MediatR;
using System.Reflection;
using Ticket.Application.Behaviors;

namespace Ticket.Application.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            //// Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
            //builder.RegisterAssemblyTypes(typeof(CreateContract).GetTypeInfo().Assembly)
            //    .AsClosedTypesOf(typeof(IRequestHandler<,>));


            //builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        }
    }
}






//using FluentValidation;
//using MediatR;
//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using System.Text;
//using Ticket.Application.Behaviors;
//using Ticket.Application.CommandHandlers;
//using Ticket.Application.Commands;
//using Ticket.Application.Validations;
//using Ticket.Domain.IRepositories;
//using Ticket.Infrastructure.Repositories;

//using Autofac;
//using Autofac.Extensions.DependencyInjection;
//using Autofac.Extras.DynamicProxy;
//using Castle.DynamicProxy;
//using System.Transactions;
//using System.Linq;
//using Framework.Attributes;

//namespace Ticket.Application.Infrastructure.AutofacModules
//{
//    public class MediatorModule : Autofac.Module
//    {

//        protected override void Load(ContainerBuilder builder)
//        {
//            builder.Register<ServiceFactory>(ctx =>
//            {
//                var c = ctx.Resolve<IComponentContext>();
//                return t => c.Resolve(t);
//            });


//            //builder.RegisterDecorator(typeof(TransactionalInterceptor), typeof(IRequestHandler<,>));
//            //// Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands


//            builder.RegisterAssemblyTypes(typeof(CreateAssessmentCommand).GetTypeInfo().Assembly)
//               .AsClosedTypesOf(typeof(IRequestHandler<,>));

//            //// Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
//            //builder.RegisterAssemblyTypes(typeof(ValidateOrAddBuyerAggregateWhenOrderStartedDomainEventHandler).GetTypeInfo().Assembly)
//            //    .AsClosedTypesOf(typeof(INotificationHandler<>));

//            // Register the Command's Validators (Validators based on FluentValidation library)


//            builder
//                .RegisterAssemblyTypes(typeof(CreateTicketCommandValidator).GetTypeInfo().Assembly)
//                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
//                .AsImplementedInterfaces();

//            builder
//                .RegisterAssemblyTypes(typeof(UpdateTicketCommandValidator).GetTypeInfo().Assembly)
//                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
//                .AsImplementedInterfaces();

//            builder
//                .RegisterAssemblyTypes(typeof(DeleteTicketCommandValidator).GetTypeInfo().Assembly)
//                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
//                .AsImplementedInterfaces();

//            builder
//                .RegisterAssemblyTypes(typeof(CreateAttachmentCommandValidator).GetTypeInfo().Assembly)
//                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
//                .AsImplementedInterfaces();

//            builder
//                .RegisterAssemblyTypes(typeof(DeleteAttachmentCommandValidator).GetTypeInfo().Assembly)
//                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
//                .AsImplementedInterfaces();




//            builder
//               .RegisterAssemblyTypes(typeof(CreateAssessmentCommandValidator).GetTypeInfo().Assembly)
//               .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
//               .AsImplementedInterfaces();


//            //builder.Register<SingleInstanceFactory>(context =>
//            //{
//            //    var componentContext = context.Resolve<IComponentContext>();
//            //    return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
//            //});

//            //builder.Register<MultiInstanceFactory>(context =>
//            //{
//            //    var componentContext = context.Resolve<IComponentContext>();

//            //    return t =>
//            //    {
//            //        var resolved = (IEnumerable<object>)componentContext.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
//            //        return resolved;
//            //    };
//            //});

//            //builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
//            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));



//            //builder.RegisterGeneric(typeof(TransactionalBehavior<,>)).As(typeof(IPipelineBehavior<,>));

//        }

//    }


//}
