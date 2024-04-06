using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;


namespace PassIn.Application.UseCases.CheckIn.DoCheckIn;
public class DoAttendeeCheckInUseCase
{
    private readonly PassInDbContext _dbContext;

    public DoAttendeeCheckInUseCase()
    {
        _dbContext = new PassInDbContext();
    }

    public ResponseResgisteredJson Execute(Guid attendeeId)
    {
        Validate(attendeeId);

        var entity = new Infrastructure.Entities.CheckIn
        {
            Attendee_Id = attendeeId,
            Created_at = DateTime.UtcNow,
        };

        _dbContext.CheckIns.Add(entity);
        _dbContext.SaveChanges();
        

        return new ResponseResgisteredJson 
        {
            Id = entity.Id,
        };
    }

    private void Validate(Guid attendeeId)
    {
        var existAttendee =_dbContext.Attendees.Any(attendee => attendee.Id == attendeeId);
        if(existAttendee == false) 
        {
            throw new NotFoundException("The attendee with this Id Is not found");
        }

        var existCheckIn = _dbContext.CheckIns.Any(ch => ch.Attendee_Id == attendeeId);
        if(existCheckIn == true) 
        {
            throw new ConflictException("Attendee can not do checking twice in the same event.");
        }

    }
}
