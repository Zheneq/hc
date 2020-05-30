using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TargetSelect_LaserTargetedPull : GenericAbility_TargetSelectBase
{
	[Separator("Targeting Properties", true)]
	public float m_laserRange = 5f;

	public float m_laserWidth = 1f;

	public int m_maxTargets = 1;

	public float m_maxKnockbackDist = 50f;

	[Separator("For Pull Destination", true)]
	public bool m_casterSquareValidForKnockback = true;

	public float m_squareRangeFromCaster = 3f;

	public float m_destinationAngleDegWithBack = 360f;

	public bool m_destRequireLosFromCaster = true;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private TargetSelectMod_LaserTargetedPull m_targetSelMod;

	public override string GetUsageForEditor()
	{
		return string.Empty;
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(ability, GetLaserWidth(), GetLaserRange(), false, GetMaxTargets(), IncludeAllies(), IncludeCaster());
		abilityUtil_Targeter_Laser.SetUseMultiTargetUpdate(true);
		list.Add(abilityUtil_Targeter_Laser);
		AbilityUtil_Targeter_RampartGrab abilityUtil_Targeter_RampartGrab = new AbilityUtil_Targeter_RampartGrab(ability, AbilityAreaShape.SingleSquare, GetMaxKnockbackDist(), KnockbackType.PullToSource, GetLaserRange(), m_laserWidth, false, GetMaxTargets());
		abilityUtil_Targeter_RampartGrab.SetUseMultiTargetUpdate(true);
		list.Add(abilityUtil_Targeter_RampartGrab);
		return list;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		m_targetSelMod = (modBase as TargetSelectMod_LaserTargetedPull);
	}

	protected override void OnTargetSelModRemoved()
	{
		m_targetSelMod = null;
	}

	public float GetLaserRange()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
		}
		else
		{
			result = m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		return (m_targetSelMod == null) ? m_maxTargets : m_targetSelMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
	}

	public float GetMaxKnockbackDist()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_maxKnockbackDistMod.GetModifiedValue(m_maxKnockbackDist);
		}
		else
		{
			result = m_maxKnockbackDist;
		}
		return result;
	}

	public float GetSquareRangeFromCaster()
	{
		return (m_targetSelMod == null) ? m_squareRangeFromCaster : m_targetSelMod.m_squareRangeFromCasterMod.GetModifiedValue(m_squareRangeFromCaster);
	}

	public float GetDestinationAngleDegWithBack()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_destinationAngleDegWithBackMod.GetModifiedValue(m_destinationAngleDegWithBack);
		}
		else
		{
			result = m_destinationAngleDegWithBack;
		}
		return result;
	}

	public override bool HandleCustomTargetValidation(Ability ability, ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex > 0)
		{
			BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
			BoardSquare currentBoardSquare = caster.GetCurrentBoardSquare();
			if (boardSquareSafe == currentBoardSquare)
			{
				while (true)
				{
					return m_casterSquareValidForKnockback;
				}
			}
			if (GetSquareRangeFromCaster() > 0f)
			{
				if (currentBoardSquare.HorizontalDistanceInSquaresTo(boardSquareSafe) > GetSquareRangeFromCaster())
				{
					return false;
				}
			}
			if (m_destRequireLosFromCaster)
			{
				if (!currentBoardSquare._0013(boardSquareSafe.x, boardSquareSafe.y))
				{
					return false;
				}
			}
			Vector3 from = -1f * currentTargets[0].AimDirection;
			Vector3 to = boardSquareSafe.ToVector3() - caster.GetTravelBoardSquareWorldPosition();
			from.y = 0f;
			to.y = 0f;
			int num = Mathf.RoundToInt(Vector3.Angle(from, to));
			if ((float)num > GetDestinationAngleDegWithBack())
			{
				return false;
			}
			if (NetworkClient.active)
			{
				List<ActorData> visibleActorsInRangeByTooltipSubject = ability.Targeters[0].GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
				if (visibleActorsInRangeByTooltipSubject.Count > 0)
				{
					bool flag = false;
					for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
					{
						if (flag)
						{
							break;
						}
						BoardSquare currentBoardSquare2 = visibleActorsInRangeByTooltipSubject[i].GetCurrentBoardSquare();
						flag = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, currentBoardSquare2, false, out int _);
					}
					if (!flag)
					{
						return false;
					}
				}
			}
		}
		return base.HandleCustomTargetValidation(ability, caster, target, targetIndex, currentTargets);
	}

	private List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, out List<Vector3> targetPosForSequences, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		targetPosForSequences = new List<Vector3>();
		Vector3 laserEndPos;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(caster.GetTravelBoardSquareWorldPositionForLos(), targets[0].AimDirection, GetLaserRange(), GetLaserWidth(), caster, TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies()), false, GetMaxTargets(), false, true, out laserEndPos, nonActorTargetInfo);
		targetPosForSequences.Add(laserEndPos);
		return actorsInLaser;
	}
}
