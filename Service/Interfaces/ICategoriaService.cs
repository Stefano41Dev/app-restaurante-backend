using app_restaurante_backend.Models.DTOs.Categoria;
using EntityFrameworkPaginateCore;

namespace app_restaurante_backend.Service.Interfaces
{
    public interface ICategoriaService
    {
        Page<CategoriaResponseDTO> ObtenerCategorias(int pageNumber, int pageSize);
        CategoriaResponseDTO ObtenerCategoria(short id);
        CategoriaResponseDTO CrearCategoria(CategoriaRequestDTO categoria);
        CategoriaResponseDTO ActualizarCategoria(short id, CategoriaRequestDTO categoria);
        void EliminarCategoria(short id);
    }
}
