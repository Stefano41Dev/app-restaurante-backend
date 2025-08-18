using app_restaurante_backend.Data;
using app_restaurante_backend.Models.DTOs.Orden;
using app_restaurante_backend.Models.Entidades;
using app_restaurante_backend.Models.Enums.Ordenes;
using app_restaurante_backend.Service.Interfaces;
using EntityFrameworkPaginateCore;

namespace app_restaurante_backend.Service.Implementations
{
    public class OrdenService : IOrdenService
    {
        private readonly DbRestauranteContext _context;

        public OrdenService(DbRestauranteContext context) { 
            _context = context;
        }
        
        public OrdenResponseDto ActualizarEstado(OrdenEstadoRequestDto requestDto)
        {
            throw new NotImplementedException();
        }

        public OrdenResponseDto CrearOrden(OrdenRequestDto requestDto)
        {
            Ordene orden = new Ordene() {
                CodigoOrden = GenerarCodigo(),
                MesaId = requestDto.MesaId,
                Estado = EstadoOrden.PENDIENTE,
                FechaCreacion = DateTime.Now,
                HoraCreacion= DateTime.Now.TimeOfDay,
            };
            requestDto.Detalles.ForEach(d =>
            {
                ItemsMenu item = _context.ItemsMenus.Find(d.PlatoId)!;
                DetalleOrdene detalle = new()
                {
                    OrdenId = orden.Id,
                    PlatoId = d.PlatoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = item.Precio,
                    Subtotal = d.Cantidad * item.Precio,
                   
                };
            });
            
            throw new NotImplementedException();
        }

        public Page<OrdenResponseDto> ListaOrdenes()
        {
            throw new NotImplementedException();
        }

        public OrdenResponseDto ObtenerOrden(long id)
        {
            throw new NotImplementedException();
        }

        public string GenerarCodigo()
        {
            long ultimoId = _context.Ordenes
                           .OrderByDescending(o => o.Id)
                           .Select(o => o.Id)
                           .FirstOrDefault();

            long numero = ultimoId + 1;

            return $"COD-{numero:D7}";
        }
       

    }
}
