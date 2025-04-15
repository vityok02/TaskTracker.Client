using Domain.Dtos;
using Domain.Models;

namespace Domain.Extensions;

public static class TaskDtoExtensions
{
    public static TaskModel ToTaskModel(
        this TaskDto dto)
    {
        return new()
        {
            Name = dto.Name,
            Description = dto.Description,
            StateId = dto.StateId
        };
    }
}
