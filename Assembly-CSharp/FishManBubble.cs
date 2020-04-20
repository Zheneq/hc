using System;
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
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Always;
		AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, this.GetExplosionShape(), false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, this.ExplosionAffectEnemies(), this.ExplosionAffectAllies(), affectsCaster, affectsBestTarget);
		abilityUtil_Targeter_Shape.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Secondary, AbilityTooltipSubject.None);
		base.Targeter = abilityUtil_Targeter_Shape;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnAllies;
		if (this.m_abilityMod)
		{
			cachedEffectOnAllies = this.m_abilityMod.m_effectOnAlliesMod.GetModifiedValue(this.m_effectOnAllies);
		}
		else
		{
			cachedEffectOnAllies = this.m_effectOnAllies;
		}
		this.m_cachedEffectOnAllies = cachedEffectOnAllies;
		this.m_cachedEffectOnEnemies = ((!this.m_abilityMod) ? this.m_effectOnEnemies : this.m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(this.m_effectOnEnemies));
		StandardEffectInfo cachedExplosionEffectToAllies;
		if (this.m_abilityMod)
		{
			cachedExplosionEffectToAllies = this.m_abilityMod.m_explosionEffectToAlliesMod.GetModifiedValue(this.m_explosionEffectToAllies);
		}
		else
		{
			cachedExplosionEffectToAllies = this.m_explosionEffectToAllies;
		}
		this.m_cachedExplosionEffectToAllies = cachedExplosionEffectToAllies;
		StandardEffectInfo cachedExplosionEffectToEnemies;
		if (this.m_abilityMod)
		{
			cachedExplosionEffectToEnemies = this.m_abilityMod.m_explosionEffectToEnemiesMod.GetModifiedValue(this.m_explosionEffectToEnemies);
		}
		else
		{
			cachedExplosionEffectToEnemies = this.m_explosionEffectToEnemies;
		}
		this.m_cachedExplosionEffectToEnemies = cachedExplosionEffectToEnemies;
	}

	public AbilityAreaShape GetTargetShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_targetShapeMod.GetModifiedValue(this.m_targetShape);
		}
		else
		{
			result = this.m_targetShape;
		}
		return result;
	}

	public bool CanTargetEnemies()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canTargetEnemiesMod.GetModifiedValue(this.m_canTargetEnemies);
		}
		else
		{
			result = this.m_canTargetEnemies;
		}
		return result;
	}

	public bool CanTargetAllies()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canTargetAlliesMod.GetModifiedValue(this.m_canTargetAllies);
		}
		else
		{
			result = this.m_canTargetAllies;
		}
		return result;
	}

	public bool CanTargetSelf()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canTargetSelfMod.GetModifiedValue(this.m_canTargetSelf);
		}
		else
		{
			result = this.m_canTargetSelf;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnAllies()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnAllies != null)
		{
			result = this.m_cachedEffectOnAllies;
		}
		else
		{
			result = this.m_effectOnAllies;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnEnemies != null)
		{
			result = this.m_cachedEffectOnEnemies;
		}
		else
		{
			result = this.m_effectOnEnemies;
		}
		return result;
	}

	public int GetInitialHitHealingToAllies()
	{
		return (!this.m_abilityMod) ? this.m_initialHitHealingToAllies : this.m_abilityMod.m_initialHitHealingToAlliesMod.GetModifiedValue(this.m_initialHitHealingToAllies);
	}

	public int GetInitialHitDamageToEnemies()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_initialHitDamageToEnemiesMod.GetModifiedValue(this.m_initialHitDamageToEnemies);
		}
		else
		{
			result = this.m_initialHitDamageToEnemies;
		}
		return result;
	}

	public int GetNumTurnsBeforeFirstExplosion()
	{
		return (!this.m_abilityMod) ? this.m_numTurnsBeforeFirstExplosion : this.m_abilityMod.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(this.m_numTurnsBeforeFirstExplosion);
	}

	public int GetNumExplosionsBeforeEnding()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_numExplosionsBeforeEndingMod.GetModifiedValue(this.m_numExplosionsBeforeEnding);
		}
		else
		{
			result = this.m_numExplosionsBeforeEnding;
		}
		return result;
	}

	public AbilityAreaShape GetExplosionShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_explosionShapeMod.GetModifiedValue(this.m_explosionShape);
		}
		else
		{
			result = this.m_explosionShape;
		}
		return result;
	}

	public bool ExplosionIgnoresLineOfSight()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_explosionIgnoresLineOfSightMod.GetModifiedValue(this.m_explosionIgnoresLineOfSight);
		}
		else
		{
			result = this.m_explosionIgnoresLineOfSight;
		}
		return result;
	}

	public bool ExplosionCanAffectEffectHolder()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_explosionCanAffectEffectHolderMod.GetModifiedValue(this.m_explosionCanAffectEffectHolder);
		}
		else
		{
			result = this.m_explosionCanAffectEffectHolder;
		}
		return result;
	}

	public int GetExplosionHealingToAllies()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_explosionHealingToAlliesMod.GetModifiedValue(this.m_explosionHealingToAllies);
		}
		else
		{
			result = this.m_explosionHealingToAllies;
		}
		return result;
	}

	public int GetExplosionDamageToEnemies()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_explosionDamageToEnemiesMod.GetModifiedValue(this.m_explosionDamageToEnemies);
		}
		else
		{
			result = this.m_explosionDamageToEnemies;
		}
		return result;
	}

	public StandardEffectInfo GetExplosionEffectToAllies()
	{
		StandardEffectInfo result;
		if (this.m_cachedExplosionEffectToAllies != null)
		{
			result = this.m_cachedExplosionEffectToAllies;
		}
		else
		{
			result = this.m_explosionEffectToAllies;
		}
		return result;
	}

	public StandardEffectInfo GetExplosionEffectToEnemies()
	{
		StandardEffectInfo result;
		if (this.m_cachedExplosionEffectToEnemies != null)
		{
			result = this.m_cachedExplosionEffectToEnemies;
		}
		else
		{
			result = this.m_explosionEffectToEnemies;
		}
		return result;
	}

	public bool ExplosionAffectAllies()
	{
		bool result;
		if (this.GetExplosionHealingToAllies() <= 0)
		{
			result = this.GetExplosionEffectToAllies().m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool ExplosionAffectEnemies()
	{
		return this.GetExplosionDamageToEnemies() > 0 || this.GetExplosionEffectToEnemies().m_applyEffect;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManBubble))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_FishManBubble);
			this.Setup();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManBubble abilityMod_FishManBubble = modAsBase as AbilityMod_FishManBubble;
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManBubble) ? this.m_effectOnAllies : abilityMod_FishManBubble.m_effectOnAlliesMod.GetModifiedValue(this.m_effectOnAllies), "EffectOnAllies", this.m_effectOnAllies, true);
		StandardEffectInfo effectInfo;
		if (abilityMod_FishManBubble)
		{
			effectInfo = abilityMod_FishManBubble.m_effectOnEnemiesMod.GetModifiedValue(this.m_effectOnEnemies);
		}
		else
		{
			effectInfo = this.m_effectOnEnemies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnEnemies", this.m_effectOnEnemies, true);
		string name = "InitialHitHealingToAllies";
		string empty = string.Empty;
		int val;
		if (abilityMod_FishManBubble)
		{
			val = abilityMod_FishManBubble.m_initialHitHealingToAlliesMod.GetModifiedValue(this.m_initialHitHealingToAllies);
		}
		else
		{
			val = this.m_initialHitHealingToAllies;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "InitialHitDamageToEnemies";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_FishManBubble)
		{
			val2 = abilityMod_FishManBubble.m_initialHitDamageToEnemiesMod.GetModifiedValue(this.m_initialHitDamageToEnemies);
		}
		else
		{
			val2 = this.m_initialHitDamageToEnemies;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "NumTurnsBeforeFirstExplosion";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_FishManBubble)
		{
			val3 = abilityMod_FishManBubble.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(this.m_numTurnsBeforeFirstExplosion);
		}
		else
		{
			val3 = this.m_numTurnsBeforeFirstExplosion;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		base.AddTokenInt(tokens, "NumExplosionsBeforeEnding", string.Empty, (!abilityMod_FishManBubble) ? this.m_numExplosionsBeforeEnding : abilityMod_FishManBubble.m_numExplosionsBeforeEndingMod.GetModifiedValue(this.m_numExplosionsBeforeEnding), false);
		string name4 = "ExplosionHealingToAllies";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_FishManBubble)
		{
			val4 = abilityMod_FishManBubble.m_explosionHealingToAlliesMod.GetModifiedValue(this.m_explosionHealingToAllies);
		}
		else
		{
			val4 = this.m_explosionHealingToAllies;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		string name5 = "ExplosionDamageToEnemies";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_FishManBubble)
		{
			val5 = abilityMod_FishManBubble.m_explosionDamageToEnemiesMod.GetModifiedValue(this.m_explosionDamageToEnemies);
		}
		else
		{
			val5 = this.m_explosionDamageToEnemies;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_FishManBubble)
		{
			effectInfo2 = abilityMod_FishManBubble.m_explosionEffectToAlliesMod.GetModifiedValue(this.m_explosionEffectToAllies);
		}
		else
		{
			effectInfo2 = this.m_explosionEffectToAllies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "ExplosionEffectToAllies", this.m_explosionEffectToAllies, true);
		StandardEffectInfo effectInfo3;
		if (abilityMod_FishManBubble)
		{
			effectInfo3 = abilityMod_FishManBubble.m_explosionEffectToEnemiesMod.GetModifiedValue(this.m_explosionEffectToEnemies);
		}
		else
		{
			effectInfo3 = this.m_explosionEffectToEnemies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "ExplosionEffectToEnemies", this.m_explosionEffectToEnemies, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.GetInitialHitDamageToEnemies() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetInitialHitDamageToEnemies());
		}
		if (this.GetInitialHitHealingToAllies() > 0)
		{
			AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Secondary, this.GetInitialHitHealingToAllies());
		}
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetExplosionDamageToEnemies());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Secondary, this.GetExplosionHealingToAllies());
		this.GetEffectOnEnemies().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		this.GetEffectOnAllies().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				if (base.Targeter is AbilityUtil_Targeter_Shape)
				{
					AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = base.Targeter as AbilityUtil_Targeter_Shape;
					ActorData lastCenterSquareActor = abilityUtil_Targeter_Shape.m_lastCenterSquareActor;
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					if (lastCenterSquareActor != null)
					{
						dictionary[AbilityTooltipSymbol.Absorb] = ((!(targetActor == lastCenterSquareActor)) ? 0 : this.GetEffectOnAllies().m_effectData.m_absorbAmount);
						Dictionary<AbilityTooltipSymbol, int> dictionary2 = dictionary;
						AbilityTooltipSymbol key = AbilityTooltipSymbol.Healing;
						int value;
						if (targetActor != lastCenterSquareActor)
						{
							value = this.GetExplosionHealingToAllies();
						}
						else
						{
							value = 0;
						}
						dictionary2[key] = value;
					}
					else
					{
						dictionary[AbilityTooltipSymbol.Absorb] = 0;
					}
				}
			}
		}
		return dictionary;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, this.CanTargetEnemies(), this.CanTargetAllies(), this.CanTargetSelf(), Ability.ValidateCheckPath.Ignore, true, true, false);
	}
}
