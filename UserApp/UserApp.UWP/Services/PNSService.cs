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
    public class PNSService : IPNSService
    {
        public async Task<string> GetHandleAsync()
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            return channel.Uri;
        }
    }
}
