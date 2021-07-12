using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Api.Core.Models
{
    public sealed class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; private set; }

        [EmailAddress] [NotNull] public string Email { get; set; }

        [PasswordPropertyText] [NotNull] public string Password { get; set; }

        [Required]
        [StringLengthAttribute(60, MinimumLength = 2)]
        public string Name { get; set; }
    }
}