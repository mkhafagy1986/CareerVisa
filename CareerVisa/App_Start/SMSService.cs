using CareerVisa.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CareerVisa.App_Start
{
    public class SMSService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var twilioModel = db.Twilios.ToList().First();
            // Twilio Begin
            var Twilio = new Twilio.TwilioRestClient(
              twilioModel.SMSAccountIdentification,
              twilioModel.SMSAccountPassword);
            var result = Twilio.SendMessage(
              twilioModel.SMSAccountFrom,
              message.Destination, message.Body
            );
            //Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
             Trace.TraceInformation(result.Status);
            //Twilio doesn't currently have an async API, so return success.
             return Task.FromResult(0);
            // Twilio End
        }

        Task IIdentityMessageService.SendAsync(IdentityMessage message)
        {
            throw new NotImplementedException();
        }
    }
}