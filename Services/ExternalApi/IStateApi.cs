﻿using Domain.Dtos;
using Domain.Models;
using Refit;

namespace Services.ExternalApi;

public interface IStateApi : IApi
{
    [Post("/projects/{projectId}/states")]
    Task<IApiResponse<StateDto>> CreateAsync(Guid projectId, StateModel model);

    [Get("/projects/{projectId}/states/{stateId}")]
    Task<IApiResponse<StateDto>> GetAsync(Guid projectId, Guid stateId);

    [Get("/projects/{projectId}/states")]
    Task<IApiResponse<IEnumerable<StateDto>>> GetListAsync(Guid projectId);

    [Put("/projects/{projectId}/states/{stateId}")]
    Task<IApiResponse<StateDto>> UpdateAsync(Guid projectId, Guid stateId, StateModel model);

    [Patch("/projects/{projectId}/states/{stateId}/order")]
    Task<IApiResponse> ReorderAsync(Guid projectId, Guid stateId, ReorderStateModel model);

    [Delete("/projects/{projectId}/states/{stateId}")]
    Task<IApiResponse> DeleteAsync(Guid projectId, Guid stateId);
}
