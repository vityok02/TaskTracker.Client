﻿using Domain.Dtos;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Components;
using Services.Interfaces;

namespace Client.Components.Modules.Identity;

public sealed partial class Register
{
    private readonly IIdentityService _identityService;
    private readonly NavigationManager _navManager;

    public Register(
        IIdentityService identityService,
        NavigationManager navManager)
    {
        _identityService = identityService;
        _navManager = navManager;
    }

    private RegisterModel RegisterModel { get; set; } = new RegisterModel();
    private string ErrorMessage { get; set; } = string.Empty;

    public async Task Submit()
    {
        if (RegisterModel.Password != RegisterModel.ConfirmedPassword)
        {
            ErrorMessage = "Passwords do not match";
            return;
        }

        var result = await _identityService.RegisterAsync(RegisterModel);

        if (result.IsFailure)
        {
            return;
        }

        _navManager.NavigateTo("/");
    }
}
