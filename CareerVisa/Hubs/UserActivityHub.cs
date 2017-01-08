using System.Linq;

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
        private static readonly ConcurrentDictionary<string, string> Users = new ConcurrentDictionary<string, string>();
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

            Users.TryAdd(Context.ConnectionId, Context.User.Identity.GetUserId());

            if (Context.User.IsInRole("JobSeeker"))
                OnlineJobSeekers.Add(Context.User.Identity.GetUserId());
            else if (Context.User.IsInRole("Employer"))
                OnlineEmployers.Add(Context.User.Identity.GetUserId());

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
            string ConnectionId = Users.Where(user => user.Value == Context.User.Identity.GetUserId()).ToList().First().Value;
            if(ConnectionId !=null && ConnectionId != Context.ConnectionId)
            {
                string UserName = Context.User.Identity.GetUserId();
                Users.TryRemove(ConnectionId,out UserName);

                if (OnlineEmployers.ToList().Exists(i => i == UserName))
                {
                    OnlineEmployers.Remove(OnlineEmployers.FirstOrDefault(i => i == UserName));
                }
                else if (OnlineJobSeekers.ToList().Exists(i => i == UserName))
                {
                    OnlineJobSeekers.Remove(OnlineJobSeekers.FirstOrDefault(i => i == UserName));
                }
            }
           
            Send(OnlineEmployers.Count, OnlineJobSeekers.Count);
            return base.OnDisconnected(stopCalled);
        }

    }
}