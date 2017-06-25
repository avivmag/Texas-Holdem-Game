using Backend.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeL
{
    class SystemUserCache
    {
        /// <summary>
        /// the newer the node is in the list the latter it is in the list
        /// </summary>
        List<SystemUser> list;
        int max;

        public SystemUserCache(int max)
        {
            this.max = max;
            list = new List<SystemUser>();
        }

        /// <summary>
        /// works by a given Id.
        /// </summary>
        /// <param name="user"></param>
        public void addOrUpdate(SystemUser user)
        {
            List<SystemUser> filtered = list.Where(su => su.id == user.id).ToList();
            if (filtered.Count != 0)
                list.Remove(filtered[0]);

            list.Add(user);

            if (list.Count >= max)
                list.RemoveRange(0, ((int)(0.25*max)));
        }

        public SystemUser getById(int id)
        {
            List<SystemUser> filtered = list.Where(su => su.id == id).ToList();
            if (filtered.Count == 0)
                return null;

            list.Remove(filtered[0]);
            list.Add(filtered[0]);

            return filtered[0];
        }
        public SystemUser getByName(string UserName)
        {
            List<SystemUser> filtered = list.Where(su => su.name.Equals(UserName)).ToList();
            if (filtered.Count == 0)
                return null;

            list.Remove(filtered[0]);
            list.Add(filtered[0]);

            return filtered[0];
        }
        public SystemUser getByEmail(string Email)
        {
            List<SystemUser> filtered = list.Where(su => su.email.Equals(Email)).ToList();
            if (filtered.Count == 0)
                return null;

            list.Remove(filtered[0]);
            list.Add(filtered[0]);

            return filtered[0];
        }
        public void remove(int id)
        {
            List<SystemUser> filtered = list.Where(su => su.id == id).ToList();
            if (filtered.Count != 0)
                list.Remove(filtered[0]);
        }
    }
}
