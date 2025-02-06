﻿using Microsoft.AspNetCore.Http;
using Services.Interfaces;

namespace Services.Services;

public class CookieManager : ICookieManager
{
    private readonly IHttpContextAccessor _contextAccessor;

    public CookieManager(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public void Set(string key, string value)
    {
        _contextAccessor.HttpContext.Response.Cookies
            .Append(key, value);
    }

    public string Get(string key)
    {
        return _contextAccessor.HttpContext.Request.Cookies[key];
    }

    public void Remove(string key)
    {
        _contextAccessor.HttpContext?.Response.Cookies
            .Delete(key);
    }
}