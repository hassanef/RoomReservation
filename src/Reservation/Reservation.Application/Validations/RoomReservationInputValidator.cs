using FluentValidation;
using Reservation.Application.Commands;
using Reservation.Domain.IRepositories;
using Reservation.Domain.Utils;
using System;

namespace Reservation.Application.Validations
{
    public class RoomReservationInputValidator : AbstractValidator<CreateRoomReservationCommand>
    {
        public RoomReservationInputValidator(IRoomReservationRepository roomReservationRepository,
                                             IOfficeRepository roomRepository,
                                             ILocationRepository locationRepository,
                                             IClock clock)
        {
            RuleFor(model => model.RoomId)
               .NotEmpty()
               .MustAsync(async (roomId, cancellation) =>
               {
                   var count = await roomRepository.CountAsync(x => x.Id == roomId);

                   return count == 1;
               }).WithMessage("room not found!")
                .DependentRules(() =>
                {
                    RuleFor(model => model)
                        .NotEmpty()
                        .MustAsync(async (model, cancellation) =>
                        {
                            var count = await roomReservationRepository.CountAsync(x => x.RoomId == model.RoomId &&
                                                                                  (model.StartDate <= x.Period.Start && model.EndDate >= x.Period.Start) ||
                                                                                  (model.StartDate >= x.Period.Start && model.EndDate <= x.Period.End) ||
                                                                                  (model.StartDate >= x.Period.Start && model.StartDate <= x.Period.End) ||
                                                                                  (model.EndDate >= x.Period.Start && model.EndDate <= x.Period.End));

                            return count == 0;
                        }).WithMessage("room is busy in selected date time!");

                });

            RuleFor(model => model.StartDate)
               .Must(startDate =>
               {
                   if (startDate <= clock.Now())
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
                  if (endDate <= clock.Now())
                      return false;
                  return true;
              }).WithMessage("EndDate should be greather than current time!")
                .DependentRules(() =>
                {
                    RuleFor(model => model)
                      .MustAsync(async (model, cancellation) =>
                      {
                          var location = await locationRepository.SingleOrDefaultAsync(l => l.Id == model.LocationId);
                          if (location == null)
                              return false;
                          return true;
                      }).WithMessage("Location not exist!")
                        .DependentRules(() =>
                        {
                            RuleFor(model => model)
                              .MustAsync(async (model, cancellation) =>
                              {
                                  var location = await locationRepository.SingleOrDefaultAsync(l => l.Id == model.LocationId);
                                  if (model.EndDate.TimeOfDay > location.End)
                                      return false;
                                  return true;
                              }).WithMessage("EndDate can not be greather than valid time!");
                        });
                });


        }
    }
}