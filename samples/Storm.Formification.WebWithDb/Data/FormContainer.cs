using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Storm.Formification.WebWithDb.Data
{
    public class FormContainer
    {
        public Guid Id { get; set; }

        public IdentityUser? User { get; set; }

        public string DocumentId { get; set; } = Guid.NewGuid().ToString();

        public string SecretId { get; set; } = Guid.NewGuid().ToString();

        public Type? FormType { get; set; }

        public FormContainer(IdentityUser user)
        {
            User = user;
        }

        protected FormContainer()
        {

        }
    }

    public class FormContainerEntityConfiguration : IEntityTypeConfiguration<FormContainer>
    {
        public void Configure(EntityTypeBuilder<FormContainer> builder)
        {
            var typeConverter = new ValueConverter<Type?, string>(
                v => v == null ? string.Empty : v.AssemblyQualifiedName,
                v => Type.GetType(v, false, true));

            builder.HasKey(p => p.Id);
            builder.Property(p => p.FormType).HasConversion(typeConverter).IsRequired();
        }
    }
}
