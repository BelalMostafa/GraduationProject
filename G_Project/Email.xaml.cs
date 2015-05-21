using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace G_Project
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }


        private void btn_sigup_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SignUp));
        }

        private async void btn_login_Click(object sender, RoutedEventArgs e)
        {
            //user.Email = "email";
            //user.Password = "123";

            UserModel user = new UserModel();
            user.Email = txt_email.Text;
            user.Password = txt_pass.Password;


            string BaseAddress = "http://mts.somee.com:80/api/User/Login";
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseAddress);
            var Serialize = JsonConvert.SerializeObject(user); //IEnumerable<UserModel>);
            var content = new StringContent(Serialize, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(new Uri(BaseAddress), content);
            if (response.ReasonPhrase == "OK")
            {
                var reply = await response.Content.ReadAsStringAsync();
                var user_reply = JsonConvert.DeserializeObject<UserModel>(reply) as UserModel;
                this.Frame.Navigate(typeof(Home),user_reply);
            }


            

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
