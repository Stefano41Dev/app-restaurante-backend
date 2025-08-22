using EntityFrameworkPaginateCore;
using app_restaurante_backend.Models.DTOs.Mesa;
namespace app_restaurante_backend.Service.Interfaces
{
    public interface IMesaService
    {
        Page<MesaResponseDTO> ObtenerMesas( int pageNumber, int pageSize);
        MesaResponseDTO ObtenerMesa(int id);
        MesaResponseDTO AgregarMesa(MesaRequestDTO mesa);
        MesaResponseDTO ActualizarMesa(int id, MesaRequestDTO mesa);
        void EliminarMesa(int id);
        MesaResponseDTO CambiarEstadoMesa(int id, MesaEstadoRequestDTO estado);
        Page<MesaResponseDTO> ObtenerMesasDisponibles(int pageNumber, int pageSize);

        Page<MesaResponseDTO> ObtenerMesasConOrdenPendiente(int pageNumber, int pageSize);
    }
}
