using Microsoft.EntityFrameworkCore;
using VHBurguer.Contexts;
using VHBurguer.Domains;
using VHBurguer.Interfaces;

namespace VHBurguer.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly VH_BurguerContext _context;
        public ProdutoRepository(VH_BurguerContext context)
        {
            _context = context;
        }

        public List<Produto> Listar()
        {
            List<Produto> produtos = _context.Produto.Include(produto => produto.Categoria).Include(usuario => usuario.Usuario).ToList();
            return produtos;
        }

        public Produto ObterPorId(int id)
        {
            Produto? produto = _context.Produto.Include(produtoDb => produtoDb.Categoria).Include(produtoDb => produtoDb.Usuario).FirstOrDefault(produtoDb => produtoDb.ProdutoID == id);
            return produto;
        }

        public bool NomeExiste(string nome, int? id = null)
        {
            // AsQueryable -> Monta a consulta para executar passo a passo
            // Monta a consulta na tabela produto
            // Não executa nada no banco ainda
            var produtoConsultado = _context.Produto.AsQueryable();

            if (id.HasValue)
            {
                produtoConsultado = produtoConsultado.Where(produto => produto.ProdutoID != id.Value);
            }

            return produtoConsultado.Any(produto => produto.Nome == nome);
        }

        public byte[] ObterImagem(int id)
        {
            var produto = _context.Produto.Where(produtoDb => produtoDb.ProdutoID == id).Select(produtoDb => produtoDb.Imagem).FirstOrDefault();

            return produto;
        }

        public void Adicionar(Produto produto, List<int> categoriaIds)
        {
            List<Categoria> categorias = _context.Categoria.Where(categoria => categoriaIds.Contains(categoria.CategoriaID)).ToList();

            produto.Categoria = categorias;

            _context.Produto.Add(produto);
            _context.SaveChanges();
        }

        public void Atualizar(Produto produto, List<int> categoriaIds)
        {
            Produto? produtoBanco = _context.Produto.Include(produto => produto.Categoria).FirstOrDefault(produtoAu => produtoAu.ProdutoID == produto.ProdutoID);

            if (produtoBanco == null) return;

            produtoBanco.Nome = produto.Nome;
            produtoBanco.Descricao = produto.Descricao;
            produtoBanco.Preco = produto.Preco;

            if (produto.Imagem != null && produto.Imagem.Length > 0)
            {
                produtoBanco.Imagem = produto.Imagem;
            }

            if (produto.StatusProduto.HasValue)
            {
                produtoBanco.StatusProduto = produto.StatusProduto;
            }

            var categorias = _context.Categoria.Where(categoria => categoriaIds.Contains(categoria.CategoriaID)).ToList();

            produtoBanco.Categoria.Clear();

            foreach (var categoria in categorias)
            {
                produtoBanco.Categoria.Add(categoria);
            }
            
            _context.SaveChanges();   
        }


        public void Remover(int id)
        {
            Produto? produto = _context.Produto.FirstOrDefault(produto => produto.ProdutoID == id);

            if (produto == null) return;

            _context.Produto.Remove(produto);
            _context.SaveChanges();
        }
    }
}
