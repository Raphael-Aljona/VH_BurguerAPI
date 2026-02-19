using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VHBurguer.Applications.Services;
using VHBurguer.Domains;
using VHBurguer.DTOs.AutenticacaoDto;
using VHBurguer.Exceptions;

namespace VHBurguer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private readonly AutenticacaoService _autenticacaoService;

        public AutenticacaoController(AutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("login")]
        public ActionResult Login(LoginDto loginDto)
        {
            try
            {
                var token = _autenticacaoService.Login(loginDto);

                return StatusCode(200, token);
            }
            catch (DomainException ex)
            {
                return StatusCode(400, ex.Message);
            }
        }
    }
}
