using app_restaurante_backend.Models.DTOs.Orden;
using EntityFrameworkPaginateCore;

namespace app_restaurante_backend.Service.Interfaces
{
    public interface IOrdenService
    {
        OrdenResponseDto CrearOrden(OrdenRequestDto requestDto);
        Page<OrdenResponseDto> ListaOrdenes();
        OrdenResponseDto ObtenerOrden(long id);
        OrdenResponseDto ActualizarEstado(OrdenEstadoRequestDto requestDto);
    }
}
