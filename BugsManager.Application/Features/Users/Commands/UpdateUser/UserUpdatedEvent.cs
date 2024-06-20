using BugsManager.Domain.Common;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Users.Commands.UpdateUser
{
    public class UserUpdatedEvent: BaseEvent
    {
        public User _user { get; }
        public UserUpdatedEvent(User user)
        {
            _user = user;
        }
    }
}
