using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserApp.Services
{
    public interface INotificationService
    {
        Task<RegistrationDescription> GetRegistrationDescriptionAsync(IEnumerable<string> tags);
    }
}
