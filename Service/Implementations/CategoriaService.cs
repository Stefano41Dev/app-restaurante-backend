using app_restaurante_backend.Data;
using app_restaurante_backend.Models.DTOs.Categoria;
using app_restaurante_backend.Models.Entidades;
using app_restaurante_backend.Service.Interfaces;
using EntityFrameworkPaginateCore;
using Microsoft.EntityFrameworkCore;

namespace app_restaurante_backend.Service.Implementations
{
    public class CategoriaService : ICategoriaService
    {
        private readonly DbRestauranteContext _context;
        public CategoriaService(DbRestauranteContext context)
        {
            _context = context;
        }
        CategoriaResponseDTO ICategoriaService.ActualizarCategoria(short id, CategoriaRequestDTO categoriaDto)
        {
            if (_context.CategoriasItems.Any(c => c.Nombre == categoriaDto.Nombre && c.Activo == true))
            {
                throw new Exception("Ya existe una categoria con ese nombre");
            }
            if (categoriaDto.PrecioMinimo < 1)
            {
                throw new Exception("El precio minimo no puede ser 0");
            }

            var categoriaBuscada = _context.CategoriasItems.FirstOrDefault(c => c.Id == id && c.Activo == true);
            
            if(categoriaBuscada == null)
            {
                throw new Exception("No se encontro la categoria");
            }
            
            categoriaBuscada.Nombre = categoriaDto.Nombre;
            categoriaBuscada.Descripcion = categoriaDto.Descripcion;
            categoriaBuscada.PrecioMinimo = categoriaDto.PrecioMinimo;
            _context.CategoriasItems.Update(categoriaBuscada);

            if (_context.SaveChanges() > 0)
            {
                return new CategoriaResponseDTO
                (
                    categoriaBuscada.Id,
                    categoriaBuscada.Nombre!,
                    categoriaBuscada.Descripcion!,
                    categoriaBuscada.PrecioMinimo ?? 0
                );
            }
            else
            {
                throw new Exception("No se pudo actualizar la categoria");
            }
        }

        CategoriaResponseDTO ICategoriaService.CrearCategoria(CategoriaRequestDTO categoriaDto)
        {
            if(_context.CategoriasItems.Any(c => c.Nombre == categoriaDto.Nombre && c.Activo == true))
            {
                throw new Exception("Ya existe una categoria con ese nombre");
            }
            if(categoriaDto.PrecioMinimo < 1)
            {
                throw new Exception("El precio minimo no puede ser 0");
            }
            CategoriasItem categoria = new CategoriasItem()
            {
                Nombre = categoriaDto.Nombre,
                Descripcion = categoriaDto.Descripcion,
                PrecioMinimo = categoriaDto.PrecioMinimo,
                Activo = true
            };
            _context.CategoriasItems.Add(categoria);
            if (_context.SaveChanges() > 0)
            {
               return new CategoriaResponseDTO
                (
                    categoria.Id,
                    categoria.Nombre!,
                    categoria.Descripcion!,
                    categoria.PrecioMinimo ?? 0
                );
            }
            else
            {
                throw new Exception("No se pudo crear la categoria");
            }

        }

        void ICategoriaService.EliminarCategoria(short id)
        {
            var categoriaBuscada = _context.CategoriasItems
                .Include(p => p.ItemsMenus)
                .FirstOrDefault(c => c.Id == id && c.Activo == true);
            
            if (categoriaBuscada == null)
            {
                throw new Exception("No se encontro la categoria");
            }
            categoriaBuscada.eliminarCategoria();
            _context.CategoriasItems.Update(categoriaBuscada);
            _context.SaveChanges();
        }

        CategoriaResponseDTO ICategoriaService.ObtenerCategoria(short id)
        {
            var resultado = _context.CategoriasItems.Where(c => c.Id == id && c.Activo == true)
                .Select(c => new CategoriaResponseDTO
                (
                    c.Id,
                    c.Nombre!,
                    c.Descripcion!,
                    c.PrecioMinimo ?? 0
                )).FirstOrDefault();
            if(resultado == null)
            {
                throw new Exception("No se encontro la categoria");
            }
            return resultado!;
        }

        Page<CategoriaResponseDTO> ICategoriaService.ObtenerCategorias(int pageNumber, int pageSize)
        {
            var query = _context.CategoriasItems
         .Where(b => b.Activo == true)
         .Select(c => new CategoriaResponseDTO
         (
             c.Id,
             c.Nombre!,
             c.Descripcion!,
             c.PrecioMinimo ?? 0
         ));

            var paginaMapeada = query.Paginate(pageNumber, pageSize);

            return paginaMapeada;
        }
    }
}
