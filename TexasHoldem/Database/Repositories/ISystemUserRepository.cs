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
        bool Add(SystemUser systemUser);
        bool Update(SystemUser systemUser);
        bool Remove(SystemUser systemUser);
        SystemUser GetById(int systemUserId);
        SystemUser GetByName(string name);
        SystemUser GetByEmail(string email);
        IList<SystemUser> GetByRestrictions(IDictionary<string, string> restrictions, string orderBy, bool ascending, int? limit);

    }
}
