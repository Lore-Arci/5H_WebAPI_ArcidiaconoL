using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Room {
    public int RoomId { get; set; }
    public int RoomNumber { get; set; }
    public string RoomType { get; set; }
    public float NightPrice { get; set; }
    public bool IsAvailable { get; set; }
    // Avoiding loop
    [JsonIgnore]
    public virtual ICollection<Booking> Bookings { get; set; }
}