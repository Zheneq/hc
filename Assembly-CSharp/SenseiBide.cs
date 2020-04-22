using System.Collections.Generic;
using UnityEngine;

public class SenseiBide : Ability
{
	[Header("-- Targeting --")]
	public bool m_targetingIgnoreLos;

	[Separator("Effect on Cast Target", "cyan")]
	public StandardActorEffectData m_onCastTargetEffectData;

	[Header("-- Additional Effect on targeted actor, for shielding, etc")]
	public StandardEffectInfo m_additionalTargetHitEffect;

	[Separator("For Explosion Hits", "cyan")]
	public float m_explosionRadius = 1.5f;

	public bool m_ignoreLos;

	[Header("-- Explosion Hit --")]
	public int m_maxDamage = 50;

	public int m_baseDamage;

	public float m_damageMult = 1f;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Heal portion of absorb remaining")]
	public float m_absorbMultForHeal;

	[Header("-- Damage portion of initial damage, on turns after")]
	public float m_multOnInitialDamageForSubseqHits;

	[Separator("Extra Heal on Heal AoE Ability", true)]
	public int m_extraHealOnHealAoeIfQueued;

	[Header("-- Animation --")]
	public int m_explosionAnimIndex;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	[Header("    Used by effect when actual explosion happens")]
	public GameObject m_onExplosionSequencePrefab;

	private AbilityMod_SenseiBide m_abilityMod;

	private StandardActorEffectData m_cachedOnCastTargetEffectData;

	private StandardEffectInfo m_cachedAdditionalTargetHitEffect;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (1)
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
			m_abilityName = "SenseiBide";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		AbilityUtil_Targeter_AoE_AroundActor abilityUtil_Targeter_AoE_AroundActor = new AbilityUtil_Targeter_AoE_AroundActor(this, GetExplosionRadius(), IgnoreLos());
		abilityUtil_Targeter_AoE_AroundActor.SetAffectedGroups(true, false, false);
		abilityUtil_Targeter_AoE_AroundActor.m_allyOccupantSubject = AbilityTooltipSubject.Tertiary;
		abilityUtil_Targeter_AoE_AroundActor.m_enemyOccupantSubject = AbilityTooltipSubject.Quaternary;
		base.Targeter = abilityUtil_Targeter_AoE_AroundActor;
		base.Targeter.SetShowArcToShape(true);
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Art --</color>\nFor Persistent sequence, specify on " + Ability.SetupNoteVarName("On Cast Target Effect Data") + "'s sequence field";
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardActorEffectData cachedOnCastTargetEffectData;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (1)
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
			cachedOnCastTargetEffectData = m_abilityMod.m_onCastTargetEffectDataMod.GetModifiedValue(m_onCastTargetEffectData);
		}
		else
		{
			cachedOnCastTargetEffectData = m_onCastTargetEffectData;
		}
		m_cachedOnCastTargetEffectData = cachedOnCastTargetEffectData;
		StandardEffectInfo cachedAdditionalTargetHitEffect;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedAdditionalTargetHitEffect = m_abilityMod.m_additionalTargetHitEffectMod.GetModifiedValue(m_additionalTargetHitEffect);
		}
		else
		{
			cachedAdditionalTargetHitEffect = m_additionalTargetHitEffect;
		}
		m_cachedAdditionalTargetHitEffect = cachedAdditionalTargetHitEffect;
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
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
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	public bool TargetingIgnoreLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (1)
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
			result = m_abilityMod.m_targetingIgnoreLosMod.GetModifiedValue(m_targetingIgnoreLos);
		}
		else
		{
			result = m_targetingIgnoreLos;
		}
		return result;
	}

	public StandardActorEffectData GetOnCastTargetEffectData()
	{
		return (m_cachedOnCastTargetEffectData == null) ? m_onCastTargetEffectData : m_cachedOnCastTargetEffectData;
	}

	public StandardEffectInfo GetAdditionalTargetHitEffect()
	{
		return (m_cachedAdditionalTargetHitEffect == null) ? m_additionalTargetHitEffect : m_cachedAdditionalTargetHitEffect;
	}

	public float GetExplosionRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (5)
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
			result = m_abilityMod.m_explosionRadiusMod.GetModifiedValue(m_explosionRadius);
		}
		else
		{
			result = m_explosionRadius;
		}
		return result;
	}

	public bool IgnoreLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
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
			result = m_abilityMod.m_ignoreLosMod.GetModifiedValue(m_ignoreLos);
		}
		else
		{
			result = m_ignoreLos;
		}
		return result;
	}

	public int GetMaxDamage()
	{
		return (!m_abilityMod) ? m_maxDamage : m_abilityMod.m_maxDamageMod.GetModifiedValue(m_maxDamage);
	}

	public int GetBaseDamage()
	{
		return (!m_abilityMod) ? m_baseDamage : m_abilityMod.m_baseDamageMod.GetModifiedValue(m_baseDamage);
	}

	public float GetDamageMult()
	{
		float result;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_damageMultMod.GetModifiedValue(m_damageMult);
		}
		else
		{
			result = m_damageMult;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
		{
			while (true)
			{
				switch (1)
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
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public float GetAbsorbMultForHeal()
	{
		float result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_absorbMultForHealMod.GetModifiedValue(m_absorbMultForHeal);
		}
		else
		{
			result = m_absorbMultForHeal;
		}
		return result;
	}

	public float GetMultOnInitialDamageForSubseqHits()
	{
		float result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_multOnInitialDamageForSubseqHitsMod.GetModifiedValue(m_multOnInitialDamageForSubseqHits);
		}
		else
		{
			result = m_multOnInitialDamageForSubseqHits;
		}
		return result;
	}

	public int GetExtraHealOnHealAoeIfQueued()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (1)
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
			result = m_abilityMod.m_extraHealOnHealAoeIfQueuedMod.GetModifiedValue(m_extraHealOnHealAoeIfQueued);
		}
		else
		{
			result = m_extraHealOnHealAoeIfQueued;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetOnCastTargetEffectData().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		GetAdditionalTargetHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_onCastTargetEffectData.AddTooltipTokens(tokens, "OnCastTargetEffectData");
		AbilityMod.AddToken_EffectInfo(tokens, m_additionalTargetHitEffect, "AdditionalTargetHitEffect", m_additionalTargetHitEffect);
		AddTokenInt(tokens, "MaxDamage", string.Empty, m_maxDamage);
		AddTokenInt(tokens, "BaseDamage", string.Empty, m_baseDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "ExtraHealOnHealAoeIfQueued", string.Empty, m_extraHealOnHealAoeIfQueued);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, false, true, true, ValidateCheckPath.Ignore, TargetingIgnoreLos(), true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SenseiBide))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_SenseiBide);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
