using System.Collections.Generic;
using UnityEngine;

public class FishManBubble : Ability
{
	[Space(10f)]
	[Header("-- Targeting")]
	public AbilityAreaShape m_targetShape;
	public bool m_canTargetEnemies = true;
	public bool m_canTargetAllies;
	public bool m_canTargetSelf = true;
	[Header("-- Initial Hit")]
	public StandardEffectInfo m_effectOnAllies;
	public StandardEffectInfo m_effectOnEnemies;
	public int m_initialHitHealingToAllies;
	public int m_initialHitDamageToEnemies;
	[Header("-- Explosion Data")]
	public int m_numTurnsBeforeFirstExplosion = 1;
	public int m_numExplosionsBeforeEnding = 1;
	public AbilityAreaShape m_explosionShape;
	public bool m_explosionIgnoresLineOfSight;
	public bool m_explosionCanAffectEffectHolder;
	[Header("-- Explosion Hit")]
	public int m_explosionHealingToAllies;
	public int m_explosionDamageToEnemies;
	public StandardEffectInfo m_explosionEffectToAllies;
	public StandardEffectInfo m_explosionEffectToEnemies;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_bubbleSequencePrefab;
	public GameObject m_explosionSequencePrefab;
	public float m_persistentSequenceRemoveDelay = 0.5f;

