using System.Linq;

namespace CareerVisa.Hubs
{
    using System.Collections.Generic;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.AspNet.Identity;
    using Models;

    [HubName("userActivityHub")]
    public class UserActivityHub : Hub
    {
        /// <summary>
        /// The count of users connected.
        /// </summary>
        public static List<OnlineUserViewModel> Users = new List<OnlineUserViewModel>();
        public static List<OnlineUserViewModel> OnlineEmployers = new List<OnlineUserViewModel>();
        public static List<OnlineUserViewModel> OnlineJobSeekers = new List<OnlineUserViewModel>();

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
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    var user = db.Users.FirstOrDefault(i => i.UserName == Context.User.Identity.Name);

                    if (!Users.ToList().Exists(i => i.ConnectionId == Context.ConnectionId && i.User == user))
                    {
                        OnlineUserViewModel tuble = new OnlineUserViewModel();
                        tuble.ConnectionId = Context.ConnectionId;
                        tuble.User = user;

                        Users.Add(tuble);

                        if (Context.User.IsInRole("JobSeeker"))
                            OnlineJobSeekers.Add(tuble);
                        else if (Context.User.IsInRole("Employer"))
                            OnlineEmployers.Add(tuble);
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Context.User.Identity.Name))
                {
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
            if (Users.ToList().Exists(i => i.ConnectionId == Context.ConnectionId))
            {
                Users.Remove(Users.FirstOrDefault(i => i.ConnectionId == Context.ConnectionId));

                if (OnlineEmployers.ToList().Exists(i => i.ConnectionId == Context.ConnectionId))
                {
                    OnlineEmployers.Remove(OnlineEmployers.FirstOrDefault(i => i.ConnectionId == Context.ConnectionId));
                }
                else if (OnlineJobSeekers.ToList().Exists(i => i.ConnectionId == Context.ConnectionId))
                {
                    OnlineJobSeekers.Remove(OnlineJobSeekers.FirstOrDefault(i => i.ConnectionId == Context.ConnectionId));
                }
            }

            Send(OnlineEmployers.Count, OnlineJobSeekers.Count);
            return base.OnDisconnected(stopCalled);
        }

        
    }
}