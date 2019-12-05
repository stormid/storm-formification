using System;
using System.Threading;
using System.Threading.Tasks;
using Storm.Formification.WebWithDb.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.WebWithDb.Forms
{
    public class FormContainerActions<TForm> : IFormActions<TForm> where TForm : class, new()
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IFormDataStore<TForm> dataStore;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<IdentityUser> userManager;

        public FormContainerActions(ApplicationDbContext dbContext, IFormDataStore<TForm> dataStore, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.dataStore = dataStore;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<TForm?> Retrieve(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var currentClaimsPrincipal = httpContextAccessor.HttpContext.User;
            var currentUser = await userManager.GetUserAsync(currentClaimsPrincipal);

            if(currentUser == null)
            {
                throw new UnauthorizedAccessException();
            }

            var formContainer = await dbContext
                .Set<FormContainer>()
                .FirstOrDefaultAsync(f => f.User == currentUser && f.Id == id && f.FormType == typeof(TForm), cancellationToken);

            if(formContainer == null)
            {
                return new TForm();
            }

            var form = await dataStore.Retrieve(formContainer.DocumentId, formContainer.SecretId);

            return form;
        }

        public async Task<Guid> Save(Guid id, TForm form, CancellationToken cancellationToken = default(CancellationToken))
        {
            var currentClaimsPrincipal = httpContextAccessor.HttpContext.User;
            var currentUser = await userManager.GetUserAsync(currentClaimsPrincipal);

            if (currentUser == null)
            {
                throw new UnauthorizedAccessException();
            }

            var formContainer = await dbContext.Set<FormContainer>().FirstOrDefaultAsync(f => f.Id == id && f.User == currentUser, cancellationToken);

            if(formContainer == null)
            {
                formContainer = new FormContainer(currentUser);
                await dbContext.AddAsync(formContainer);
            }
            else
            {
                dbContext.Update(formContainer);
            }

            formContainer.FormType = typeof(TForm);

            var result = await dataStore.Save(formContainer.DocumentId, formContainer.SecretId, form);

            formContainer.DocumentId = result?.DocumentId ?? throw new NullReferenceException("document id is null");
            formContainer.SecretId = result?.SecretId ?? throw new NullReferenceException("secret id is null");
            await dbContext.SaveChangesAsync();

            return formContainer.Id;
        }
    }
}
