using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Uttt.Micro.Libro.Aplicacion;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace Uttt.Micro.Libro.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LibroMaterialController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LibroMaterialController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Unit>> Crear(Nuevo.Ejecuta data)
        {
            return await _mediator.Send(data);
        }

        [HttpGet]
        public async Task<ActionResult<List<LibroMaterialDto>>> GetLibros()
        {
            return await _mediator.Send(new Consulta.Ejecuta());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<LibroMaterialDto>> GetLibroUnico( Guid id)
        {
            return await _mediator.Send(new ConsultaFiltro.LibroUnico
            {
                LibroId = id
            });
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> Actualizar(Guid id, Actualizar.ActualizarLibro data)
        {
            data.LibroId = id;
            return await _mediator.Send(data);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> Eliminar(Guid id)
        {
            return await _mediator.Send(new Eliminar.Ejecuta { LibroId = id });
        }

        [HttpGet("instancia")]
        public IActionResult ObtenerInstancia()
        {
            var hostname = System.Net.Dns.GetHostName();
            return Ok($"📦 Respuesta desde instancia: {hostname}");
        }

        [HttpGet("protegido")]
        public IActionResult Protegido()
        {
            return Ok("Tienes acceso con un token válido.");
        }


    }
}
