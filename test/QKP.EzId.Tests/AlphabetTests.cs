using FluentAssertions;

namespace QKP.EzId.Tests;

public class AlphabetTests
{
    [Fact]
    public void Given_valid_alphabet_when_creating_instance_then_characters_array_matches_input()
    {
        // Given
        const string alphabetString = "0123456789ABCDEF";

        // When
        var alphabet = new Alphabet(alphabetString);

        // Then
        alphabet.Characters.Should().BeEquivalentTo(alphabetString.ToCharArray());
    }

    [Fact]
    public void Given_valid_alphabet_when_creating_instance_then_reverse_table_contains_correct_indices()
    {
        // Given
        const string alphabetString = "0123456789ABCDEF";

        // When
        var alphabet = new Alphabet(alphabetString);

        // Then
        for (int i = 0; i < alphabetString.Length; i++)
        {
            alphabet.ReverseTable[alphabetString[i]].Should().Be(i);
        }
    }

    [Fact]
    public void Given_empty_string_when_creating_instance_then_collections_are_empty()
    {
        // Given
        const string emptyAlphabet = "";

        // When
        var alphabet = new Alphabet(emptyAlphabet);

        // Then
        alphabet.Characters.Should().BeEmpty();
        alphabet.ReverseTable.Should().BeEmpty();
    }

    [Fact]
    public void Given_specific_alphabet_when_looking_up_indices_then_returns_correct_positions()
    {
        // Given
        const string alphabetString = "XYZ123";
        var alphabet = new Alphabet(alphabetString);

        // When/Then
        alphabet.ReverseTable['X'].Should().Be(0);
        alphabet.ReverseTable['Y'].Should().Be(1);
        alphabet.ReverseTable['Z'].Should().Be(2);
        alphabet.ReverseTable['1'].Should().Be(3);
        alphabet.ReverseTable['2'].Should().Be(4);
        alphabet.ReverseTable['3'].Should().Be(5);
    }

    [Fact]
    public void Given_alphabet_with_duplicates_when_creating_instance_then_maps_to_first_occurrences()
    {
        // Given
        const string alphabetWithDuplicates = "AABBCC";

        // When
        var alphabet = new Alphabet(alphabetWithDuplicates);

        // Then
        alphabet.ReverseTable['A'].Should().Be(0);
        alphabet.ReverseTable['B'].Should().Be(2);
        alphabet.ReverseTable['C'].Should().Be(4);
        alphabet.ReverseTable.Count.Should().Be(3);
    }
}
