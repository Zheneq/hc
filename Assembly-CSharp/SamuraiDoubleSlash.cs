using System;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiDoubleSlash : Ability
{
	[Header("-- Targeting")]
	public bool m_penetrateLineOfSight;

	public bool m_coneFirstSlash;

	public bool m_coneSecondSlash;

	public float m_maxAngleBetween = 120f;

	[Header("    Cone(s)")]
	public float m_coneWidthAngle = 180f;

	public float m_coneBackwardOffset;

	public float m_coneLength = 2.5f;

	[Header("    Laser(s)")]
	public float m_laserWidth = 1.5f;

	public float m_laserLength = 2.5f;

	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 0x14;

	public int m_overlapExtraDamage = 0xF;

	public StandardEffectInfo m_targetHitEffect;

	[Header("-- Extra Effect if SelfBuff ability is used on same turn")]
	public StandardEffectInfo m_extraEnemyHitEffectIfSelfBuffed;

	[Header("-- Sequences")]
	public GameObject m_coneCastSequencePrefab;

	public GameObject m_laserCastSequencePrefab;

	private AbilityMod_SamuraiDoubleSlash m_abilityMod;

	private Samurai_SyncComponent m_syncComponent;

	private StandardEffectInfo m_cachedTargetHitEffect;

	private StandardEffectInfo m_cachedExtraEnemyHitEffectIfSelfBuffed;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Double Slash";
		}
		this.m_syncComponent = base.ActorData.GetComponent<Samurai_SyncComponent>();
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		int i = 0;
		while (i < this.GetExpectedNumberOfTargeters())
		{
			if (i != 0)
			{
				goto IL_2B;
			}
			if (this.m_coneFirstSlash)
			{
				goto IL_4B;
			}
			goto IL_2B;
			IL_E8:
			i++;
			continue;
			IL_4B:
			AbilityUtil_Targeter_DirectionCone abilityUtil_Targeter_DirectionCone = new AbilityUtil_Targeter_DirectionCone(this, this.GetConeWidthAngle(), this.GetConeLength(), this.GetConeBackwardOffset(), this.PenetrateLineOfSight(), true, true, false, false, -1, true);
			abilityUtil_Targeter_DirectionCone.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_DirectionCone.m_getClampedAimDirection = new AbilityUtil_Targeter_DirectionCone.ClampedAimDirectionDelegate(this.GetTargeterClampedAimDirection);
			base.Targeters.Add(abilityUtil_Targeter_DirectionCone);
			goto IL_E8;
			IL_2B:
			if (i == 1)
			{
				if (this.m_coneSecondSlash)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						goto IL_4B;
					}
				}
			}
			AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, this.GetLaserWidth(), this.GetLaserLength(), this.PenetrateLineOfSight(), -1, false, false);
			abilityUtil_Targeter_Laser.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_Laser.m_getClampedAimDirection = new AbilityUtil_Targeter_Laser.ClampedAimDirectionDelegate(this.GetTargeterClampedAimDirection);
			base.Targeters.Add(abilityUtil_Targeter_Laser);
			goto IL_E8;
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	public override bool ShouldAutoConfirmIfTargetingOnEndTurn()
	{
		return true;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return Mathf.Max(this.GetConeLength(), this.GetLaserLength());
	}

	private void SetCachedFields()
	{
		this.m_cachedTargetHitEffect = ((!this.m_abilityMod) ? this.m_targetHitEffect : this.m_abilityMod.m_targetHitEffectMod.GetModifiedValue(this.m_targetHitEffect));
		StandardEffectInfo cachedExtraEnemyHitEffectIfSelfBuffed;
		if (this.m_abilityMod)
		{
			cachedExtraEnemyHitEffectIfSelfBuffed = this.m_abilityMod.m_extraEnemyHitEffectIfSelfBuffedMod.GetModifiedValue(this.m_extraEnemyHitEffectIfSelfBuffed);
		}
		else
		{
			cachedExtraEnemyHitEffectIfSelfBuffed = this.m_extraEnemyHitEffectIfSelfBuffed;
		}
		this.m_cachedExtraEnemyHitEffectIfSelfBuffed = cachedExtraEnemyHitEffectIfSelfBuffed;
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
		}
		else
		{
			result = this.m_penetrateLineOfSight;
		}
		return result;
	}

	public float GetMaxAngleBetween()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxAngleBetweenMod.GetModifiedValue(this.m_maxAngleBetween);
		}
		else
		{
			result = this.m_maxAngleBetween;
		}
		return result;
	}

	public float GetConeWidthAngle()
	{
		return (!this.m_abilityMod) ? this.m_coneWidthAngle : this.m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(this.m_coneWidthAngle);
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
		}
		else
		{
			result = this.m_coneBackwardOffset;
		}
		return result;
	}

	public float GetConeLength()
	{
		return (!this.m_abilityMod) ? this.m_coneLength : this.m_abilityMod.m_coneLengthMod.GetModifiedValue(this.m_coneLength);
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
		}
		else
		{
			result = this.m_laserWidth;
		}
		return result;
	}

	public float GetLaserLength()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserLengthMod.GetModifiedValue(this.m_laserLength);
		}
		else
		{
			result = this.m_laserLength;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public int GetOverlapExtraDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_overlapExtraDamageMod.GetModifiedValue(this.m_overlapExtraDamage);
		}
		else
		{
			result = this.m_overlapExtraDamage;
		}
		return result;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedTargetHitEffect != null)
		{
			result = this.m_cachedTargetHitEffect;
		}
		else
		{
			result = this.m_targetHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEnemyHitEffectIfSelfBuffed()
	{
		StandardEffectInfo result;
		if (this.m_cachedExtraEnemyHitEffectIfSelfBuffed != null)
		{
			result = this.m_cachedExtraEnemyHitEffectIfSelfBuffed;
		}
		else
		{
			result = this.m_extraEnemyHitEffectIfSelfBuffed;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "Damage", "damage in the cone", this.m_damageAmount, false);
		base.AddTokenInt(tokens, "Damage_Overlap", "damage if hit by both cones", this.m_damageAmount + this.m_overlapExtraDamage, false);
		base.AddTokenInt(tokens, "Cone_Angle", "angle of the damage cone", (int)this.m_coneWidthAngle, false);
		base.AddTokenInt(tokens, "Cone_Length", "range of the damage cone", Mathf.RoundToInt(this.m_coneLength), false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_targetHitEffect, "TargetHitEffect", this.m_targetHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_extraEnemyHitEffectIfSelfBuffed, "ExtraEnemyHitEffectIfSelfBuffed", this.m_extraEnemyHitEffectIfSelfBuffed, true);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.GetDamageAmount())
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			Ability.AddNameplateValueForOverlap(ref dictionary, base.Targeters[i], targetActor, currentTargeterIndex, this.GetDamageAmount(), this.GetOverlapExtraDamage(), AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		}
		if (this.m_syncComponent != null)
		{
			if (dictionary.ContainsKey(AbilityTooltipSymbol.Damage))
			{
				Dictionary<AbilityTooltipSymbol, int> dictionary2;
				(dictionary2 = dictionary)[AbilityTooltipSymbol.Damage] = dictionary2[AbilityTooltipSymbol.Damage] + this.m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
			}
		}
		return dictionary;
	}

	public Vector3 GetTargeterClampedAimDirection(Vector3 aimDir, Vector3 prevAimDir)
	{
		aimDir.y = 0f;
		aimDir.Normalize();
		float maxAngleBetween = this.GetMaxAngleBetween();
		if (maxAngleBetween > 0f)
		{
			if (maxAngleBetween < 360f)
			{
				aimDir = Vector3.RotateTowards(prevAimDir, aimDir, 0.0174532924f * maxAngleBetween, 0f);
			}
		}
		return aimDir;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiDoubleSlash))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SamuraiDoubleSlash);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
