using app_restaurante_backend.Models.DTOs.ItemMenu;
using app_restaurante_backend.Service.Interfaces;
using app_restaurante_backend.Data;
using EntityFrameworkPaginateCore;
using app_restaurante_backend.Models.Enums.ItemMenu;
using app_restaurante_backend.Models.DTOs.Categoria;
using app_restaurante_backend.Models.Entidades;
using Microsoft.EntityFrameworkCore;

namespace app_restaurante_backend.Service.Implementations
{
    public class ItemMenuService : IItemMenuService
    {
        private readonly DbRestauranteContext _context;
        public ItemMenuService(DbRestauranteContext context)
        {
            _context = context;
        }
        ItemMenuResponseDTO IItemMenuService.ActualizarItemMenu(int id, ItemMenuRequestDTO itemMenuDto)
        {
            var itemBuscado = _context.ItemsMenus
                .Include(i => i.Categoria)
                .FirstOrDefault(c => c.Id == id && c.Activo == true);

            if (itemBuscado == null)
            {
                throw new Exception("No se encontro el item");
            }
            if (_context.ItemsMenus.Any(i => i.Nombre == itemMenuDto.Nombre && i.Activo == true))
            {
                throw new Exception("Ya existe un item con ese nombre");
            }
            if (itemMenuDto.Precio < itemBuscado.Categoria.PrecioMinimo)
            {
                throw new Exception("El precio no puede ser menor al precio minimo de su categoria");
            }
            if (itemMenuDto.Precio < 1)
            {
                throw new Exception("El precio no puede ser 0");
            }
            if (!Enum.IsDefined(typeof(EstadoItemMenu), itemMenuDto.estado))
            {
                throw new Exception("El estado no es valido");
            }

            itemBuscado.Nombre = itemMenuDto.Nombre;
            itemBuscado.Descripcion = itemMenuDto.Descripcion;
            itemBuscado.Precio = itemMenuDto.Precio;
            itemBuscado.EnlaceImagen = itemMenuDto.EnlaceImagen;
            itemBuscado.CategoriaId = itemMenuDto.CategoriaId;
            itemBuscado.Estado = Enum.Parse<EstadoItemMenu>(itemMenuDto.estado.ToUpper(), true);
            _context.ItemsMenus.Update(itemBuscado);

            if (_context.SaveChanges() > 0)
            {
                return new ItemMenuResponseDTO(
                    itemBuscado.Id,
                    itemBuscado.Nombre!,
                    itemBuscado.Descripcion!,
                    itemBuscado.Precio ?? 0,
                    itemBuscado.EnlaceImagen ?? string.Empty,
                    new CategoriaResponseDTO(itemBuscado.CategoriaId, itemBuscado.Categoria!.Nombre!, itemBuscado.Categoria!.Descripcion!, itemBuscado.Categoria!.PrecioMinimo ?? 0),
                    itemBuscado.Estado
                );
            }
            else
            {
                throw new Exception("No se pudo actualizar el item del menu");
            }
        }

        ItemMenuResponseDTO IItemMenuService.AgregarItemMenu(ItemMenuRequestDTO itemMenuDto)
        {
            var categoria = _context.CategoriasItems.FirstOrDefault(c => c.Id == itemMenuDto.CategoriaId);
            if (categoria == null)
            {
                throw new Exception("No se encontro la categoria");
            }
            if (_context.ItemsMenus.Any(i=>i.Nombre == itemMenuDto.Nombre && i.Activo==true))
            {
                throw new Exception("Ya existe un item con ese nombre");
            }
            if (itemMenuDto.Precio < categoria.PrecioMinimo)
            {
                throw new Exception("El precio no puede ser menor al precio minimo de su categoria");
            }
            if (itemMenuDto.Precio < 1)
            {
                throw new Exception("El precio no puede ser 0");
            }
            if (!Enum.IsDefined(typeof(EstadoItemMenu), itemMenuDto.estado )) { 
                throw new Exception("El estado no es valido");
            }
            var NuevoItemMenu = new ItemsMenu()
            {
                Nombre = itemMenuDto.Nombre,
                Descripcion = itemMenuDto.Descripcion,
                Precio = itemMenuDto.Precio,
                EnlaceImagen = itemMenuDto.EnlaceImagen,
                CategoriaId = itemMenuDto.CategoriaId,
                Estado = Enum.Parse<EstadoItemMenu>(itemMenuDto.estado.ToUpper(), true),
                Activo = true,
            };  
            
            _context.ItemsMenus.Add(NuevoItemMenu);

            if(_context.SaveChanges() > 0)
            {

                return new ItemMenuResponseDTO(
                    NuevoItemMenu.Id,
                    NuevoItemMenu.Nombre,
                    NuevoItemMenu.Descripcion,
                    NuevoItemMenu.Precio ?? 0,
                    NuevoItemMenu.EnlaceImagen ?? string.Empty,
                    new CategoriaResponseDTO(categoria.Id, categoria.Nombre!, categoria.Descripcion!, categoria.PrecioMinimo??0),
                    NuevoItemMenu.Estado
                );
            }
            else
            {
                throw new Exception("No se pudo agregar el item al menu");
            }
                
        }

