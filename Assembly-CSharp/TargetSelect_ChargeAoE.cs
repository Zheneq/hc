using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect_ChargeAoE : GenericAbility_TargetSelectBase
{
	[Separator("Targeting Properties", true)]
	public float m_radiusAroundStart = 2f;

	public float m_radiusAroundEnd = 2f;

	public float m_rangeFromLine = 2f;

	public bool m_trimPathOnTargetHit;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public bool m_seqUseTrimmedDestAsTargetPos;

	private int m_maxTargets;

	private TargetSelectMod_ChargeAoE m_targetSelMod;

	public override string GetUsageForEditor()
	{
		return "Intended for single click charge abilities, with line and AoE on either end.\n" + GetContextUsageStr(ContextKeys.s_InEndAoe.GetName(), "on hit actor, 1 if in AoE near end of laser, 0 otherwise") + GetContextUsageStr(ContextKeys.s_ChargeEndPos.GetName(), "non-actor specific, charge end position", false);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.s_InEndAoe.GetName());
		names.Add(ContextKeys.s_ChargeEndPos.GetName());
	}

	public float GetRadiusAroundStart()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_radiusAroundStartMod.GetModifiedValue(m_radiusAroundStart);
		}
		else
		{
			result = m_radiusAroundStart;
		}
		return result;
	}

	public float GetRadiusAroundEnd()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_radiusAroundEndMod.GetModifiedValue(m_radiusAroundEnd);
		}
		else
		{
			result = m_radiusAroundEnd;
		}
		return result;
	}

	public float GetRangeFromLine()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_rangeFromLineMod.GetModifiedValue(m_rangeFromLine);
		}
		else
		{
			result = m_rangeFromLine;
		}
		return result;
	}

	public bool TrimPathOnTargetHit()
	{
		bool result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_trimPathOnTargetHitMod.GetModifiedValue(m_trimPathOnTargetHit);
		}
		else
		{
			result = m_trimPathOnTargetHit;
		}
		return result;
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(ability, GetRadiusAroundStart(), GetRadiusAroundEnd(), GetRangeFromLine(), m_maxTargets, false, IgnoreLos());
		abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
		abilityUtil_Targeter_ChargeAoE.TrimPathOnTargetHit = TrimPathOnTargetHit();
		abilityUtil_Targeter_ChargeAoE.ForceAddTargetingActor = IncludeCaster();
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		list.Add(abilityUtil_Targeter_ChargeAoE);
		return list;
	}

	public override bool HandleCustomTargetValidation(Ability ability, ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (boardSquareSafe != null && boardSquareSafe.IsValidForGameplay())
		{
			if (boardSquareSafe != caster.GetCurrentBoardSquare())
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						int numSquaresInPath;
						return KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out numSquaresInPath);
					}
					}
				}
			}
		}
		return false;
	}

	public override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public static BoardSquare GetTrimOnHitDestination(AbilityTarget currentTarget, BoardSquare startSquare, float lineHalfWidthInSquares, ActorData caster, List<Team> relevantTeams, bool forServer)
	{
		BoardSquare boardSquare = Board.Get().GetSquare(currentTarget.GridPos);
		bool collision;
		Vector3 collisionNormal;
		Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(caster, startSquare.ToVector3(), boardSquare.ToVector3(), out collision, out collisionNormal);
		if (collision)
		{
			boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(startSquare.ToVector3(), abilityLineEndpoint);
		}
		BoardSquarePathInfo chargePath = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquare, startSquare, false);
		TrimChargePathOnActorHit(chargePath, startSquare, lineHalfWidthInSquares, caster, relevantTeams, forServer, out BoardSquare destSquare);
		return destSquare;
	}

	public static void TrimChargePathOnActorHit(BoardSquarePathInfo chargePath, BoardSquare startSquare, float lineHalfWidthInSquares, ActorData caster, List<Team> relevantTeams, bool forServer, out BoardSquare destSquare)
	{
		destSquare = startSquare;
		if (chargePath == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (chargePath == null)
		{
			return;
		}
		while (true)
		{
			if (chargePath.next == null)
			{
				return;
			}
			while (true)
			{
				if (!(lineHalfWidthInSquares > 0f))
				{
					return;
				}
				while (true)
				{
					destSquare = chargePath.GetPathEndpoint().square;
					Vector3 worldPositionForLoS = startSquare.GetOccupantLoSPos();
					Vector3 worldPositionForLoS2 = destSquare.GetOccupantLoSPos();
					List<ActorData> actors = AreaEffectUtils.GetActorsInBoxByActorRadius(worldPositionForLoS, worldPositionForLoS2, 2f * lineHalfWidthInSquares, false, caster, relevantTeams);
					actors.Remove(caster);
					if (forServer)
					{
					}
					else
					{
						TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
					}
					Vector3 vector = worldPositionForLoS2 - worldPositionForLoS;
					vector.y = 0f;
					vector.Normalize();
					TargeterUtils.SortActorsByDistanceToPos(ref actors, worldPositionForLoS, vector);
					if (actors.Count <= 0)
					{
						return;
					}
					while (true)
					{
						ActorData actorData = actors[0];
						Vector3 projectionPoint = VectorUtils.GetProjectionPoint(vector, worldPositionForLoS, actorData.GetTravelBoardSquareWorldPositionForLos());
						BoardSquarePathInfo next = chargePath.next;
						float num = VectorUtils.HorizontalPlaneDistInWorld(projectionPoint, next.square.ToVector3());
						while (true)
						{
							if (next.next == null)
							{
								return;
							}
							float num2 = VectorUtils.HorizontalPlaneDistInWorld(projectionPoint, next.next.square.ToVector3());
							if (num2 > num)
							{
								if (next.square.IsValidForGameplay())
								{
									break;
								}
							}
							num = num2;
							next = next.next;
						}
						next.next.prev = null;
						next.next = null;
						destSquare = next.square;
						return;
					}
				}
			}
		}
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		m_targetSelMod = (modBase as TargetSelectMod_ChargeAoE);
	}

	protected override void OnTargetSelModRemoved()
	{
		m_targetSelMod = null;
	}
}
