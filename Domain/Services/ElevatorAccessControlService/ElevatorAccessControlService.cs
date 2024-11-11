using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorAccessControlService
{
    public class ElevatorAccessControlService : IElevatorAccessControlService
    {
        private readonly HashSet<Guid> _authorizedUserIds = new HashSet<Guid>
    {
        Guid.Parse("e9a1b31a-5f4f-4df2-b916-2e5217a2f567"), // Example user GUID
        Guid.Parse("2a47955e-7556-4a7e-a6a3-54c9ed5d3e28")  // Another authorized user GUID
    };

        public async Task<(ErrorCode? errorCode, bool hasAccess)> AuthorizeUser(Guid userId)
        {
            await Task.Delay(1000);

            if (_authorizedUserIds.Contains(userId))
            {
                return (null, true); // User is authorized
            }
            else
            {
                return (ErrorCode.ElevatorAccessDenied, false); // User is not authorized
            }
        }
    }
}
