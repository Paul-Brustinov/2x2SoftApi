using _2x2SoftApi.Area.Common.Store;
using A2v10.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2x2SoftApi.Area.Crm
{
    public class PersonStore : Store<Person>
    {

        public PersonStore(IDbContext dbContext, string name, string schema) : base(dbContext, name, schema)
        {
        }

        public async Task<Person> FindByEmailAsync(string email)
        {
            var person = _cache.MapIds.FirstOrDefault(x => x.Value.Emails.Any(e => e.Value == email)).Value;
            if (person != null) return person;

            person = await _dbContext.LoadAsync<Person>(null, _entitySchema + "." + _entityName + "FindByEmail", new { Email = email });
            _cache.CacheEntity(person);
            return person;
        }
    }
}
