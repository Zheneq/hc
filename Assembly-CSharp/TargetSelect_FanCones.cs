using AbilityContextNamespace;
using System.Collections.Generic;
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
		return GetContextUsageStr(ContextKeys.s_HitCount.GetName(), "on every hit actor, number of cone hits on target");
	}

	public override void ListContextNamesForEditor(List<string> names)
	{
		names.Add(ContextKeys.s_HitCount.GetName());
	}

	public override void Initialize()
	{
		SetCachedFields();
		ConeTargetingInfo coneInfo = GetConeInfo();
		coneInfo.m_affectsAllies = IncludeAllies();
		coneInfo.m_affectsEnemies = IncludeEnemies();
		coneInfo.m_affectsCaster = IncludeCaster();
		coneInfo.m_penetrateLos = IgnoreLos();
	}

	private void SetCachedFields()
	{
		ConeTargetingInfo cachedConeInfo;
		if (m_targetSelMod != null)
		{
			cachedConeInfo = m_targetSelMod.m_coneInfoMod.GetModifiedValue(m_coneInfo);
		}
		else
		{
			cachedConeInfo = m_coneInfo;
		}
		m_cachedConeInfo = cachedConeInfo;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return (m_cachedConeInfo == null) ? m_coneInfo : m_cachedConeInfo;
	}

	public int GetConeCount()
	{
		int result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_coneCountMod.GetModifiedValue(m_coneCount);
		}
		else
		{
			result = m_coneCount;
		}
		return result;
	}

	public float GetConeStartOffsetInAimDir()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_coneStartOffsetInAimDirMod.GetModifiedValue(m_coneStartOffsetInAimDir);
		}
		else
		{
			result = m_coneStartOffsetInAimDir;
		}
		return result;
	}

	public float GetConeStartOffsetToSides()
	{
		return (m_targetSelMod == null) ? m_coneStartOffsetToSides : m_targetSelMod.m_coneStartOffsetToSidesMod.GetModifiedValue(m_coneStartOffsetToSides);
	}

	public float GetConeStartOffsetInConeDir()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_coneStartOffsetInConeDirMod.GetModifiedValue(m_coneStartOffsetInConeDir);
		}
		else
		{
			result = m_coneStartOffsetInConeDir;
		}
		return result;
	}

	public float GetAngleInBetween()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_angleInBetweenMod.GetModifiedValue(m_angleInBetween);
		}
		else
		{
			result = m_angleInBetween;
		}
		return result;
	}

	public bool ChangeAngleByCursorDistance()
	{
		bool result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_changeAngleByCursorDistanceMod.GetModifiedValue(m_changeAngleByCursorDistance);
		}
		else
		{
			result = m_changeAngleByCursorDistance;
		}
		return result;
	}

	public float GetTargeterMinAngle()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_targeterMinAngleMod.GetModifiedValue(m_targeterMinAngle);
		}
		else
		{
			result = m_targeterMinAngle;
		}
		return result;
	}

	public float GetTargeterMaxAngle()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_targeterMaxAngleMod.GetModifiedValue(m_targeterMaxAngle);
		}
		else
		{
			result = m_targeterMaxAngle;
		}
		return result;
	}

	public float GetStartAngleOffset()
	{
		float result;
		if (m_targetSelMod != null)
		{
			result = m_targetSelMod.m_startAngleOffsetMod.GetModifiedValue(m_startAngleOffset);
		}
		else
		{
			result = m_startAngleOffset;
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
		AbilityUtil_Targeter_TricksterCones abilityUtil_Targeter_TricksterCones = new AbilityUtil_Targeter_TricksterCones(ability, GetConeInfo(), GetConeCount(), GetConeCount, GetConeOrigins, GetConeDirections, GetFreePosForAim, false, UseCasterPosForLoS());
		abilityUtil_Targeter_TricksterCones.m_customDamageOriginDelegate = GetDamageOriginForTargeter;
		List<AbilityUtil_Targeter> list = new List<AbilityUtil_Targeter>();
		list.Add(abilityUtil_Targeter_TricksterCones);
		return list;
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
		int coneCount = GetConeCount();
		int num = coneCount / 2;
		bool flag = coneCount % 2 == 0;
		float num2 = GetConeStartOffsetInAimDir() * Board.SquareSizeStatic;
		float num3 = GetConeStartOffsetToSides() * Board.SquareSizeStatic;
		for (int i = 0; i < coneCount; i++)
		{
			Vector3 b = Vector3.zero;
			if (num2 != 0f)
			{
				b = num2 * aimDirection;
			}
			if (num3 > 0f)
			{
				if (flag)
				{
					if (i < num)
					{
						b -= (float)(num - i) * num3 * normalized;
					}
					else
					{
						b += (float)(i - num + 1) * num3 * normalized;
					}
				}
				else if (i < num)
				{
					b -= (float)(num - i) * num3 * normalized;
				}
				else if (i > num)
				{
					b += (float)(i - num) * num3 * normalized;
				}
			}
			list.Add(travelBoardSquareWorldPositionForLos + b);
		}
		if (GetConeStartOffsetInConeDir() > 0f)
		{
			List<Vector3> coneDirections = GetConeDirections(currentTarget, targeterFreePos, caster);
			float d = GetConeStartOffsetInConeDir() * Board.SquareSizeStatic;
			for (int j = 0; j < coneDirections.Count; j++)
			{
				list[j] += d * coneDirections[j];
			}
		}
		return list;
	}

	public virtual List<Vector3> GetConeDirections(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		float num = GetAngleInBetween();
		int coneCount = GetConeCount();
		if (ChangeAngleByCursorDistance())
		{
			float num2;
			if (coneCount > 1)
			{
				num2 = AbilityCommon_FanLaser.CalculateFanAngleDegrees(currentTarget, caster, GetTargeterMinAngle(), GetTargeterMaxAngle(), m_targeterMinInterpDistance, m_targeterMaxInterpDistance, 0f);
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
		float num5 = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection) + GetStartAngleOffset();
		float num6 = num5 - 0.5f * (float)(coneCount - 1) * num;
		for (int i = 0; i < coneCount; i++)
		{
			list.Add(VectorUtils.AngleDegreesToVector(num6 + (float)i * num));
		}
		return list;
	}

	protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		m_targetSelMod = (modBase as TargetSelectMod_FanCones);
	}

	protected override void OnTargetSelModRemoved()
	{
		m_targetSelMod = null;
	}
}
