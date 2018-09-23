using Core.External;
using System.Collections.Generic;

public interface IGameAi
{
	/// <summary>
	/// Game setup information
	/// </summary>
	void GameStart(int numberOfPlayers, int numberOfRounds, int seatAtTable);

	/// <summary>
	/// Initial cards at the start of the round
	/// </summary>
	/// <returns>Cards that the algorithm gives away</returns>
	InitialMove RoundStart(IEnumerable<Card> cards);

	/// <summary>
	/// Cards received after round start
	/// </summary>
	/// <param name="fromLeft">Card received from left</param>
	/// <param name="fromRight">Card received from right</param>
	void ReceivedCards(Card fromLeft, Card fromRight);

	Card Play(Card[] cardsPlayed);

	void TrickFinished(Core.External.TrickResult result);

	/// <summary>
	/// This is called when round ends
	/// </summary>
	/// <param name="scores">Score each seat received</param>
	void RoundEnded(int[] scores);
}