EzId is a lightweight .NET library for generating unique, sortable, and human-friendly readable identifiers. It implements a 64 bit long ID generation algorithm inspired by Twitter Snowflake
and comes packed with a value type that encodes it in a 13-character base32 string.

## Usage example ###

```csharp
using QKP.EzId;

// Create an EzIdGenerator with a unique generator ID (0-1023)
var generator = new EzIdGenerator<EzId>(generatorId: 1);

// Generate a new ID
EzId id = generator.GetNextId();

// Convert to string
string idString = id.ToString(); // Returns a 13-character base32 string, eg. "07047XF6Q8YPA"

// Parse from string
EzId parsedId = EzId.Parse(idString);
```
