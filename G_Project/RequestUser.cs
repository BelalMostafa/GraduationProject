using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace G_Project
{
    class RequestUser
    {
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string ProfilePicture { get; set; }
        public int RequestID { get; set; }
        public BitmapImage Picture { get; set; }

    }
}
