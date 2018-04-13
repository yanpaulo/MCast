using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;

namespace UserApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        private bool isScanning;
        private MobileBarcodeScanner scanner;
        private NotificationHubClient hub;

        public StartPage()
        {
            InitializeComponent();
            scanner = new MobileBarcodeScanner();
            hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://yanscorp.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=MVZlt7280W+ncv+ad1QoA8Y5GG5bxQpB/8zTPe9YnBo=", "MCastNotificationHub");
        }


        private async void Button_Clicked(object sender, EventArgs args)
        {
            string id = IdEntry.Text;
            IdEntry.Text = "";
            await RegisterAsync(id);
        }


        private async void QR_Clicked(object sender, EventArgs args)
        {
            try
            {
                isScanning = true;
                var code = await scanner.Scan();
                if (code != null)
                {
                    await RegisterAsync(code.Text); 
                }
            }
            finally
            {
                isScanning = false;
            }
        }


        private async void CleanButton_Clicked(object sender, EventArgs args)
        {
            var registrations = await hub.GetAllRegistrationsAsync(100);
            foreach (var registration in registrations)
            {
                await hub.DeleteRegistrationAsync(registration);
            }
            await DisplayAlert("Aviso", "Removido", "Ok");
        }


        private async Task RegisterAsync(string id)
        {
            var registration = await DependencyService.Get<INotificationService>().GetRegistrationDescriptionAsync(new[] { $"group:{id}" });
            registration.RegistrationId = await hub.CreateRegistrationIdAsync();
            //registration.id
            var description = await hub.CreateOrUpdateRegistrationAsync(registration);
            await DisplayAlert("Aviso", "Registrado", "Ok");
        }

        protected override bool OnBackButtonPressed()
        {
            if (isScanning)
            {
                scanner.Cancel();
                return true;
            }
            return base.OnBackButtonPressed();
        }

    }
}