using Entidades.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entidades.Entidades
{
    public class ApplicationUser : IdentityUser
    {
        [Column("USR_IDADE")]
        public int Idade { get; set; }
        [Column("USR_CELULAR")]
        public string Celular { get; set; }
        [Column("USR_TIPO")]
        public TipoUsuario ? Tipo { get; set; }    
    }
}
