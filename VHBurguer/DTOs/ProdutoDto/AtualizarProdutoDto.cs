namespace VHBurguer.DTOs.ProdutoDto
{
    public class AtualizarProdutoDto
    {
        public string Nome { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public decimal Preco { get; set; }
        public IFormFile Imagem { get; set; } = null!;
        public List<int> CategoriaIds { get; set; } = new();
        public bool? StatusProduto { get; set;  }
    }
}
