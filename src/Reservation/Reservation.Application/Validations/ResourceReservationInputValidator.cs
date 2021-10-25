using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Reservation.Application.Commands;
using Reservation.Domain.AggregatesModel;
using Reservation.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Application.Validations
{
    public class ResourceReservationInputValidator : AbstractValidator<ResourceReservationCommand>
    {
        public ResourceReservationInputValidator(IRoomReservationRepository roomReservationRepository,
                                                 IResourceRepository resourceRepository)
        {

            RuleFor(model => model.ResourceId)
                 .NotEmpty()
                 .MustAsync(async (resourceId, cancellation) =>
                 {
                     var count = await resourceRepository.CountAsync(x => x.Id == resourceId && x.Type == ResourceType.Movable);

                     return count == 1;
                 }).WithMessage("resource not found or not movable!");

            RuleFor(model => model.RoomReservationId)
                .NotEmpty()
                .MustAsync(async (roomReservationId, cancellation) =>
                {
                    var count = await roomReservationRepository.CountAsync(x => x.Id == roomReservationId);

                    return count == 1;
                }).WithMessage("roomReservation not found!")
             .DependentRules(() =>
              {
                  RuleFor(model => model)
                   .NotEmpty()
                   .MustAsync(async (model, cancellation) =>
                   {
                       var result = await roomReservationRepository.AnyAsync(x => x.ResourceReservations.Any(r => r.RoomReservationId == model.RoomReservationId && r.ResourceId == model.ResourceId));

                       return !result;
                   }).WithMessage("roomReservation has this resource!");
              });
        }
    }
}