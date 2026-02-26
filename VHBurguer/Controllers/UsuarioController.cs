using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VHBurguer.Applications.Services;
using VHBurguer.DTOs.UsuarioDto;
using VHBurguer.Exceptions;

namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;

        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<LerUsuarioDto>> Listar()
        {
            List<LerUsuarioDto> usuarios = _service.Listar();

            return Ok(usuarios); //Status code
        }

        [HttpGet("{id}")]
        public ActionResult<LerUsuarioDto> ObterPorId(int id)
        {
            try
            {
                LerUsuarioDto usuarioDto = _service.ObterPorId(id);
                return Ok(usuarioDto);

            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpGet("email/{email}")]
        public ActionResult<LerUsuarioDto> ObterPorEmail(string email)
        {
            try
            {
                LerUsuarioDto usuarioDto = _service.ObterPorEmail(email);
                return Ok(usuarioDto);
            }
            catch (DomainException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult<LerUsuarioDto> Adicionar(CriarUsuarioDto criarUsuarioDto)
        {
            try
            {
                LerUsuarioDto usuarioDto = _service.Adicionar(criarUsuarioDto);
                if (usuarioDto == null) return NotFound();

                return StatusCode(201, usuarioDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<LerUsuarioDto> Atualizar(int id, CriarUsuarioDto criarUsuarioDto)
        {
            try
            {
                LerUsuarioDto usuarioAtualizado = _service.Atualizar(id, criarUsuarioDto);

                return StatusCode(200, usuarioAtualizado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // Remove os dados
        // no nosso banco o delete vai inativar o usuário
        // por conta da trigger (processo chamado de soft delete)
        [HttpDelete("{id}")]
        public IActionResult Remover(int id)
        {
            try
            {
                _service.Remover(id);
                return StatusCode(204, id);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
