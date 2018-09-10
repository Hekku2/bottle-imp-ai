using System.Linq;

namespace Core.Rules
{
	public static class InitialMoveRule
	{
		/// <summary>
		/// Verifies that cards given by player are not duplicate and that player has those cards
		/// </summary>
		/// <param name="hand">Player's cards</param>
		/// <param name="initialMove">Players attempted move</param>
		/// <returns>True, if move is legal</returns>
		public static bool IsLegal(Hand hand, InitialMove initialMove)
		{
			//Check that cards are different
			var givenCards = new[] { initialMove.ToBottle, initialMove.ToLeft, initialMove.ToRight };
			if (givenCards.Distinct().Count() != givenCards.Length)
			{
				return false;
			}

			//Check that player actually has those cards
			return givenCards.All(hand.HasCard);
		}
	}
}
