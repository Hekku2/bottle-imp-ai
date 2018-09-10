namespace Core.Rules
{
	public static class IsWinningRule
	{
		public static bool Wins(int bottlePrice, Card currentWinner, Card challenger)
		{
			if (currentWinner == null)
			{
				return true;
			}

			if (currentWinner.Number > bottlePrice)
			{
				if (challenger.Number < bottlePrice)
				{
					return true;
				}
				if (challenger.Number > currentWinner.Number)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
			else
			{
				if (challenger.Number > bottlePrice)
				{
					return false;
				}

				if (challenger.Number > currentWinner.Number)
				{
					return true;
				}
				return false;
			}
		}
	}
}
