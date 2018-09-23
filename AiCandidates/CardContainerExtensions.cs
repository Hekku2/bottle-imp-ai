using System.Collections.Generic;
using System.Linq;

public static class CardContainerExtensions
{
    public static Card LowestCard(this IEnumerable<Card> cards) 
    {
        return cards.OrderBy(card => card.Number).FirstOrDefault();
    }

    public static Card LowestSafeCard(this IEnumerable<Card> cards, int bottlePrice, Card[] cardsPlayed) 
    {
        var bottleTaker = cardsPlayed.Where(card => card.Number < bottlePrice).HighestCard();

        return cards.Where(card =>
            (bottleTaker != null && bottleTaker.Number > card.Number) || 
            card.Number > bottlePrice).LowestCard();
    }

    public static Card LowestWinningCard(this IEnumerable<Card> cards, int currentWinningNumber) 
    {
        return cards.Where(card => card.Number > currentWinningNumber).LowestCard();
    }

    public static Card HighestCard(this IEnumerable<Card> cards) 
    {
        return cards.OrderByDescending(card => card.Number).FirstOrDefault();
    }
}