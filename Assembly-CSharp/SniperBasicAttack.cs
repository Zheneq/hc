using System.Collections.Generic;
using UnityEngine;

public class SniperBasicAttack : Ability
{
	public int m_laserDamageAmount = 5;
	public int m_minDamageAmount;
	public int m_damageChangePerHit;
	public LaserTargetingInfo m_laserInfo;
	public StandardEffectInfo m_laserHitEffect;

	private AbilityMod_SniperBasicAttack m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Sniper Shot";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserRange(), GetLaserPenetratesLoS(), GetMaxTargets());
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_laserDamageAmount > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		}
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (Targeter is AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser)
		{
			ActorData actorData = GetComponent<ActorData>();
			if (actorData != null)
			{
				List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContexts = abilityUtil_Targeter_Laser.GetHitActorContext();
				for (int i = 0; i < hitActorContexts.Count; i++)
				{
					AbilityUtil_Targeter_Laser.HitActorContext hitActorContext = hitActorContexts[i];
					if (hitActorContext.actor == targetActor)
					{
						Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						if (targetActor.GetTeam() != actorData.GetTeam())
						{
							int hitOrder = i;
							dictionary[AbilityTooltipSymbol.Damage] =
								GetDamageAmountByHitOrder(hitOrder, hitActorContext.squaresFromCaster);
						}
						return dictionary;
					}
				}
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SniperBasicAttack abilityMod_SniperBasicAttack = modAsBase as AbilityMod_SniperBasicAttack;
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, abilityMod_SniperBasicAttack != null
			? abilityMod_SniperBasicAttack.m_damageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		AddTokenInt(tokens, "MinDamageAmount", string.Empty, abilityMod_SniperBasicAttack != null
			? abilityMod_SniperBasicAttack.m_minDamageMod.GetModifiedValue(m_minDamageAmount)
			: m_minDamageAmount);
		AddTokenInt(tokens, "DamageChangePerHit", string.Empty, abilityMod_SniperBasicAttack != null
			? abilityMod_SniperBasicAttack.m_damageChangePerHitMod.GetModifiedValue(m_damageChangePerHit)
			: m_damageChangePerHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", m_laserHitEffect);
	}

	public int GetDamageAmountByHitOrder(int hitOrder, float distanceFromCasterInSquares)
	{
		int damage = GetBaseDamage();
		if (GetFarDistanceThreshold() > 0f
		    && distanceFromCasterInSquares > GetFarDistanceThreshold()
		    && GetFarEnemyDamageAmount() > 0)
		{
			damage = GetFarEnemyDamageAmount();
		}
		int b = damage + hitOrder * GetDamageChangePerHit();
		return Mathf.Max(GetMinDamage(), b);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SniperBasicAttack))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		
		m_abilityMod = abilityMod as AbilityMod_SniperBasicAttack;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserInfo.width)
			: m_laserInfo.width;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useTargetDataOverrides && m_abilityMod.m_targetDataOverrides.Length > 0
				? m_abilityMod.m_targetDataOverrides[0].m_range
				: m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserInfo.range)
			: m_laserInfo.range;
	}

	public bool GetLaserPenetratesLoS()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_useTargetDataOverrides
		       && m_abilityMod.m_targetDataOverrides.Length > 0
			? !m_abilityMod.m_targetDataOverrides[0].m_checkLineOfSight
			: m_laserInfo.penetrateLos;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_laserInfo.maxTargets)
			: m_laserInfo.maxTargets;
	}

	public int GetBaseDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public int GetMinDamage()
	{
		return Mathf.Max(
			0,
			m_abilityMod != null
				? m_abilityMod.m_minDamageMod.GetModifiedValue(m_minDamageAmount)
				: m_minDamageAmount);
	}

	public int GetDamageChangePerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageChangePerHitMod.GetModifiedValue(m_damageChangePerHit)
			: m_damageChangePerHit;
	}

	public float GetFarDistanceThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_farDistanceThreshold
			: 0f;
	}

	public int GetFarEnemyDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_farEnemyDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}
}
