using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Storm.Formification.Core.Infrastructure;
using Storm.Formification.WebWithDb.Data;
using Microsoft.EntityFrameworkCore;

namespace Storm.Formification.WebWithDb.Forms.Preferences
{
    public class AvailableUsersDatasource : IChoiceDataSource
    {
        private readonly ApplicationDbContext dbContext;

        public AvailableUsersDatasource(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<ChoiceItem>> GetAsync()
        {
            var users = await dbContext.Users.ToListAsync();

            return users.Select(u => new ChoiceItem { Value = u.Id, Text = u.Email });
        }

        public async Task<ChoiceItem> GetAsync(string value)
        {
            var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Id == value);

            if(user != null)
            {
                return new ChoiceItem { Value = user.Id, Text = user.Email };
            }

            return new ChoiceItem();
        }
    }
}
