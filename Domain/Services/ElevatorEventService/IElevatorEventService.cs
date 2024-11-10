using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorEventService
{
    public interface IElevatorEventService
    {
        event Action<int, Direction> OnFloorChanged;
        void RaiseFloorChangedEvent(int floor, Direction direction);
    }
}
