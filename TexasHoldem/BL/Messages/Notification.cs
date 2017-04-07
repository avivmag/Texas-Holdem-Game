using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Messages
{
	abstract class Notification
	{
		private List<User.SystemUser> subscribers = new List<User.SystemUser>();
		void subscribe(User.SystemUser systemUser)
		{
			subscribers.Add(systemUser);
		}
		void unsubscrive(User.SystemUser systemUser)
		{
			subscribers.Remove(systemUser);
		}
		void notify(String str)
		{
			foreach(User.SystemUser systemUser in subscribers)
			{
				systemUser.update(str);
			}
		}
	}
}
