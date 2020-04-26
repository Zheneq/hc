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
			int squareX;
			if (square != null)
			{
				squareX = square.x;
			}
			else
			{
				squareX = -1;
			}
			m_squareX = squareX;
			int squareY;
			if (square != null)
			{
				squareY = square.y;
			}
			else
			{
				squareY = -1;
			}
			m_squareY = squareY;
			for (int i = 0; i < 5; i++)
			{
				List<int> abilityCooldowns = m_abilityCooldowns;
				int item;
				if (abilityData != null)
				{
					item = abilityData.GetCooldownRemaining((AbilityData.ActionType)i);
				}
				else
				{
					item = 0;
				}
				abilityCooldowns.Add(item);
			}
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public int m_numRewindTurns = 1;
}
