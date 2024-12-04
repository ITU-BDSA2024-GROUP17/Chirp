using Chirp.Core.Entities;

namespace Chirp.Core.Interfaces;

/// <summary>
/// This model is no longer need because the webforms that required this have been updated.
/// </summary>
public interface ICheepModel
{
    public string CheepMessage { get; set; }
    public IEnumerable<Cheep> Cheeps { get; set; }
}
