namespace CareerVisa.Utils
{
    using System.Collections.Generic;

    using Microsoft.AspNet.SignalR;
    using Microsoft.AspNet.SignalR.Hubs;
    using Microsoft.AspNet.Identity;

    [HubName("userActivityHub")]
    public class UserActivityHub : Hub
    {
        /// <summary>
        /// The count of users connected.
        /// </summary>
        public static List<string> Users = new List<string>(); 
        public static List<string> Employers = new List<string>(); 
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
            string clientId = GetClientId();

            if (Users.IndexOf(clientId) == -1)
            {
                Users.Add(clientId);

                if (Context.User.IsInRole("JobSeeker"))
                    OnlineJobSeekers.Add(clientId);
                else if(Context.User.IsInRole("Employer"))
                    OnlineEmployers.Add(clientId);
            }

            // Send the current count of users
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
            string clientId = GetClientId();
            if (Users.IndexOf(clientId) == -1)
            {
                Users.Add(clientId);
                if (Context.User.IsInRole("JobSeeker"))
                    OnlineJobSeekers.Add(clientId);
                else if (Context.User.IsInRole("Employer"))
                    OnlineEmployers.Add(clientId);
            }

            // Send the current count of users
            Send(OnlineEmployers.Count, OnlineJobSeekers.Count);

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
            string clientId = GetClientId();

            if (Users.IndexOf(clientId) > -1)
            {
                Users.Remove(clientId);
                if (Context.User.IsInRole("JobSeeker"))
                    OnlineJobSeekers.Remove(clientId);
                else if (Context.User.IsInRole("Employer"))
                    OnlineEmployers.Remove(clientId);
            }

            // Send the current count of users
            Send(OnlineEmployers.Count, OnlineJobSeekers.Count);

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// Get's the currently connected Id of the client.
        /// This is unique for each client and is used to identify
        /// a connection.
        /// </summary>
        /// <returns>The client Id.</returns>
        private string GetClientId()
        {
            string clientId = "";
            if (Context.QueryString["clientId"] != null)
            {
                // clientId passed from application 
                clientId = this.Context.QueryString["clientId"];
            }

            if (string.IsNullOrEmpty(clientId.Trim()))
            {
                clientId = Context.User.Identity.GetUserId();
                //clientId = Context.ConnectionId;
            }

            return clientId;
        }
    }
}