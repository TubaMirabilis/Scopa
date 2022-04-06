using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scopa2
{
    static class Deck
    {
        static public List<Card> cards;
        static public void CreateDeck()
        {
            cards = new List<Card>();
            for (int suit = (int)Card.SuitType.Clubs; suit <= (int)Card.SuitType.Diamonds; suit++)
            {
                for (int rank = (int)Card.Rank.Ace; rank <= (int)Card.Rank.Ten; rank++)
                {
                    Card card = new Card((Card.Rank)rank, (Card.SuitType)suit);
                    cards.Add(card);
                }
            }
        }
        static public void Shuffle(int numTimes)
        {
            int cardCount = cards.Count();
            Random rand = new Random();

            for (int time = 0; time < numTimes; time++)
            {
                for (var index = 0; index < cardCount; index++)
                {
                    int indexSwapPosition = rand.Next(cardCount);
                    Card temp = cards[indexSwapPosition];
                    cards[indexSwapPosition] = cards[index];
                    cards[index] = temp;
                }
            }

        }
    }
}