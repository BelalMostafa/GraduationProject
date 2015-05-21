using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace G_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Home : Page
    {
        public Home()
        {
            this.InitializeComponent();
        }

        UserModel user ;
        List<object> param = new List<object>();

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as UserModel;
            param.Add(user);
            txt_user_name.Text = user.Name;
            if (user.ProfilePicture != null)
            {
                var image = await Base64ToImage(user.ProfilePicture);
                user.Picture = image;
                img_user.ImageSource=user.Picture;
            }
        }

        private async Task<BitmapImage> Base64ToImage(string base64)
        {
            var bitmap = new BitmapImage();
            var buffer = Convert.FromBase64String(base64);

            using (var stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(buffer.AsBuffer());
                stream.Seek(0);
                bitmap.SetSource(stream);
            }

            return bitmap;
        }

        private async void btn_logout_Click(object sender, RoutedEventArgs e)
        {
            string BaseAddress = "http://mts.somee.com:80/api/User/LogOut?Id=" + user.UserID;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseAddress);
            StringContent con = new StringContent(string.Empty);
            HttpResponseMessage response = await httpClient.PostAsync(new Uri(BaseAddress),con);
        }

        private void btn_friends_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Friends_Page), user);
        }

        private void btn_me_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Profile), param);
        }
        
    }
}
