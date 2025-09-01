using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPITask.Models;

namespace WebAPITask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MainDbContext _context;
        private readonly IJwtService _jwtService;

        public UserController(MainDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;

        }

        [Authorize]
        [HttpGet("listar")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<UserListDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<IEnumerable<UserListDto>>> ListarUsuario()
        {
            try
            {
                var usuarios = await _context.Database
                    .SqlQueryRaw<User>("EXEC sp_list_all_users")
                    .ToListAsync();

                var resultado = usuarios.Select(u => new
                {
                    n_id = u.n_id,
                    s_name = u.s_name,
                    s_lastname = u.s_lastname,
                    s_username = u.s_username,
                });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error interno del servidor" });
            }
        }

        [HttpPost("login")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(object), 401)]
        [ProducesResponseType(typeof(object), 500)]
        public async Task<IActionResult> ObtenerToken([FromBody] LoginDto loginDto)
        {
            try
            {
                var usernameParam = new SqlParameter("@usernameParam", loginDto.username);
                var passwordParam = new SqlParameter("@passwordParam", loginDto.password);

                var result = await _context.UserResults
                    .FromSqlRaw("EXEC sp_login @usernameParam={0}, @passwordParam={1}",
                                usernameParam, passwordParam)
                    .ToListAsync();

                var user = result.FirstOrDefault();

                if (user == null || user.mensaje == "errorlogin")
                {
                    return Unauthorized(new { success = false, message = "El usuario no existe o los datos son incorrectos" });
                }

                var token = _jwtService.GenerateToken(new User
                {
                    n_id = user.n_id ?? 0,
                    s_name = user.s_name!,
                    s_lastname = user.s_lastname!,
                    s_username = user.s_username!,
                    s_description = user.s_description!
                });

                return Ok(new
                {
                    success = true,
                    message = "Login correcto",
                    token,
                    user
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error interno del servidor", error = ex.Message });
            }
        }
    }
}