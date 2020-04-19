using System;
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
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(ability, this.GetLaserWidth(), this.GetLaserRange(), false, this.GetMaxTargets(), base.IncludeAllies(), base.IncludeCaster());
		abilityUtil_Targeter_Laser.SetUseMultiTargetUpdate(true);
		list.Add(abilityUtil_Targeter_Laser);
		AbilityUtil_Targeter_RampartGrab abilityUtil_Targeter_RampartGrab = new AbilityUtil_Targeter_RampartGrab(ability, AbilityAreaShape.SingleSquare, this.GetMaxKnockbackDist(), KnockbackType.PullToSource, this.GetLaserRange(), this.m_laserWidth, false, this.GetMaxTargets());
		abilityUtil_Targeter_RampartGrab.SetUseMultiTargetUpdate(true);
		list.Add(abilityUtil_Targeter_RampartGrab);
		return list;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_LaserTargetedPull);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}

	public float GetLaserRange()
	{
		float result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_LaserTargetedPull.GetLaserRange()).MethodHandle;
			}
			result = this.m_targetSelMod.m_laserRangeMod.GetModifiedValue(this.m_laserRange);
		}
		else
		{
			result = this.m_laserRange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_LaserTargetedPull.GetLaserWidth()).MethodHandle;
			}
			result = this.m_targetSelMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		return (this.m_targetSelMod == null) ? this.m_maxTargets : this.m_targetSelMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
	}

	public float GetMaxKnockbackDist()
	{
		float result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_LaserTargetedPull.GetMaxKnockbackDist()).MethodHandle;
			}
			result = this.m_targetSelMod.m_maxKnockbackDistMod.GetModifiedValue(this.m_maxKnockbackDist);
		}
		else
		{
			result = this.m_maxKnockbackDist;
		}
		return result;
	}

	public float GetSquareRangeFromCaster()
	{
		return (this.m_targetSelMod == null) ? this.m_squareRangeFromCaster : this.m_targetSelMod.m_squareRangeFromCasterMod.GetModifiedValue(this.m_squareRangeFromCaster);
	}

	public float GetDestinationAngleDegWithBack()
	{
		float result;
		if (this.m_targetSelMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_LaserTargetedPull.GetDestinationAngleDegWithBack()).MethodHandle;
			}
			result = this.m_targetSelMod.m_destinationAngleDegWithBackMod.GetModifiedValue(this.m_destinationAngleDegWithBack);
		}
		else
		{
			result = this.m_destinationAngleDegWithBack;
		}
		return result;
	}

	public override bool HandleCustomTargetValidation(Ability ability, ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex > 0)
		{
			BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
			BoardSquare boardSquare2 = caster.\u0012();
			if (boardSquare == boardSquare2)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TargetSelect_LaserTargetedPull.HandleCustomTargetValidation(Ability, ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
				}
				return this.m_casterSquareValidForKnockback;
			}
			if (this.GetSquareRangeFromCaster() > 0f)
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
				if (boardSquare2.HorizontalDistanceInSquaresTo(boardSquare) > this.GetSquareRangeFromCaster())
				{
					return false;
				}
			}
			if (this.m_destRequireLosFromCaster)
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
				if (!boardSquare2.\u0013(boardSquare.x, boardSquare.y))
				{
					return false;
				}
			}
			Vector3 from = -1f * currentTargets[0].AimDirection;
			Vector3 to = boardSquare.ToVector3() - caster.\u0016();
			from.y = 0f;
			to.y = 0f;
			int num = Mathf.RoundToInt(Vector3.Angle(from, to));
			if ((float)num > this.GetDestinationAngleDegWithBack())
			{
				return false;
			}
			if (NetworkClient.active)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				List<ActorData> visibleActorsInRangeByTooltipSubject = ability.Targeters[0].GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
				if (visibleActorsInRangeByTooltipSubject.Count > 0)
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
					bool flag = false;
					for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
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
						if (flag)
						{
							break;
						}
						BoardSquare startSquare = visibleActorsInRangeByTooltipSubject[i].\u0012();
						int num2;
						flag = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquare, startSquare, false, out num2);
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
		Vector3 item;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(caster.\u0015(), targets[0].AimDirection, this.GetLaserRange(), this.GetLaserWidth(), caster, TargeterUtils.GetRelevantTeams(caster, base.IncludeAllies(), base.IncludeEnemies()), false, this.GetMaxTargets(), false, true, out item, nonActorTargetInfo, null, false, true);
		targetPosForSequences.Add(item);
		return actorsInLaser;
	}
}
