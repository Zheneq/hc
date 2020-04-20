using System;
using System.Collections.Generic;
using UnityEngine;

public class NekoHomingDisc : Ability
{
	[Separator("Targeting", true)]
	public float m_laserLength = 6.5f;

	public float m_laserWidth = 1f;

	public int m_maxTargets = 1;

	[Header("-- Disc return end radius")]
	public float m_discReturnEndRadius;

	[Separator("On Cast Hit", true)]
	public StandardEffectInfo m_onCastEnemyHitEffect;

	[Separator("On Enemy Hit", true)]
	public int m_targetDamage = 0x19;

	public int m_returnTripDamage = 0xA;

	public bool m_returnTripIgnoreCover = true;

	public float m_extraReturnDamagePerDist;

	public StandardEffectInfo m_returnTripEnemyEffect;

	[Separator("Cooldown Reduction", true)]
	public int m_cdrIfHitNoOneOnCast;

	public int m_cdrIfHitNoOneOnReturn;

	[Header("Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_returnTripSequencePrefab;

	public GameObject m_persistentDiscSequencePrefab;

	private AbilityMod_NekoHomingDisc m_abilityMod;

	private Neko_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedOnCastEnemyHitEffect;

	private StandardEffectInfo m_cachedReturnTripEnemyEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Homing Disc";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		this.m_syncComp = base.GetComponent<Neko_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_Laser(this, this.GetLaserWidth(), this.GetLaserLength(), false, this.GetMaxTargets(), false, false);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedOnCastEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedOnCastEnemyHitEffect = this.m_abilityMod.m_onCastEnemyHitEffectMod.GetModifiedValue(this.m_onCastEnemyHitEffect);
		}
		else
		{
			cachedOnCastEnemyHitEffect = this.m_onCastEnemyHitEffect;
		}
		this.m_cachedOnCastEnemyHitEffect = cachedOnCastEnemyHitEffect;
		StandardEffectInfo cachedReturnTripEnemyEffect;
		if (this.m_abilityMod)
		{
			cachedReturnTripEnemyEffect = this.m_abilityMod.m_returnTripEnemyEffectMod.GetModifiedValue(this.m_returnTripEnemyEffect);
		}
		else
		{
			cachedReturnTripEnemyEffect = this.m_returnTripEnemyEffect;
		}
		this.m_cachedReturnTripEnemyEffect = cachedReturnTripEnemyEffect;
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

	public int GetMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public float GetDiscReturnEndRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(this.m_discReturnEndRadius);
		}
		else
		{
			result = this.m_discReturnEndRadius;
		}
		return result;
	}

	public StandardEffectInfo GetOnCastEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedOnCastEnemyHitEffect != null)
		{
			result = this.m_cachedOnCastEnemyHitEffect;
		}
		else
		{
			result = this.m_onCastEnemyHitEffect;
		}
		return result;
	}

	public int GetTargetDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_targetDamageMod.GetModifiedValue(this.m_targetDamage);
		}
		else
		{
			result = this.m_targetDamage;
		}
		return result;
	}

	public int GetReturnTripDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_returnTripDamageMod.GetModifiedValue(this.m_returnTripDamage);
		}
		else
		{
			result = this.m_returnTripDamage;
		}
		return result;
	}

	public bool ReturnTripIgnoreCover()
	{
		return (!this.m_abilityMod) ? this.m_returnTripIgnoreCover : this.m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(this.m_returnTripIgnoreCover);
	}

	public float GetExtraReturnDamagePerDist()
	{
		return (!this.m_abilityMod) ? this.m_extraReturnDamagePerDist : this.m_abilityMod.m_extraReturnDamagePerDistMod.GetModifiedValue(this.m_extraReturnDamagePerDist);
	}

	public StandardEffectInfo GetReturnTripEnemyEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedReturnTripEnemyEffect != null)
		{
			result = this.m_cachedReturnTripEnemyEffect;
		}
		else
		{
			result = this.m_returnTripEnemyEffect;
		}
		return result;
	}

	public int GetCdrIfHitNoOneOnCast()
	{
		return (!this.m_abilityMod) ? this.m_cdrIfHitNoOneOnCast : this.m_abilityMod.m_cdrIfHitNoOneOnCastMod.GetModifiedValue(this.m_cdrIfHitNoOneOnCast);
	}

	public int GetCdrIfHitNoOneOnReturn()
	{
		return (!this.m_abilityMod) ? this.m_cdrIfHitNoOneOnReturn : this.m_abilityMod.m_cdrIfHitNoOneOnReturnMod.GetModifiedValue(this.m_cdrIfHitNoOneOnReturn);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_onCastEnemyHitEffect, "OnCastEnemyHitEffect", this.m_onCastEnemyHitEffect, true);
		base.AddTokenInt(tokens, "TargetDamage", string.Empty, this.m_targetDamage, false);
		base.AddTokenInt(tokens, "ReturnTripDamage", string.Empty, this.m_returnTripDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_returnTripEnemyEffect, "ReturnTripEnemyEffect", this.m_returnTripEnemyEffect, true);
		base.AddTokenInt(tokens, "CdrIfHitNoOneOnCast", string.Empty, this.m_cdrIfHitNoOneOnCast, false);
		base.AddTokenInt(tokens, "CdrIfHitNoOneOnReturn", string.Empty, this.m_cdrIfHitNoOneOnReturn, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.GetTargetDamage()),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, this.GetReturnTripDamage())
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserLength();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NekoHomingDisc))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_NekoHomingDisc);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
