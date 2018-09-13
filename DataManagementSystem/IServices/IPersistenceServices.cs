using DataManagementSystem.Repositories;
using System.Collections.Generic;

namespace DataManagementSystem.IServices
{
    public interface IPersistenceServices
    {
        void Insert(IEnumerable<DbFileNode> dbFileNodes);
    }
}
