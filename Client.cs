using System.Text.Json.Serialization;

public class Client {
    public int ClientId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    // Avoiding loop
    [JsonIgnore]
    public virtual ICollection<Booking> Bookings { get; set; }
}