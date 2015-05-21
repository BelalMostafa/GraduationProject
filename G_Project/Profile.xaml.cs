using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class Profile : Page
    {
        public Profile()
        {
            this.InitializeComponent();
        }

        UserModel user;
        FriendModel friend;
        List<object> param = new List<object>();

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            param=e.Parameter as List<object>;
            user = param[0] as UserModel;

            if(param.Count>1)
            {
                friend = param[1] as FriendModel;
                txt_user_name.Text = friend.Name;
                if (friend.ProfilePicture != null)
                {
                    var image = await Base64ToImage(friend.ProfilePicture);
                    friend.Picture = image;
                    img_user.ImageSource = image;
                }


                int user_id = user.UserID;
                int friend_id = friend.UserID;
                string BaseAddress = "http://mts.somee.com:80/api/Friend/GetFriendByID?UserID=" + user_id + "&FriendID=" + friend_id;
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(BaseAddress);
                HttpResponseMessage response = await httpClient.GetAsync(new Uri(BaseAddress));
                var reply = await response.Content.ReadAsStringAsync();
                FriendModel is_friend = JsonConvert.DeserializeObject<FriendModel>(reply) as FriendModel;
                if (is_friend != null)
                {
                    //friend's profile
                    btn_edit_Add.Content = "Un Friend";
                }
                else
                {
                    //not friend's profile
                    btn_edit_Add.Content = "Add Friend";                    
                }


            }
            else if (param.Count == 1)
            {
                //user's profile
                txt_user_name.Text = user.Name;
                if (user.ProfilePicture != null)
                {
                    var image = await Base64ToImage(user.ProfilePicture);
                    user.Picture = image;
                    img_user.ImageSource = image;
                }
                btn_edit_Add.Content = "Edit Profile";
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

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }
    }
}
