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

        public StartPage()
        {
            InitializeComponent();
            scanner = new MobileBarcodeScanner();
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
            var ws = new WebService();
            await ws.UnRegister(await DependencyService.Get<INotificationService>().GetHandleAsync(""));
            await DisplayAlert("Aviso", "Removido", "Ok");
        }


        private async Task RegisterAsync(string id)
        {
            var ns = DependencyService.Get<INotificationService>();
            var ws = new WebService();
            await ws.Register(await ns.GetHandleAsync(id));
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