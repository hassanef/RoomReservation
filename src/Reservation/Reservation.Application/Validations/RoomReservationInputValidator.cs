using FluentValidation;
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
    public class RoomReservationInputValidator : AbstractValidator<RoomReservationCommand>
    {
        public RoomReservationInputValidator(IRoomReservationRepository roomReservationRepository,
                                             IRoomRepository roomRepository)
        {
             RuleFor(model => model.RoomId)
                .NotEmpty()
                .MustAsync(async (roomId, cancellation) =>
                {
                    var count = await roomRepository.CountAsync(x => x.Id == roomId);

                    return count == 1;
                }).WithMessage("room not found!");

            RuleFor(model => model)
              .NotEmpty()
              .MustAsync(async (model, cancellation) =>
              {
                  var count = await roomReservationRepository.CountAsync(x => x.Id == model.RoomId &&
                                                                        (model.StartDate < x.Period.Start && model.EndDate > x.Period.Start) ||
                                                                        (model.StartDate > x.Period.Start && model.EndDate < x.Period.End) ||
                                                                        (model.StartDate > x.Period.Start && model.StartDate < x.Period.End) ||
                                                                        (model.EndDate > x.Period.Start && model.EndDate < x.Period.End));

                  return count == 0;
              }).WithMessage("room is busy in selected date time!");

            RuleFor(model => model.StartDate)
               .Must(startDate =>
               {
                   if (startDate <= DateTime.Now)
                       return false;
                   if (startDate.TimeOfDay < new TimeSpan(08, 0, 0))
                       return false;
                   return true;
               }).WithMessage("StartDate should be greather than current time and greather than 08:00AM!")
                .DependentRules(() =>
                {
                    RuleFor(model => model)
                      .Must(model =>
                      {
                          if (model.StartDate >= model.EndDate)
                              return false;
                          return true;
                      }).WithMessage("StartDate can not be greather than EndDate or equal with EndDate!");
                });

            RuleFor(model => model.EndDate)
              .Must(endDate =>
              {
                  if (endDate <= DateTime.Now)
                      return false;
                  return true;
              }).WithMessage("EndDate should be greather than current time!")
              .DependentRules(() =>
              {
                  RuleFor(x => x)
                    .Must(x =>
                    {
                        if (x.Location == Location.Amsterdam && x.EndDate.TimeOfDay > new TimeSpan(17, 0, 0))
                            return false;
                        return true;
                    }).WithMessage("EndDate can not be greather than 17:00PM in Amsterdam!");
              })
              .DependentRules(() =>
              {
                  RuleFor(model => model)
                    .Must(model =>
                    {
                        if (model.Location == Location.Berlin && model.EndDate.TimeOfDay > new TimeSpan(20, 0, 0))
                            return false;
                        return true;
                    }).WithMessage("EndDate can not be greather than 17:00PM in Berlin!");
              });

        }
    }
}