# Generic Pattern Explanation: Eliminating Casting

## The Problem We Solved

Previously, each handler had to cast from `PersonBaseDto` to the specific type:

```csharp
protected override Task<string> HandleCoreAsync(PersonBaseDto person)
{
    var employee = (EmployeeDto)person;  // ❌ Manual casting required
    // ...
}
```

## The Generic Solution

We implemented a **two-level interface hierarchy** using generics:

### 1. Non-Generic Marker Interface

```csharp
public interface IPersonHandler
{
    PersonType HandlesType { get; }
    Task<string> HandleAsync(PersonBaseDto person);
}
```

**Purpose**: Allows the resolver to store all handlers in a heterogeneous collection (`Dictionary<PersonType, IPersonHandler>`).

### 2. Generic Interface

```csharp
public interface IPersonHandler<TDto> : IPersonHandler 
    where TDto : PersonBaseDto
{
    Task<string> HandleAsync(TDto person);
}
```

**Purpose**: Provides strongly-typed method signature for derived classes.

### 3. Generic Base Class

```csharp
public abstract class PersonHandlerBase<TDto> : IPersonHandler<TDto> 
    where TDto : PersonBaseDto
{
    // Implements non-generic interface (for resolver)
    public async Task<string> HandleAsync(PersonBaseDto person)
    {
        ValidateCommonProperties(person);
        return await HandleAsync((TDto)person);  // ⚠️ One cast here
    }

    // Implements generic interface (for type safety)
    public async Task<string> HandleAsync(TDto person)
    {
        ValidateCommonProperties(person);
        return await HandleCoreAsync(person);  // ✅ No cast needed
    }

    // Derived classes implement this
    protected abstract Task<string> HandleCoreAsync(TDto person);
}
```

**Key Points**:
- Base class implements BOTH interfaces
- Casting happens **once** in the base class (not in every handler)
- Derived classes work with strongly-typed `TDto` parameter

### 4. Concrete Handlers (No Casting!)

```csharp
public sealed class EmployeeHandler : PersonHandlerBase<EmployeeDto>
{
    public override PersonType HandlesType => PersonType.Employee;

    protected override Task<string> HandleCoreAsync(EmployeeDto employee)
    {
        // ✅ No casting! employee is already EmployeeDto
        return Task.FromResult($"Employee: {employee.Name}...");
    }
}
```

## How It Works

### Type Flow Diagram

```
Program.cs → Resolver
    ↓
IPersonHandler (non-generic)
    ↓
PersonHandlerBase<TDto>.HandleAsync(PersonBaseDto)
    ↓ (single cast)
PersonHandlerBase<TDto>.HandleAsync(TDto)
    ↓ (no cast)
ConcreteHandler.HandleCoreAsync(TDto)
```

### Example: Employee Request

1. **Program.cs** calls resolver with `PersonType.Employee`
2. **Resolver** returns `IPersonHandler` (non-generic interface)
3. **Endpoint** calls `HandleAsync(PersonBaseDto)` on the handler
4. **Base class** casts `PersonBaseDto` → `EmployeeDto` (ONE TIME)
5. **Base class** calls generic `HandleAsync(EmployeeDto)`
6. **Derived handler** receives strongly-typed `EmployeeDto` (NO CAST)

## Benefits

### ✅ Type Safety
```csharp
// Before: Could accidentally access wrong property after bad cast
var employee = (EmployeeDto)person;  // What if person is actually StudentDto?

// After: Compiler guarantees correct type
protected override Task<string> HandleCoreAsync(EmployeeDto employee)
// employee is ALWAYS EmployeeDto - compiler enforced!
```

### ✅ No Repetition
```csharp
// Before: Every handler had casting code
var employee = (EmployeeDto)person;
var manager = (ManagerDto)person;
var student = (StudentDto)person;

// After: Casting centralized in base class
// Derived classes just receive the correct type
```

### ✅ IntelliSense Support
```csharp
// Before: IDE doesn't know specific type
person.EmployeeNumber  // ❌ Not available on PersonBaseDto

// After: IDE knows exact type
employee.EmployeeNumber  // ✅ IntelliSense shows all EmployeeDto properties
```

