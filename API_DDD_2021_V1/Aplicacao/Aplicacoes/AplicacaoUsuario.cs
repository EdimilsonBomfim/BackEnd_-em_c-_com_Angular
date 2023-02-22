using Aplicacao.Interface;
using Dominio.Interfaces;
using System.Threading.Tasks;

namespace Aplicacao.Aplicacoes
{
    public class AplicacaoUsuario : IAplicacaoUsuario
    {
        IUsuario _IUsuario;
        public AplicacaoUsuario(IUsuario IUsuario)
        {
            _IUsuario = IUsuario;
        }
        public async Task<bool> AdicionaUsuario(string email, string senha, int Idade, string celular)
        {
            return await _IUsuario.AdicionaUsuario(email, senha, Idade, celular);
        }

        public async Task<bool> ExisteUsuario(string email, string senha)
        {
            return await _IUsuario.ExisteUsuario(email, senha);
        }

        public async Task<string> RetornaIdUsuario(string email)
        {
            return await _IUsuario.RetornaIdUsuario(email);
        }
    }
}
