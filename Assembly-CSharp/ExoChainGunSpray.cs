using System;
using System.Collections.Generic;
using UnityEngine;

public class ExoChainGunSpray : Ability
{
	[Space(20f)]
	[Header("-- Cone to Sweep Across")]
	public float m_coneBackwardOffset;

	public float m_coneLength = 2.5f;

	public float m_minConeAngle = 10f;

	public float m_maxConeAngle = 90f;

	public int m_maxTargets;

	public float m_multiClickConeEdgeWidth = 0.2f;

	public bool m_penetrateLineOfSight;

	[Header("-- Damage and Effects")]
	public int m_minDamageAmount = 15;

	public int m_maxDamageAmount = 35;

	public StandardEffectInfo m_targetHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Old Painless";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (GetExpectedNumberOfTargeters() > 1)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					ClearTargeters();
					for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
					{
						AbilityUtil_Targeter_SweepMultiClickCone abilityUtil_Targeter_SweepMultiClickCone = new AbilityUtil_Targeter_SweepMultiClickCone(this, GetMinConeAngle(), GetMaxConeAngle(), GetConeLength(), m_coneBackwardOffset, m_multiClickConeEdgeWidth, m_penetrateLineOfSight, GetMaxTargets());
						abilityUtil_Targeter_SweepMultiClickCone.SetAffectedGroups(true, false, false);
						base.Targeters.Add(abilityUtil_Targeter_SweepMultiClickCone);
					}
					while (true)
					{
						switch (5)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				}
			}
		}
		Log.Error("ExoChainGunSpray requires 2 targeters, please update the Target Data array in the character prefab.");
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, GetNumTargets());
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < targets.Count; i++)
		{
			list.Add(targets[i].FreePos);
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return list;
		}
	}

	private float GetMinConeAngle()
	{
		return m_minConeAngle;
	}

	private float GetMaxConeAngle()
	{
		return m_maxConeAngle;
	}

	private float GetConeLength()
	{
		return m_coneLength;
	}

	private int GetMaxTargets()
	{
		return m_maxTargets;
	}

	private int GetMinDamageAmount()
	{
		return m_minDamageAmount;
	}

	private int GetMaxDamageAmount()
	{
		return m_maxDamageAmount;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetMinDamageAmount());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (currentTargeterIndex > 0)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityUtil_Targeter_SweepMultiClickCone abilityUtil_Targeter_SweepMultiClickCone = base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SweepMultiClickCone;
			int damageForSweepAngle = GetDamageForSweepAngle(abilityUtil_Targeter_SweepMultiClickCone.sweepAngle);
			Ability.AddNameplateValueForSingleHit(ref symbolToValue, abilityUtil_Targeter_SweepMultiClickCone, targetActor, damageForSweepAngle);
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Min_Damage", "damage dealt by the widest possible cone", GetMinDamageAmount());
		AddTokenInt(tokens, "Max_Damage", "damage dealt by the narrowest possible cone", GetMaxDamageAmount());
		AddTokenInt(tokens, "Min_Cone_Angle", "the narrowest cone", (int)GetMinConeAngle());
		AddTokenInt(tokens, "Max_Cone_Angle", "the widest cone", (int)GetMaxConeAngle());
		AddTokenInt(tokens, "Cone_Length", "range of the damage cone", Mathf.RoundToInt(GetConeLength()));
	}

	private Vector3 GetTargeterClampedAimDirection(Vector3 startAimDirection, Vector3 endAimDirection, out float sweepAngle, out float coneCenterDegrees)
	{
		float num = VectorUtils.HorizontalAngle_Deg(startAimDirection);
		sweepAngle = Vector3.Angle(startAimDirection, endAimDirection);
		float maxConeAngle = GetMaxConeAngle();
		float minConeAngle = GetMinConeAngle();
		if (maxConeAngle > 0f)
		{
			while (true)
			{
				switch (2)
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
			if (sweepAngle > maxConeAngle)
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
				endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, (float)Math.PI / 180f * (sweepAngle - maxConeAngle), 0f);
				sweepAngle = maxConeAngle;
				goto IL_00a5;
			}
		}
		if (minConeAngle > 0f && sweepAngle < minConeAngle)
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
			endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, (float)Math.PI / 180f * (sweepAngle - minConeAngle), 0f);
			sweepAngle = minConeAngle;
		}
		goto IL_00a5;
		IL_00a5:
		coneCenterDegrees = num;
		Vector3 vector = Vector3.Cross(startAimDirection, endAimDirection);
		if (vector.y > 0f)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			coneCenterDegrees -= sweepAngle * 0.5f;
		}
		else
		{
			coneCenterDegrees += sweepAngle * 0.5f;
		}
		return endAimDirection;
	}

	private int GetDamageForSweepAngle(float sweepAngle)
	{
		float num = GetMaxDamageAmount() - GetMinDamageAmount();
		float num2 = GetMaxConeAngle() - GetMinConeAngle();
		float value = 1f - (sweepAngle - GetMinConeAngle()) / num2;
		value = Mathf.Clamp(value, 0f, 1f);
		return GetMinDamageAmount() + Mathf.RoundToInt(num * value);
	}
}
