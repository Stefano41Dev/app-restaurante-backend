using app_restaurante_backend.Data;
using app_restaurante_backend.Models.DTOs.Categoria;
using app_restaurante_backend.Models.DTOs.DetalleOrden;
using app_restaurante_backend.Models.DTOs.Orden;
using app_restaurante_backend.Models.Entidades;
using app_restaurante_backend.Models.Enums.Ordenes;
using app_restaurante_backend.Service.Interfaces;
using EntityFrameworkPaginateCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace app_restaurante_backend.Service.Implementations
{
    public class OrdenService : IOrdenService
    {
        private readonly DbRestauranteContext _context;

        public OrdenService(DbRestauranteContext context)
        {
            _context = context;
        }

        public OrdenResponseDto ActualizarEstado(long id, OrdenEstadoRequestDto requestDto)
        {
            var orden = _context.Ordenes
                .Include(o => o.Mesa)
                .Include(o => o.DetalleOrdenes)
                .ThenInclude(d => d.Plato)
                .FirstOrDefault(o => o.Id == id);

            if (orden == null)
                throw new Exception("No se encontró la orden.");

            if (!Enum.TryParse<EstadoOrden>(requestDto.EstadoOrden, true, out var nuevoEstado))
                throw new Exception("Estado de orden no válido.");


            if (orden.Estado == EstadoOrden.COMPLETADA || orden.Estado == EstadoOrden.CANCELADA)
                throw new Exception("No se puede modificar una orden finalizada o cancelada.");


            if (orden.Estado == nuevoEstado)
                throw new Exception("La orden ya se encuentra en ese estado.");

            orden.Estado = nuevoEstado;

            if (nuevoEstado == EstadoOrden.COMPLETADA || nuevoEstado == EstadoOrden.CANCELADA)
            {
                // Liberar la mesa
                if (orden.Mesa != null)
                {
                    orden.Mesa.Estado = EstadoMesa.LIBRE;
                    _context.Mesas.Update(orden.Mesa);
                }
                orden.Activo = false;
            }

            _context.Ordenes.Update(orden);
            _context.SaveChanges();

            return new OrdenResponseDto(
                orden.Id,
                orden.CodigoOrden ?? string.Empty,
                orden.MesaId,
                orden.Mesa.Numero,
                orden.Estado.ToString(),
                orden.FechaCreacion ?? DateTime.UtcNow,
                orden.HoraCreacion ?? TimeSpan.Zero,
                orden.MontoSubtotal ?? 0,
                orden.MontoTotal ?? 0,
                orden.DetalleOrdenes.Select(d => new DetalleOrdenResponseDto(
                    d.Id,
                    d.Plato.Nombre ?? string.Empty,
                    d.Cantidad ?? 0,
                    d.PrecioUnitario ?? 0,
                    d.Igv ?? 0,
                    d.Subtotal ?? 0,
                    d.Total ?? 0
                )).ToList()
            );
        }
        public OrdenResponseDto CrearOrden(OrdenRequestDto requestDto)
        {
            if (requestDto.Detalles == null || requestDto.Detalles.Count == 0)
            {
                throw new ArgumentException("La lista de detalles no puede estar vacía.");
            }
            if (requestDto.MesaId <= 0)
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
            if (_context.Mesas
                .Where(m => m.Id == requestDto.MesaId && m.Estado == EstadoMesa.OCUPADO)
                .Any())
            {
                throw new InvalidOperationException("La mesa ya está ocupada.");
            }

            Ordene orden = new()
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
                    .Include(o => o.DetalleOrdenes)
                    .ThenInclude(d => d.Plato)
                    .Where(o => o.Id == orden.Id)
                    .FirstOrDefault();
                if (ordenConDetalles == null)
                {
                    throw new Exception("Orden no encontrada después de guardarla.");
                }

                return new OrdenResponseDto(
                     ordenConDetalles.Id,
                     ordenConDetalles.CodigoOrden!,
                     ordenConDetalles.MesaId,
                     ordenConDetalles.Mesa.Numero,
                     ordenConDetalles.Estado.ToString()!,
                     ordenConDetalles.FechaCreacion!.Value,
                     ordenConDetalles.HoraCreacion!.Value,
                     ordenConDetalles.MontoSubtotal ?? 0,
                     ordenConDetalles.MontoTotal ?? 0,
                     ordenConDetalles.DetalleOrdenes.Select(d => new DetalleOrdenResponseDto(
                         d.Id,
                         d.Plato.Nombre!,
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
        public Page<OrdenResponseDto> ListaOrdenesDeHoy(int pageNumber, int pageSize)
        {
            var hoy = DateTime.Now.Date;

            var ordenes = _context.Ordenes
                .Include(o => o.DetalleOrdenes)
                .ThenInclude(d => d.Plato)
                .Where(o => o.FechaCreacion.HasValue && o.FechaCreacion.Value.Date == hoy)
                .Select(o => new OrdenResponseDto(
                    o.Id,
                    o.CodigoOrden ?? string.Empty,
                    o.MesaId,
                    o.Mesa.Numero,
                    o.Estado.ToString() ?? string.Empty,
                    o.FechaCreacion ?? DateTime.UtcNow,
                    o.HoraCreacion ?? TimeSpan.Zero,
                    o.MontoSubtotal ?? 0,
                    o.MontoTotal ?? 0,
                    o.DetalleOrdenes.Select(d => new DetalleOrdenResponseDto(
                        d.Id,
                        d.Plato.Nombre ?? string.Empty,
                        d.Cantidad ?? 0,
                        d.PrecioUnitario ?? 0,
                        d.Igv ?? 0,
                        d.Subtotal ?? 0,
                        d.Total ?? 0
                    )).ToList()
                ));

            return ordenes.Paginate(pageNumber, pageSize);
        }
        public OrdenResponseDto ObtenerOrden(long id)
        {
            var Orden = _context.Ordenes
                .Where(c => c.Id == id)
                .Include(o => o.DetalleOrdenes)
                .ThenInclude(d => d.Plato)
                .Select(o => new OrdenResponseDto(
                    o.Id,
                    o.CodigoOrden ?? string.Empty,
                    o.MesaId,
                    o.Mesa.Numero,
                    o.Estado.ToString() ?? string.Empty,
                    o.FechaCreacion ?? DateTime.UtcNow,
                    o.HoraCreacion ?? TimeSpan.Zero,
                    o.MontoSubtotal ?? 0,
                    o.MontoTotal ?? 0,
                    o.DetalleOrdenes.Select(d => new DetalleOrdenResponseDto(
                        d.Id,
                        d.Plato.Nombre ?? string.Empty,
                        d.Cantidad ?? 0,
                        d.PrecioUnitario ?? 0,
                        d.Igv ?? 0,
                        d.Subtotal ?? 0,
                        d.Total ?? 0
                    )).ToList()
                )).FirstOrDefault();

            if (Orden == null)
            {
                throw new Exception("No se encontro la Orden");
            }
            return Orden;

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
        public OrdenResponseDto ActualizarOrden(long id, OrdenActualizarRequestDTO requestDto)
        {
            var orden = _context.Ordenes
                .Include(o => o.DetalleOrdenes)
                .ThenInclude(d => d.Plato)
                .FirstOrDefault(o => o.Id == id);
            if (orden == null)
            {
                throw new Exception("No se encontró la orden.");
            }
            if (orden.Estado == EstadoOrden.COMPLETADA || orden.Estado == EstadoOrden.PREPARANDO || orden.Estado == EstadoOrden.CANCELADA)
            {
                throw new Exception("No se puede actualizar una orden que ya ha sido completada, cancelada o está en preparación.");
            }
            if (requestDto.Detalles == null || requestDto.Detalles.Count == 0)
            {
                throw new ArgumentException("La lista de detalles no puede estar vacía.");
            }

            orden.DetalleOrdenes.Clear();
            orden.MontoSubtotal = 0;
            orden.MontoTotal = 0;
            orden.Activo = true;
            requestDto.Detalles.ForEach(d =>
            {
                ItemsMenu item = _context.ItemsMenus.Find(d.PlatoId);
                if (item == null)
                {
                    throw new ArgumentException($"El plato con ID {d.PlatoId} no existe.");
                }
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

            if (_context.SaveChanges() > 0)
            {
                var ordenConDetalles = _context.Ordenes
                    .Include(o => o.Mesa)
                    .Include(o => o.DetalleOrdenes)
                   .ThenInclude(d => d.Plato)
                   .Where(o => o.Id == orden.Id)
                   .FirstOrDefault();
                if (ordenConDetalles == null)
                {
                    throw new Exception("Orden no encontrada después de guardarla.");
                }

                return new OrdenResponseDto(
                     ordenConDetalles.Id,
                     ordenConDetalles.CodigoOrden!,
                     ordenConDetalles.MesaId,
                     ordenConDetalles.Mesa.Numero,
                     ordenConDetalles.Estado.ToString()!,
                     ordenConDetalles.FechaCreacion!.Value,
                     ordenConDetalles.HoraCreacion!.Value,
                     ordenConDetalles.MontoSubtotal ?? 0,
                     ordenConDetalles.MontoTotal ?? 0,
                     ordenConDetalles.DetalleOrdenes.Select(d => new DetalleOrdenResponseDto(
                         d.Id,
                         d.Plato.Nombre!,
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
                throw new Exception("No se pudo actualizar la orden");
            }

        }
        public Page<OrdenResponseDto> ListaOrdenes(int pageNumber, int pageSize)
        {
            var ordenes = _context.Ordenes
                .Include(o => o.Mesa)
                .Include(o => o.DetalleOrdenes)
                .ThenInclude(d => d.Plato)
                .Select(o => new OrdenResponseDto(
                    o.Id,
                    o.CodigoOrden ?? string.Empty,
                    o.MesaId,
                    o.Mesa.Numero,
                    o.Estado.ToString() ?? string.Empty,
                    o.FechaCreacion ?? DateTime.UtcNow,
                    o.HoraCreacion ?? TimeSpan.Zero,
                    o.MontoSubtotal ?? 0,
                    o.MontoTotal ?? 0,
                    o.DetalleOrdenes.Select(d => new DetalleOrdenResponseDto(
                        d.Id,
                        d.Plato.Nombre ?? string.Empty,
                        d.Cantidad ?? 0,
                        d.PrecioUnitario ?? 0,
                        d.Igv ?? 0,
                        d.Subtotal ?? 0,
                        d.Total ?? 0
                    )).ToList()
                ));
            return ordenes.Paginate(pageNumber, pageSize);
        }
        public void DesactivarOrdenes()
        {
            List<Ordene> lista = _context.Ordenes
                 .Where(o => o.Activo == true && (o.Estado == EstadoOrden.COMPLETADA || o.Estado == EstadoOrden.CANCELADA))
                 .ToList();
            if (lista.Count > 0)
            {
                lista.ForEach(o =>
                {
                    o.Activo = false;
                    _context.Ordenes.Update(o);
                });
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("No hay órdenes activas para desactivar.");
            }
        }
    }
}
