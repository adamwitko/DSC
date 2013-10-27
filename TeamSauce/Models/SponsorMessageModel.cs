using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeamSauce.Models
{
    public class SponsorMessageModel
    {
        public string MessageType { get; set; }
        public string Message { get; set; }
        public string Sender { get; set; }
        public DateTime Time { get; set; }
        public string TeamId { get; set; }
    }
}