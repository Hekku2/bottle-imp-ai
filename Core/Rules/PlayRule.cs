namespace Core.Rules
{
	public static class PlayRule
	{
		public static bool IsLegal(Hand hand, Card card, Card start)
		{
			if (!hand.HasCard(card))
			{
				return false;
			}

			if (start == null)
			{
				return true;
			}
			
			// User must play same type if available
			return card.Type == start.Type || !hand.HasType(start.Type);
		}
	}
}
