using HotelReservationSystem;

var hotel = new RoomManagement();
hotel.InitialRoomType(RoomType.Single, 1, 8, 30);
hotel.InitialRoomType(RoomType.Double, 2, 5, 50);
hotel.InitialRoomType(RoomType.Family, 4, 3, 85);

for (int i = 0; i < 20; i++)
{
    PrintSolution(i);
}


void PrintSolution(int numberOfGuests)
{
    Console.WriteLine("Input - " + numberOfGuests);
    var result = hotel.FindOptimalAvailiableRooms(numberOfGuests);
    Console.WriteLine(result == null ? "No option" : $"{string.Join(" ", result.GetValueOrDefault().rooms.Select(x => x.type.ToString()))} - {result.GetValueOrDefault().amount}");
    Console.WriteLine();
}
