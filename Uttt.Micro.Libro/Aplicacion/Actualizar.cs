using FluentValidation;
using MediatR;
using Uttt.Micro.Libro.Persistencia;

namespace Uttt.Micro.Libro.Aplicacion
{
    public class Actualizar
    {
        public class ActualizarLibro : IRequest<Unit>
        {
            public Guid LibroId { get; set; }
            public string Titulo { get; set; }
            public DateTime? FechaPublicacion { get; set; }
            public Guid? AutorLibro { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<ActualizarLibro>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.LibroId).NotEmpty();
                RuleFor(x => x.Titulo).NotEmpty();
                RuleFor(x => x.FechaPublicacion).NotEmpty();
                RuleFor(x => x.AutorLibro).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<ActualizarLibro, Unit>
        {
            private readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(ActualizarLibro request, CancellationToken cancellationToken)
            {
                var libro = await _contexto.LibreriasMateriales.FindAsync(request.LibroId);

                if (libro == null)
                {
                    throw new Exception("Libro no encontrado");
                }

                libro.Titulo = request.Titulo ?? libro.Titulo;
                libro.FechaPublicacion = request.FechaPublicacion ?? libro.FechaPublicacion;
                libro.AutorLibro = request.AutorLibro ?? libro.AutorLibro;

                var resultado = await _contexto.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo actualizar el libro");
            }
        }
    }
}
