using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee;
public class RegisterAttendeeOnEventUseCase
{
    private readonly PassInDbContext _dbcontext;
    public RegisterAttendeeOnEventUseCase()
    {
        _dbcontext = new PassInDbContext();
    }
    public ResponseResgisteredJson Execute(Guid eventId, RequestRegisterEventJson request)
    {
        Validate(eventId, request);

        var entity = new Infrastructure.Entities.Attendee
        {
            Email = request.Email,
            Name = request.Name,
            Event_Id = eventId,
            Created_At = DateTime.UtcNow,
        };

        _dbcontext.Attendees.Add(entity);
        _dbcontext.SaveChanges();

        return new ResponseResgisteredJson
        {
            Id = entity.Id,
        };
    }

    private void Validate(Guid eventId, RequestRegisterEventJson request)
    {
        var eventEntity = _dbcontext.Events.Find(eventId);

        if (eventEntity is null)
            throw new NotFoundException("An Event with this id dont exist.");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ErrorOnValidateException("The Name is invalid.");

        var emailIsVAlid = EmailIsVAlid(request.Email);
        if (emailIsVAlid == false)
        {
            throw new ErrorOnValidateException("The e-mail is invalid.");
        }

        var attendeeAlreadyRegistered = _dbcontext
            .Attendees
            .Any(attedee => attedee.Email.Equals(request.Email) && attedee.Event_Id == eventId);

        if (attendeeAlreadyRegistered)
        {
            throw new ConflictException("You can not resgister twice on the same event.");
        }
        
        var attendeesForEvent = _dbcontext.Attendees.Count(attendee => attendee.Event_Id == eventId);

        if(attendeesForEvent == eventEntity.Maximum_Attendees)
        {
            throw new ErrorOnValidateException("There is no room for this event.");
        }
    }

    private bool EmailIsVAlid(string email)
    {
        try
        {
            new MailAddress(email);

            return true;
        }
        catch 
        {
            return false;
        }

    }
}
