using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_FanCones : GenericAbility_TargetSelectBase
{
	[Separator("Targeting Properties", true)]
	public ConeTargetingInfo m_coneInfo;

	[Space(10f)]
	public int m_coneCount = 3;

	[Header("Starting offset, move towards forward/aim direction")]
	public float m_coneStartOffsetInAimDir;

	[Header("Starting offset, move towards left/right")]
	public float m_coneStartOffsetToSides;

	[Header("Starting offset, move towards each cone's direction")]
	public float m_coneStartOffsetInConeDir;

	[Header("-- If Fixed Angle")]
	public float m_angleInBetween = 10f;

	[Header("-- If Interpolating Angle")]
	public bool m_changeAngleByCursorDistance = true;

	public float m_targeterMinAngle;

	public float m_targeterMaxAngle = 180f;

	public float m_startAngleOffset;

	[Space(10f)]
	public float m_targeterMinInterpDistance = 0.5f;

	public float m_targeterMaxInterpDistance = 4f;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private TargetSelectMod_FanCones m_targetSelMod;

	private ConeTargetingInfo m_cachedConeInfo;

	public override string GetUsageForEditor()
	{
		return base.GetContextUsageStr(ContextKeys.symbol_0019.GetName(), "on every hit actor, number of cone hits on target", true);
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.symbol_0019.GetName());
	}

	public override void Initialize()
	{
		this.SetCachedFields();
		ConeTargetingInfo coneInfo = this.GetConeInfo();
		coneInfo.m_affectsAllies = base.IncludeAllies();
		coneInfo.m_affectsEnemies = base.IncludeEnemies();
		coneInfo.m_affectsCaster = base.IncludeCaster();
		coneInfo.m_penetrateLos = base.IgnoreLos();
	}

	private void SetCachedFields()
	{
		ConeTargetingInfo cachedConeInfo;
		if (this.m_targetSelMod != null)
		{
			cachedConeInfo = this.m_targetSelMod.m_coneInfoMod.GetModifiedValue(this.m_coneInfo);
		}
		else
		{
			cachedConeInfo = this.m_coneInfo;
		}
		this.m_cachedConeInfo = cachedConeInfo;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return (this.m_cachedConeInfo == null) ? this.m_coneInfo : this.m_cachedConeInfo;
	}

	public int GetConeCount()
	{
		int result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_coneCountMod.GetModifiedValue(this.m_coneCount);
		}
		else
		{
			result = this.m_coneCount;
		}
		return result;
	}

	public float GetConeStartOffsetInAimDir()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_coneStartOffsetInAimDirMod.GetModifiedValue(this.m_coneStartOffsetInAimDir);
		}
		else
		{
			result = this.m_coneStartOffsetInAimDir;
		}
		return result;
	}

	public float GetConeStartOffsetToSides()
	{
		return (this.m_targetSelMod == null) ? this.m_coneStartOffsetToSides : this.m_targetSelMod.m_coneStartOffsetToSidesMod.GetModifiedValue(this.m_coneStartOffsetToSides);
	}

	public float GetConeStartOffsetInConeDir()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_coneStartOffsetInConeDirMod.GetModifiedValue(this.m_coneStartOffsetInConeDir);
		}
		else
		{
			result = this.m_coneStartOffsetInConeDir;
		}
		return result;
	}

	public float GetAngleInBetween()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_angleInBetweenMod.GetModifiedValue(this.m_angleInBetween);
		}
		else
		{
			result = this.m_angleInBetween;
		}
		return result;
	}

	public bool ChangeAngleByCursorDistance()
	{
		bool result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_changeAngleByCursorDistanceMod.GetModifiedValue(this.m_changeAngleByCursorDistance);
		}
		else
		{
			result = this.m_changeAngleByCursorDistance;
		}
		return result;
	}

	public float GetTargeterMinAngle()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_targeterMinAngleMod.GetModifiedValue(this.m_targeterMinAngle);
		}
		else
		{
			result = this.m_targeterMinAngle;
		}
		return result;
	}

	public float GetTargeterMaxAngle()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_targeterMaxAngleMod.GetModifiedValue(this.m_targeterMaxAngle);
		}
		else
		{
			result = this.m_targeterMaxAngle;
		}
		return result;
	}

	public float GetStartAngleOffset()
	{
		float result;
		if (this.m_targetSelMod != null)
		{
			result = this.m_targetSelMod.m_startAngleOffsetMod.GetModifiedValue(this.m_startAngleOffset);
		}
		else
		{
			result = this.m_startAngleOffset;
		}
		return result;
	}

	protected virtual bool UseCasterPosForLoS()
	{
		return false;
	}

	protected virtual bool CustomLoS(ActorData actor, ActorData caster)
	{
		return true;
	}

	public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		AbilityUtil_Targeter_TricksterCones abilityUtil_Targeter_TricksterCones = new AbilityUtil_Targeter_TricksterCones(ability, this.GetConeInfo(), this.GetConeCount(), new AbilityUtil_Targeter_TricksterCones.GetCurrentNumberOfConesDelegate(this.GetConeCount), new AbilityUtil_Targeter_TricksterCones.GetConeInfoDelegate(this.GetConeOrigins), new AbilityUtil_Targeter_TricksterCones.GetConeInfoDelegate(this.GetConeDirections), new AbilityUtil_Targeter_TricksterCones.GetClampedTargetPosDelegate(this.GetFreePosForAim), false, this.UseCasterPosForLoS());
		abilityUtil_Targeter_TricksterCones.m_customDamageOriginDelegate = new AbilityUtil_Targeter_TricksterCones.DamageOriginDelegate(this.GetDamageOriginForTargeter);
		return new List<AbilityUtil_Targeter>
		{
			abilityUtil_Targeter_TricksterCones
		};
	}

	private Vector3 GetDamageOriginForTargeter(AbilityTarget currentTarget, Vector3 defaultOrigin, ActorData actorToAdd, ActorData caster)
	{
		return caster.GetTravelBoardSquareWorldPosition();
	}

	public Vector3 GetFreePosForAim(AbilityTarget currentTarget, ActorData caster)
	{
		return currentTarget.FreePos;
	}

	public virtual List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		Vector3 aimDirection = currentTarget.AimDirection;
		Vector3 normalized = Vector3.Cross(aimDirection, Vector3.up).normalized;
		int coneCount = this.GetConeCount();
		int num = coneCount / 2;
		bool flag = coneCount % 2 == 0;
		float num2 = this.GetConeStartOffsetInAimDir() * Board.SquareSizeStatic;
		float num3 = this.GetConeStartOffsetToSides() * Board.SquareSizeStatic;
		for (int i = 0; i < coneCount; i++)
		{
			Vector3 vector = Vector3.zero;
			if (num2 != 0f)
			{
				vector = num2 * aimDirection;
			}
			if (num3 > 0f)
			{
				if (flag)
				{
					if (i < num)
					{
						vector -= (float)(num - i) * num3 * normalized;
					}
					else
					{
						vector += (float)(i - num + 1) * num3 * normalized;
					}
				}
				else if (i < num)
				{
					vector -= (float)(num - i) * num3 * normalized;
				}
				else if (i > num)
				{
					vector += (float)(i - num) * num3 * normalized;
				}
			}
			list.Add(travelBoardSquareWorldPositionForLos + vector);
		}
		if (this.GetConeStartOffsetInConeDir() > 0f)
		{
			List<Vector3> coneDirections = this.GetConeDirections(currentTarget, targeterFreePos, caster);
			float d = this.GetConeStartOffsetInConeDir() * Board.SquareSizeStatic;
			for (int j = 0; j < coneDirections.Count; j++)
			{
				List<Vector3> list2;
				int index;
				(list2 = list)[index = j] = list2[index] + d * coneDirections[j];
			}
		}
		return list;
	}

	public virtual List<Vector3> GetConeDirections(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		float num = this.GetAngleInBetween();
		int coneCount = this.GetConeCount();
		if (this.ChangeAngleByCursorDistance())
		{
			float num2;
			if (coneCount > 1)
			{
				num2 = AbilityCommon_FanLaser.CalculateFanAngleDegrees(currentTarget, caster, this.GetTargeterMinAngle(), this.GetTargeterMaxAngle(), this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance, 0f);
			}
			else
			{
				num2 = 0f;
			}
			float num3 = num2;
			float num4;
			if (coneCount > 1)
			{
				num4 = num3 / (float)(coneCount - 1);
			}
			else
			{
				num4 = 0f;
			}
			num = num4;
		}
		float num5 = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection) + this.GetStartAngleOffset();
		float num6 = num5 - 0.5f * (float)(coneCount - 1) * num;
		for (int i = 0; i < coneCount; i++)
		{
			list.Add(VectorUtils.AngleDegreesToVector(num6 + (float)i * num));
		}
		return list;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		this.m_targetSelMod = (modBase as TargetSelectMod_FanCones);
	}

	protected override void OnTargetSelModRemoved()
	{
		this.m_targetSelMod = null;
	}
}
