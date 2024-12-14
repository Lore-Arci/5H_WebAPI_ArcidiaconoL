public class Booking {
    public int BookingId { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public float TotalAmount { get; set; }
    public Room Room { get; set; }

    public Client Client { get; set; }
}