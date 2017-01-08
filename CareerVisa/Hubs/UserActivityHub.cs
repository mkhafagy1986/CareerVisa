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

    [HubName("userActivityHub")]
    public class UserActivityHub : Hub
    {
        /// <summary>
        /// The count of users connected.
        /// </summary>
        public static List<LoggedInUser> Users = new List<LoggedInUser>();
        public static List<LoggedInUser> OnlineEmployers = new List<LoggedInUser>();
        public static List<LoggedInUser> OnlineJobSeekers = new List<LoggedInUser>();

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
                    var user = db.LoggedInUsers.FirstOrDefault(i => i.UserId == Context.User.Identity.Name);

                    if ((user != null) && (!Users.ToList().Exists(i => i.UserId == user.UserId)))
                    {

                        Users.Add(user);

                        if (Context.User.IsInRole("JobSeeker"))
                            OnlineJobSeekers.Add(user);
                        else if (Context.User.IsInRole("Employer"))
                            OnlineEmployers.Add(user);
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
            string SessionId = GetSessionId();
            if (Users.ToList().Exists(i => i.ConnectionId == SessionId))
            {
                Users.Remove(Users.FirstOrDefault(i => i.ConnectionId == SessionId));

                if (OnlineEmployers.ToList().Exists(i => i.ConnectionId == SessionId))
                {
                    OnlineEmployers.Remove(OnlineEmployers.FirstOrDefault(i => i.ConnectionId == SessionId));
                }
                else if (OnlineJobSeekers.ToList().Exists(i => i.ConnectionId == SessionId))
                {
                    OnlineJobSeekers.Remove(OnlineJobSeekers.FirstOrDefault(i => i.ConnectionId == SessionId));
                }
            }

            Send(OnlineEmployers.Count, OnlineJobSeekers.Count);
            return base.OnDisconnected(stopCalled);
        }

    }
}