	private StandardEffectInfo m_cachedEffectOnAllies;
	private StandardEffectInfo m_cachedEffectOnEnemies;
	private StandardEffectInfo m_cachedExplosionEffectToAllies;
	private StandardEffectInfo m_cachedExplosionEffectToEnemies;
	private AbilityMod_FishManBubble m_abilityMod;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		AbilityUtil_Targeter_Shape targeter = new AbilityUtil_Targeter_Shape(
			this,
			GetExplosionShape(),
			false,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			ExplosionAffectEnemies(),
			ExplosionAffectAllies(),
			AbilityUtil_Targeter.AffectsActor.Never,
			AbilityUtil_Targeter.AffectsActor.Always);
		targeter.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Secondary);
		Targeter = targeter;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnAllies = m_abilityMod != null
			? m_abilityMod.m_effectOnAlliesMod.GetModifiedValue(m_effectOnAllies)
			: m_effectOnAllies;
		m_cachedEffectOnEnemies = m_abilityMod != null
			? m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies)
			: m_effectOnEnemies;
		m_cachedExplosionEffectToAllies = m_abilityMod != null
			? m_abilityMod.m_explosionEffectToAlliesMod.GetModifiedValue(m_explosionEffectToAllies)
			: m_explosionEffectToAllies;
		m_cachedExplosionEffectToEnemies = m_abilityMod != null
			? m_abilityMod.m_explosionEffectToEnemiesMod.GetModifiedValue(m_explosionEffectToEnemies)
			: m_explosionEffectToEnemies;
	}

	public AbilityAreaShape GetTargetShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targetShapeMod.GetModifiedValue(m_targetShape)
			: m_targetShape;
	}

	public bool CanTargetEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canTargetEnemiesMod.GetModifiedValue(m_canTargetEnemies)
			: m_canTargetEnemies;
	}

	public bool CanTargetAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canTargetAlliesMod.GetModifiedValue(m_canTargetAllies)
			: m_canTargetAllies;
	}

	public bool CanTargetSelf()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canTargetSelfMod.GetModifiedValue(m_canTargetSelf)
			: m_canTargetSelf;
	}

	public StandardEffectInfo GetEffectOnAllies()
	{
		return m_cachedEffectOnAllies ?? m_effectOnAllies;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		return m_cachedEffectOnEnemies ?? m_effectOnEnemies;
	}

	public int GetInitialHitHealingToAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_initialHitHealingToAlliesMod.GetModifiedValue(m_initialHitHealingToAllies)
			: m_initialHitHealingToAllies;
	}

	public int GetInitialHitDamageToEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_initialHitDamageToEnemiesMod.GetModifiedValue(m_initialHitDamageToEnemies)
			: m_initialHitDamageToEnemies;
	}

	public int GetNumTurnsBeforeFirstExplosion()
	{
		return m_abilityMod != null
			? m_abilityMod.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(m_numTurnsBeforeFirstExplosion)
			: m_numTurnsBeforeFirstExplosion;
	}

	public int GetNumExplosionsBeforeEnding()
	{
		return m_abilityMod != null
			? m_abilityMod.m_numExplosionsBeforeEndingMod.GetModifiedValue(m_numExplosionsBeforeEnding)
			: m_numExplosionsBeforeEnding;
	}

	public AbilityAreaShape GetExplosionShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionShapeMod.GetModifiedValue(m_explosionShape)
			: m_explosionShape;
	}

	public bool ExplosionIgnoresLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionIgnoresLineOfSightMod.GetModifiedValue(m_explosionIgnoresLineOfSight)
			: m_explosionIgnoresLineOfSight;
	}

	public bool ExplosionCanAffectEffectHolder()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionCanAffectEffectHolderMod.GetModifiedValue(m_explosionCanAffectEffectHolder)
			: m_explosionCanAffectEffectHolder;
	}

	public int GetExplosionHealingToAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionHealingToAlliesMod.GetModifiedValue(m_explosionHealingToAllies)
			: m_explosionHealingToAllies;
	}

	public int GetExplosionDamageToEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionDamageToEnemiesMod.GetModifiedValue(m_explosionDamageToEnemies)
			: m_explosionDamageToEnemies;
	}

	public StandardEffectInfo GetExplosionEffectToAllies()
	{
		return m_cachedExplosionEffectToAllies ?? m_explosionEffectToAllies;
	}

	public StandardEffectInfo GetExplosionEffectToEnemies()
	{
		return m_cachedExplosionEffectToEnemies ?? m_explosionEffectToEnemies;
	}

	public bool ExplosionAffectAllies()
	{
		return GetExplosionHealingToAllies() > 0 || GetExplosionEffectToAllies().m_applyEffect;
	}

	public bool ExplosionAffectEnemies()
	{
		return GetExplosionDamageToEnemies() > 0 || GetExplosionEffectToEnemies().m_applyEffect;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_FishManBubble))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_FishManBubble;
		Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManBubble abilityMod_FishManBubble = modAsBase as AbilityMod_FishManBubble;
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManBubble != null
			? abilityMod_FishManBubble.m_effectOnAlliesMod.GetModifiedValue(m_effectOnAllies)
			: m_effectOnAllies, "EffectOnAllies", m_effectOnAllies);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManBubble != null
			? abilityMod_FishManBubble.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies)
			: m_effectOnEnemies, "EffectOnEnemies", m_effectOnEnemies);
		AddTokenInt(tokens, "InitialHitHealingToAllies", string.Empty, abilityMod_FishManBubble != null
			? abilityMod_FishManBubble.m_initialHitHealingToAlliesMod.GetModifiedValue(m_initialHitHealingToAllies)
			: m_initialHitHealingToAllies);
		AddTokenInt(tokens, "InitialHitDamageToEnemies", string.Empty, abilityMod_FishManBubble != null
			? abilityMod_FishManBubble.m_initialHitDamageToEnemiesMod.GetModifiedValue(m_initialHitDamageToEnemies)
			: m_initialHitDamageToEnemies);
		AddTokenInt(tokens, "NumTurnsBeforeFirstExplosion", string.Empty, abilityMod_FishManBubble != null
			? abilityMod_FishManBubble.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(m_numTurnsBeforeFirstExplosion)
			: m_numTurnsBeforeFirstExplosion);
		AddTokenInt(tokens, "NumExplosionsBeforeEnding", string.Empty, abilityMod_FishManBubble != null
			? abilityMod_FishManBubble.m_numExplosionsBeforeEndingMod.GetModifiedValue(m_numExplosionsBeforeEnding)
			: m_numExplosionsBeforeEnding);
		AddTokenInt(tokens, "ExplosionHealingToAllies", string.Empty, abilityMod_FishManBubble != null
			? abilityMod_FishManBubble.m_explosionHealingToAlliesMod.GetModifiedValue(m_explosionHealingToAllies)
			: m_explosionHealingToAllies);
		AddTokenInt(tokens, "ExplosionDamageToEnemies", string.Empty, abilityMod_FishManBubble != null
			? abilityMod_FishManBubble.m_explosionDamageToEnemiesMod.GetModifiedValue(m_explosionDamageToEnemies)
			: m_explosionDamageToEnemies);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManBubble != null
			? abilityMod_FishManBubble.m_explosionEffectToAlliesMod.GetModifiedValue(m_explosionEffectToAllies)
			: m_explosionEffectToAllies, "ExplosionEffectToAllies", m_explosionEffectToAllies);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManBubble != null
			? abilityMod_FishManBubble.m_explosionEffectToEnemiesMod.GetModifiedValue(m_explosionEffectToEnemies)
			: m_explosionEffectToEnemies, "ExplosionEffectToEnemies", m_explosionEffectToEnemies);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		if (GetInitialHitDamageToEnemies() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref number, AbilityTooltipSubject.Enemy, GetInitialHitDamageToEnemies());
		}
		if (GetInitialHitHealingToAllies() > 0)
		{
			AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Secondary, GetInitialHitHealingToAllies());
		}
		AbilityTooltipHelper.ReportDamage(ref number, AbilityTooltipSubject.Enemy, GetExplosionDamageToEnemies());
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Secondary, GetExplosionHealingToAllies());
		GetEffectOnEnemies().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Enemy);
		GetEffectOnAllies().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Secondary);
		return number;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null
		    || !tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary)
		    || !(Targeter is AbilityUtil_Targeter_Shape))
		{
			return null;
		}

		AbilityUtil_Targeter_Shape targeter = Targeter as AbilityUtil_Targeter_Shape;
		ActorData lastCenterSquareActor = targeter.m_lastCenterSquareActor;
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (lastCenterSquareActor != null)
		{
			dictionary[AbilityTooltipSymbol.Absorb] = targetActor == lastCenterSquareActor
				? GetEffectOnAllies().m_effectData.m_absorbAmount
				: 0;
			dictionary[AbilityTooltipSymbol.Healing] = targetActor != lastCenterSquareActor
				? GetExplosionHealingToAllies()
				: 0;
		}
		else
		{
			dictionary[AbilityTooltipSymbol.Absorb] = 0;
		}
		return dictionary;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return CanTargetActorInDecision(
			caster,
			target.GetCurrentBestActorTarget(),
			CanTargetEnemies(),
			CanTargetAllies(),
			CanTargetSelf(),
			ValidateCheckPath.Ignore,
			true,
			true);
	}
}
