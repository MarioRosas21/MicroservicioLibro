using FluentValidation;
using MediatR;
using Uttt.Micro.Libro.Persistencia;

namespace Uttt.Micro.Libro.Aplicacion
{
    public class Eliminar
    {
        public class Ejecuta : IRequest<Unit>
        {
            public Guid LibroId { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<Ejecuta>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.LibroId).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta, Unit>
        {
            private readonly ContextoLibreria _contexto;

            public Manejador(ContextoLibreria contexto)
            {
                _contexto = contexto;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var libro = await _contexto.LibreriasMateriales.FindAsync(request.LibroId);

                if (libro == null)
                {
                    throw new Exception("Libro no encontrado");
                }

                _contexto.LibreriasMateriales.Remove(libro);

                var resultado = await _contexto.SaveChangesAsync();

                if (resultado > 0)
                {
                    return Unit.Value;
                }

                throw new Exception("No se pudo eliminar el libro");
            }
        }
    }
}
