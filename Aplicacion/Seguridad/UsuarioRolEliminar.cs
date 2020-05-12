using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Dominio;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Aplicacion.Seguridad
{
    public class UsuarioRolEliminar
    {
        public class Ejecuta : IRequest
        {
            public string UserName { get; set; }
            public string RolNombre { get; set; }
        }

        public class EjecutaValidador : AbstractValidator<Ejecuta>
        {
            public EjecutaValidador ()
            {
                RuleFor(x => x.UserName).NotEmpty();
                RuleFor(x => x.RolNombre).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<Ejecuta>
        {
            private readonly UserManager<Usuario> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public Manejador(UserManager<Usuario> userManager, RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            public async Task<Unit> Handle(Ejecuta request, CancellationToken cancellationToken)
            {
                var role = await _roleManager.FindByNameAsync(request.RolNombre);

                if (role == null)
                {
                    throw new ManejadorError.ManejadorExcepcion(HttpStatusCode.NotFound,
                    new { Mensaje = "El rol no existe" });
                }

                var usuarioIden = await _userManager.FindByNameAsync(request.UserName);

                if (usuarioIden == null)
                {
                    throw new ManejadorError.ManejadorExcepcion(HttpStatusCode.NotFound,
                    new { Mensaje = "El usuario no existe" });
                }

                var resultado = await _userManager.RemoveFromRoleAsync(usuarioIden, request.RolNombre);

                if (resultado.Succeeded)
                {
                    return Unit.Value;
                }

                throw new System.Exception("No se pudo eliminar el rol");
            }
        }
    }
}