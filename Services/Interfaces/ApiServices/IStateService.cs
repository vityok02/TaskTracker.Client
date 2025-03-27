using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;

namespace Services.Interfaces.ApiServices;

public interface IStateService
{
    public Task<Result<StateDto>> CreateAsync(Guid projectId, StateModel model);

    public Task<Result<StateDto>> GetAsync(Guid projectId, Guid stateId);

    public Task<Result<IEnumerable<StateDto>>> GetAllAsync(Guid projectId);

    public Task<Result> UpdateAsync(Guid projectId, Guid stateId, StateModel model);

    Task<Result> ReorderAsync(Guid projectId, Guid stateId, ReorderStateModel model);

    public Task<Result> DeleteAsync(Guid projectId, Guid stateId);
}
