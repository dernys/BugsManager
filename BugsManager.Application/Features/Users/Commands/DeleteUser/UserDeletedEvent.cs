using BugsManager.Domain.Common;
using BugsManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsManager.Application.Features.Users.Commands.DeleteUser
{
    public class UserDeletedEvent: BaseEvent
    {
        public User _user { get;  }

        public UserDeletedEvent(User user)
        {
            _user = user;
        }
    }
}
