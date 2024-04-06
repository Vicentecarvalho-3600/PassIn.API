using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Attendees.GetAllByIdEventId;
public class GetAllAttendessByEventIdUseCase
{
    private readonly PassInDbContext _dbContext;

    public GetAllAttendessByEventIdUseCase()
    {
        _dbContext = new PassInDbContext();
    }

    public ResponseAllAttendeesJson Execute(Guid eventId)
    {
        var entity = _dbContext.Events.Include(ev => ev.Attendees).ThenInclude(attendees => attendees.CheckIn).FirstOrDefault(ev => ev.Id == eventId);

        if (entity is null)
            throw new NotFoundException("An Event with this id dont exist.");

        return new ResponseAllAttendeesJson
        {
            Attendees = entity.Attendees.Select(attendee => new ResponseAttendeeJson
            {
                Id = attendee.Id,
                Name = attendee.Name,
                Email = attendee.Email,
                CreatedAt = attendee.Created_At,
                CheckedInAt = attendee.CheckIn?.Created_at
            }).ToList()
        };

    }
}
