# friendly-spork

Extensions to integrate Auth0 .NET SDK with ASP.NET Core.

## Status

[![Build status](https://ci.appveyor.com/api/projects/status/bkxboveka5fv24ig/branch/master?svg=true)](https://ci.appveyor.com/project/adriangodong/friendly-spork/branch/master)

[Changelog](CHANGELOG.md)

## Installation/Usage

1. Install the [FriendlySpork package from NuGet](https://www.nuget.org/packages/FriendlySpork).

2. Add the following code in your dependency injection logic (`Startup.ConfigureServices` in ASP.NET Core):

```
services.AddAuth0ManagementApi(options =>
{
    options.Domain = "your Auth0 domain";
    options.ClientId = "client id";
    options.ClientSecret = "client secret";
    options.ReuseAccessToken = false;
});
```

## Why FriendlySpork?

GitHub provided the name when creating the repository.
