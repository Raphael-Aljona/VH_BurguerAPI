using VHBurguer.Domains;

namespace VHBurguer.Interfaces
{
    public interface IPromocaoRepository
    {
        List<Promocao> Listar();
        Promocao obterPorId(int id);
        bool NomeExiste(string nome, int? id = null);
        void Adicionar (Promocao promocao);
        void Atualizar(Promocao promocao);
        void Remover(int id);
    }
}
