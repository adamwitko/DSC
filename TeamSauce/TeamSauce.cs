using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace TeamSauce
{
    public class TeamSauce : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}