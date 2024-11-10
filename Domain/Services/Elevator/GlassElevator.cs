using Domain.ElevatorAccessControlService;
using Domain.ElevatorDispatch;
using Domain.ElevatorMovementService;
using Domain.ElevatorPassengerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class GlassElevator : IElevator
    {

        private readonly IElevatorOccupantService _occupantService;

        public GlassElevator(IElevatorOccupantService occupantService)
        {
            _occupantService = occupantService;

        }

        public async Task<ErrorCode?> LoadOccupants(Elevator elevator, int count)
        {
            return await _occupantService.LoadOccupants(elevator, count);
        }

        public async Task<bool> AddOccupants(Elevator elevator, int count)
        {
            return await _occupantService.AddOccupants(elevator, count);
        }
    }

}
