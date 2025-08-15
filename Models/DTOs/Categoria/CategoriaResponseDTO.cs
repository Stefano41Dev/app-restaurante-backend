namespace app_restaurante_backend.Models.DTOs.Categoria
{
    public record CategoriaResponseDTO(
        short Id,
        string Nombre,
        string Descripcion,
        double PrecioMinimo
        )
    {
    }
}
