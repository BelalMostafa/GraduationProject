using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_Project
{
    class RequestModel
    {
        public int RequestId { get; set; }
        public int SenderID { get; set; }
        public int RecieverID { get; set; }
        public Nullable<bool> State { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
