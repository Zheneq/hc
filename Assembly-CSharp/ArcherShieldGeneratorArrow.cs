using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ArcherShieldGeneratorArrow : Ability
{
	[Header("-- Ground effect")]
	public bool m_penetrateLoS;
	public bool m_affectsEnemies;
	public bool m_affectsAllies;
	public bool m_affectsCaster;
	public int m_lessAbsorbPerTurn = 5;
	public StandardGroundEffectInfo m_groundEffectInfo;
	public StandardEffectInfo m_directHitEnemyEffect;
	public StandardEffectInfo m_directHitAllyEffect;
	[Header("-- Extra effect for shielding that last different number of turns from main effect, etc")]
	public StandardEffectInfo m_extraAllyHitEffect;
	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_castSequencePrefab;
	
	private AbilityMod_ArcherShieldGeneratorArrow m_abilityMod;
	private Archer_SyncComponent m_syncComp;
	private StandardGroundEffectInfo m_cachedGroundEffect;
	private StandardEffectInfo m_cachedDirectHitEnemyEffect;
	private StandardEffectInfo m_cachedDirectHitAllyEffect;
	private StandardEffectInfo m_cachedExtraAllyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Shield Generator Arrow";
		}
		m_syncComp = GetComponent<Archer_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			GetGroundEffectInfo().m_groundEffectData.shape,
			PenetrateLoS(),
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			AffectsEnemies(),
			AffectsAllies(),
			AffectsCaster()
				? AbilityUtil_Targeter.AffectsActor.Possible
				: AbilityUtil_Targeter.AffectsActor.Never);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherShieldGeneratorArrow))
		{
			m_abilityMod = abilityMod as AbilityMod_ArcherShieldGeneratorArrow;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private void SetCachedFields()
	{
		m_cachedGroundEffect = m_groundEffectInfo;
		m_cachedDirectHitEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_directHitEnemyEffectMod.GetModifiedValue(m_directHitEnemyEffect)
			: m_directHitEnemyEffect;
		m_cachedDirectHitAllyEffect = m_abilityMod != null
			? m_abilityMod.m_directHitAllyEffectMod.GetModifiedValue(m_directHitAllyEffect)
			: m_directHitAllyEffect;
		m_cachedExtraAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_extraAllyHitEffectMod.GetModifiedValue(m_extraAllyHitEffect)
			: m_extraAllyHitEffect;
	}

	public bool PenetrateLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS)
			: m_penetrateLoS;
	}

	public bool AffectsEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_affectsEnemiesMod.GetModifiedValue(m_affectsEnemies)
			: m_affectsEnemies;
	}

	public bool AffectsAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_affectsAlliesMod.GetModifiedValue(m_affectsAllies)
			: m_affectsAllies;
	}

	public bool AffectsCaster()
	{
		return m_abilityMod != null
			? m_abilityMod.m_affectsCasterMod.GetModifiedValue(m_affectsCaster)
			: m_affectsCaster;
	}

	private StandardGroundEffectInfo GetGroundEffectInfo()
	{
		return m_cachedGroundEffect ?? m_groundEffectInfo;
	}

	public int GetLessAbsorbPerTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lessAbsorbPerTurnMod.GetModifiedValue(m_lessAbsorbPerTurn)
			: m_lessAbsorbPerTurn;
	}

	public StandardEffectInfo GetDirectHitEnemyEffect()
	{
		return m_cachedDirectHitEnemyEffect ?? m_directHitEnemyEffect;
	}

	public StandardEffectInfo GetDirectHitAllyEffect()
	{
		return m_cachedDirectHitAllyEffect ?? m_directHitAllyEffect;
	}

	public StandardEffectInfo GetExtraAllyHitEffect()
	{
		return m_cachedExtraAllyHitEffect ?? m_extraAllyHitEffect;
	}

	public int GetCooldownReductionOnDash()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownReductionOnDash.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraAbsorbPerEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAbsorbPerEnemyHit.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraAbsorbIfEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAbsorbIfEnemyHit.GetModifiedValue(0)
			: 0;
	}

	public int GetExtraAbsorbIfOnlyOneAllyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAbsorbIfOnlyOneAllyHit.GetModifiedValue(0)
			: 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "LessAbsorbPerTurn", string.Empty, m_lessAbsorbPerTurn);
		AbilityMod.AddToken_EffectInfo(tokens, m_directHitEnemyEffect, "DirectHitEnemyEffect", m_directHitEnemyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_directHitAllyEffect, "DirectHitAllyEffect", m_directHitAllyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_extraAllyHitEffect, "ExtraAllyHitEffect", m_extraAllyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_groundEffectInfo.m_applyGroundEffect)
		{
			m_groundEffectInfo.m_groundEffectData.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
		}
		if (AffectsAllies())
		{
			GetDirectHitAllyEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		if (AffectsCaster())
		{
			GetDirectHitAllyEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		if (AffectsEnemies())
		{
			GetDirectHitEnemyEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (m_syncComp != null && targetActor.GetTeam() == ActorData.GetTeam())
		{
			int extraAbsorb = m_syncComp.m_extraAbsorbForShieldGenerator;
			List<AbilityUtil_Targeter.ActorTarget> actorsInRange = Targeters[currentTargeterIndex].GetActorsInRange();
			if (!actorsInRange.IsNullOrEmpty())
			{
				int enemiesInRange = actorsInRange.Count(t => t.m_actor.GetTeam() != ActorData.GetTeam());
				if (actorsInRange.Count - enemiesInRange == 1)
				{
					extraAbsorb += GetExtraAbsorbIfOnlyOneAllyHit();
				}
				extraAbsorb += GetExtraAbsorbPerEnemyHit() * enemiesInRange;
				if (enemiesInRange > 0)
				{
					extraAbsorb += GetExtraAbsorbIfEnemyHit();
				}
			}
			int absorb = GetDirectHitAllyEffect().m_effectData.m_absorbAmount + extraAbsorb;
			StandardEffectInfo extraAllyHitEffect = GetExtraAllyHitEffect();
			if (extraAllyHitEffect.m_applyEffect && extraAllyHitEffect.m_effectData.m_absorbAmount > 0)
			{
				absorb += extraAllyHitEffect.m_effectData.m_absorbAmount;
			}
			dictionary[AbilityTooltipSymbol.Absorb] = absorb;
		}
		return dictionary;
	}
}
