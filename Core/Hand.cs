using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    /// <summary>
    /// Represents player's hand containing cards
    /// 
    /// Basically just a wrapper for list
    /// </summary>
    public class Hand
    {
        private List<Card> _cards;

        public Hand(IEnumerable<Card> cards)
        {
            _cards = cards.ToList();
        }

        public bool HasCard(Card card)
        {
            return _cards.Contains(card);
        }

        public bool HasType(CardType color)
        {
            var types = _cards.Select(card => card.Type);
            return types.Contains(color);
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
        }

        public Card RemoveCard(Card card)
        {
            if (!_cards.Remove(card))
            {
                throw new ArgumentException("Trying to remove card that doesn't exist!");
            };
            return card;
        }

        public Card[] Cards()
        {
            return _cards.ToArray();
        }
    }
}
