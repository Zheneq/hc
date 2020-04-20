using System;
using System.Collections.Generic;
using UnityEngine;

public class NPCBrain_Tutorial : NPCBrain
{
	public NPCBrain_Tutorial.AttackPattern m_attackPattern = NPCBrain_Tutorial.AttackPattern.AttackPlayerIfInLoS;

	private Transform m_destination;

	public NPCBrain_Tutorial.MovementPattern m_movementPattern;

	public float m_attackRange = 5f;

	public override NPCBrain Create(BotController bot, Transform destination)
	{
		NPCBrain_Tutorial npcbrain_Tutorial = bot.gameObject.AddComponent<NPCBrain_Tutorial>();
		npcbrain_Tutorial.m_attackPattern = this.m_attackPattern;
		npcbrain_Tutorial.m_movementPattern = this.m_movementPattern;
		npcbrain_Tutorial.m_destination = destination;
		npcbrain_Tutorial.m_attackRange = this.m_attackRange;
		return npcbrain_Tutorial;
	}

	public bool IsPlayerInLoS()
	{
		ActorData component = base.GetComponent<ActorData>();
		bool flag = component.IsVisibleToOpposingTeam();
		ActorData localPlayer = SinglePlayerManager.Get().GetLocalPlayer();
		if (localPlayer == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NPCBrain_Tutorial.IsPlayerInLoS()).MethodHandle;
			}
			return false;
		}
		ActorStatus component2 = localPlayer.GetComponent<ActorStatus>();
		bool flag2;
		if (localPlayer.IsHiddenInBrush())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!component2.HasStatus(StatusType.CantHideInBrush, true))
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
				flag2 = component2.HasStatus(StatusType.Revealed, true);
				goto IL_7A;
			}
		}
		flag2 = true;
		IL_7A:
		bool flag3 = flag2;
		bool result;
		if (flag)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			result = flag3;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public ActorData FindNearestEnemy()
	{
		ActorData result = null;
		ActorData component = base.GetComponent<ActorData>();
		BoardSquare currentBoardSquare = component.GetCurrentBoardSquare();
		float num = float.MaxValue;
		List<ActorData> actors = GameFlowData.Get().GetActors();
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData.GetTeam() != component.GetTeam())
				{
					BoardSquare currentBoardSquare2 = actorData.GetCurrentBoardSquare();
					if (currentBoardSquare2 != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(NPCBrain_Tutorial.FindNearestEnemy()).MethodHandle;
						}
						float num2 = currentBoardSquare.HorizontalDistanceOnBoardTo(currentBoardSquare2);
						if (num2 < num)
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
							num = num2;
							result = actorData;
						}
					}
				}
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
		return result;
	}

	public void MoveToDestination()
	{
		if (this.m_destination != null)
		{
			ActorData component = base.GetComponent<ActorData>();
			ActorMovement component2 = base.GetComponent<ActorMovement>();
			ActorTurnSM component3 = base.GetComponent<ActorTurnSM>();
			BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_destination);
			if (component != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(NPCBrain_Tutorial.MoveToDestination()).MethodHandle;
				}
				if (component2 != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (component3 != null)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (boardSquare != null)
						{
							BoardSquare closestMoveableSquareTo = component2.GetClosestMoveableSquareTo(boardSquare, true);
							if (closestMoveableSquareTo != null)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (closestMoveableSquareTo != component.GetCurrentBoardSquare())
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
									component3.SelectMovementSquareForMovement(closestMoveableSquareTo);
								}
							}
						}
					}
				}
			}
		}
	}

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
}
