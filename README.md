# EzId

EzId is a lightweight .NET library for generating unique, sortable, and human-friendly readable identifiers that look like "070-47XF6Q8-YPB". It implements a 64 bit long ID generation algorithm inspired by Twitter Snowflake
and comes packed with a value type that encodes it in a 15-character base32 string.

## Features

- Generates unique 64-bit identifiers encoded in Crockford's base32 format
- Thread-safe ID generation
- Supports up to 1024 concurrent generators
- Generates up to 4096 unique IDs per millisecond per generator
- IDs are sortable by creation time
- Human-friendly readable format

## Installation

### Using .NET CLI

```bash
dotnet add package QKP.EzId
```

### Using NuGet Package Manager

1. Open the NuGet Package Manager in Visual Studio
2. Search for `QKP.EzId`
3. Click "Install"

## Usage

### Basic Usage

```csharp
using QKP.EzId;

// Create an EzIdGenerator with a unique generator ID (0-1023)
var generator = new EzIdGenerator<EzId>(generatorId: 1);

// Generate a new ID
EzId id = generator.GetNextId();

// Convert to string
string idString = id.ToString(); // Returns a 15-character base32 string eg. "070-47XF6Q8-YPA"

// Parse from string
EzId parsedId = EzId.Parse(idString);

// Implement your own ID type, if you have multiple entities and want them to have own ID type
public class FooId(long id) : EzId(id)
{
}

// Create an FooIdGenerator with a unique generator ID (0-1023)
var fooGenerator = new EzIdGenerator<FooId>(generatorId: 1);

// Generate a new ID
FooId fooId = fooGenerator.GetNextId();

// Convert to string
string fooIdString = fooId.ToString(); // Returns a 15-character base 32 string eg. "070-47XF6Q8-YPB"

```

### Important: Generator ID

The `generatorId` parameter is crucial for preventing ID collisions across different generators. It must be:

- A unique number between 0 and 1023 (10 bits)
- Consistent for each generator instance
- Different for each concurrent generator in your distributed system

For example:
- In a distributed system, use different generator IDs for each server/node
- In a multi-threaded application, use different generator IDs for each thread
- In a microservice architecture, assign unique generator IDs to each service instance

```csharp
// Example for distributed system
var node1Generator = new EzIdGenerator<EzId>(generatorId: 1);  // For Node 1
var node2Generator = new EzIdGenerator<EzId>(generatorId: 2);  // For Node 2
```

### ID Structure

Each generated ID consists of:
- 1 bit unused 
- 41 bits for timestamp (milliseconds since epoch)
- 10 bits for generator ID (0-1023)
- 12 bits for sequence number (0-4095)

This structure ensures that:
- IDs are sortable by creation time
- Each generator can create up to 4096 unique IDs per millisecond
- Up to 1024 generators can operate concurrently without collisions

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the Apache License - see the [LICENSE](LICENSE) file for details.
