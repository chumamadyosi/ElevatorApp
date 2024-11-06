using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ElevatorAccessControlService
{
    public interface IElevatorAccessControlService
    {
        Task< (ErrorCode? erroCode, bool hasAccess)> AuthorizeUser(Guid userId);
    }
}
