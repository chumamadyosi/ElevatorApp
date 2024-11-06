using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorPassengerService
{
    public interface IElevatorOccupantService
    {
        Task<ErrorCode?> LoadOccupantsAsync(Elevator elevator, int count, OccupantType occupantType); //count can be passagers or weight this means we round off weight(up for discussion and decimals might not matter that much)
        bool AddOccupants(Elevator elevator, int count, OccupantType occupantType);
    }
}
