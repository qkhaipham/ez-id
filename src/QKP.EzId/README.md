EzId is a lightweight .NET library for generating unique, sortable, and human-friendly identifiers. It implements a Snowflake-inspired ID generation algorithm that produces 13-character base32 encoded strings.

## Usage example ###

```csharp
using QKP.EzId;

// Create an EzIdGenerator with a unique generator ID (0-1023)
var generator = new EzIdGenerator<EzId>(generatorId: 1);

// Generate a new ID
EzId id = generator.GetNextId();

// Convert to string
string idString = id.ToString(); // Returns a 13-character base32 string

// Parse from string
EzId parsedId = EzId.Parse(idString);
```
