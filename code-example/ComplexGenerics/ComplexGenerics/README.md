# Enum-Based Polymorphic Service Resolution

A complete ASP.NET 9 Minimal API reference implementation demonstrating the **Strategy Pattern** combined with a **Resolver/Factory Pattern** for elegant, maintainable polymorphic behavior based on enum values.

## 🎯 Architecture Overview

This implementation eliminates conditional logic (`switch`, `if`, `ServiceProvider.GetService`) by leveraging:

1. **Strategy Pattern**: Each person type has its own handler implementing `IPersonHandler`
2. **Resolver Pattern**: Maps `PersonType` enum → handler via dictionary lookup
3. **Dependency Injection**: All handlers are registered in DI; resolver receives them via `IEnumerable<T>`

### Key Design Decisions

- **No switch statements**: Handler resolution uses dictionary lookup (O(1))
- **No ServiceProvider.GetService**: Handlers injected via `IEnumerable<IPersonHandler>`
- **Type-safe polymorphism**: Enum drives both deserialization and handler selection
- **Clean separation**: Common validation + type-specific logic in separate services

## 📁 Project Structure

```
ComplexGenerics/
├── Enums/
│   └── PersonType.cs              # Enum defining person types
├── Dtos/
│   ├── PersonBaseDto.cs           # Base DTO with common properties
│   ├── EmployeeDto.cs             # Employee-specific fields
│   ├── ManagerDto.cs              # Manager-specific fields
│   ├── StudentDto.cs              # Student-specific fields
│   └── PersonRequest.cs           # API request wrapper
├── Services/
│   ├── Common/
│   │   ├── IPersonCommonService.cs    # Shared validation interface
│   │   └── PersonCommonService.cs     # Common validation logic
│   ├── Handlers/
│   │   ├── IPersonHandler.cs          # Strategy interface
│   │   ├── EmployeeHandler.cs         # Employee strategy
│   │   ├── ManagerHandler.cs          # Manager strategy
│   │   └── StudentHandler.cs          # Student strategy
│   └── Resolver/
│       ├── IPersonHandlerResolver.cs   # Resolver interface
│       └── PersonHandlerResolver.cs    # Dictionary-based resolver
├── Tests/
│   ├── ValidEmployee.http         # Happy path: Employee
│   ├── ValidManager.http          # Happy path: Manager
│   ├── ValidStudent.http          # Happy path: Student
│   ├── InvalidEnumValue.http      # Error: Unknown enum
│   ├── PayloadMismatch.http       # Error: Wrong DTO
│   ├── ValidationFailure.http     # Error: Invalid data
│   └── AllTests.http              # Complete test suite
└── Program.cs                     # DI registration + endpoint definition
```

## 🔧 How It Works

### 1. Client Request Flow

```json
{
  "personType": 1,  // Enum value (Employee)
  "payload": {      // Raw JSON (deserialized based on enum)
    "id": "EMP-001",
    "name": "John Doe",
    "age": 35,
    "department": "Engineering",
    "salary": 95000.50,
    "employeeNumber": "E123456"
  }
}
```

### 2. Request Processing Pipeline

```
POST /person/handle
    ↓
1. Deserialize PersonRequest (enum + JsonElement)
    ↓
2. Switch on enum to deserialize correct DTO type
    ↓
3. Validate common properties (age, ID) via IPersonCommonService
    ↓
4. Resolve handler via IPersonHandlerResolver (no switch!)
    ↓
5. Execute handler's type-specific business logic
    ↓
6. Return result
```

### 3. Resolver Pattern Implementation

The resolver builds a dictionary at startup from all registered handlers:

```csharp
public sealed class PersonHandlerResolver : IPersonHandlerResolver
{
    private readonly Dictionary<PersonType, IPersonHandler> _handlers;

    public PersonHandlerResolver(IEnumerable<IPersonHandler> handlers)
    {
        // Automatic discovery via DI - no manual registration needed
        _handlers = handlers.ToDictionary(h => h.HandlesType, h => h);
    }

    public IPersonHandler? Resolve(PersonType personType)
    {
        // O(1) lookup, zero conditional logic
        return _handlers.GetValueOrDefault(personType);
    }
}
```

### 4. Handler Registration (Program.cs)

```csharp
// Register all handlers - resolver auto-discovers them
builder.Services.AddSingleton<IPersonHandler, EmployeeHandler>();
builder.Services.AddSingleton<IPersonHandler, ManagerHandler>();
builder.Services.AddSingleton<IPersonHandler, StudentHandler>();

// Resolver receives IEnumerable<IPersonHandler> via DI
builder.Services.AddSingleton<IPersonHandlerResolver, PersonHandlerResolver>();
```

## 🧪 Testing

Use the provided `.http` files in Visual Studio:

