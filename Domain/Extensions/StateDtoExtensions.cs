using Domain.Dtos;
using Domain.Models;

namespace Domain.Extensions;

public static class StateDtoExtensions
{
    public static StateModel ToStateModel(this StateDto dto)
    {
        return new StateModel
        {
            Name = dto.Name,
            Description = dto.Description
        };
    }
}
