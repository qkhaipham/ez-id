using FluentAssertions;

namespace QKP.EzId.Tests
{
    public class AlphabetTests
    {
        [Fact]
        public void Given_valid_alphabet_when_creating_instance_then_characters_array_matches_input()
        {
            const string alphabetString = "0123456789ABCDEF";

            // Act
            var alphabet = new Alphabet(alphabetString);

            // Assert
            alphabet.Characters.Should().BeEquivalentTo(alphabetString.ToCharArray());
        }

        [Fact]
        public void Given_valid_alphabet_when_creating_instance_then_reverse_table_contains_correct_indices()
        {
            const string alphabetString = "0123456789ABCDEF";

            // Act
            var alphabet = new Alphabet(alphabetString);

            // Assert
            for (int i = 0; i < alphabetString.Length; i++)
            {
                alphabet.ReverseTable[alphabetString[i]].Should().Be(i);
            }
        }

        [Fact]
        public void Given_empty_string_when_creating_instance_then_collections_are_empty()
        {
            const string emptyAlphabet = "";

            // Act
            var alphabet = new Alphabet(emptyAlphabet);

            // Assert
            alphabet.Characters.Should().BeEmpty();
            alphabet.ReverseTable.Should().BeEmpty();
        }

        [Fact]
        public void Given_specific_alphabet_when_looking_up_indices_then_returns_correct_positions()
        {
            const string alphabetString = "XYZ123";

            // Act
            var alphabet = new Alphabet(alphabetString);

            // Assert
            alphabet.ReverseTable['X'].Should().Be(0);
            alphabet.ReverseTable['Y'].Should().Be(1);
            alphabet.ReverseTable['Z'].Should().Be(2);
            alphabet.ReverseTable['1'].Should().Be(3);
            alphabet.ReverseTable['2'].Should().Be(4);
            alphabet.ReverseTable['3'].Should().Be(5);
        }
    }
}
