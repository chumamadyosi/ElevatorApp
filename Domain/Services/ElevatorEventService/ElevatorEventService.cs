namespace Domain.ElevatorEventService
{
    public class ElevatorEventService : IElevatorEventService
    {
        // Event declaration
        public event Action<int, Direction> OnFloorChanged;

        // Method to raise the event
        public void RaiseFloorChangedEvent(int floor, Direction direction)
        {
            OnFloorChanged?.Invoke(floor, direction);  // Safely raise the event if there are any subscribers
        }
    }
}
