using Domain.Abstract;
using Domain.Dtos;
using Domain.Models;
using Services.Extensions;
using Services.ExternalApi;
using Services.Interfaces.ApiServices;

namespace Services.ApiServices;

public class StateService : IStateService
{
    private readonly IStateApi _stateApi;

    public StateService(IStateApi stateApi)
    {
        _stateApi = stateApi;
    }

    public async Task<Result<StateDto>> CreateAsync(Guid projectId, StateModel model)
    {
        var response = await _stateApi
            .CreateAsync(projectId, model);

        return response.HandleResponse();
    }

    public async Task<Result<StateDto>> GetAsync(Guid projectId, Guid stateId)
    {
        var response = await _stateApi
            .GetAsync(projectId, stateId);

        return response.HandleResponse();
    }

    public async Task<Result<IEnumerable<StateDto>>> GetAllAsync(Guid projectId)
    {
        var response = await _stateApi
            .GetListAsync(projectId);

        return response.HandleResponse();
    }

    public async Task<Result> UpdateAsync(Guid projectId, Guid stateId, StateModel model)
    {
        var response = await _stateApi
            .UpdateAsync(projectId, stateId, model);

        return response.HandleResponse();
    }

    public async Task<Result> ReorderAsync(Guid projectId, Guid stateId, ReorderStateModel model)
    {
        var response = await _stateApi
            .ReorderAsync(projectId, stateId, model);

        return response.HandleResponse();
    }

    public async Task<Result> DeleteAsync(Guid projectId, Guid stateId)
    {
        var response = await _stateApi
            .DeleteAsync(projectId, stateId);

        return response.HandleResponse();
    }
}
