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
	public int m_minDamageAmount = 0xF;

	public int m_maxDamageAmount = 0x23;

	public StandardEffectInfo m_targetHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Old Painless";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.GetExpectedNumberOfTargeters() > 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoChainGunSpray.SetupTargeter()).MethodHandle;
			}
			base.ClearTargeters();
			for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
			{
				AbilityUtil_Targeter_SweepMultiClickCone abilityUtil_Targeter_SweepMultiClickCone = new AbilityUtil_Targeter_SweepMultiClickCone(this, this.GetMinConeAngle(), this.GetMaxConeAngle(), this.GetConeLength(), this.m_coneBackwardOffset, this.m_multiClickConeEdgeWidth, this.m_penetrateLineOfSight, this.GetMaxTargets());
				abilityUtil_Targeter_SweepMultiClickCone.SetAffectedGroups(true, false, false);
				base.Targeters.Add(abilityUtil_Targeter_SweepMultiClickCone);
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		else
		{
			Log.Error("ExoChainGunSpray requires 2 targeters, please update the Target Data array in the character prefab.", new object[0]);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, base.GetNumTargets());
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < targets.Count; i++)
		{
			list.Add(targets[i].FreePos);
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(ExoChainGunSpray.CalcPointsOfInterestForCamera(List<AbilityTarget>, ActorData)).MethodHandle;
		}
		return list;
	}

	private float GetMinConeAngle()
	{
		return this.m_minConeAngle;
	}

	private float GetMaxConeAngle()
	{
		return this.m_maxConeAngle;
	}

	private float GetConeLength()
	{
		return this.m_coneLength;
	}

	private int GetMaxTargets()
	{
		return this.m_maxTargets;
	}

	private int GetMinDamageAmount()
	{
		return this.m_minDamageAmount;
	}

	private int GetMaxDamageAmount()
	{
		return this.m_maxDamageAmount;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetMinDamageAmount());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		if (currentTargeterIndex > 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoChainGunSpray.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			AbilityUtil_Targeter_SweepMultiClickCone abilityUtil_Targeter_SweepMultiClickCone = base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SweepMultiClickCone;
			int damageForSweepAngle = this.GetDamageForSweepAngle(abilityUtil_Targeter_SweepMultiClickCone.sweepAngle);
			Ability.AddNameplateValueForSingleHit(ref result, abilityUtil_Targeter_SweepMultiClickCone, targetActor, damageForSweepAngle, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "Min_Damage", "damage dealt by the widest possible cone", this.GetMinDamageAmount(), false);
		base.AddTokenInt(tokens, "Max_Damage", "damage dealt by the narrowest possible cone", this.GetMaxDamageAmount(), false);
		base.AddTokenInt(tokens, "Min_Cone_Angle", "the narrowest cone", (int)this.GetMinConeAngle(), false);
		base.AddTokenInt(tokens, "Max_Cone_Angle", "the widest cone", (int)this.GetMaxConeAngle(), false);
		base.AddTokenInt(tokens, "Cone_Length", "range of the damage cone", Mathf.RoundToInt(this.GetConeLength()), false);
	}

	private unsafe Vector3 GetTargeterClampedAimDirection(Vector3 startAimDirection, Vector3 endAimDirection, out float sweepAngle, out float coneCenterDegrees)
	{
		float num = VectorUtils.HorizontalAngle_Deg(startAimDirection);
		sweepAngle = Vector3.Angle(startAimDirection, endAimDirection);
		float maxConeAngle = this.GetMaxConeAngle();
		float minConeAngle = this.GetMinConeAngle();
		if (maxConeAngle > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoChainGunSpray.GetTargeterClampedAimDirection(Vector3, Vector3, float*, float*)).MethodHandle;
			}
			if (sweepAngle > maxConeAngle)
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
				endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, 0.0174532924f * (sweepAngle - maxConeAngle), 0f);
				sweepAngle = maxConeAngle;
				goto IL_A5;
			}
		}
		if (minConeAngle > 0f && sweepAngle < minConeAngle)
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
			endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, 0.0174532924f * (sweepAngle - minConeAngle), 0f);
			sweepAngle = minConeAngle;
		}
		IL_A5:
		coneCenterDegrees = num;
		if (Vector3.Cross(startAimDirection, endAimDirection).y > 0f)
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
		float num = (float)(this.GetMaxDamageAmount() - this.GetMinDamageAmount());
		float num2 = this.GetMaxConeAngle() - this.GetMinConeAngle();
		float num3 = 1f - (sweepAngle - this.GetMinConeAngle()) / num2;
		num3 = Mathf.Clamp(num3, 0f, 1f);
		return this.GetMinDamageAmount() + Mathf.RoundToInt(num * num3);
	}
}
