namespace Fidepolis.Entities;
public sealed record Showtime(int Id, int MovieId, DateTime StartsAt, string Room, string Language, decimal Price);