1. **ValidEmployee.http**: Successful employee processing
2. **ValidManager.http**: Successful manager processing
3. **ValidStudent.http**: Successful student processing
4. **InvalidEnumValue.http**: Unknown enum value (400)
5. **PayloadMismatch.http**: Wrong DTO for enum type (400)
6. **ValidationFailure.http**: Common validation errors (400)
7. **AllTests.http**: Complete test suite with edge cases

### Running Tests in Visual Studio

1. Open any `.http` file
2. Click the **Run** button (green play icon) next to each request
3. View response in the adjacent pane

### Example Test Results

**Success Response (200 OK):**
```json
{
  "message": "Employee processed successfully: John Doe (ID: EMP-001, Employee#: E123456) works in Engineering department with salary $95,000.50"
}
```

**Validation Error (400 Bad Request):**
```json
{
  "error": "Age must be between 0 and 150."
}
```

**Payload Mismatch (400 Bad Request):**
```json
{
  "error": "Invalid JSON payload",
  "details": "The JSON value could not be converted to ..."
}
```

## 🚀 Running the Application

```bash
cd ComplexGenerics
dotnet run
```

Navigate to: `https://localhost:7001/person/handle`

Or use the OpenAPI explorer (Development mode): `https://localhost:7001/openapi/v1.json`

## 📊 Benefits of This Pattern

### ✅ Maintainability
- **Add new person type**: Create DTO + Handler, register in DI → done
- **No central switch statement** to maintain
- **Each handler is independent** and testable

### ✅ Performance
- **O(1) handler lookup** via dictionary
- **No reflection** or dynamic type resolution at runtime
- **Singleton services** (no allocation overhead)

### ✅ Type Safety
- **Compile-time safety** for handler implementations
- **Explicit DTO contracts** per person type
- **No casting errors** at runtime

### ✅ SOLID Principles
- **Single Responsibility**: Each handler does one thing
- **Open/Closed**: Add types without modifying existing code
- **Dependency Inversion**: Depend on abstractions (interfaces)

## 🔄 Adding a New Person Type

1. **Add enum value**:
   ```csharp
   public enum PersonType { Employee = 1, Manager = 2, Student = 3, Contractor = 4 }
   ```

2. **Create DTO**:
   ```csharp
   public sealed class ContractorDto : PersonBaseDto
   {
       public required string ContractEndDate { get; init; }
       public required decimal HourlyRate { get; init; }
   }
   ```

3. **Create handler**:
   ```csharp
   public sealed class ContractorHandler : IPersonHandler
   {
       public PersonType HandlesType => PersonType.Contractor;
       public Task<string> HandleAsync(PersonBaseDto person) { /* ... */ }
   }
   ```

4. **Register in DI**:
   ```csharp
   builder.Services.AddSingleton<IPersonHandler, ContractorHandler>();
   ```

5. **Add deserialization case** (only place where switch is needed):
   ```csharp
   PersonBaseDto person = request.PersonType switch
   {
       // ... existing cases
       PersonType.Contractor => request.Payload.Deserialize<ContractorDto>() 
           ?? throw new InvalidOperationException("Failed to deserialize Contractor payload."),
       _ => throw new ArgumentException($"Unsupported PersonType: {request.PersonType}")
   };
   ```

That's it! The resolver automatically discovers the new handler via DI.

## 📝 Design Trade-offs

### Why Not Visitor Pattern?
- **Requirement**: No Visitor pattern (per spec)
- **Strategy + Resolver** is simpler for enum-based dispatch

### Why Switch for Deserialization?
- **Type-safe deserialization** requires knowing target type at compile time
- **Switch is isolated** to one location (endpoint)
- **Handler resolution** remains switch-free (the main architectural goal)

### Why Dictionary Over Attributes?
- **Explicit registration** in DI is clearer
- **No reflection overhead** at runtime
- **Easier to test** and debug

## 🎓 Educational Value

This implementation demonstrates:
- ✅ Strategy Pattern with DI
- ✅ Factory/Resolver Pattern
- ✅ Enum-driven polymorphism
- ✅ Clean Architecture principles
- ✅ ASP.NET 9 Minimal APIs
- ✅ Production-quality error handling
- ✅ Comprehensive testing approach

Perfect for teaching polymorphism, DI patterns, and modern C# API design.

## 📚 Further Reading

- [Strategy Pattern - Refactoring Guru](https://refactoring.guru/design-patterns/strategy)
- [Factory Method Pattern](https://refactoring.guru/design-patterns/factory-method)
- [ASP.NET Core DI Documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)
- [Minimal APIs Overview](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)

---

**License**: MIT  
**Target Framework**: .NET 9  
**Author**: Senior ASP.NET Architect Reference Implementation
