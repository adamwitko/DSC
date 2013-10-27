using System;

namespace TeamSauce.Models
{
    public class TeamMessageModel
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public DateTime Time { get; set; }
    }
}