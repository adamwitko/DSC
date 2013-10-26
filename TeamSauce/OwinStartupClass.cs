﻿using Microsoft.Owin;
using Owin;
using TeamSauce.Connections.TeamChatConnection;
using TeamSauce.Hubs;

[assembly: OwinStartup(typeof(TeamSauce.Startup))]
namespace TeamSauce
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR<TeamChatConnection>("/teamchat");
            app.MapSignalR();
        }
    }
}