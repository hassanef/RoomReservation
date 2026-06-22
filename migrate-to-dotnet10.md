# Feature: Migrate solution from .NET Core 3.1 to .NET 10

## Requirements

Upgrade the entire solution from .NET Core 3.1 to .NET 10.

### Acceptance Criteria

* All projects target .NET 10.
* The solution builds successfully.
* Existing functionality continues to work.
* All unit tests pass.
* Deprecated APIs are replaced.
* NuGet packages are updated to compatible versions.

## Entities

* Web API project
* Application layer
* Domain layer
* Infrastructure layer
* Test projects

## Approach

Perform the migration incrementally.

1. Analyze the existing solution and identify all projects.
2. Update target frameworks.
3. Update NuGet packages.
4. Resolve compilation errors.
5. Replace obsolete APIs.
6. Run tests and verify behavior.

## Structure

The migration may affect:

* API layer
* Application layer
* Infrastructure layer
* Test projects
* Build configuration files

## Operations

1. Inspect all `.csproj` files.
2. Identify packages incompatible with .NET 10.
3. Update `TargetFramework` values.
4. Upgrade packages.
5. Build the solution.
6. Fix compilation errors.
7. Run tests.
8. Document breaking changes.

## Norms

* Preserve Clean Architecture boundaries.
* Do not change business behavior unless necessary.
* Prefer minimal changes.
* Keep public APIs backward compatible if possible.

## Safeguards

* Do not remove existing features.
* Do not introduce architectural changes unrelated to the migration.
* Document all breaking changes and assumptions.
