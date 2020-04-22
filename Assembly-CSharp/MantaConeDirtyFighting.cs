using System.Collections.Generic;
using UnityEngine;

public class MantaConeDirtyFighting : Ability
{
	[Header("-- Targeting")]
	public float m_coneRange = 4f;

	public float m_coneWidth = 60f;

	public bool m_penetrateLoS;

	public int m_maxTargets = 5;

	public float m_coneBackwardOffset;

	[Header("-- Hit Damage/Effects")]
	public int m_onCastDamageAmount;

	public StandardActorEffectData m_dirtyFightingEffectData;

	public StandardEffectInfo m_enemyHitEffectData;

	public StandardEffectInfo m_effectOnTargetFromExplosion;

	[Header("-- On Reaction Hit/Explosion Triggered")]
	public int m_effectExplosionDamage = 30;

	[Tooltip("whether allies other than yourself should be able to trigger the explosion")]
	public bool m_explodeOnlyFromSelfDamage;

	public int m_techPointGainPerExplosion = 5;

	public int m_healAmountPerExplosion;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_effectOnExplosionSequencePrefab;

	private AbilityMod_MantaConeDirtyFighting m_abilityMod;

	private StandardActorEffectData m_cachedDirtyFightingEffectData;

	private StandardEffectInfo m_cachedEnemyHitEffectData;

	private StandardEffectInfo m_cachedEffectOnTargetFromExplosion;

	private StandardEffectInfo m_cachedEffectOnTargetWhenExpiresWithoutExplosion;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Dirty Fighting Cone";
		}
		SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeRange();
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedDirtyFightingEffectData;
		if ((bool)m_abilityMod)
		{
			cachedDirtyFightingEffectData = m_abilityMod.m_dirtyFightingEffectDataMod.GetModifiedValue(m_dirtyFightingEffectData);
		}
		else
		{
			cachedDirtyFightingEffectData = m_dirtyFightingEffectData;
		}
		m_cachedDirtyFightingEffectData = cachedDirtyFightingEffectData;
		StandardEffectInfo cachedEnemyHitEffectData;
		if ((bool)m_abilityMod)
		{
			cachedEnemyHitEffectData = m_abilityMod.m_enemyHitEffectDataMod.GetModifiedValue(m_enemyHitEffectData);
		}
		else
		{
			cachedEnemyHitEffectData = m_enemyHitEffectData;
		}
		m_cachedEnemyHitEffectData = cachedEnemyHitEffectData;
		StandardEffectInfo cachedEffectOnTargetFromExplosion;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnTargetFromExplosion = m_abilityMod.m_effectOnTargetFromExplosionMod.GetModifiedValue(m_effectOnTargetFromExplosion);
		}
		else
		{
			cachedEffectOnTargetFromExplosion = m_effectOnTargetFromExplosion;
		}
		m_cachedEffectOnTargetFromExplosion = cachedEffectOnTargetFromExplosion;
		m_cachedEffectOnTargetWhenExpiresWithoutExplosion = ((!m_abilityMod) ? null : m_abilityMod.m_effectOnTargetWhenExpiresWithoutExplosionMod.GetModifiedValue(null));
	}

	public float GetConeRange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneRangeMod.GetModifiedValue(m_coneRange);
		}
		else
		{
			result = m_coneRange;
		}
		return result;
	}

	public float GetConeWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneWidthMod.GetModifiedValue(m_coneWidth);
		}
		else
		{
			result = m_coneWidth;
		}
		return result;
	}

	public bool PenetrateLoS()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS);
		}
		else
		{
			result = m_penetrateLoS;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		return (!m_abilityMod) ? m_maxTargets : m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
		}
		else
		{
			result = m_coneBackwardOffset;
		}
		return result;
	}

	public int GetOnCastDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_onCastDamageAmountMod.GetModifiedValue(m_onCastDamageAmount);
		}
		else
		{
			result = m_onCastDamageAmount;
		}
		return result;
	}

	public StandardActorEffectData GetDirtyFightingEffectData()
	{
		StandardActorEffectData result;
		if (m_cachedDirtyFightingEffectData != null)
		{
			result = m_cachedDirtyFightingEffectData;
		}
		else
		{
			result = m_dirtyFightingEffectData;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffectData()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffectData != null)
		{
			result = m_cachedEnemyHitEffectData;
		}
		else
		{
			result = m_enemyHitEffectData;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnTargetFromExplosion()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnTargetFromExplosion != null)
		{
			result = m_cachedEffectOnTargetFromExplosion;
		}
		else
		{
			result = m_effectOnTargetFromExplosion;
		}
		return result;
	}

	public StandardActorEffectData GetEffectOnTargetWhenExpiresWithoutExplosion()
	{
		object result;
		if (m_cachedEffectOnTargetWhenExpiresWithoutExplosion != null)
		{
			if (m_cachedEffectOnTargetWhenExpiresWithoutExplosion.m_applyEffect)
			{
				result = m_cachedEffectOnTargetWhenExpiresWithoutExplosion.m_effectData;
				goto IL_0036;
			}
		}
		result = null;
		goto IL_0036;
		IL_0036:
		return (StandardActorEffectData)result;
	}

	public int GetEffectExplosionDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_effectExplosionDamageMod.GetModifiedValue(m_effectExplosionDamage);
		}
		else
		{
			result = m_effectExplosionDamage;
		}
		return result;
	}

	public bool ExplodeOnlyFromSelfDamage()
	{
		return (!m_abilityMod) ? m_explodeOnlyFromSelfDamage : m_abilityMod.m_explodeOnlyFromSelfDamageMod.GetModifiedValue(m_explodeOnlyFromSelfDamage);
	}

	public int GetTechPointGainPerExplosion()
	{
		return (!m_abilityMod) ? m_techPointGainPerExplosion : m_abilityMod.m_techPointGainPerExplosionMod.GetModifiedValue(m_techPointGainPerExplosion);
	}

	public int GetHealAmountPerExplosion()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healPerExplosionMod.GetModifiedValue(m_healAmountPerExplosion);
		}
		else
		{
			result = m_healAmountPerExplosion;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_MantaConeDirtyFighting))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_MantaConeDirtyFighting);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, GetConeWidth(), GetConeRange(), m_coneBackwardOffset, PenetrateLoS(), true, true, false, false, GetMaxTargets());
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "OnCastDamageAmount", string.Empty, m_onCastDamageAmount);
		m_dirtyFightingEffectData.AddTooltipTokens(tokens, "DirtyFightingEffectData");
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffectData, "EnemyHitEffectData", m_enemyHitEffectData);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTargetFromExplosion, "EffectOnTargetFromExplosion", m_effectOnTargetFromExplosion);
		AddTokenInt(tokens, "EffectExplosionDamage", string.Empty, m_effectExplosionDamage);
		AddTokenInt(tokens, "TechPointGainPerExplosion", string.Empty, m_techPointGainPerExplosion);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetOnCastDamageAmount());
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Tertiary, GetEffectExplosionDamage());
		GetEnemyHitEffectData().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		return numbers;
	}
}
