using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.Interface.Genericos
{
    public interface IGenericaAplicacao<T> where T : class
    {
        //Utilizar classes basicas para crud em qualuer API
        Task Adicionar(T Objeto);
        Task Atualizar(T Objeto);
        Task Excluir(T Objeto);
        Task<T> BuscaPorId(int Id);
        Task<List<T>> Listar();
    }
}
