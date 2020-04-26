using System.Collections.Generic;

public class ClashAtEndOfEvade
{
	public List<ActorData> m_participants;

	public BoardSquare m_clashSquare;

	public ClashAtEndOfEvade(List<ActorData> participants, BoardSquare clashSquare)
	{
		m_participants = participants;
		m_clashSquare = clashSquare;
	}
}
