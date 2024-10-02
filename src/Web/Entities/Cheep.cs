using System.ComponentModel.DataAnnotations;

namespace Web.Entities;

public class Cheep
{
    [Key]
    public required string Id {get; set;}
    public required string Message {get; set;}
    public required long Timestamp {get; set;}
    public required Author Author {get; set;}
}
