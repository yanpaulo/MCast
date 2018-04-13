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
		}
	}
}