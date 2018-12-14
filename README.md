# friendly-spork

Extensions to integrate Auth0 .NET SDK with ASP.NET Core.

## Status

[![Build status](https://ci.appveyor.com/api/projects/status/bkxboveka5fv24ig/branch/master?svg=true)](https://ci.appveyor.com/project/adriangodong/friendly-spork/branch/master)

[Changelog](CHANGELOG.md)

## Installation/Usage

1. Get it from Nuget

2. For ASP.NET Core, add the following code in your `Startup.ConfigureServices` method:

```
services.AddAuth0ManagementApi(options =>
{
    options.Domain = "your Auth0 domain";
    options.ClientId = "client id";
    options.ClientSecret = "client secret";
});
```

## Why FriendlySpork?

GitHub provided the name when creating the repository.
