using app_restaurante_backend.Models.DTOs.Mesa;
using app_restaurante_backend.Service.Interfaces;
using EntityFrameworkPaginateCore;
using app_restaurante_backend.Data;
using app_restaurante_backend.Models.Enums.Ordenes;
using app_restaurante_backend.Models.Entidades;
namespace app_restaurante_backend.Service.Implementations
{
    public class MesaService : IMesaService
    {
        private readonly DbRestauranteContext _context;
        public MesaService(DbRestauranteContext context)
        {
            _context = context;
        }

        public Page<MesaResponseDTO> ObtenerMesasDisponibles(int pageNumber, int pageSize)
        {
            var mesasDisponibles = _context.Mesas
                .Where(m=> m.Estado == EstadoMesa.LIBRE && m.Activo == true)
                .Select(m => new MesaResponseDTO
                (
                    m.Id,
                    m.Numero!,
                    m.Capacidad ?? 0,
                    m.Estado.ToString()
                ));
            return mesasDisponibles.Paginate(pageNumber, pageSize);
           
        }

        MesaResponseDTO IMesaService.ActualizarMesa(int id, MesaRequestDTO mesaDTO)
        {
            var mesaBuscada = _context.Mesas.FirstOrDefault(m => m.Id == id && m.Activo == true);
            if (mesaBuscada == null)
            {
                throw new Exception("No se encontro la mesa");
            }
            if (string.IsNullOrEmpty(mesaDTO.Numero))
            {
                throw new Exception("El numero de la mesa no puede estar vacio");
            }
            if (mesaDTO.Capacidad < 1)
            {
                throw new Exception("La capacidad de la mesa no puede ser menor a 1");
            }
            if (!Enum.IsDefined(typeof(EstadoMesa), mesaDTO.Estado))
            {
                throw new Exception("Estado de mesa no valido");
            }
            if (_context.Mesas.Any(m => m.Numero == mesaDTO.Numero && m.Activo == true))
            
            if (mesaBuscada.Estado != EstadoMesa.LIBRE)
            {
                throw new Exception("No se puede actualizar una mesa que no esta libre");
            }
            mesaBuscada.Numero = mesaDTO.Numero;
            mesaBuscada.Capacidad = mesaDTO.Capacidad;
            mesaBuscada.Estado = Enum.Parse<EstadoMesa>(mesaDTO.Estado, true);
           
            _context.Mesas.Update(mesaBuscada);
            if (_context.SaveChanges() > 0)
            {
                return new MesaResponseDTO
                (
                    mesaBuscada.Id,
                    mesaBuscada.Numero!,
                    mesaBuscada.Capacidad ?? 0,
                    mesaBuscada.Estado.ToString()
                );
            }
            else
            {
                throw new Exception("No se pudo actualizar la mesa");
            }
        }

        MesaResponseDTO IMesaService.AgregarMesa(MesaRequestDTO mesaDto)
        {
            if(string.IsNullOrEmpty(mesaDto.Numero))
            {
                throw new Exception("El numero de la mesa no puede estar vacio");
            }
            if(mesaDto.Capacidad < 1)
            {
                throw new Exception("La capacidad de la mesa no puede ser menor a 1");
            }
            if (!Enum.IsDefined(typeof(EstadoMesa), mesaDto.Estado))
            {
                throw new Exception("Estado de mesa no valido");
            }
            if (_context.Mesas.Any(m => m.Numero == mesaDto.Numero && m.Activo == true))
            {
                throw new Exception("Ya existe una mesa con ese numero");
            }
            var nuevaMesa = new Mesa
            {
                Numero = mesaDto.Numero,
                Capacidad = mesaDto.Capacidad,
                Estado = Enum.Parse<EstadoMesa>(mesaDto.Estado, true),
                Activo = true
            };
            _context.Mesas.Add(nuevaMesa);
            if (_context.SaveChanges() > 0)
            {
                return new MesaResponseDTO
                (
                    nuevaMesa.Id,
                    nuevaMesa.Numero!,
                    nuevaMesa.Capacidad ?? 0,
                    nuevaMesa.Estado.ToString()
                );
            }
            else
            {
                throw new Exception("No se pudo agregar la mesa");
            }
            //throw new NotImplementedException();
        }

        MesaResponseDTO IMesaService.CambiarEstadoMesa(int id, MesaEstadoRequestDTO estado)
        {
            var mesaBuscada = _context.Mesas.FirstOrDefault(m => m.Id == id && m.Activo == true);
            if (mesaBuscada == null)
            {
                throw new Exception("No se encontro la mesa");
            }
            if (!Enum.IsDefined(typeof(EstadoMesa), estado.nombreEstado))
            {
                throw new Exception("Estado de mesa no valido");
            }
            mesaBuscada.Estado = Enum.Parse<EstadoMesa>( estado.nombreEstado, true);
            _context.Mesas.Update(mesaBuscada);
            if (_context.SaveChanges() > 0)
            {
                return new MesaResponseDTO
                (
                    mesaBuscada.Id,
                    mesaBuscada.Numero!,
                    mesaBuscada.Capacidad ?? 0,
                    mesaBuscada.Estado.ToString()
                );
            }
            else
            {
                throw new Exception("No se pudo actualizar el estado de la mesa");
            }
        }

        void IMesaService.EliminarMesa(int id)
        {
            var mesaBuscada = _context.Mesas.FirstOrDefault(m => m.Id == id && m.Activo == true);
            if (mesaBuscada == null)
            {
                throw new Exception("No se encontro la mesa");
            }
            if(mesaBuscada.Estado != EstadoMesa.LIBRE)
            {
                throw new Exception("No se puede eliminar una mesa que no esta libre");
            }
            mesaBuscada.Activo = false;
            _context.Mesas.Update(mesaBuscada);
            if (_context.SaveChanges() <= 0)
            {
                throw new Exception("No se pudo eliminar la mesa");
            }
        }

        MesaResponseDTO IMesaService.ObtenerMesa(int id)
        {
            var mesa = _context.Mesas.Where(m => m.Id == id && m.Activo == true)
                .Select(m => new MesaResponseDTO
                (
                    m.Id,
                    m.Numero!,
                    m.Capacidad ?? 0,
                    m.Estado.ToString()    
                )).FirstOrDefault();
            if(mesa == null)
            {
                throw new Exception("No se encontro la mesa");
            }
                return mesa;
        }

        Page<MesaResponseDTO> IMesaService.ObtenerMesas(int pageNumber, int pageSize)
        {
            var Mesas = _context.Mesas
                .Where(m => m.Activo == true)
                .Select(m => new MesaResponseDTO
            (
                m.Id,
                m.Numero!,
                m.Capacidad ?? 0,
                m.Estado.ToString() 
            ));
            return Mesas.Paginate(pageNumber, pageSize);
        }
    }
}
