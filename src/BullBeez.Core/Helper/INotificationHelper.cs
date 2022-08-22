using BullBeez.Core.DTO;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BullBeez.Core.Helper
{
    public interface INotificationHelper
    {
        Task<string> SendNotification(NotificationModel requestModel);
    }
}