        ItemMenuResponseDTO IItemMenuService.CambiarEstadoItemMenu(int id, ItemMenuEstadoRequestDTO estado) 
        {
            var itemBuscado = _context.ItemsMenus
                .Include(i => i.Categoria)
                .FirstOrDefault(c => c.Id == id && c.Activo == true);
            if (itemBuscado == null)
            {
                throw new Exception("No se encontro el item");
            }
            if (!Enum.IsDefined(typeof(EstadoItemMenu), estado.nombreEstado))
            {
                throw new Exception("El estado no es valido");
            }
            itemBuscado.Estado = Enum.Parse<EstadoItemMenu>(estado.nombreEstado.ToUpper(), true);

            _context.ItemsMenus.Update(itemBuscado);
            _context.SaveChanges();

            return new ItemMenuResponseDTO(
                itemBuscado.Id,
                itemBuscado.Nombre!,
                itemBuscado.Descripcion!,
                itemBuscado.Precio ?? 0,
                itemBuscado.EnlaceImagen ?? string.Empty,
                new CategoriaResponseDTO(itemBuscado.CategoriaId, itemBuscado.Categoria!.Nombre!, itemBuscado.Categoria!.Descripcion!, itemBuscado.Categoria!.PrecioMinimo ?? 0),
                itemBuscado.Estado
            );

        }

        void IItemMenuService.EliminarItemMenu(int id)
        {
            var itemBuscado = _context.ItemsMenus.FirstOrDefault(c => c.Id == id && c.Activo == true);
            if (itemBuscado == null)
            {
                throw new Exception("No se encontro el item");
            }
            itemBuscado.Activo = false;
            _context.ItemsMenus.Update(itemBuscado);
            _context.SaveChanges();
        }

        ItemMenuResponseDTO IItemMenuService.ObtenerItemMenu(int id)
        {
            var itemBuscado = _context.ItemsMenus
                .Include(i => i.Categoria)
                .FirstOrDefault(c => c.Id == id && c.Activo == true);
            if (itemBuscado == null)
            {
                throw new Exception("No se encontro el Menu");
            }
            return new ItemMenuResponseDTO(
                itemBuscado.Id,
                itemBuscado.Nombre!,
                itemBuscado.Descripcion!,
                itemBuscado.Precio ?? 0,
                itemBuscado.EnlaceImagen ?? string.Empty,
                new CategoriaResponseDTO(itemBuscado.CategoriaId, itemBuscado.Categoria!.Nombre!, itemBuscado.Categoria!.Descripcion!, itemBuscado.Categoria!.PrecioMinimo ?? 0),
                itemBuscado.Estado
            );
        }

        Page<ItemMenuResponseDTO> IItemMenuService.ObtenerItemsMenu(int pageNumber, int pageSize)
        {
            var items = _context.ItemsMenus.Include(c => c.Categoria)
                .Where(i=> i.Activo == true)
                .Select(i => new ItemMenuResponseDTO(
                i.Id,
                i.Nombre!,
                i.Descripcion!,
                i.Precio ?? 0,
                i.EnlaceImagen ?? string.Empty,
                new CategoriaResponseDTO(i.CategoriaId, i.Categoria!.Nombre!, i.Categoria!.Descripcion!, i.Categoria!.PrecioMinimo ?? 0),
                i.Estado
            ));

            return items.Paginate(pageNumber, pageSize);
        }

        Page<ItemMenuResponseDTO> IItemMenuService.ObtenerItemsMenuPorCategoria(int categoriaId, int pageNumber, int pageSize)
        {
            var items = _context.ItemsMenus.Where(i => i.CategoriaId == categoriaId && i.Activo == true)
                .Include(i => i.Categoria)
                .OrderBy(i => i.Id)
                .Select(i => new ItemMenuResponseDTO(
                    i.Id,
                    i.Nombre!,
                    i.Descripcion!,
                    i.Precio ?? 0,
                    i.EnlaceImagen ?? string.Empty,
                    new CategoriaResponseDTO(i.CategoriaId, i.Categoria!.Nombre!, i.Categoria!.Descripcion!, i.Categoria!.PrecioMinimo ?? 0),
                    i.Estado
                    ));
            return items.Paginate(pageNumber, pageSize);
        }

        Page<ItemMenuResponseDTO> IItemMenuService.ObtenerItemsMenuPorNombre(string nombreItemMenu,string nombreCategoria, int pageNumber, int pageSize)
        {
           var itemMenuNombre = _context.ItemsMenus.Include(c => c.Categoria)
                .Where(i => i.Nombre!.Contains(nombreItemMenu) && i.Categoria.Nombre!.Contains(nombreCategoria) && i.Activo == true)
                .Select(i => new ItemMenuResponseDTO(
                    i.Id,
                    i.Nombre!,
                    i.Descripcion!,
                    i.Precio ?? 0,
                    i.EnlaceImagen ?? string.Empty,
                    new CategoriaResponseDTO(i.CategoriaId, i.Categoria!.Nombre!, i.Categoria!.Descripcion!, i.Categoria!.PrecioMinimo ?? 0),
                    i.Estado
                ));
            return itemMenuNombre.Paginate(pageNumber, pageSize);
        }
    }
}
