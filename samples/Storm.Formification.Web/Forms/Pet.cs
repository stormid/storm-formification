using Storm.Formification.Core;
using FluentValidation;
using static Storm.Formification.Core.Forms;

namespace Storm.Formification.Web.Forms
{
    public static class Pet
    {
        public class Validator : AbstractValidator<PetForm>
        {
            public Validator()
            {
                RuleFor(r => r.Name).NotEmpty();
            }
        }
    }

    public enum PetSpeciesType
    {
        Dog,
        Cat,
        Parrot,
        Rabbit,
    }

    [Info("Pet")]
    public class PetForm
    {
        [Text]
        public string Name { get; set; }

        [Date]
        public DateModel DateOfBirth { get; set; }

        [Choice]
        public PetSpeciesType Species { get; set; }
    }
}
