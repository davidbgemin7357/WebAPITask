using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebAPITask.Models;
using Task = WebAPITask.Models.Task;


namespace WebAPITask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly MainDbContext _context;

        public TaskController(MainDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("listarPorEmpleado")]
        public async Task<ActionResult<IEnumerable<object>>> ListarTareaPorEmpleado(int userId, bool isDeleted)
        {
            try
            {
                var tareas = await _context.Database
                    .SqlQueryRaw<Task>("EXEC GetTasksByUser @p0, @p1", userId, isDeleted)
                    .ToListAsync();

                var resultado = tareas.Select(u => new
                {
                    n_id = u.n_id,
                    n_employee_id = u.n_employee_id,
                    s_name = u.s_name,
                    s_lastname = u.s_lastname,
                    s_title = u.s_title,
                    s_description = u.s_description,
                    d_created = u.d_created,
                    d_updated = u.d_updated,
                    b_status = u.b_status
                });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Ok(new object[] { });
            }
        }

        [Authorize]
        [HttpGet("buscarportexto")]
        public async Task<ActionResult<IEnumerable<object>>> ListarTareaPorTexto(string text, int userId)
        {
            try
            {
                var tareas = await _context.Database
                    .SqlQueryRaw<Task>("EXEC sp_getTaskByText @p0, @p1", text, userId)
                    .ToListAsync();

                var resultado = tareas.Select(u => new
                {
                    n_id = u.n_id,
                    n_employee_id = u.n_employee_id,
                    s_name = u.s_name,
                    s_lastname = u.s_lastname,
                    s_title = u.s_title,
                    s_description = u.s_description,
                    d_created = u.d_created,
                    d_updated = u.d_updated,
                    b_status = u.b_status
                });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Ok(new object[] { });
            }
        }

        [Authorize]
        [HttpPost("eliminar/{taskId}")]
        public async Task<IActionResult> EliminarTarea(int taskId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC sp_soft_delete_task @p0", taskId);
                return Ok(new { message = "Tarea eliminada con éxito." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Ocurrió un error al procesar la solicitud.", error = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("listar")]
        public async Task<ActionResult<IEnumerable<object>>> ListarTarea(bool isAdmin, int userId)
        {
            try
            {
                var tareas = await _context.Database
                    .SqlQueryRaw<Task>("EXEC sp_get_alltaks @p0, @p1", isAdmin, userId)
                    .ToListAsync();

                var resultado = tareas.Select(u => new
                {
                    n_id = u.n_id,
                    n_employee_id = u.n_employee_id,
                    s_name = u.s_name,
                    s_lastname = u.s_lastname,
                    s_title = u.s_title,
                    s_description = u.s_description,
                    d_created = u.d_created,
                    d_updated = u.d_updated,
                    b_status = u.b_status
                });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return Ok(new object[] { });
            }
        }


        [Authorize]
        [HttpPost("gestionar")]
        public async Task<IActionResult> GestionarTarea([FromBody] TaskDto taskDto)
        {
            if (taskDto == null)
            {
                return BadRequest(new { success = false, message = "Los datos de la tarea no pueden ser nulos." });
            }

            try
            {
                var result = await _context.Set<SpResult>()
                    .FromSqlRaw("EXEC sp_manage_task @n_id, @n_user_id, @n_employee_id, @s_title, @s_description",
                        new SqlParameter("n_id", taskDto.n_id),
                        new SqlParameter("n_user_id", taskDto.n_user_id),
                        new SqlParameter("n_employee_id", taskDto.n_employee_id),
                        new SqlParameter("s_title", taskDto.s_title),
                        new SqlParameter("s_description", taskDto.s_description))
                    .ToListAsync();

                if (result.Any())
                {
                    var spMessage = result.First().Message;
                    if (spMessage == "WrongRequest")
                    {
                        return StatusCode(StatusCodes.Status403Forbidden, new { success = false, message = "No tiene permiso para realizar esta acción" });
                    }
                }

                string message = (taskDto.n_id == 0) ? "Tarea creada con éxito." : "Tarea actualizada con éxito.";
                return StatusCode(StatusCodes.Status201Created, new { success = true, message = message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "Ocurrió un error al gestionar la tarea.", error = ex.Message });
            }
        }
    }
}


public class SpResult
{
    public string Message { get; set; }
}