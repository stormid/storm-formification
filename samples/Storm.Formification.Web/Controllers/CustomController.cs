using Storm.Formification.Web.Forms;
using Microsoft.AspNetCore.Mvc;
using System;

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
