using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VHBurguer.Applications.Services;
using VHBurguer.DTOs.PromocaoDto;
using VHBurguer.Exceptions;

namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocaoController : ControllerBase
    {
        private readonly PromocaoService _service;

        public PromocaoController(PromocaoService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerPromocaoDto>> Listar()
        {
            List<LerPromocaoDto> promocaoDtos = _service.Listar();

            return StatusCode(200, promocaoDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<LerPromocaoDto> ObterPorId(int id)
        {
            try
            {
                LerPromocaoDto promocaoDto = _service.ObterPorId(id);

                return StatusCode(200, promocaoDto);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Adicionar(CriarPromocaoDto criarPromocaoDto)
        {
            try
            {
                _service.Adicionar(criarPromocaoDto);

                return StatusCode(201, criarPromocaoDto);
            }
            catch (DomainException ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult Atualizar(int id, CriarPromocaoDto atualizar)
        {
            try
            {
                _service.Atualizar(id, atualizar);
                return StatusCode(204);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Remover(int id)
        {
            try
            {
                _service.Remover(id);
                return StatusCode(204);
            }catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
