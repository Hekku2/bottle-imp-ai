using System.Collections.Generic;
using System.Linq;

public static class Extensions
{
    public static Card LowestCard(this IEnumerable<Card> cards) 
    {
        return cards.OrderBy(card => card.Number).FirstOrDefault();
    }

    public static Card LowestSafeCard(this IEnumerable<Card> cards, int bottlePrice) 
    {
        return cards.Where(card => card.Number > bottlePrice).LowestCard();
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