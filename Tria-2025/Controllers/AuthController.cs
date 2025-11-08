using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tria_2025.Interface;
using Tria_2025.Models;

namespace SafeScribe_cp05.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        // ----------------------------------------------------------------------
        // ENDPOINT 1: REGISTRAR
        // ----------------------------------------------------------------------
        [HttpPost("registrar")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registrar([FromBody] FuncionarioRegistroDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var success = await _tokenService.RegisterUserAsync(user.Nome, user.Email, user.Senha, user.Cargo);

            if (!success)
                return BadRequest(new { Message = "E-mail já está em uso." });

            return StatusCode(StatusCodes.Status201Created, new { Message = "Usuário registrado com sucesso." });
        }

        // ----------------------------------------------------------------------
        // ENDPOINT 2: LOGIN
        // ----------------------------------------------------------------------
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] FuncionarioLoginDTO user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = await _tokenService.LoginAndGenerateTokenAsync(user.Email, user.Password);

            if (string.IsNullOrEmpty(token))
                return Unauthorized(new { Message = "Credenciais inválidas." });

            return Ok(new { Token = token });
        }



    }
}
