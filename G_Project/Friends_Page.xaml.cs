using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
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
    public sealed partial class Friends_Page : Page
    {
        public Friends_Page()
        {
            this.InitializeComponent();
        }

        UserModel user;
        List<object> param = new List<object>();

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as UserModel;
            param.Add(user);

            //get friends list
            int user_id = user.UserID;
            List<FriendModel> friend_list = await GetFriendList(user_id);
            Friend_list.Source = friend_list;

            //get request list
            List<RequestModel> requests = await GetRequestList(user_id);

            List<RequestUser> request_users = new List<RequestUser>();
            foreach (var item in requests)
            {
                //get user by id
                RequestUser user3 = await GetUserByID(item.SenderID, item.RequestId);
                request_users.Add(user3);
            }


            request_list.Source = request_users;

        }


        private async Task<List<FriendModel>> GetFriendList(int UserID)
        {
            string BaseAddress = "http://mts.somee.com:80/api/Friend/GetAllFriends?UserID=" + UserID;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseAddress);
            HttpResponseMessage response = await httpClient.GetAsync(new Uri(BaseAddress));
            var readresponse = await response.Content.ReadAsStringAsync();
            List<FriendModel> friend_list = JsonConvert.DeserializeObject<List<FriendModel>>(readresponse) as List<FriendModel>;

            foreach (var item in friend_list)
            {
                try
                {
                    item.Picture = await Base64ToImage(item.ProfilePicture);
                }
                catch { }
            }
            return friend_list;
        }

        private async Task<List<RequestModel>> GetRequestList(int UserID)
        {
            //get all requests
            string BaseAddress2 = "http://mts.somee.com:80/api/Friend/GetAllFriendRequests?UserID=" + UserID;
            HttpClient httpClient2 = new HttpClient();
            httpClient2.BaseAddress = new Uri(BaseAddress2);
            HttpResponseMessage response2 = await httpClient2.GetAsync(new Uri(BaseAddress2));
            var reply = await response2.Content.ReadAsStringAsync();
            List<RequestModel> requests = JsonConvert.DeserializeObject<List<RequestModel>>(reply) as List<RequestModel>;
            return requests;
        }

        private async Task<RequestUser> GetUserByID(int SenderID, int RequestID)
        {
            string BaseAddress3 = "http://mts.somee.com:80/api/User/GetUserByID?UserID=" + SenderID;
            HttpClient httpClient3 = new HttpClient();
            httpClient3.BaseAddress = new Uri(BaseAddress3);
            HttpResponseMessage response3 = await httpClient3.GetAsync(new Uri(BaseAddress3));
            var reply3 = await response3.Content.ReadAsStringAsync();
            RequestUser user3 = JsonConvert.DeserializeObject<RequestUser>(reply3) as RequestUser;
            try
            {
                user3.RequestID = RequestID;
                user3.Picture = await Base64ToImage(user3.ProfilePicture);
            }
            catch { }
            return user3;
        }

        private async Task<bool> SendFriendRequest(int UserID, int FriendID)
        {
            try
            {
                string BaseAddress = "http://mts.somee.com:80/api/Friend/SendFriendRequest?UserID=" + UserID;
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(BaseAddress);
                var s = JsonConvert.SerializeObject(FriendID);
                var r = new StringContent(s, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(new Uri(BaseAddress), r);
                return true;
            }
            catch { return false; }
        }

        private async Task<bool> AcceptFriendRequest(int UserID, int RequestID)
        {
            try
            {
                string BaseAddress = "http://mts.somee.com:80/api/Friend/AcceptFriendRequest?UserID=" + UserID;
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(BaseAddress);
                var s = JsonConvert.SerializeObject(RequestID);
                var r = new StringContent(s, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(new Uri(BaseAddress), r);
                return true;
            }
            catch { return false; }
        }

        private async Task<bool> RejectFriendRequest(int UserID, int RequestID)
        {
            try
            {
                string BaseAddress = "http://mts.somee.com:80/api/Friend/RejectFriendRequest?UserID=" + UserID;
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(BaseAddress);
                var s = JsonConvert.SerializeObject(RequestID);
                var r = new StringContent(s, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(new Uri(BaseAddress), r);
                return true;
            }
            catch { return false; }
        }

        private async Task<List<FriendModel>> GetSuggestionsByEmails(string Emails)
        {
            string BaseAddress = "http://mts.somee.com:80/api/Friend/GetSuggestionsByEmails" + Emails;
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseAddress);
            HttpResponseMessage response = await httpClient.GetAsync(new Uri(BaseAddress));
            var reply = await response.Content.ReadAsStringAsync();
            List<FriendModel> suggests = JsonConvert.DeserializeObject<List<FriendModel>>(reply) as List<FriendModel>;
            foreach (var item in suggests)
            {
                try
                {
                    item.Picture = await Base64ToImage(item.ProfilePicture);
                }
                catch { }
            }
            return suggests;
        }

        private async Task<bool> UnFriend(int FriendID)
        {
            try
            {
                string BaseAddress = "http://mts.somee.com:80/api/Friend/UnFriend?UserID=" + user.UserID;
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(BaseAddress);
                var s = JsonConvert.SerializeObject(FriendID);
                var r = new StringContent(s, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(new Uri(BaseAddress), r);
                return true;
            }
            catch { return false; }

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

        private void SearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Search_Box.Visibility = Visibility.Visible;
            SearchImage.Visibility = Visibility.Collapsed;
        }

        private void lst_friends_ItemClick(object sender, ItemClickEventArgs e)
        {
            FriendModel friend = e.ClickedItem as FriendModel;
            param.Add(friend);
            this.Frame.Navigate(typeof(Profile), param);
        }

        private async void btn_suggest_Click(object sender, RoutedEventArgs e)
        {
            string Emails = "?";
            //ContactPicker p = new ContactPicker();
            //p.CommitButtonText = "SelectAll";
            //p.SelectionMode = ContactSelectionMode.Fields;
            //p.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);


            //IList<Contact> c = (IList<Contact>)await p.PickContactsAsync();
            //List<string> emails = new List<string>();

            //foreach (Contact item in c)
            //{
            //    var email = item.Emails.First();
            //    var email2 = email.Address;
            //    emails.Add(email2);
            //}

            //int count = emails.Count;

            //for (int i = 0; i < count; i++)
            //{
            //    try
            //    {
            //        string email = emails[i].ToString();
            //        if (count > 1)
            //        {
            //            Emails += "Emails=" + email + "&";
            //        }
            //        else if (count == 1)
            //        {
            //            Emails += "Emails=" + email;
            //        }
            //    }
            //    catch { }
            //    count--;
            //}
            Emails = "?Emails=abuzeid987@yahoo.com&Emails=mo.adwi@yahoo.com&Emails=podythelegend@yahoo.com";

            List<FriendModel> suggests = await GetSuggestionsByEmails(Emails);
            suggest_list.Source = suggests;

        }

        private void lst_suggest_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private async void img_add_Click(object sender, RoutedEventArgs e)
        {
            Button f = e.OriginalSource as Button;
            int Friend_Id = int.Parse(f.TabIndex.ToString());
            bool reply = await SendFriendRequest(user.UserID, Friend_Id);
            if (reply == true)
            {
                string Emails = "?Emails=abuzeid987@yahoo.com&Emails=mo.adwi@yahoo.com";
                List<FriendModel> suggests = await GetSuggestionsByEmails(Emails);
                suggest_list.Source = suggests;
            }
        }

        private async void btn_unfriend_Click(object sender, RoutedEventArgs e)
        {
            Button f = e.OriginalSource as Button;
            int User_Id = int.Parse(f.TabIndex.ToString());
            bool reply = await UnFriend(User_Id);
            if (reply == true)
            {
                List<FriendModel> friend_list = await GetFriendList(user.UserID);
                Friend_list.Source = friend_list;
            }
        }

        private async void Btn_Accept_Click(object sender, RoutedEventArgs e)
        {
            Button f = e.OriginalSource as Button;
            int RequestID = int.Parse(f.TabIndex.ToString());
            bool reply = await AcceptFriendRequest(user.UserID, RequestID);
            if (reply == true)
            {
                List<FriendModel> friend_list = await GetFriendList(user.UserID);
                Friend_list.Source = friend_list;

                List<RequestModel> requests = await GetRequestList(user.UserID);
                List<RequestUser> request_users = new List<RequestUser>();
                foreach (var item in requests)
                {
                    RequestUser user3 = await GetUserByID(item.SenderID, item.RequestId);
                    request_users.Add(user3);
                }
                request_list.Source = request_users;
            }
        }

        private async void Btn_Decline_Click(object sender, RoutedEventArgs e)
        {
            Button f = e.OriginalSource as Button;
            int RequestID = int.Parse(f.TabIndex.ToString());
            bool reply = await RejectFriendRequest(user.UserID, RequestID);
            if (reply == true)
            {
                List<FriendModel> friend_list = await GetFriendList(user.UserID);
                Friend_list.Source = friend_list;

                List<RequestModel> requests = await GetRequestList(user.UserID);
                List<RequestUser> request_users = new List<RequestUser>();
                foreach (var item in requests)
                {
                    RequestUser user3 = await GetUserByID(item.SenderID, item.RequestId);
                    request_users.Add(user3);
                }
                request_list.Source = request_users;
            }
        }
    }
}
