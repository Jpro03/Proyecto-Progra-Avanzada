using System.Text.Json;
using Fidepolis.Entities;
namespace Fidepolis.Data;
public sealed class JsonCinemaRepository : ICinemaRepository
{
    private readonly string _path;
    private readonly IReadOnlyList<Movie> _movies;
    private readonly IReadOnlyList<Showtime> _showtimes;
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true, PropertyNameCaseInsensitive = true };
    public JsonCinemaRepository(string dataDirectory)
    {
        Directory.CreateDirectory(dataDirectory);
        _path = Path.Combine(dataDirectory, "purchases.json");
        _movies =
        [
            new(1, "Horizonte Infinito", "Ciencia ficción", "+12", 128, "Una tripulación descubre una señal misteriosa al borde del sistema solar y debe decidir si responde.", "/images/horizonte.svg"),
            new(2, "La Última Llave", "Suspenso", "+15", 110, "Un antiguo secreto familiar aparece cuando una joven encuentra una llave que abre una habitación olvidada.", "/images/llave.svg"),
            new(3, "Risas en Ruta", "Comedia", "Todo público", 96, "Tres amigos emprenden un viaje improvisado lleno de encuentros y situaciones inesperadas.", "/images/risas.svg"),
            new(4, "Reino de Cristal", "Fantasía", "+7", 119, "Una aprendiz de guardiana deberá devolver la luz a un reino escondido entre montañas.", "/images/reino.svg"),
            new(5, "El Caso del Faro", "Misterio", "+12", 104, "Una investigadora llega a un pueblo costero para descifrar extrañas pistas de un faro.", "/images/faro.svg"),
            new(6, "A Toda Velocidad", "Acción", "+12", 115, "Una piloto novata compite contra el tiempo para recuperar un invento perdido.", "/images/velocidad.svg")
        ];
        var today = DateTime.Today;
        var result = new List<Showtime>();
        var id = 1;
        foreach (var day in Enumerable.Range(0, 7))
        foreach (var movie in _movies)
        {
            var hours = new[] { 14, 18, 21 };
            var hour = hours[(movie.Id + day) % hours.Length];
            result.Add(new Showtime(id++, movie.Id, today.AddDays(day).AddHours(hour), $"Sala {(movie.Id % 3) + 1}", movie.Id % 2 == 0 ? "Subtitulada" : "Doblada", 3200m + (movie.Id % 3) * 500m));
        }
        _showtimes = result;
    }
    public IReadOnlyList<Movie> GetMovies() => _movies;
    public IReadOnlyList<Showtime> GetShowtimes() => _showtimes;
    public IReadOnlyList<Purchase> GetPurchases()
    {
        if (!File.Exists(_path)) return [];
        var json = File.ReadAllText(_path);
        return JsonSerializer.Deserialize<List<Purchase>>(json, Options) ?? [];
    }
    public void SavePurchases(IReadOnlyList<Purchase> purchases)
    {
        var temp = _path + ".tmp";
        File.WriteAllText(temp, JsonSerializer.Serialize(purchases, Options));
        File.Move(temp, _path, overwrite: true);
    }
}
