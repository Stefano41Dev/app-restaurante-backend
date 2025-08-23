using app_restaurante_backend.Models.DTOs.Categoria;
using EntityFrameworkPaginateCore;

namespace app_restaurante_backend.Service.Interfaces
{
    public interface ICategoriaService
    {
        List<CategoriaResponseDTO> ObtenerCategorias();
        CategoriaResponseDTO ObtenerCategoria(short id);
        CategoriaResponseDTO CrearCategoria(CategoriaRequestDTO categoria);
        CategoriaResponseDTO ActualizarCategoria(short id, CategoriaRequestDTO categoria);
        void EliminarCategoria(short id);
    }
}
