using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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
		return "Intended for single click charge abilities, with line and AoE on either end.\n" + base.GetContextUsageStr(ContextKeys.symbol_0004.GetName(), "on hit actor, 1 if in AoE near end of laser, 0 otherwise", true) + base.GetContextUsageStr(ContextKeys.symbol_0016.GetName(), "non-actor specific, charge end position", false);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.symbol_0004.GetName());
		names.Add(ContextKeys.symbol_0016.GetName());
	}

	public float GetRadiusAroundStart()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_radiusAroundStartMod.GetModifiedValue(this.m_radiusAroundStart);
		}
		else
		{
			result = this.m_radiusAroundStart;
		}
		return result;
	}

	public float GetRadiusAroundEnd()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_radiusAroundEndMod.GetModifiedValue(this.m_radiusAroundEnd);
		}
		else
		{
			result = this.m_radiusAroundEnd;
		}
		return result;
	}

	public float GetRangeFromLine()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_rangeFromLineMod.GetModifiedValue(this.m_rangeFromLine);
		}
		else
		{
			result = this.m_rangeFromLine;
		}
		return result;
	}

	public bool TrimPathOnTargetHit()
	{
		bool result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_trimPathOnTargetHitMod.GetModifiedValue(this.m_trimPathOnTargetHit);
		}
		else
		{
			result = this.m_trimPathOnTargetHit;
		}
		return result;
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(ability, this.GetRadiusAroundStart(), this.GetRadiusAroundEnd(), this.GetRangeFromLine(), this.m_maxTargets, false, base.IgnoreLos());
		abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(base.IncludeEnemies(), base.IncludeAllies(), base.IncludeCaster());
		abilityUtil_Targeter_ChargeAoE.TrimPathOnTargetHit = this.TrimPathOnTargetHit();
		abilityUtil_Targeter_ChargeAoE.ForceAddTargetingActor = base.IncludeCaster();
		return new List<AbilityUtil_Targeter>
		{
			abilityUtil_Targeter_ChargeAoE
		};
	}

	public override bool HandleCustomTargetValidation(Ability ability, ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null && boardSquareSafe.IsBaselineHeight())
		{
			if (boardSquareSafe != caster.GetCurrentBoardSquare())
			{
				int num;
				return KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out num);
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
		BoardSquare boardSquare = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		bool flag;
		Vector3 vector;
		Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(caster, startSquare.ToVector3(), boardSquare.ToVector3(), out flag, out vector, null);
		if (flag)
		{
			boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(startSquare.ToVector3(), abilityLineEndpoint, false, false, float.MaxValue);
		}
		BoardSquarePathInfo chargePath = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquare, startSquare, false);
		BoardSquare result;
		TargetSelect_ChargeAoE.TrimChargePathOnActorHit(chargePath, startSquare, lineHalfWidthInSquares, caster, relevantTeams, forServer, out result);
		return result;
	}

	public unsafe static void TrimChargePathOnActorHit(BoardSquarePathInfo chargePath, BoardSquare startSquare, float lineHalfWidthInSquares, ActorData caster, List<Team> relevantTeams, bool forServer, out BoardSquare destSquare)
	{
		destSquare = startSquare;
		if (chargePath == null)
		{
			return;
		}
		if (chargePath != null)
		{
			if (chargePath.next != null)
			{
				if (lineHalfWidthInSquares > 0f)
				{
					destSquare = chargePath.GetPathEndpoint().square;
					Vector3 worldPositionForLoS = startSquare.GetWorldPositionForLoS();
					Vector3 worldPositionForLoS2 = destSquare.GetWorldPositionForLoS();
					List<ActorData> actorsInBoxByActorRadius = AreaEffectUtils.GetActorsInBoxByActorRadius(worldPositionForLoS, worldPositionForLoS2, 2f * lineHalfWidthInSquares, false, caster, relevantTeams, null, null);
					actorsInBoxByActorRadius.Remove(caster);
					if (forServer)
					{
					}
					else
					{
						TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInBoxByActorRadius);
					}
					Vector3 vector = worldPositionForLoS2 - worldPositionForLoS;
					vector.y = 0f;
					vector.Normalize();
					TargeterUtils.SortActorsByDistanceToPos(ref actorsInBoxByActorRadius, worldPositionForLoS, vector);
					if (actorsInBoxByActorRadius.Count > 0)
					{
						ActorData actorData = actorsInBoxByActorRadius[0];
						Vector3 projectionPoint = VectorUtils.GetProjectionPoint(vector, worldPositionForLoS, actorData.GetTravelBoardSquareWorldPositionForLos());
						BoardSquarePathInfo next = chargePath.next;
						float num = VectorUtils.HorizontalPlaneDistInWorld(projectionPoint, next.square.ToVector3());
						while (next.next != null)
						{
							float num2 = VectorUtils.HorizontalPlaneDistInWorld(projectionPoint, next.next.square.ToVector3());
							if (num2 > num)
							{
								if (next.square.IsBaselineHeight())
								{
									next.next.prev = null;
									next.next = null;
									destSquare = next.square;
									break;
								}
							}
							num = num2;
							next = next.next;
						}
					}
				}
			}
		}
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_ChargeAoE);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}
}
