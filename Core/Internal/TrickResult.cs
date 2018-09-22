using Core.Internal;

public class TrickResult
{
    public int BottlePrice { get; set; }
    public Seat Winner { get; set; }
    public Seat BottleOwner { get; set; }
    public Card[] WinnerPrice { get; set; }
}