using Storm.Formification.Core;
using FluentValidation;
using static Storm.Formification.Core.Forms;
using System;

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

    [Info("Pet", "d6a6f755-7844-4f01-8c6f-34bf69d3c08c")]
    public class PetForm
    {
        [Text]
        public string Name { get; set; }

        [Date]
        public DateTime DateOfBirth { get; set; }

        [Choice]
        public PetSpeciesType Species { get; set; }
    }
}
