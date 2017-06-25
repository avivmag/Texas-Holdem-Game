using NHibernate;
using NHibernate.Criterion;
using Database.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories
{
    public class SystemUserRepository : ISystemUserRepository
    {
        public void Add(SystemUser systemUser)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(systemUser);
                    transaction.Commit();
                }
        }

        //public ICollection<SystemUser> GetByCategory(string category)
        //{
        //    using (ISession session = NHibernateHelper.OpenSession())
        //    {
        //        var systemUser = session
        //            .CreateCriteria(typeof(SystemUser))
        //            .Add(Restrictions.Eq("Category", category))
        //            .List<SystemUser>();
        //        return systemUser;
        //    }
        //}

        public SystemUser GetById(int systemUserId)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                return session.Get<SystemUser>(systemUserId);
        }

        public SystemUser GetByName(string name)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                SystemUser systemUser = session
                    .CreateCriteria(typeof(SystemUser))
                    .Add(Restrictions.Eq("UserName", name))
                    .UniqueResult<SystemUser>();
                return systemUser;
            }
        }


        public IList<SystemUser> GetByRestrictions(IDictionary<string, string> restrictions, string orderBy, bool ascending, int? limit)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            {
                NHibernate.
                ICriteria criteria = session.CreateCriteria<SystemUser>();
                foreach (KeyValuePair<string, string> pair in restrictions)
                    criteria = criteria.Add(Restrictions.Eq(pair.Key, pair.Value));

                if (limit != null)
                    criteria = criteria.SetMaxResults((int) limit);

                if(orderBy != null)
                    criteria = criteria.AddOrder(new Order(orderBy, ascending));
                
                return criteria.List<SystemUser>();
            }
        }

        public void Remove(SystemUser systemUser)
        {
            using (ISession session = NHibernateHelper.OpenSession())
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Delete(systemUser);
                transaction.Commit();
            }
        }

        public void Update(SystemUser systemUser)
        {
            using (ISession session = NHibernateHelper.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(systemUser);
                    transaction.Commit();
                }
        }
    }
}
