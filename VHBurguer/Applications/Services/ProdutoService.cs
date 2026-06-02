using VHBurguer.Applications.ContentSafety;
using VHBurguer.Applications.Conversions;
using VHBurguer.Applications.Rules;
using VHBurguer.Domains;
using VHBurguer.DTOs.ProdutoDto;
using VHBurguer.Exceptions;
using VHBurguer.Interfaces;

namespace VHBurguer.Applications.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _repository;
        private readonly IContentSafetyRepository _contentSafetyRepository;

        public ProdutoService(IProdutoRepository repository, IContentSafetyRepository contentSafetyRepository)
        {
            _repository = repository;
            _contentSafetyRepository = contentSafetyRepository;
        }

        public List<LerProdutoDto> Listar()
        {
            List<Produto> produtos = _repository.Listar();

            List<LerProdutoDto> produtosDto = produtos.Select(ProdutoParaDto.ConverterParaDto).ToList();

            return produtosDto;
        }

        public LerProdutoDto ObterPorId(int id)
        {
            Produto produto = _repository.ObterPorId(id);

            if (produto == null)
            {
                throw new DomainException("Produto não encontrado");
            }

            return ProdutoParaDto.ConverterParaDto(produto);
        }

        private async Task ValidarConteudoProdutoAsync(string nome, string descricao)
        {
            string textoParaValidar = $@"
                Nome do produto: {nome}
                Descrição do produto: {descricao}";

            var resultado = await _contentSafetyRepository.ValidarConteudo(textoParaValidar);

            if (!resultado.aprovado)
            {
                throw new DomainException(resultado.msg);
            }
        }

        private static void ValidarCadastro(CriarProdutoDto produtoDto)
        {
            // Código feito assim para testes, mudar futuramente para tratar os erros de uma maneira melhor
            if (string.IsNullOrWhiteSpace(produtoDto.Nome) 
                || string.IsNullOrWhiteSpace(produtoDto.Descricao) ||
                produtoDto.Imagem == null || produtoDto.Imagem.Length == 0 ||
                produtoDto.CategoriaIds == null || produtoDto.CategoriaIds.Count == 0)
            {
                throw new DomainException("Todas as informações devem ser adicionadas");
            }

            if (produtoDto.Preco < 0)
            {
                throw new DomainException("Preço deve ser maior que 0");
            }
        }

        public byte[] ObterImagem(int id)
        {
            byte[] imagem = _repository.ObterImagem(id);

            if(imagem == null || imagem.Length == 0)
            {
                throw new DomainException("Imagem não encontrada");
            }

            return imagem;
        }

        public async Task<LerProdutoDto> Adicionar(CriarProdutoDto produtoDto, int id)
        {
            ValidarCadastro(produtoDto);
            await ValidarConteudoProdutoAsync(produtoDto.Nome, produtoDto.Descricao);

            if (_repository.NomeExiste(produtoDto.Nome))
            {
                throw new DomainException("Produto já existente");
            }

            Produto produto = new Produto
            {
                Nome = produtoDto.Nome,
                Preco = produtoDto.Preco,
                Descricao = produtoDto.Descricao,
                Imagem = ImagemParaBytes.ConverterImagem(produtoDto.Imagem),
                StatusProduto = true,
                UsuarioID = id
            };

            _repository.Adicionar(produto, produtoDto.CategoriaIds);

            return ProdutoParaDto.ConverterParaDto(produto);
        }

        public LerProdutoDto Atualizar(int produtoId, AtualizarProdutoDto atualizarProdutoDto)
        {
            HorarioAlteracaoProduto.ValidarHorario();

            Produto produto = _repository.ObterPorId(produtoId);

            if (produto == null) throw new DomainException("Produto não encontrado");

            if (_repository.NomeExiste(atualizarProdutoDto.Nome, id: produtoId))
            {
                throw new DomainException("Já existe outro produto com esse nome");
            }

            if (atualizarProdutoDto.CategoriaIds.Count == 0 || atualizarProdutoDto.CategoriaIds == null) {
                throw new DomainException("Produto deve ter ao menos uma categoria");
            }

            if(atualizarProdutoDto.Preco < 0)
            {
                throw new DomainException("Preço deve ser maior que 0");
            }

            produto.Nome = atualizarProdutoDto.Nome;
            produto.Preco = atualizarProdutoDto.Preco;
            produto.Descricao = atualizarProdutoDto.Descricao;

            if(atualizarProdutoDto != null && atualizarProdutoDto.Imagem.Length > 0)
            {
                produto.Imagem = ImagemParaBytes.ConverterImagem(atualizarProdutoDto.Imagem);
            }

            if (atualizarProdutoDto.StatusProduto.HasValue)
            {
                produto.StatusProduto = atualizarProdutoDto.StatusProduto.Value; 
            }

            _repository.Atualizar(produto, atualizarProdutoDto.CategoriaIds);

            return ProdutoParaDto.ConverterParaDto(produto);
        }

        public void Remover(int id)
        {
            HorarioAlteracaoProduto.ValidarHorario();

            Produto produto = _repository.ObterPorId(id);

            if (produto == null) throw new DomainException("Produto não encontrado");

            _repository.Remover(id);
        }
    }
}
