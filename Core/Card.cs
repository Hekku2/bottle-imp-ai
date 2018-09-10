using Core;

public class Card
{
    public CardType Type { get; }
    public int Number { get; }
	public int Score { get; }

    public Card(CardType type, int number, int score)
	{
        Type = type;
        Number = number;
		Score = score;
    }

	public override bool Equals(object obj)
	{
		if (!(obj is Card candidate))
		{
			return false;
		}

		return candidate.Type == Type && candidate.Number == Number;
	}

	public override int GetHashCode()
	{
		var hashCode = -603095755;
		hashCode = hashCode * -1521134295 + Type.GetHashCode();
		hashCode = hashCode * -1521134295 + Number.GetHashCode();
		return hashCode;
	}
}