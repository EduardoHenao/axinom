using DataManagementSystem.IServices;
using DataManagementSystem.Repositories;
using System.Collections.Generic;

namespace DataManagementSystem.Services
{
    /*
     * a service to handle DbFileNode entities to be persisted
     */
    public class PersistenceServices : IPersistenceServices
    {
        private readonly DataManagementSystemContext _dbcontext;

        public PersistenceServices(DataManagementSystemContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public void Insert(IEnumerable<DbFileNode> dbFileNodes)
        {
            foreach(var dbFileNode in dbFileNodes)
            {
                _dbcontext.Add(dbFileNode);
            }
            _dbcontext.SaveChanges();
        }
    }
}
