namespace PassIn.Infrastructure.Entities;
public class Event
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Maximun_Attendees { get; set; } = string.Empty;
 
}
