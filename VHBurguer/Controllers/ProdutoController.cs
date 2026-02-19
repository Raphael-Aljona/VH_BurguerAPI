using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using VHBurguer.Applications.Services;
using VHBurguer.DTOs.ProdutoDto;
using VHBurguer.Exceptions;

namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly ProdutoService _service;
        public ProdutoController(ProdutoService produtoService)
        {
            _service = produtoService;
        }

        private int ObterUsuarioIdLogado()
        {
            // busca no token o valor armazenado como id do usuário
            string? idTexto = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(idTexto))
            {
                throw new DomainException("Usuário não autenticado");
            }

            // Converte o ID que veio como texto para inteiro
            // No banco o usuárioId é int e não string
            return int.Parse(idTexto);
        }

        [HttpGet("{id}/imagem")]
        public ActionResult ObterImagem(int id)
        {
            try
            {
                var imagem = _service.ObterImagem(id);
                return File(imagem, "image/jpeg");
            } catch (DomainException ex)
            {
                return StatusCode(404, ex.Message);
            }
        }

        [HttpGet]
        public ActionResult<List<LerProdutoDto>> Listar()
        {
            List<LerProdutoDto> produtos = _service.Listar();

            return StatusCode(200, produtos);
        }

        [HttpGet("{id}")]
        public ActionResult<LerProdutoDto> ObterPorId(int id)
        {
            LerProdutoDto produtoDto = _service.ObterPorId(id);

            return StatusCode(200, produtoDto);
        }
    }
}
