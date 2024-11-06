namespace Domain
{
    public class Elevator
    {
        public int Id { get; set; }
        public int CurrentFloor { get; set; } = 1;
        public Direction Direction { get; set; } = Direction.Stationary;
        public int MaxFloor { get; set; }
        public int PassengerCount { get; set; } = 0;
        public int MaxPassengerCount { get; set; }


        public ElevatorType Type { get; set; }  // New property to distinguish elevator types
        public int SpeedInMillisecondsPerFloor { get; set; }  // Speed of the elevator
        public int MaxWeightCapacity { get; set; }
        public int CurrentWeight { get; set; }
    }
}
