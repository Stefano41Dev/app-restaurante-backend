using app_restaurante_backend.Models.DTOs.Orden;
using EntityFrameworkPaginateCore;

namespace app_restaurante_backend.Service.Interfaces
{
    public interface IOrdenService
    {
        OrdenResponseDto CrearOrden(OrdenRequestDto requestDto);
        Page<OrdenResponseDto> ListaOrdenesDeHoy(int NumberPage,int SizePage);
        OrdenResponseDto ObtenerOrden(long id);
        OrdenResponseDto ActualizarEstado(long id,OrdenEstadoRequestDto requestDto);
        OrdenResponseDto ActualizarOrden(long id, OrdenActualizarRequestDTO requestDto);
        Page<OrdenResponseDto> ListaOrdenes(int NumberPage, int SizePage);
        OrdenResponseDto ObtenerOrdenPendientePorMesa (short idMesa);
        OrdenResponseDto MarcarOrdenPagada(long id);
        void DesactivarOrdenes();
    }
}
