using Aplicacao.Interface;
using Dominio.Interfaces;
using Dominio.Interfaces.InterfaceServicos;
using Entidades.Entidades;
using Entidades.Notificacoes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aplicacao.Aplicacoes
{
    public class AplicacaoNoticia : IAplicacaoNoticia
    {
        INoticia _INoticia;
        IServicoNoticia _ServicoNoticia;
        public AplicacaoNoticia(INoticia INoticia, IServicoNoticia IServicoNoticia)
        {
            _INoticia = INoticia;
            _ServicoNoticia = IServicoNoticia;
        }
        #region Metodos customizaveis    
        public async Task AdicionarNoticia(Noticia noticia)
        {
            await _ServicoNoticia.AdicionarNoticia(noticia);    
        }
        public async Task AtualizarNoticia(Noticia noticia)
        {
            await _ServicoNoticia.AtualizarNoticia(noticia);

        }
        public async Task<List<Noticia>> ListarNoticiasAtivas()
        {
            return await _ServicoNoticia.ListarNoticiasAtivas();

        }
        #endregion
        #region Metodos Genericos
        public async Task Adicionar(Noticia Objeto)
        {
            await _INoticia.Adicionar(Objeto);
        }
        public async Task Atualizar(Noticia Objeto)
        {
            await _INoticia.Atualizar(Objeto);
        }
        public async Task<Noticia> BuscaPorId(int Id)
        {
            return await _INoticia.BuscaPorId(Id);
        }
        public async Task Excluir(Noticia Objeto)
        {
            await _INoticia.Excluir(Objeto);
        }
        public async Task<List<Noticia>> Listar()
        {
            return await _INoticia.Listar();
        }
        #endregion

    }
}
