using VHBurguer.Contexts;
using VHBurguer.Domains;
using VHBurguer.Interfaces;

namespace VHBurguer.Repositories
{
    public class LogAlteracaoProdutoRepository : ILogAlteracaoProdutoRepository
    {
        private readonly VH_BurguerContext _context;
        
        public LogAlteracaoProdutoRepository (VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Log_AlteracaoProduto> Listar()
        {
            List<Log_AlteracaoProduto> listaAlteracoes = _context.Log_AlteracaoProduto.OrderByDescending(l => l.DataAlteracao).ToList();

            return listaAlteracoes;
        }

        public List<Log_AlteracaoProduto> ListarPorProduto(int produtoId)
        {
            List<Log_AlteracaoProduto> alteracaoProduto = _context.Log_AlteracaoProduto.Where(l => l.ProdutoID == produtoId).OrderByDescending(l => l.DataAlteracao).ToList();

            return alteracaoProduto;
        }
    }
}
