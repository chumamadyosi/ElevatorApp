using Domain.ElevatorEventService;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.DomainServives.ElevatorEvent
{
    public class ElevatorEventServiceTests
    {
        private readonly ElevatorEventService _elevatorEventService;

        public ElevatorEventServiceTests()
        {
            _elevatorEventService = new ElevatorEventService();
        }

        [Fact]
        public void Should_invoke_OnFloorChanged_event_when_RaiseFloorChangedEvent_is_called()
        {
            // Arrange
            int floor = 3;
            Direction direction = Direction.Up;
            bool eventInvoked = false;

            // Subscribe to the event
            _elevatorEventService.OnFloorChanged += (f, d) =>
            {
                // Assert that the event parameters match the expected values
                eventInvoked = (f == floor && d == direction);
            };

            // Act
            _elevatorEventService.RaiseFloorChangedEvent(floor, direction);

            // Assert
            Assert.True(eventInvoked, "The event was not invoked correctly.");
        }

        [Fact]
        public void Should_not_invoke_OnFloorChanged_event_when_no_subscribers()
        {
            // Arrange
            int floor = 3;
            Direction direction = Direction.Up;
            bool eventInvoked = false;

            // Act (no subscribers)
            _elevatorEventService.RaiseFloorChangedEvent(floor, direction);

            // Assert
            Assert.False(eventInvoked, "The event was incorrectly invoked.");
        }
    }
}
