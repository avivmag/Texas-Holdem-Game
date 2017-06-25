using Database.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public interface ISystemUserRepository
    {
        void Add(SystemUser systemUser);
        void Update(SystemUser systemUser);
        void Remove(SystemUser systemUser);
        SystemUser GetById(int systemUserId);
        SystemUser GetByName(string name);
        IList<SystemUser> GetByRestrictions(IDictionary<string, string> restrictions, string orderBy, bool ascending, int? limit);

    }
}
