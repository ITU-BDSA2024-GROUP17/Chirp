using Chirp.Core.Entities;

namespace Chirp.Core.Interfaces;

public interface ICheepModel
{
    public string CheepMessage { get; set; }
    public IEnumerable<Cheep> Cheeps { get; set; }
}
