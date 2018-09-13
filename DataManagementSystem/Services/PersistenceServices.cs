using DataManagementSystem.IServices;
using DataManagementSystem.Repositories;
using System.Collections.Generic;

namespace DataManagementSystem.Services
{
    public class PersistenceServices : IPersistenceServices
    {
        private readonly DataManagementSystemContext _dbcontext;

        public PersistenceServices(DataManagementSystemContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async void Insert(IEnumerable<DbFileNode> dbFileNodes)
        {
            foreach(var dbFileNode in dbFileNodes)
            {
                await _dbcontext.AddAsync(dbFileNode);
            }
            await _dbcontext.SaveChangesAsync();
        }
    }
}
