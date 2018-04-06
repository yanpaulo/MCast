using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using UserApp.Services;
using UserApp.UWP.Services;
using Windows.Networking.PushNotifications;

[assembly: Xamarin.Forms.Dependency(typeof(PNSService))]
namespace UserApp.UWP.Services
{
    public class PNSService : INotificationService
    {
        public async Task<RegistrationDescription> GetRegistrationDescriptionAsync(IEnumerable<string> tags)
        {
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            var payload = @"<toast><visual><binding template=""ToastText01""><text id=""1"">$(message)</text></binding></visual></toast>";
            return new WindowsTemplateRegistrationDescription(channel.Uri, payload, tags);
        }
    }
}
