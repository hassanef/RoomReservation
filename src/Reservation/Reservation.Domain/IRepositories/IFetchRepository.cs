using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Domain.IRepositories
{
    public interface IFetchRepository<T> where T : class
    {
        IQueryable<T> FetchMultiWithTracking(Expression<Func<T, bool>> predicate = null);

        IQueryable<T> FetchMulti(Expression<Func<T, bool>> predicate = null);

        Task<Boolean> AnyAsync(Expression<Func<T, bool>> predicate);

        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null);

        T SingleOrDefault(Expression<Func<T, bool>> predicate);

        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

        int Count(Expression<Func<T, bool>> predicate = null);

        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

    }
}
