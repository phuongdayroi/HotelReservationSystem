using System.Linq;

namespace HotelReservationSystem
{
    public class RoomManagement
    {
        public Dictionary<RoomType, Room> RoomsDict = [];
        public Dictionary<RoomType, int> AvailableRoomDict = [];
        public RoomManagement()
        {

        }


        public void InitialRoomType(RoomType type, int capacity, int numberOfRooms, double price)
        {
            RoomsDict[type] = new Room { Type = type, Capacity = capacity, Price = price };
            AvailableRoomDict[type] = numberOfRooms;
        }

        public int GetCapacity(RoomType type)
        {
            return RoomsDict[type]?.Capacity ?? 0;
        }

        public void FindAvailableOptions(
            int numberOfGuests,
            ref Dictionary<List<(RoomType type, int numberOfRooms)>, (double amount, int capacity)> markOptionDicts,
            List<(RoomType traveledRoom, int numberOfRooms)> traveledRooms, List<RoomType> remainingRoomTypes)
        {
            remainingRoomTypes ??= [];
            var capacity = numberOfGuests - traveledRooms.Sum(x => x.numberOfRooms);
            if (capacity <= 0) return;

            foreach (var type in remainingRoomTypes)
            {
                var newRemainingRoomTypes = remainingRoomTypes?.Except([type]).ToList() ?? [];
                for (var i = 1; i <= capacity; i++)
                {
                    var newTaveledRooms = new List<(RoomType type, int numberOfRoom)>(traveledRooms)
                        {
                            (type, i)
                        };

                    if (newRemainingRoomTypes?.Count > 0)
                    {

                        FindAvailableOptions(numberOfGuests, ref markOptionDicts, newTaveledRooms, newRemainingRoomTypes);
                    }

                    var caculatedOption = CalculateOption(newTaveledRooms);
                    if (!markOptionDicts.ContainsKey(newTaveledRooms) && caculatedOption.capacity >= numberOfGuests)
                    {
                        markOptionDicts.Add(newTaveledRooms, caculatedOption);
                    }
                }
            }
        }


        public (double amount, int capacity) CalculateOption(List<(RoomType type, int capacity)> roomCounts)
        {
            var capacity = 0;
            var amount = 0.0;
            foreach (var (type, numberOfRooms) in roomCounts)
            {
                var room = RoomsDict[type];
                capacity += room.Capacity * numberOfRooms;
                amount += room.Price * numberOfRooms;
            }

            return (amount, capacity);
        }

        public (List<(RoomType type, int numberOfRooms)> rooms, double amount, int capacity)? FindOptimalAvailiableRooms(int numberOfGuests)
        {
            var totalCapacity = TotalOfAvaliableSlots();
            if (numberOfGuests <= 0 || numberOfGuests > totalCapacity) return null;
            var traveledRooms = new List<(RoomType type, int numberOfRooms)>();
            var remainingRoomTypes = AvailableRoomDict.Where(x => x.Value > 0).Select(x => x.Key).ToList();
            var markOptionDicts = new Dictionary<List<(RoomType type, int numberOfRooms)>, (double amount, int capacity)>();
            FindAvailableOptions(numberOfGuests, ref markOptionDicts, traveledRooms, remainingRoomTypes);
            var options = markOptionDicts.OrderBy(x => x.Value.amount).ToList();
            var lowestPrice = options.First().Value.amount;

            var bestOption = options.Where(x => x.Value.amount == lowestPrice).OrderBy(x => x.Value.capacity).FirstOrDefault();
            return (bestOption.Key, bestOption.Value.amount, bestOption.Value.capacity);
        }

        private int TotalOfAvaliableSlots()
        {
            return AvailableRoomDict.Sum(x => x.Value);
        }

    }
}
