using Storm.Formification.Web.Forms;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using System.Linq;
using Storm.Formification.Core;
using System.ComponentModel.DataAnnotations;

namespace Storm.Formification.Web.Controllers
{
    public class FormModel
    {
        public Guid Id { get; set; }
        public BankAccountForm BankAccount { get; set; }
        public PetForm Pet { get; set; }
    }

    [Route("[controller]/[action]")]
    public class CustomController : Controller
    {
        private readonly IFormLocator formLocator;

        public CustomController(IFormLocator formLocator)
        {
            this.formLocator = formLocator;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TypedForm()
        {
            var model = new FormModel
            {
                Id = Guid.NewGuid(),
                BankAccount = new BankAccountForm(),
                Pet = new PetForm()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult TypedForm(FormModel model)
        {
            if(ModelState.IsValid)
            {

            }

            return View(model);
        }
    }
}
