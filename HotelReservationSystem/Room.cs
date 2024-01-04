namespace HotelReservationSystem
{
    public enum RoomType
    {
        Single,
        Double,
        Family
    }
    public class Room
    {
        public RoomType Type { get; set; }
        public int Capacity { get; set; }
        public double Price { get; set; }
    }
}
