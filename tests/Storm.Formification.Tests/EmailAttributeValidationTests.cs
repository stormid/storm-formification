using FluentAssertions;
using Storm.Formification.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Storm.Formification.Tests
{
    public class EmailAttributeValidationTests
    {
        [Theory]
        [InlineData("email@example.com")]
        [InlineData("firstname.lastname@example.com")]
        [InlineData("email@subdomain.example.com")]
        [InlineData("firstname+lastname@example.com")]
        [InlineData("email@123.123.123.123")]
        [InlineData("1234567890@example.com")]
        [InlineData("email@example-one.com")]
        [InlineData("_______@example.com")]
        [InlineData("email@example.name")]
        [InlineData("email@example.museum")]
        [InlineData("email@example.co.jp")]
        [InlineData("firstname-lastname@example.com")]
        public void IsValidEmailAddress(string input)
        {
            var emailAttribute = new Forms.EmailAttribute();

            emailAttribute.IsValid(input).Should().BeTrue();
        }

        [Theory]
        [InlineData("plainaddress")]
        [InlineData("#@%^%#$@#$@#.com")]
        [InlineData("@example.com")]
        [InlineData("Joe Smith <email@example.com>")]
        [InlineData("email.example.com")]
        [InlineData("email@example@example.com")]
        [InlineData("あいうえお@example.com")]
        [InlineData("email@example.com (Joe Smith)")]
        [InlineData("email@-example.com")]
        [InlineData("email@example..com")]
        public void IsNotValidEmailAddress(string input)
        {
            var emailAttribute = new Forms.EmailAttribute();

            emailAttribute.IsValid(input).Should().BeFalse();
        }
    }
}

