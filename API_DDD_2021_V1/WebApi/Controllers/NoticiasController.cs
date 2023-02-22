using Aplicacao.Interface;
using Entidades.Entidades;
using Entidades.Notificacoes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Model;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoticiasController : ControllerBase
    {
        private readonly IAplicacaoNoticia _IAplicationNoticia;
        public NoticiasController(IAplicacaoNoticia IAplicationNoticia)
        {
            _IAplicationNoticia = IAplicationNoticia;
        }

        [Authorize]
        [Produces("application/json")]
        [HttpPost("api/ListarNoticias")]
        public async Task<List<Noticia>> ListarNoticias()
        {
            return await _IAplicationNoticia.ListarNoticiasAtivas();
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPost("api/AdicionarNoticia")]
        public async Task<List<Notifica>> AdicionaNoticia(NoticiaModel noticia)
        {
            var noticiaNova = new Noticia();
            noticiaNova.Titulo = noticia.Titulo;
            noticiaNova.Informacao = noticia.informacao;
            noticiaNova.Ativo = true;
            noticiaNova.UserId  = await RetornarIdUsuarioLogado();
            await _IAplicationNoticia.AdicionarNoticia(noticiaNova);
            return noticiaNova.Notificacoes;
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPost("api/AtualizarNoticia")]
        public async Task<List<Notifica>> AtualizarNoticia(NoticiaModel noticia)
        {
            var noticiaJaCadastrada = await _IAplicationNoticia.BuscaPorId(noticia.IdNoticia);
            noticiaJaCadastrada.Titulo = noticia.Titulo;
            noticiaJaCadastrada.Informacao = noticia.informacao;
            noticiaJaCadastrada.UserId = await RetornarIdUsuarioLogado();
            await _IAplicationNoticia.AtualizarNoticia(noticiaJaCadastrada);
            return noticiaJaCadastrada.Notificacoes;
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPost("api/BuscarPorId")]
        public async Task<Noticia> BuscarPorId(NoticiaModel noticia)
        {
            var noticiaCadastrada = await _IAplicationNoticia.BuscaPorId(noticia.IdNoticia);
            return noticiaCadastrada;
        }
        [Authorize]
        [Produces("application/json")]
        [HttpPost("api/ExcluirNoticia")]
        public async Task<List<Notifica>> ExcluirNoticia(NoticiaModel noticia)
        {
            var noticiaCadastrada = await _IAplicationNoticia.BuscaPorId(noticia.IdNoticia);
            await _IAplicationNoticia.Excluir(noticiaCadastrada);
            return noticiaCadastrada.Notificacoes;
        }

        private async Task<string> RetornarIdUsuarioLogado()
        {
            if( User!= null)
            {
                var idusuario = User.FindFirst("idUsuario");
                return idusuario.Value;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
