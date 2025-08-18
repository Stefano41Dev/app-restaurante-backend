using app_restaurante_backend.Data;
using app_restaurante_backend.Models.DTOs.Orden;
using app_restaurante_backend.Models.Entidades;
using app_restaurante_backend.Models.Enums.Ordenes;
using app_restaurante_backend.Service.Interfaces;
using app_restaurante_backend.Models.DTOs.DetalleOrden;
using app_restaurante_backend.Models.DTOs.Categoria;
using EntityFrameworkPaginateCore;
using Microsoft.EntityFrameworkCore;

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
            if (requestDto.Detalles == null || requestDto.Detalles.Count == 0)
            {
                throw new ArgumentException("La lista de detalles no puede estar vacía.");
            }
            if(requestDto.MesaId <= 0)
            {
                throw new ArgumentException("El ID de la mesa debe ser mayor que cero.");
            }
            if (requestDto.Detalles.Any(d => d.PlatoId <= 0 || d.Cantidad <= 0))
            {
                throw new ArgumentException("Los IDs de los platos y las cantidades deben ser mayores que cero.");
            }
            if (_context.Mesas.Find(requestDto.MesaId) == null)
            {
                throw new ArgumentException("La mesa especificada no existe.");
            }
            if (requestDto.Detalles.Any(d => _context.ItemsMenus.Find(d.PlatoId) == null))
            {
                throw new ArgumentException("Uno o más platos especificados no existen.");
            }
            if( _context.Mesas
                .Where(m => m.Id == requestDto.MesaId && m.Estado == EstadoMesa.OCUPADO)
                .Any())
            {
                throw new InvalidOperationException("La mesa ya está ocupada.");
            }
            Ordene orden = new Ordene()
            {
                CodigoOrden = GenerarCodigo(),
                MesaId = requestDto.MesaId,
                Estado = EstadoOrden.PENDIENTE,
                FechaCreacion = DateTime.Now,
                HoraCreacion = DateTime.Now.TimeOfDay,
                MontoSubtotal = 0, 
                MontoTotal = 0,
                Activo = true,
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
                };
                detalle.Subtotal = detalle.PrecioUnitario * detalle.Cantidad;
                detalle.Total = detalle.Subtotal + (detalle.Subtotal * detalle.Igv);

                orden.MontoSubtotal += detalle.Subtotal;
                orden.MontoTotal += detalle.Total;

                orden.DetalleOrdenes.Add(detalle);
            });
            _context.Mesas.Where(m => m.Id == requestDto.MesaId)
                .ExecuteUpdate(m => m.SetProperty(m => m.Estado, EstadoMesa.OCUPADO));
            _context.Ordenes.Add(orden);
            if (_context.SaveChanges() > 0)
            {
                var ordenConDetalles = _context.Ordenes
                    .Include(o=>o.DetalleOrdenes)
                    .ThenInclude(d => d.Plato)
                    .ThenInclude(p => p.Categoria)
                    .Where(o => o.Id == orden.Id)
                    .FirstOrDefault();
                if (ordenConDetalles == null)
                {
                    throw new Exception("Orden no encontrada después de guardarla.");
                }

                return new OrdenResponseDto(
                     ordenConDetalles.Id,
                     ordenConDetalles.CodigoOrden,
                     ordenConDetalles.MesaId,
                     ordenConDetalles.Estado.ToString(),
                     ordenConDetalles.FechaCreacion.Value,
                     ordenConDetalles.HoraCreacion.Value,
                     ordenConDetalles.MontoSubtotal ?? 0,
                     ordenConDetalles.MontoTotal ?? 0,
                     ordenConDetalles.DetalleOrdenes.Select(d => new DetalleOrdenResponseDto(
                         d.Id,
                         d.Plato.Nombre,
                         new CategoriaResponseDTO(
                             d.Plato.CategoriaId,
                             d.Plato.Categoria.Nombre,
                             d.Plato.Categoria.Descripcion,
                             d.Plato.Categoria.PrecioMinimo ?? 0
                         ),
                         d.Cantidad ?? 0,
                         d.PrecioUnitario ?? 0,
                         d.Igv ?? 0,
                         d.Subtotal ?? 0,
                         d.Total ?? 0
                     )).ToList()
                 );
            }
            else
            {
                throw new Exception("No se pudo crear la orden");
            }
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
