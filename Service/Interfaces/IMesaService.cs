using EntityFrameworkPaginateCore;
using app_restaurante_backend.Models.DTOs.Mesa;
namespace app_restaurante_backend.Service.Interfaces
{
    public interface IMesaService
    {
        List<MesaResponseDTO> ObtenerMesas();
        MesaResponseDTO ObtenerMesa(int id);
        MesaResponseDTO AgregarMesa(MesaRequestDTO mesa);
        MesaResponseDTO ActualizarMesa(int id, MesaRequestDTO mesa);
        void EliminarMesa(int id);
        MesaResponseDTO CambiarEstadoMesa(int id, MesaEstadoRequestDTO estado);
        List<MesaResponseDTO> ObtenerMesasDisponibles();

        List<MesaResponseDTO> ObtenerMesasConOrdenPendiente();
    }
}
