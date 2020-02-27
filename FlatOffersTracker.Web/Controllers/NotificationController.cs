using FlatOffersTracker.Cqrs.Commands;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FlatOffersTracker.Web.Controllers
{
    public class NotificationController : Controller
    {
        private ISetNotificationsAsSeenHandler _setNotifications;

        public NotificationController(ISetNotificationsAsSeenHandler setNotifications)
        {
            _setNotifications = setNotifications;
        }

        [HttpPut("api/notification/mark-viewed")]
        public IActionResult Update(int flatOfferId)
        {
            _setNotifications.Execute(new SetNotificationsAsSeenCommand
            {
                flatOffersIds = new List<int> { flatOfferId }
            });

            return View();
        }

        [HttpPut("api/notifications/mark-viewed")]
        public IActionResult Update([FromBody]IList<int> flatOfferIds)
        {
            _setNotifications.Execute(new SetNotificationsAsSeenCommand
            {
                flatOffersIds = flatOfferIds
            });

            return View();
        }
    }
}