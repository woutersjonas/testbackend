using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jonas.Infrastructure.Database.Transactions;

public interface IUnitOfWork
{
    void BeginTransaction();
    void Commit();
    void Rollback();
    void SaveChanges();
}
