using Microsoft.Azure.NotificationHubs;
using System;
using UserApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UserApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : TabbedPage
	{
		public MainPage ()
		{
			InitializeComponent ();

            //Appearing += async (o, e) =>
            //{
            //    var hub = Microsoft.Azure.NotificationHubs.NotificationHubClient.CreateClientFromConnectionString("Endpoint = sb://yanscorp.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=Nt/JE/8Zc/F1ZwQ1mO/ZcM/NUg0CRs3DjI31IK7hTNM=", "MCastNotificationHub");
            //    var pns = await DependencyService.Get<IPNSService>().GetHandleAsync();
            //    var payload = @"<toast><visual><binding template=""ToastText01""><text id=""1"">$(message)</text></binding></visual></toast>";
            //    var registration = new WindowsTemplateRegistrationDescription(pns, payload);
            //    await hub.CreateOrUpdateRegistrationAsync(registration);
            //};
		}
	}
}