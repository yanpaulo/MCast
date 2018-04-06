using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCast.WebAPI.Services
{
    public class NotificationService
    {
        private static NotificationHubClient hub;

        public static NotificationHubClient Hub => hub ?? (hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://yanscorp.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=MVZlt7280W+ncv+ad1QoA8Y5GG5bxQpB/8zTPe9YnBo=", "MCastNotificationHub"));
    }
}
