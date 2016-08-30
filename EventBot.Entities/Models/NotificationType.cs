using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBot.Entities.Models
{
    public enum NotificationType
    {
        EventCanceled = 1,
        EventUpdated = 2,
        EventCreated = 3,
        EventJoined = 4,
        EventLeaved = 5,
        EventUserHasJoined = 6,
        EventUserHasLeaved = 7
    }
}
