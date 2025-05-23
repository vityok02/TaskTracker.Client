﻿using Client.Models;
using Microsoft.JSInterop;

namespace Client.Extensions;

public static class JSRuntimeExtensions
{
    public static async Task ToggleCameraAsync(
        this IJSRuntime jsRuntime,
        string deviceId) =>
        await jsRuntime.InvokeVoidAsync(
            "videoInterop.toggleCamera",
            deviceId);

    public static async Task ToggleMicrophoneAsync(
        this IJSRuntime jsRuntime) =>
        await jsRuntime.InvokeVoidAsync(
            "videoInterop.toggleMicrophone");

    public static async Task<Device[]> GetAudioDevicesAsync(
        this IJSRuntime jsRuntime) =>
        await jsRuntime.InvokeAsync<Device[]>(
            "videoInterop.getAudioDevices");

    public static async Task SetMicrophoneAsync(
        this IJSRuntime jsRuntime,
        string deviceId) =>
        await jsRuntime
            .InvokeVoidAsync(
            "videoInterop.setMicrophone", deviceId);

    public static ValueTask<Device[]> GetVideoDevicesAsync(
        this IJSRuntime? jsRuntime) =>
        jsRuntime?.InvokeAsync<Device[]>(
              "videoInterop.getVideoDevices") ?? new ValueTask<Device[]>();

    public static ValueTask StartVideoAsync(
        this IJSRuntime? jSRuntime,
        string deviceId,
        string selector) =>
        jSRuntime?.InvokeVoidAsync(
            "videoInterop.startVideo",
            deviceId, selector) ?? new ValueTask();

    public static ValueTask<bool> CreateOrJoinRoomAsync(
        this IJSRuntime? jsRuntime,
        string roomName,
        string token) =>
        jsRuntime?.InvokeAsync<bool>(
            "videoInterop.createOrJoinRoom",
            roomName, token) ?? new ValueTask<bool>(false);

    public static ValueTask LeaveRoomAsync(
        this IJSRuntime? jsRuntime) =>
        jsRuntime?.InvokeVoidAsync(
            "videoInterop.leaveRoom") ?? new ValueTask();
}