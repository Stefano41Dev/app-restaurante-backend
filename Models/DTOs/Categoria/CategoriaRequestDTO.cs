namespace app_restaurante_backend.Models.DTOs.Categoria
{
    public record CategoriaRequestDTO(
        string Nombre,
        string Descripcion,
        double PrecioMinimo
    )
    {
        
    }
}
