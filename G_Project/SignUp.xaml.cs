using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
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
    public sealed partial class SignUp : Page
    {
        public SignUp()
        {
            this.InitializeComponent();
        }

        string imageInStringFormat;
        private async void btn_choose_image_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var filestream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
                img_user.SetSource(filestream);

                var bytes = new Byte[0];
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    var reader = new DataReader(fileStream.GetInputStreamAt(0));
                    bytes = new Byte[fileStream.Size];
                    await reader.LoadAsync((uint)fileStream.Size);
                    reader.ReadBytes(bytes);
                }

                imageInStringFormat = Convert.ToBase64String(bytes);

            }
            else
            {

            }
        }

        private async void btn_signUp_Click(object sender, RoutedEventArgs e)
        {
            UserModel user = new UserModel();
            user.Name = txt_name.Text;
            user.Email = txt_email.Text;
            user.Phone = txt_phone.Text;
            user.Password = txt_pass.Password;
            user.ProfilePicture = imageInStringFormat;

            string BaseAddress = "http://mts.somee.com:80/api/User/Register";
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseAddress);
            var Serialize = JsonConvert.SerializeObject(user);
            var content = new StringContent(Serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(new Uri(BaseAddress), content);
            if (response.ReasonPhrase == "OK")
            {
                var reply = await response.Content.ReadAsStringAsync();
                var user_reply = JsonConvert.DeserializeObject<UserModel>(reply) as UserModel;
                this.Frame.Navigate(typeof(Home), user_reply);
            }    
            
            }

        private void btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

    }
}
