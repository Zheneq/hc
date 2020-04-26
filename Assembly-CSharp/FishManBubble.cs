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
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Never;
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Always;
		AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, GetExplosionShape(), false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, ExplosionAffectEnemies(), ExplosionAffectAllies(), affectsCaster, affectsBestTarget);
		abilityUtil_Targeter_Shape.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Secondary);
		base.Targeter = abilityUtil_Targeter_Shape;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnAllies;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnAllies = m_abilityMod.m_effectOnAlliesMod.GetModifiedValue(m_effectOnAllies);
		}
		else
		{
			cachedEffectOnAllies = m_effectOnAllies;
		}
		m_cachedEffectOnAllies = cachedEffectOnAllies;
		m_cachedEffectOnEnemies = ((!m_abilityMod) ? m_effectOnEnemies : m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies));
		StandardEffectInfo cachedExplosionEffectToAllies;
		if ((bool)m_abilityMod)
		{
			cachedExplosionEffectToAllies = m_abilityMod.m_explosionEffectToAlliesMod.GetModifiedValue(m_explosionEffectToAllies);
		}
		else
		{
			cachedExplosionEffectToAllies = m_explosionEffectToAllies;
		}
		m_cachedExplosionEffectToAllies = cachedExplosionEffectToAllies;
		StandardEffectInfo cachedExplosionEffectToEnemies;
		if ((bool)m_abilityMod)
		{
			cachedExplosionEffectToEnemies = m_abilityMod.m_explosionEffectToEnemiesMod.GetModifiedValue(m_explosionEffectToEnemies);
		}
		else
		{
			cachedExplosionEffectToEnemies = m_explosionEffectToEnemies;
		}
		m_cachedExplosionEffectToEnemies = cachedExplosionEffectToEnemies;
	}

	public AbilityAreaShape GetTargetShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_targetShapeMod.GetModifiedValue(m_targetShape);
		}
		else
		{
			result = m_targetShape;
		}
		return result;
	}

	public bool CanTargetEnemies()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canTargetEnemiesMod.GetModifiedValue(m_canTargetEnemies);
		}
		else
		{
			result = m_canTargetEnemies;
		}
		return result;
	}

	public bool CanTargetAllies()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canTargetAlliesMod.GetModifiedValue(m_canTargetAllies);
		}
		else
		{
			result = m_canTargetAllies;
		}
		return result;
	}

	public bool CanTargetSelf()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canTargetSelfMod.GetModifiedValue(m_canTargetSelf);
		}
		else
		{
			result = m_canTargetSelf;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnAllies()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnAllies != null)
		{
			result = m_cachedEffectOnAllies;
		}
		else
		{
			result = m_effectOnAllies;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnEnemies != null)
		{
			result = m_cachedEffectOnEnemies;
		}
		else
		{
			result = m_effectOnEnemies;
		}
		return result;
	}

	public int GetInitialHitHealingToAllies()
	{
		return (!m_abilityMod) ? m_initialHitHealingToAllies : m_abilityMod.m_initialHitHealingToAlliesMod.GetModifiedValue(m_initialHitHealingToAllies);
	}

	public int GetInitialHitDamageToEnemies()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_initialHitDamageToEnemiesMod.GetModifiedValue(m_initialHitDamageToEnemies);
		}
		else
		{
			result = m_initialHitDamageToEnemies;
		}
		return result;
	}

	public int GetNumTurnsBeforeFirstExplosion()
	{
		return (!m_abilityMod) ? m_numTurnsBeforeFirstExplosion : m_abilityMod.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(m_numTurnsBeforeFirstExplosion);
	}

	public int GetNumExplosionsBeforeEnding()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_numExplosionsBeforeEndingMod.GetModifiedValue(m_numExplosionsBeforeEnding);
		}
		else
		{
			result = m_numExplosionsBeforeEnding;
		}
		return result;
	}

	public AbilityAreaShape GetExplosionShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_explosionShapeMod.GetModifiedValue(m_explosionShape);
		}
		else
		{
			result = m_explosionShape;
		}
		return result;
	}

	public bool ExplosionIgnoresLineOfSight()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_explosionIgnoresLineOfSightMod.GetModifiedValue(m_explosionIgnoresLineOfSight);
		}
		else
		{
			result = m_explosionIgnoresLineOfSight;
		}
		return result;
	}

	public bool ExplosionCanAffectEffectHolder()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_explosionCanAffectEffectHolderMod.GetModifiedValue(m_explosionCanAffectEffectHolder);
		}
		else
		{
			result = m_explosionCanAffectEffectHolder;
		}
		return result;
	}

	public int GetExplosionHealingToAllies()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_explosionHealingToAlliesMod.GetModifiedValue(m_explosionHealingToAllies);
		}
		else
		{
			result = m_explosionHealingToAllies;
		}
		return result;
	}

	public int GetExplosionDamageToEnemies()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_explosionDamageToEnemiesMod.GetModifiedValue(m_explosionDamageToEnemies);
		}
		else
		{
			result = m_explosionDamageToEnemies;
		}
		return result;
	}

	public StandardEffectInfo GetExplosionEffectToAllies()
	{
		StandardEffectInfo result;
		if (m_cachedExplosionEffectToAllies != null)
		{
			result = m_cachedExplosionEffectToAllies;
		}
		else
		{
			result = m_explosionEffectToAllies;
		}
		return result;
	}

	public StandardEffectInfo GetExplosionEffectToEnemies()
	{
		StandardEffectInfo result;
		if (m_cachedExplosionEffectToEnemies != null)
		{
			result = m_cachedExplosionEffectToEnemies;
		}
		else
		{
			result = m_explosionEffectToEnemies;
		}
		return result;
	}

	public bool ExplosionAffectAllies()
	{
		int result;
		if (GetExplosionHealingToAllies() <= 0)
		{
			result = (GetExplosionEffectToAllies().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool ExplosionAffectEnemies()
	{
		return GetExplosionDamageToEnemies() > 0 || GetExplosionEffectToEnemies().m_applyEffect;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManBubble))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_FishManBubble);
					Setup();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManBubble abilityMod_FishManBubble = modAsBase as AbilityMod_FishManBubble;
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManBubble) ? m_effectOnAllies : abilityMod_FishManBubble.m_effectOnAlliesMod.GetModifiedValue(m_effectOnAllies), "EffectOnAllies", m_effectOnAllies);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_FishManBubble)
		{
			effectInfo = abilityMod_FishManBubble.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies);
		}
		else
		{
			effectInfo = m_effectOnEnemies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnEnemies", m_effectOnEnemies);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_FishManBubble)
		{
			val = abilityMod_FishManBubble.m_initialHitHealingToAlliesMod.GetModifiedValue(m_initialHitHealingToAllies);
		}
		else
		{
			val = m_initialHitHealingToAllies;
		}
		AddTokenInt(tokens, "InitialHitHealingToAllies", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_FishManBubble)
		{
			val2 = abilityMod_FishManBubble.m_initialHitDamageToEnemiesMod.GetModifiedValue(m_initialHitDamageToEnemies);
		}
		else
		{
			val2 = m_initialHitDamageToEnemies;
		}
		AddTokenInt(tokens, "InitialHitDamageToEnemies", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_FishManBubble)
		{
			val3 = abilityMod_FishManBubble.m_numTurnsBeforeFirstExplosionMod.GetModifiedValue(m_numTurnsBeforeFirstExplosion);
		}
		else
		{
			val3 = m_numTurnsBeforeFirstExplosion;
		}
		AddTokenInt(tokens, "NumTurnsBeforeFirstExplosion", empty3, val3);
		AddTokenInt(tokens, "NumExplosionsBeforeEnding", string.Empty, (!abilityMod_FishManBubble) ? m_numExplosionsBeforeEnding : abilityMod_FishManBubble.m_numExplosionsBeforeEndingMod.GetModifiedValue(m_numExplosionsBeforeEnding));
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_FishManBubble)
		{
			val4 = abilityMod_FishManBubble.m_explosionHealingToAlliesMod.GetModifiedValue(m_explosionHealingToAllies);
		}
		else
		{
			val4 = m_explosionHealingToAllies;
		}
		AddTokenInt(tokens, "ExplosionHealingToAllies", empty4, val4);
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_FishManBubble)
		{
			val5 = abilityMod_FishManBubble.m_explosionDamageToEnemiesMod.GetModifiedValue(m_explosionDamageToEnemies);
		}
		else
		{
			val5 = m_explosionDamageToEnemies;
		}
		AddTokenInt(tokens, "ExplosionDamageToEnemies", empty5, val5);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_FishManBubble)
		{
			effectInfo2 = abilityMod_FishManBubble.m_explosionEffectToAlliesMod.GetModifiedValue(m_explosionEffectToAllies);
		}
		else
		{
			effectInfo2 = m_explosionEffectToAllies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "ExplosionEffectToAllies", m_explosionEffectToAllies);
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_FishManBubble)
		{
			effectInfo3 = abilityMod_FishManBubble.m_explosionEffectToEnemiesMod.GetModifiedValue(m_explosionEffectToEnemies);
		}
		else
		{
			effectInfo3 = m_explosionEffectToEnemies;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "ExplosionEffectToEnemies", m_explosionEffectToEnemies);
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
						dictionary[AbilityTooltipSymbol.Absorb] = ((targetActor == lastCenterSquareActor) ? GetEffectOnAllies().m_effectData.m_absorbAmount : 0);
						Dictionary<AbilityTooltipSymbol, int> dictionary2 = dictionary;
						int value;
						if (targetActor != lastCenterSquareActor)
						{
							value = GetExplosionHealingToAllies();
						}
						else
						{
							value = 0;
						}
						dictionary2[AbilityTooltipSymbol.Healing] = value;
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
		return CanTargetActorInDecision(caster, currentBestActorTarget, CanTargetEnemies(), CanTargetAllies(), CanTargetSelf(), ValidateCheckPath.Ignore, true, true);
	}
}
