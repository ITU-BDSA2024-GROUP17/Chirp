using System.Data.Common;
using Chirp.Core.Entities;

namespace Chirp.Core.DTOs;

public class CreateAuthorDto
{
    public required string Id { get; set; }
    public required string UserName { get; set; }

    public CreateAuthorDto()
    {
    }


    public CreateAuthorDto(string Id, string UserName)
    {
        this.Id = Id;
        this.UserName = UserName;
    }

}
