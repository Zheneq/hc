using System.Collections.Generic;
using UnityEngine;

public class ScampAoeTether : Ability
{
	[Separator("Targeting", true)]
	public float m_aoeRadius = 5f;

	public bool m_ignoreLos;

	[Header("-- if > 0, will use this as distance to check whether tether should break")]
	public float m_tetherBreakDistanceOverride = -1f;

	[Separator("Whether to pull towards caster if target is out of range. If true, no longer do movement hits", true)]
	public bool m_pullToCasterInKnockback;

	public float m_maxKnockbackDist = 10f;

	[Separator("Disable in Shield Down mode?", true)]
	public bool m_disableIfShieldDown = true;

	[Separator("On Tether Apply", true)]
	public StandardEffectInfo m_tetherApplyEnemyEffect;

	[Separator("On Tether Break", true)]
	public int m_tetherBreakDamage = 10;

	public StandardEffectInfo m_tetherBreakEnemyEffecf;

	[Separator("Cdr if not triggered", true)]
	public int m_cdrIfNoTetherTrigger;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	public GameObject m_tetherBreakTriggerSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

	private AbilityMod_ScampAoeTether m_abilityMod;

	private StandardEffectInfo m_cachedTetherApplyEnemyEffect;

	private StandardEffectInfo m_cachedTetherBreakEnemyEffecf;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ScampAoeTether";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, GetAoeRadius(), IgnoreLos());
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, m_tetherApplyEnemyEffect, "TetherApplyEnemyEffect", m_tetherApplyEnemyEffect);
		AddTokenInt(tokens, "TetherBreakDamage", string.Empty, m_tetherBreakDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_tetherBreakEnemyEffecf, "TetherBreakEnemyEffecf", m_tetherBreakEnemyEffecf);
		AddTokenInt(tokens, "CdrIfNoTetherTrigger", string.Empty, m_cdrIfNoTetherTrigger);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.m_suitWasActiveOnTurnStart)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		return !DisableIfShieldDown();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedTetherApplyEnemyEffect;
		if (m_abilityMod != null)
		{
			cachedTetherApplyEnemyEffect = m_abilityMod.m_tetherApplyEnemyEffectMod.GetModifiedValue(m_tetherApplyEnemyEffect);
		}
		else
		{
			cachedTetherApplyEnemyEffect = m_tetherApplyEnemyEffect;
		}
		m_cachedTetherApplyEnemyEffect = cachedTetherApplyEnemyEffect;
		StandardEffectInfo cachedTetherBreakEnemyEffecf;
		if (m_abilityMod != null)
		{
			cachedTetherBreakEnemyEffecf = m_abilityMod.m_tetherBreakEnemyEffecfMod.GetModifiedValue(m_tetherBreakEnemyEffecf);
		}
		else
		{
			cachedTetherBreakEnemyEffecf = m_tetherBreakEnemyEffecf;
		}
		m_cachedTetherBreakEnemyEffecf = cachedTetherBreakEnemyEffecf;
	}

	public float GetAoeRadius()
	{
		float result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_aoeRadiusMod.GetModifiedValue(m_aoeRadius);
		}
		else
		{
			result = m_aoeRadius;
		}
		return result;
	}

	public bool IgnoreLos()
	{
		return (!(m_abilityMod != null)) ? m_ignoreLos : m_abilityMod.m_ignoreLosMod.GetModifiedValue(m_ignoreLos);
	}

	public float GetTetherBreakDistanceOverride()
	{
		float result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_tetherBreakDistanceOverrideMod.GetModifiedValue(m_tetherBreakDistanceOverride);
		}
		else
		{
			result = m_tetherBreakDistanceOverride;
		}
		return result;
	}

	public bool PullToCasterInKnockback()
	{
		return (!(m_abilityMod != null)) ? m_pullToCasterInKnockback : m_abilityMod.m_pullToCasterInKnockbackMod.GetModifiedValue(m_pullToCasterInKnockback);
	}

	public float GetMaxKnockbackDist()
	{
		float result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_maxKnockbackDistMod.GetModifiedValue(m_maxKnockbackDist);
		}
		else
		{
			result = m_maxKnockbackDist;
		}
		return result;
	}

	public bool DisableIfShieldDown()
	{
		return (!(m_abilityMod != null)) ? m_disableIfShieldDown : m_abilityMod.m_disableIfShieldDownMod.GetModifiedValue(m_disableIfShieldDown);
	}

	public StandardEffectInfo GetTetherApplyEnemyEffect()
	{
		return (m_cachedTetherApplyEnemyEffect == null) ? m_tetherApplyEnemyEffect : m_cachedTetherApplyEnemyEffect;
	}

	public int GetTetherBreakDamage()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_tetherBreakDamageMod.GetModifiedValue(m_tetherBreakDamage);
		}
		else
		{
			result = m_tetherBreakDamage;
		}
		return result;
	}

	public StandardEffectInfo GetTetherBreakEnemyEffecf()
	{
		StandardEffectInfo result;
		if (m_cachedTetherBreakEnemyEffecf != null)
		{
			result = m_cachedTetherBreakEnemyEffecf;
		}
		else
		{
			result = m_tetherBreakEnemyEffecf;
		}
		return result;
	}

	public int GetCdrIfNoTetherTrigger()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_cdrIfNoTetherTriggerMod.GetModifiedValue(m_cdrIfNoTetherTrigger);
		}
		else
		{
			result = m_cdrIfNoTetherTrigger;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetAoeRadius();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ScampAoeTether))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ScampAoeTether);
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
