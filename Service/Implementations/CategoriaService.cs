using app_restaurante_backend.Data;
using app_restaurante_backend.Models.DTOs.Categoria;
using app_restaurante_backend.Service.Interfaces;
using EntityFrameworkPaginateCore;

namespace app_restaurante_backend.Service.Implementations
{
    public class CategoriaService : ICategoriaService
    {
        private readonly DbRestauranteContext _context;
        public CategoriaService(DbRestauranteContext context)
        {
            _context = context;
        }
        CategoriaResponseDTO ICategoriaService.ActualizarCategoria(short id, CategoriaRequestDTO categoria)
        {
            throw new NotImplementedException();
        }

        CategoriaResponseDTO ICategoriaService.CrearCategoria(CategoriaRequestDTO categoria)
        {
            throw new NotImplementedException();
        }

        void ICategoriaService.EliminarCategoria(short id)
        {
            throw new NotImplementedException();
        }

        CategoriaResponseDTO ICategoriaService.ObtenerCategoria(short id)
        {
            throw new NotImplementedException();
        }

        List<CategoriaResponseDTO> ICategoriaService.ObtenerCategorias(int pageNumber, int pageSize)
        {
            var listaCategoria = _context.CategoriasItems.Where(b=>b.Activo == true).Paginate(pageNumber,pageSize);
           return listaCategoria.Results.Select(c => new CategoriaResponseDTO(
                c.Id,
                c.Nombre!,
                c.Descripcion!,
                c.PrecioMinimo ?? 0
                )).ToList();
        }
    }
}
