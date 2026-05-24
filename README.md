# Faithlife.DataAnnotations

**Faithlife.DataAnnotations** provides helpers for working with [DataAnnotations](https://learn.microsoft.com/dotnet/api/system.componentmodel.dataannotations) from `System.ComponentModel`.

[![NuGet](https://img.shields.io/nuget/v/Faithlife.DataAnnotations.svg)](https://www.nuget.org/packages/Faithlife.DataAnnotations)

## Overview

This library includes:

* [ValidatorUtility](./src/Faithlife.DataAnnotations/ValidatorUtility.cs), which validates an object with `Validator.TryValidateObject` or `Validator.ValidateObject` using `validateAllProperties: true`.
* [ValidateObjectAttribute](./src/Faithlife.DataAnnotations/ValidateObjectAttribute.cs), which validates a property value that has its own data annotations.
* [ValidateItemsAttribute](./src/Faithlife.DataAnnotations/ValidateItemsAttribute.cs), which validates each item in a collection property and can optionally allow null items.

## Usage

Use `ValidatorUtility` when you want simple validation helpers for a single object.

```csharp
var results = ValidatorUtility.GetValidationResults(model);
if (results.Count != 0)
{
    throw new ValidationException(results[0].ErrorMessage);
}
```

Use `ValidateObjectAttribute` and `ValidateItemsAttribute` on nested values that should be validated along with their containing object.

```csharp
public sealed class UserRequest
{
    [ValidateObject]
    public AddressRequest? Address { get; set; }

    [ValidateItems]
    public IReadOnlyList<RoleRequest> Roles { get; set; } = [];
}
```

## Contributing

See [Contributing](./CONTRIBUTING.md) for setup and contribution guidelines. For a history of notable changes, see the [Release Notes](./ReleaseNotes.md).