### ✅ Compile-Time Errors
```csharp
// If you try to access wrong property:
protected override Task<string> HandleCoreAsync(EmployeeDto employee)
{
    var gpa = employee.Gpa;  // ❌ Compile error - EmployeeDto has no Gpa property
}
```

## Pattern Comparison

### Before (Casting in Each Handler)
```csharp
public abstract class PersonHandlerBase : IPersonHandler
{
    protected abstract Task<string> HandleCoreAsync(PersonBaseDto person);
}

public sealed class EmployeeHandler : PersonHandlerBase
{
    protected override Task<string> HandleCoreAsync(PersonBaseDto person)
    {
        var employee = (EmployeeDto)person;  // ❌ Cast required
        return Task.FromResult($"{employee.Name}...");
    }
}
```

**Issues**:
- Casting in every handler
- No compile-time type safety
- Runtime cast exceptions possible

### After (Generic Base Class)
```csharp
public abstract class PersonHandlerBase<TDto> : IPersonHandler<TDto>
    where TDto : PersonBaseDto
{
    protected abstract Task<string> HandleCoreAsync(TDto person);
}

public sealed class EmployeeHandler : PersonHandlerBase<EmployeeDto>
{
    protected override Task<string> HandleCoreAsync(EmployeeDto employee)
    {
        // ✅ No cast - employee is already correct type
        return Task.FromResult($"{employee.Name}...");
    }
}
```

**Advantages**:
- No casting in derived classes
- Compile-time type safety
- Better IDE support
- One cast location (base class)

## Why Two Interfaces?

### Non-Generic Interface (IPersonHandler)
**Purpose**: Collection compatibility

```csharp
// Resolver needs to store different handler types in one dictionary
Dictionary<PersonType, IPersonHandler> _handlers;

// This only works with a non-generic interface:
_handlers[PersonType.Employee] = new EmployeeHandler();    // PersonHandlerBase<EmployeeDto>
_handlers[PersonType.Manager] = new ManagerHandler();      // PersonHandlerBase<ManagerDto>
_handlers[PersonType.Student] = new StudentHandler();      // PersonHandlerBase<StudentDto>

// If we only had IPersonHandler<TDto>, we couldn't store them together:
// ❌ Dictionary<PersonType, IPersonHandler<???>>;  // What type goes here?
```

### Generic Interface (IPersonHandler<TDto>)
**Purpose**: Type-safe handler implementation

```csharp
// Each handler specifies its exact type:
EmployeeHandler : PersonHandlerBase<EmployeeDto>  // Works with EmployeeDto
ManagerHandler  : PersonHandlerBase<ManagerDto>   // Works with ManagerDto
StudentHandler  : PersonHandlerBase<StudentDto>   // Works with StudentDto
```

## Adding New Person Type

With generics, adding a new type is even cleaner:

```csharp
// 1. Create DTO
public sealed class ContractorDto : PersonBaseDto
{
    public required decimal HourlyRate { get; init; }
}

// 2. Create Handler (no casting needed!)
public sealed class ContractorHandler : PersonHandlerBase<ContractorDto>
{
    public override PersonType HandlesType => PersonType.Contractor;

    protected override Task<string> HandleCoreAsync(ContractorDto contractor)
    {
        // ✅ contractor is already ContractorDto - no cast!
        return Task.FromResult($"Contractor: {contractor.Name}, Rate: ${contractor.HourlyRate}");
    }
}

// 3. Register in DI
builder.Services.AddSingleton<IPersonHandler, ContractorHandler>();
```

## Summary

| Aspect | Before | After (Generics) |
|--------|--------|------------------|
| **Casting in handlers** | Every handler | None |
| **Casting location** | Distributed | Centralized (base class) |
| **Type safety** | Runtime only | Compile-time |
| **IntelliSense** | Limited | Full support |
| **Error detection** | Runtime | Compile-time |
| **Code duplication** | High | None |
| **Maintainability** | Lower | Higher |

The generic pattern provides **compile-time type safety** while maintaining the ability to store all handlers in a heterogeneous collection for the resolver pattern! 🎉
