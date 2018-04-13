using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.Services;
using UserApp.UWP.Services;
using Windows.Networking.PushNotifications;

[assembly: Xamarin.Forms.Dependency(typeof(PNSService))]
namespace UserApp.UWP.Services
{
    public class PNSService : INotificationService
    {
        public async Task<ClientData> GetHandleAsync(string group)
        {
            return new ClientData
            {
                Handle = (await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync()).Uri,
                Group = group
            };
        }
    }
}
