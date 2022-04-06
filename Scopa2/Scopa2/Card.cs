using System;
using System.Collections.Generic;
using System.Text;

namespace Scopa2
{
    public class Card
    {
        public Card(Rank value, SuitType suit)
        {
            if (!Enum.IsDefined(typeof(SuitType), suit))
                throw new ArgumentOutOfRangeException("suit");
            if (!Enum.IsDefined(typeof(Rank), value))
                throw new ArgumentOutOfRangeException("rank");

            Suit = suit;
            FaceValue = value;
        }

        public enum SuitType
        {
            Clubs,
            Spades,
            Hearts,
            Diamonds
        }

        public enum Rank
        {
            Ace = 1,
            Two,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten
        }
        public Rank FaceValue { get; set; }
        public SuitType Suit { get; set; }
    }
}