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
            string id = IdEntry.Text;
            IdEntry.Text = "";
            await RegisterAsync(id);
            await DisplayAlert("Aviso", "Registrado", "Ok");
        }

        private static async Task RegisterAsync(string id)
        {
            var hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://yanscorp.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=Nt/JE/8Zc/F1ZwQ1mO/ZcM/NUg0CRs3DjI31IK7hTNM=", "MCastNotificationHub");
            var registration = await DependencyService.Get<INotificationService>().GetRegistrationDescriptionAsync(new[] { $"group:{id}" });
            registration.RegistrationId = await hub.CreateRegistrationIdAsync();
            //registration.id
            var description = await hub.CreateOrUpdateRegistrationAsync(registration);
        }

        private async void QR_Clicked(object sender, EventArgs args)
        {
            
        }

    }
}