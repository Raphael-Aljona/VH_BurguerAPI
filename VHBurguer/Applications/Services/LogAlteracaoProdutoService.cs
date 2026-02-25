using VHBurguer.Domains;
using VHBurguer.DTOs.LogProdutoDto;
using VHBurguer.Interfaces;

namespace VHBurguer.Applications.Services
{
    public class LogAlteracaoProdutoService
    {
        private readonly ILogAlteracaoProdutoRepository _repository;

        public LogAlteracaoProdutoService(ILogAlteracaoProdutoRepository repository)
        {
            _repository = repository;
        }

        public List<LerLogProdutoDto> Listar()
        {
            List<Log_AlteracaoProduto> logs = _repository.Listar();

            List<LerLogProdutoDto> listaLogProduto = logs.Select(logs => new LerLogProdutoDto
            {
                LogID = logs.Log_AlteracaoProdutoID,
                ProdutoID = logs.ProdutoID,
                DataAlteracao = logs.DataAlteracao,
                NomeAnterior = logs.NomeAnterior,
                PrecoAnterior = logs.PrecoAnterior
            }).ToList();

            return listaLogProduto;
        }

        public List<LerLogProdutoDto> ListarPorProduto(int produtoId)
        {
            List<Log_AlteracaoProduto> logs = _repository.ListarPorProduto(produtoId);

            List<LerLogProdutoDto> listaLogProduto = logs.Select(log => new LerLogProdutoDto
            {
                DataAlteracao = log.DataAlteracao,
                ProdutoID = log.ProdutoID,
                LogID = log.Log_AlteracaoProdutoID,
                NomeAnterior = log.NomeAnterior,
                PrecoAnterior = log.PrecoAnterior
            }).ToList();

            return listaLogProduto;
        }
    }
}
