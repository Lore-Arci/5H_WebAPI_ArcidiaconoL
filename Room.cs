using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Room {
    public int RoomId { get; set; }
    public int RoomNumber { get; set; }
    /*
        Using a converter so that the enum will not only stored as string in the db 
        but it will also be stored as string during serialization (c# -> json). 
        Because, for default, json read enum as integers
    */
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public RoomType RoomType { get; set; }
    public float NightPrice { get; set; }
    public bool IsAvailable { get; set; }
    // Avoiding loop
    [JsonIgnore]
    public virtual ICollection<Booking> Bookings { get; set; }
}