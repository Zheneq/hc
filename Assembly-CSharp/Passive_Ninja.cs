using System;
using System.Collections.Generic;

public class Passive_Ninja : Passive
{
	public int m_numRewindTurns = 1;

	public class NinjaRewindMemoryEntry
	{
		public int m_turn;

		public int m_health;

		public int m_squareX;

		public int m_squareY;

		public List<int> m_abilityCooldowns = new List<int>();

		public NinjaRewindMemoryEntry(int turn, int health, BoardSquare square, AbilityData abilityData)
		{
			this.m_turn = turn;
			this.m_health = health;
			int squareX;
			if (square != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(Passive_Ninja.NinjaRewindMemoryEntry..ctor(int, int, BoardSquare, AbilityData)).MethodHandle;
				}
				squareX = square.x;
			}
			else
			{
				squareX = -1;
			}
			this.m_squareX = squareX;
			int squareY;
			if (square != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				squareY = square.y;
			}
			else
			{
				squareY = -1;
			}
			this.m_squareY = squareY;
			for (int i = 0; i < 5; i++)
			{
				List<int> abilityCooldowns = this.m_abilityCooldowns;
				int item;
				if (abilityData != null)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					item = abilityData.GetCooldownRemaining((AbilityData.ActionType)i);
				}
				else
				{
					item = 0;
				}
				abilityCooldowns.Add(item);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}
}
