using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dominio.Interfaces.Genericos
{
    public interface IGenericos<T> where T : class
    {
        //Utilizar classes basicas para crud em qualuer API
        Task Adicionar(T Objeto);
        Task Atualizar(T Objeto);
        Task Excluir(T Objeto);
        Task<T> BuscaPorId(int Id);
        Task<List<T>> Listar(); 
    }
}
