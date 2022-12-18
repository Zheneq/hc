using System.Collections.Generic;

public class Passive_Ninja : Passive
{
	public class NinjaRewindMemoryEntry
	{
		public int m_turn;
		public int m_health;
		public int m_squareX;
		public int m_squareY;
		public List<int> m_abilityCooldowns = new List<int>();

		public NinjaRewindMemoryEntry(int turn, int health, BoardSquare square, AbilityData abilityData)
		{
			m_turn = turn;
			m_health = health;
			m_squareX = square != null ? square.x : -1;
			m_squareY = square != null ? square.y : -1;
			for (int i = 0; i < 5; i++)
			{
				m_abilityCooldowns.Add(abilityData != null
					? abilityData.GetCooldownRemaining((AbilityData.ActionType)i)
					: 0);
			}
		}
	}

	public int m_numRewindTurns = 1;
}
