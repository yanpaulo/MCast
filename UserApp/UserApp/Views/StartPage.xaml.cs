using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UserApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }
        

        private async void Button_Clicked(object sender, EventArgs args)
        {
            var hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://yanscorp.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=Nt/JE/8Zc/F1ZwQ1mO/ZcM/NUg0CRs3DjI31IK7hTNM=", "MCastNotificationHub");
            var pns = await DependencyService.Get<IPNSService>().GetHandleAsync();
            var payload = @"<toast><visual><binding template=""ToastText01""><text id=""1"">$(message)</text></binding></visual></toast>";
            var registration = new WindowsTemplateRegistrationDescription(pns, payload, new[] { $"group:{IdEntry.Text}" });
            registration.RegistrationId = await hub.CreateRegistrationIdAsync();
            //registration.id
            var description = await hub.CreateOrUpdateRegistrationAsync(registration);
        }

        private async void QR_Clicked(object sender, EventArgs args)
        {
            var hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://yanscorp.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=Nt/JE/8Zc/F1ZwQ1mO/ZcM/NUg0CRs3DjI31IK7hTNM=", "MCastNotificationHub");
            var pns = await DependencyService.Get<IPNSService>().GetHandleAsync();
            var payload = @"<toast><visual><binding template=""ToastText01""><text id=""1"">$(message)</text></binding></visual></toast>";
            var registration = new WindowsTemplateRegistrationDescription(pns, payload, new[] { $"group:{IdEntry.Text}" });
            registration.RegistrationId = await hub.CreateRegistrationIdAsync();
            //registration.id
            var description = await hub.CreateOrUpdateRegistrationAsync(registration);
        }

    }
}