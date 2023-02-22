using Aplicacao.Interface;
using Entidades.Entidades;
using Entidades.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Model;
using WebApi.Token;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IAplicacaoUsuario _IAplicacaoUsuario;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
     
        public UsuarioController(IAplicacaoUsuario IAplicacaoUsuario,
                                 SignInManager<ApplicationUser> singnInManager,
                                 UserManager<ApplicationUser> userManager)
        {
            _IAplicacaoUsuario = IAplicacaoUsuario;
            _signInManager = singnInManager;
            _userManager = userManager;
        }
        [AllowAnonymous]
        [Produces("Aplicattion/json")]
        [HttpPost("/Api/CriarToken")]
        public async Task<IActionResult> CriarToken([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
                return Unauthorized();
            var resultado = await _IAplicacaoUsuario.ExisteUsuario(login.email, login.senha);
            if (resultado)
            {
                //var token = new TokenJWTBuilder()
                //    .AddSecurityKey(JwtSecurityKey.Create("Secret_key-12345678"))
                //    .AddSubject("Empresa - Arsitec ")
                //    .AddIssuer("Teste.Securiry.Bearer")
                //    .AddAudience("Teste.Securiry.Bearer")
                //    .AddExpiry(5)
                //    .Builder();
                //return Ok(token.value);
                var idUsuario = await _IAplicacaoUsuario.RetornaIdUsuario(login.email);
                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("Secret_key-12345678"))
                    .AddSubject("Empresa - Arsitec ")
                    .AddIssuer("Teste.Securiry.Bearer")
                    .AddAudience("Teste.Securiry.Bearer")
                    .AddClaim("idusuario", idUsuario)
                    .AddExpiry(5)
                    .Builder();

                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }
        }
        [AllowAnonymous]
        [Produces("Aplicattion/json")]
        [HttpPost("/Api/AdicionaUsuario")]
        public async Task<IActionResult> AdicionaUsuario([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
                return Ok("Dados do usuario esta incompleto");

            var resultado = await _IAplicacaoUsuario.AdicionaUsuario(login.email, login.senha, login.idade, login.celular);
            if (resultado)
                return Ok("Usuario adicionado com sucesso");
            else
            {
                return Ok("Erro ao adicionar usuario");
            }
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CriarTokenIdentity")]
        public async Task<IActionResult> CriarTokenIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
                return Unauthorized();

            var resultado = await
                _signInManager.PasswordSignInAsync(login.email, login.senha, false, lockoutOnFailure: false);

             if (resultado.Succeeded)
            {
                var idUsuario = await _IAplicacaoUsuario.RetornaIdUsuario(login.email);

                var token = new TokenJWTBuilder()
                     .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                 .AddSubject("Empresa - Canal Dev Net Core")
                 .AddIssuer("Teste.Securiry.Bearer")
                 .AddAudience("Teste.Securiry.Bearer")
                 .AddClaim("idUsuario", idUsuario)
                 .AddExpiry(60)
                 .Builder();

                return Ok(token.value);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [Produces("Aplicattion/json")]
        [HttpPost("/Api/AdicionaUsuarioIdentity")]
        public async Task<IActionResult> AdicionaUsuarioIdentity([FromBody] Login login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha))
                return Ok("Dados do usuario esta incompleto");
            var user = new ApplicationUser
            {
                UserName = login.email,
                Email = login.email,
                Celular = login.celular,
                Tipo = TipoUsuario.Comum,
                Idade = login.idade
            };
            var resultado = await _userManager.CreateAsync(user, login.senha);
            if (resultado.Errors.Any())
            {
                return Ok(resultado.Errors);
            }

            //Geração de confirmação de criação de login
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            //Retorno email
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var retornoEmail = await _userManager.ConfirmEmailAsync(user, code);

            if (retornoEmail.Succeeded)
            {
                return Ok("Usuario adicionado com sucesso");
            }
            else
            {
                return Ok("erro ao confirmar usuario");
            }
        }
    }
}
