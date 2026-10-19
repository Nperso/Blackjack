

namespace Blackjack
{
    public class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }
        public int Value { get; set; }
        public string Name { get; set; }

        public Card(string suit, string rank, int value)
        {
            Suit = suit;
            Rank = rank;
            Value = value;
            Name = Rank + " of " + Suit;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
