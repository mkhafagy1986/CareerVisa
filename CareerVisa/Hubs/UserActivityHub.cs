﻿using System.Linq;

namespace CareerVisa.Hubs
{
    using System.Collections.Generic;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.AspNet.Identity;
    using Models;
    using System.Web.SessionState;
    using System.Web;
    using Models.Entities;
    using System.Collections.Concurrent;

    [HubName("userActivityHub")]
    public class UserActivityHub : Hub
    {
        /// <summary>
        /// The count of users connected.
        /// </summary>

        private static List<System.Tuple<string, string>> Users = new List<System.Tuple<string, string>>();
        //private static readonly ConcurrentDictionary<string, string> Users = new ConcurrentDictionary<string, string>();
        //public static List<LoggedInUser> Users = new List<LoggedInUser>();
        public static List<string> OnlineEmployers = new List<string>();
        public static List<string> OnlineJobSeekers = new List<string>();

        /// <summary>
        /// Sends the update user count to the listening view.
        /// </summary>
        /// <param name="count">
        /// The count.
        /// </param>
        public void Send(int OnlineEmployersCount, int OnlineJobSeekersCount)
        {
            // Call the addNewMessageToPage method to update clients.
            var context = GlobalHost.ConnectionManager.GetHubContext<UserActivityHub>();
            context.Clients.All.updateUsersOnlineCount(OnlineEmployersCount, OnlineJobSeekersCount);
        }


        /// <summary>
        /// The OnConnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>

        public override System.Threading.Tasks.Task OnConnected()
        {

            if (Context.User.Identity.IsAuthenticated)
            {
                var user = Context.User.Identity.Name;

                if (!Users.ToList().Exists(i => i.Item2 == user))
                {
                    Users.Add(new System.Tuple<string, string>(Context.ConnectionId, user));

                    if (Context.User.IsInRole("JobSeeker"))
                        OnlineJobSeekers.Add(user);
                    else if (Context.User.IsInRole("Employer"))
                        OnlineEmployers.Add(user);
                }
                
            }
            else
            {
                if (string.IsNullOrEmpty(Context.User.Identity.Name))
                {
                    //RemoverOfflineUser(Context.ConnectionId);
                    OnDisconnected(true);
                }
            }
            Send(OnlineEmployers.Count, OnlineJobSeekers.Count);

            return base.OnConnected();
        }

        /// <summary>
        /// The OnReconnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }

        /// <summary>
        /// The OnDisconnected event.
        /// </summary>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            Send(OnlineEmployers.Count, OnlineJobSeekers.Count);
            return base.OnDisconnected(stopCalled);
        }
        public static string GetConnectionIdByName(string UserId)
        {
            var UserTuble = Users.FirstOrDefault(user => user.Item2 == UserId);
            if (UserTuble != null)
                return UserTuble.Item1;
            return "";
        }

        public static void RemoverOfflineUser(string ConnectionId)
        {
            string UserName = "";
            if (Users.ToList().Exists(i => i.Item1 == ConnectionId))
            {
                UserName = Users.FirstOrDefault(i => i.Item1 == ConnectionId).Item2;
                Users.Remove(Users.FirstOrDefault(i => i.Item1 == ConnectionId));
            }
            if (OnlineEmployers.ToList().Exists(i => i == UserName))
            {
                OnlineEmployers.Remove(OnlineEmployers.FirstOrDefault(i => i == UserName));
            }
            else if (OnlineJobSeekers.ToList().Exists(i => i == UserName))
            {
                OnlineJobSeekers.Remove(OnlineJobSeekers.FirstOrDefault(i => i == UserName));
            }

        }
    }


}