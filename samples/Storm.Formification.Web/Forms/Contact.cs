using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static Storm.Formification.Core.Forms;
using System;
using System.ComponentModel;
using Storm.Formification.Core.Infrastructure;

namespace Storm.Formification.Web.Forms
{
    public enum PersonGenderType
    {
        [Display(Name = "Prefer not to say")]
        PreferNotToSay = 0,

        [Display(Name = "Male")]
        Male = 1,

        [Display(Name = "Female")]
        Female = 2,
    }

    [Info("Contact", "e63b897e-a88f-410a-ba3a-17f446195b85")]
    public class ContactForm
    {
        [Text]
        public string? Firstname { get; set; }

        [Text]
        public string? MiddleNames { get; set; }

        [Text]
        public string? Lastname { get; set; }

        [Choice]
        public PersonGenderType Gender { get; set; }
        
        [Email]
        public string? Email { get; set; }

        [Date]
        [DisplayName("Date of birth")]
        public DateTime DateOfBirth { get; set; }

        [Date]
        [HintLabel("Optional")]
        public DateTime? PreferredDate { get; set; }

        [Upload]
        public IFormFile? Passport { get; set; }
    }

    public class Validator : AbstractValidator<ContactForm>
    {
        public Validator()
        {
            RuleFor(r => r.DateOfBirth).LessThan(DateTime.Now.Date);
        }
    }
}
