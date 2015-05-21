using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace G_Project
{
    class UserModel
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public bool LoggedIn { get; set; }
        public string ProfilePicture { get; set; }
        public BitmapImage Picture { get; set; }
    }
}
