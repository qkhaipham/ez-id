namespace QKP.EzId;

/// <summary>
/// Represents an alphabet of characters that can be used in an encoding.
/// </summary>
public record Alphabet
{
    /// <summary>
    /// An array of characters that can be used in this alphabet representation.
    /// </summary>
    public char[] Characters { get; } = [];

    /// <summary>
    /// A dictionary that holds the integer value of a character in the alphabet.
    /// </summary>
    public Dictionary<char, int> ReverseTable { get; } = [];

    public Alphabet(string alphabet)
    {
        Characters = alphabet.ToCharArray();
        SetReverseTable();
    }

    private void SetReverseTable()
    {
        for (int i = 0; i < Characters.Length; i++)
        {
            ReverseTable.Add(Characters[i], i);
        }
    }
}
