﻿using InstagramProjectBack.Models.Interfaces;
using InstagramProjectBack.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace InstagramProjectBack.SignalR
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
           
            await base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {

            await base.OnDisconnectedAsync(exception);


        }


    }
}
