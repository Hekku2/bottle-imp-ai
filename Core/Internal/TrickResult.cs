using Core.Internal;

public class TrickResult
{
	private readonly Card[] _winnerPrice;
	public int BottlePrice { get; }
    public Seat Winner { get; }
    public Seat BottleOwner { get; }

	public TrickResult(int bottlePrice, Seat winner, Seat bottleOwner, Card[] winnerPrice)
	{
		BottlePrice = bottlePrice;
		Winner = winner;
		BottleOwner = bottleOwner;
		_winnerPrice = winnerPrice;
	}

	public Card[] WinnerPrice()
	{           
		return (Card[])_winnerPrice.Clone();
	}
}