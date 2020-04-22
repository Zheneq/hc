using System.Collections.Generic;
using UnityEngine;

public class NPCBrain_Tutorial : NPCBrain
{
	public enum AttackPattern
	{
		AlwaysAttackPlayer,
		AttackPlayerIfInLoS,
		AttackNearestEnemy,
		NeverAttack
	}

	public enum MovementPattern
	{
		StayStill,
		MoveOncePlayerInLoS,
		MoveAsap
	}

	public AttackPattern m_attackPattern = AttackPattern.AttackPlayerIfInLoS;

	private Transform m_destination;

	public MovementPattern m_movementPattern;

	public float m_attackRange = 5f;

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Tutorial nPCBrain_Tutorial = bot.gameObject.AddComponent<NPCBrain_Tutorial>();
		nPCBrain_Tutorial.m_attackPattern = m_attackPattern;
		nPCBrain_Tutorial.m_movementPattern = m_movementPattern;
		nPCBrain_Tutorial.m_destination = destination;
		nPCBrain_Tutorial.m_attackRange = m_attackRange;
		return nPCBrain_Tutorial;
	}

	public bool IsPlayerInLoS()
	{
		ActorData component = GetComponent<ActorData>();
		bool flag = component.IsVisibleToOpposingTeam();
		ActorData localPlayer = SinglePlayerManager.Get().GetLocalPlayer();
		if (localPlayer == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return false;
			}
		}
		ActorStatus component2 = localPlayer.GetComponent<ActorStatus>();
		int num;
		if (localPlayer.IsHiddenInBrush())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!component2.HasStatus(StatusType.CantHideInBrush))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num = (component2.HasStatus(StatusType.Revealed) ? 1 : 0);
				goto IL_007a;
			}
		}
		num = 1;
		goto IL_007a;
		IL_007a:
		bool flag2 = (byte)num != 0;
		int result;
		if (flag)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			result = (flag2 ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public ActorData FindNearestEnemy()
	{
		ActorData result = null;
		ActorData component = GetComponent<ActorData>();
		BoardSquare currentBoardSquare = component.GetCurrentBoardSquare();
		float num = float.MaxValue;
		List<ActorData> actors = GameFlowData.Get().GetActors();
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current.GetTeam() != component.GetTeam())
				{
					BoardSquare currentBoardSquare2 = current.GetCurrentBoardSquare();
					if (currentBoardSquare2 != null)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						float num2 = currentBoardSquare.HorizontalDistanceOnBoardTo(currentBoardSquare2);
						if (num2 < num)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							num = num2;
							result = current;
						}
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	public void MoveToDestination()
	{
		if (!(m_destination != null))
		{
			return;
		}
		ActorData component = GetComponent<ActorData>();
		ActorMovement component2 = GetComponent<ActorMovement>();
		ActorTurnSM component3 = GetComponent<ActorTurnSM>();
		BoardSquare boardSquare = Board.Get().GetBoardSquare(m_destination);
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(component2 != null))
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (!(component3 != null))
				{
					return;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					if (!(boardSquare != null))
					{
						return;
					}
					BoardSquare closestMoveableSquareTo = component2.GetClosestMoveableSquareTo(boardSquare);
					if (!(closestMoveableSquareTo != null))
					{
						return;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						if (closestMoveableSquareTo != component.GetCurrentBoardSquare())
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								component3.SelectMovementSquareForMovement(closestMoveableSquareTo);
								return;
							}
						}
						return;
					}
				}
			}
		}
	}
}
