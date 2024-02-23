using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class FishManGeyser : Ability
{
	[Serializable]
	public class ShapeToDamage : ShapeToDataBase
	{
		public int m_damage;

		public ShapeToDamage(AbilityAreaShape shape, int damage)
		{
			m_shape = shape;
			m_damage = damage;
		}
	}

	[Header("-- Shape on Cast, list from smaller to larger. Cast Shape is smallest")]
	public AbilityAreaShape m_castShape = AbilityAreaShape.Five_x_Five;
	public List<ShapeToDamage> m_additionalShapeToDamage = new List<ShapeToDamage>();
	[Header("-- Initial Cast")]
	public bool m_castPenetratesLoS;
	public int m_damageToEnemiesOnCast;
	public int m_healingToAlliesOnCast = 25;
	public int m_healOnCasterPerEnemyHit;
	public bool m_applyKnockbackOnCast;
	public StandardEffectInfo m_effectToEnemiesOnCast;
	public StandardEffectInfo m_effectToAlliesOnCast;
	[Header("-- Knockback on Cast")]
	public float m_knockbackDistOnCast;
	public KnockbackType m_knockbackTypeOnCast = KnockbackType.AwayFromSource;
	[Header("-- Effect on Enemies on start of Next Turn")]
	public StandardEffectInfo m_enemyEffectOnNextTurn;
	[Header("-- Eel effect on enemies hit")]
	public bool m_applyEelEffectOnEnemies;
	public int m_eelDamage;
	public StandardEffectInfo m_eelEffectOnEnemies;
	public float m_eelRadius = 5.5f;
	[Header("-- Explosion Timing (may be depricated if not needed)")]
	public int m_turnsTillFirstExplosion = 1;
	public int m_numExplosionsBeforeEnding = 1;
	[Header("-- Effect Explode (may be depricated if not needed)")]
	public AbilityAreaShape m_explodeShape = AbilityAreaShape.Seven_x_Seven_NoCorners;
	public bool m_explodePenetratesLoS;
	public int m_damageToEnemiesOnExplode = 30;
	public int m_healingToAlliesOnExplode;
	public StandardEffectInfo m_effectToEnemiesOnExplode;
	public StandardEffectInfo m_effectToAlliesOnExplode;
	[Header("-- Knockback on Effect Explosion (may be depricated if not needed)")]
	public bool m_applyKnockbackOnExplode = true;
	public float m_knockbackDistOnExplode = 4f;
	public KnockbackType m_knockbackTypeOnExplode = KnockbackType.AwayFromSource;
	[Header("-- Sequences")]
	public GameObject m_castSequence;
	public GameObject m_hittingEnemySequence;
	public GameObject m_hittingAllySequence;
	public GameObject m_groundEffectPersistentSequence;
	public GameObject m_groundEffectExplodeSequence;

	private AbilityMod_FishManGeyser m_abilityMod;
	private StandardEffectInfo m_cachedEffectToEnemiesOnCast;
	private StandardEffectInfo m_cachedEffectToAlliesOnCast;
	private StandardEffectInfo m_cachedEnemyEffectOnNextTurn;
	private StandardEffectInfo m_cachedEelEffectOnEnemies;
	private StandardEffectInfo m_cachedEffectToEnemiesOnExplode;
	private StandardEffectInfo m_cachedEffectToAlliesOnExplode;
	private List<ShapeToDamage> m_cachedShapeToDamage = new List<ShapeToDamage>();

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (ApplyKnockbackOnCast() && RunPriority != AbilityPriority.Combat_Knockback)
		{
			Debug.LogError(new StringBuilder().Append("Authoring error on FishManGeyser-- ability's run priority is ").Append(RunPriority).Append(", but it does knockback on cast.").ToString());
		}
		List<AbilityUtil_Targeter_MultipleShapes.MultiShapeData> list = new List<AbilityUtil_Targeter_MultipleShapes.MultiShapeData>();
		foreach (ShapeToDamage shapeToDamage in m_cachedShapeToDamage)
		{
			list.Add(new AbilityUtil_Targeter_MultipleShapes.MultiShapeData
			{
				m_shape = shapeToDamage.m_shape,
				m_penetrateLoS = CastPenetratesLoS(),
				m_knockbackEnemies = ApplyKnockbackOnCast(),
				m_knockbackDistance = GetKnockbackDistOnCast(),
				m_knockbackType = GetKnockbackTypeOnCast(),
				m_affectAllies = CastCanAffectAllies(),
				m_affectSelf = CastCanAffectAllies(),
				m_affectEnemies = CastCanAffectEnemies(),
				m_subjectEnemyInShape = AbilityTooltipSubject.Primary
			});
		}
		AbilityUtil_Targeter_MultipleShapes targeter = new AbilityUtil_Targeter_MultipleShapes(this, list);
		if (GetHealOnCasterPerEnemyHit() > 0)
		{
			targeter.m_affectCasterDelegate = delegate(ActorData caster, List<ActorData> actorsSoFar)
			{
				foreach (ActorData actor in actorsSoFar)
				{
					if (caster.GetTeam() != actor.GetTeam())
					{
						return true;
					}
				}
				return false;
			};
		}
		else
		{
			targeter.m_affectCasterDelegate = null;
		}
		Targeter = targeter;
	}

	private void SetCachedFields()
	{
		m_cachedEffectToEnemiesOnCast = m_abilityMod != null
			? m_abilityMod.m_effectToEnemiesOnCastMod.GetModifiedValue(m_effectToEnemiesOnCast)
			: m_effectToEnemiesOnCast;
		m_cachedEffectToAlliesOnCast = m_abilityMod != null
			? m_abilityMod.m_effectToAlliesOnCastMod.GetModifiedValue(m_effectToAlliesOnCast)
			: m_effectToAlliesOnCast;
		m_cachedEnemyEffectOnNextTurn = m_abilityMod != null
			? m_abilityMod.m_enemyEffectOnNextTurnMod.GetModifiedValue(m_enemyEffectOnNextTurn)
			: m_enemyEffectOnNextTurn;
		m_cachedEelEffectOnEnemies = m_abilityMod != null
			? m_abilityMod.m_eelEffectOnEnemiesMod.GetModifiedValue(m_eelEffectOnEnemies)
			: m_eelEffectOnEnemies;
		m_cachedEffectToEnemiesOnExplode = m_abilityMod != null
			? m_abilityMod.m_effectToEnemiesOnExplodeMod.GetModifiedValue(m_effectToEnemiesOnExplode)
			: m_effectToEnemiesOnExplode;
		m_cachedEffectToAlliesOnExplode = m_abilityMod != null
			? m_abilityMod.m_effectToAlliesOnExplodeMod.GetModifiedValue(m_effectToAlliesOnExplode)
			: m_effectToAlliesOnExplode;
		m_cachedShapeToDamage.Clear();
		m_cachedShapeToDamage.Add(new ShapeToDamage(GetCastShape(), GetDamageToEnemiesOnCast()));
		if (m_abilityMod != null && m_abilityMod.m_useAdditionalShapeOverride)
		{
			foreach (ShapeToDamage shapeToDamage in m_abilityMod.m_additionalShapeToDamageOverride)
			{
				m_cachedShapeToDamage.Add(new ShapeToDamage(shapeToDamage.m_shape, shapeToDamage.m_damage));
			}
		}
		else
		{
			foreach (ShapeToDamage shapeToDamage in m_additionalShapeToDamage)
			{
				m_cachedShapeToDamage.Add(new ShapeToDamage(shapeToDamage.m_shape, shapeToDamage.m_damage));
			}
		}
		m_cachedShapeToDamage.Sort();
	}

	public int GetDamageForShapeIndex(int shapeIndex)
	{
		if (m_cachedShapeToDamage != null && shapeIndex < m_cachedShapeToDamage.Count)
		{
			return m_cachedShapeToDamage[shapeIndex].m_damage;
		}
		return GetDamageToEnemiesOnCast();
	}

	public AbilityAreaShape GetCastShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_castShapeMod.GetModifiedValue(m_castShape)
			: m_castShape;
	}

	public bool CastPenetratesLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_castPenetratesLoSMod.GetModifiedValue(m_castPenetratesLoS)
			: m_castPenetratesLoS;
	}

	public int GetDamageToEnemiesOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageToEnemiesOnCastMod.GetModifiedValue(m_damageToEnemiesOnCast)
			: m_damageToEnemiesOnCast;
	}

	public int GetHealingToAlliesOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healingToAlliesOnCastMod.GetModifiedValue(m_healingToAlliesOnCast)
			: m_healingToAlliesOnCast;
	}

	public int GetHealOnCasterPerEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healOnCasterPerEnemyHitMod.GetModifiedValue(m_healOnCasterPerEnemyHit)
			: m_healOnCasterPerEnemyHit;
	}

	public bool ApplyKnockbackOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_applyKnockbackOnCastMod.GetModifiedValue(m_applyKnockbackOnCast)
			: m_applyKnockbackOnCast;
	}

	public float GetKnockbackDistOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistOnCastMod.GetModifiedValue(m_knockbackDistOnCast)
			: m_knockbackDistOnCast;
	}

	public KnockbackType GetKnockbackTypeOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackTypeOnCastMod.GetModifiedValue(m_knockbackTypeOnCast)
			: m_knockbackTypeOnCast;
	}

	public StandardEffectInfo GetEffectToEnemiesOnCast()
	{
		return m_cachedEffectToEnemiesOnCast ?? m_effectToEnemiesOnCast;
	}

	public StandardEffectInfo GetEffectToAlliesOnCast()
	{
		return m_cachedEffectToAlliesOnCast ?? m_effectToAlliesOnCast;
	}

	public StandardEffectInfo GetEnemyEffectOnNextTurn()
	{
		return m_cachedEnemyEffectOnNextTurn ?? m_enemyEffectOnNextTurn;
	}

	public bool ApplyEelEffectOnEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_applyEelEffectOnEnemiesMod.GetModifiedValue(m_applyEelEffectOnEnemies)
			: m_applyEelEffectOnEnemies;
	}

	public int GetEelDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_eelDamageMod.GetModifiedValue(m_eelDamage)
			: m_eelDamage;
	}

	public StandardEffectInfo GetEelEffectOnEnemies()
	{
		return m_cachedEelEffectOnEnemies ?? m_eelEffectOnEnemies;
	}

	public float GetEelRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_eelRadiusMod.GetModifiedValue(m_eelRadius)
			: m_eelRadius;
	}

	public int GetTurnsTillFirstExplosion()
	{
		return m_abilityMod != null
			? m_abilityMod.m_turnsTillFirstExplosionMod.GetModifiedValue(m_turnsTillFirstExplosion)
			: m_turnsTillFirstExplosion;
	}

	public int GetNumExplosionsBeforeEnding()
	{
		return m_abilityMod != null
			? m_abilityMod.m_numExplosionsBeforeEndingMod.GetModifiedValue(m_numExplosionsBeforeEnding)
			: m_numExplosionsBeforeEnding;
	}

	public AbilityAreaShape GetExplodeShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explodeShapeMod.GetModifiedValue(m_explodeShape)
			: m_explodeShape;
	}

	public bool ExplodePenetratesLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explodePenetratesLoSMod.GetModifiedValue(m_explodePenetratesLoS)
			: m_explodePenetratesLoS;
	}

	public int GetDamageToEnemiesOnExplode()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageToEnemiesOnExplodeMod.GetModifiedValue(m_damageToEnemiesOnExplode)
			: m_damageToEnemiesOnExplode;
	}

	public int GetHealingToAlliesOnExplode()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healingToAlliesOnExplodeMod.GetModifiedValue(m_healingToAlliesOnExplode)
			: m_healingToAlliesOnExplode;
	}

	public bool ApplyKnockbackOnExplode()
	{
		return m_abilityMod != null
			? m_abilityMod.m_applyKnockbackOnExplodeMod.GetModifiedValue(m_applyKnockbackOnExplode)
			: m_applyKnockbackOnExplode;
	}

	public float GetKnockbackDistOnExplode()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistOnExplodeMod.GetModifiedValue(m_knockbackDistOnExplode)
			: m_knockbackDistOnExplode;
	}

	public StandardEffectInfo GetEffectToEnemiesOnExplode()
	{
		return m_cachedEffectToEnemiesOnExplode ?? m_effectToEnemiesOnExplode;
	}

	public StandardEffectInfo GetEffectToAlliesOnExplode()
	{
		return m_cachedEffectToAlliesOnExplode ?? m_effectToAlliesOnExplode;
	}

	private bool CastCanAffectEnemies()
	{
		return GetDamageToEnemiesOnCast() > 0
		       || GetEffectToEnemiesOnCast().m_applyEffect
		       || ApplyKnockbackOnCast();
	}

	private bool CastCanAffectAllies()
	{
		return GetHealingToAlliesOnCast() > 0
		       || GetEffectToAlliesOnCast().m_applyEffect;
	}

	private bool ExplosionCanAffectEnemies()
	{
		return GetDamageToEnemiesOnExplode() > 0
		       || GetEffectToEnemiesOnExplode().m_applyEffect
		       || ApplyKnockbackOnExplode();
	}

	private bool ExplosionCanAffectAllies()
	{
		return GetHealingToAlliesOnExplode() > 0
		       || GetEffectToAlliesOnExplode().m_applyEffect;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManGeyser abilityMod_FishManGeyser = modAsBase as AbilityMod_FishManGeyser;
		AddTokenInt(tokens, "DamageToEnemiesOnCast", string.Empty, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_damageToEnemiesOnCastMod.GetModifiedValue(m_damageToEnemiesOnCast)
			: m_damageToEnemiesOnCast);
		AddTokenInt(tokens, "HealingToAlliesOnCast", string.Empty, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_healingToAlliesOnCastMod.GetModifiedValue(m_healingToAlliesOnCast)
			: m_healingToAlliesOnCast);
		AddTokenInt(tokens, "HealOnCasterPerEnemyHit", string.Empty, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_healOnCasterPerEnemyHitMod.GetModifiedValue(m_healOnCasterPerEnemyHit)
			: m_healOnCasterPerEnemyHit);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_effectToEnemiesOnCastMod.GetModifiedValue(m_effectToEnemiesOnCast)
			: m_effectToEnemiesOnCast, "EffectToEnemiesOnCast", m_effectToEnemiesOnCast);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_effectToAlliesOnCastMod.GetModifiedValue(m_effectToAlliesOnCast)
			: m_effectToAlliesOnCast, "EffectToAlliesOnCast", m_effectToAlliesOnCast);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_enemyEffectOnNextTurnMod.GetModifiedValue(m_enemyEffectOnNextTurn)
			: m_enemyEffectOnNextTurn, "EnemyEffectOnNextTurn", m_enemyEffectOnNextTurn);
		AddTokenInt(tokens, "EelDamage", string.Empty, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_eelDamageMod.GetModifiedValue(m_eelDamage)
			: m_eelDamage);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_eelEffectOnEnemiesMod.GetModifiedValue(m_eelEffectOnEnemies)
			: m_eelEffectOnEnemies, "EelEffectOnEnemies", m_eelEffectOnEnemies);
		AddTokenInt(tokens, "TurnsTillFirstExplosion", string.Empty, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_turnsTillFirstExplosionMod.GetModifiedValue(m_turnsTillFirstExplosion)
			: m_turnsTillFirstExplosion);
		AddTokenInt(tokens, "NumExplosionsBeforeEnding", string.Empty, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_numExplosionsBeforeEndingMod.GetModifiedValue(m_numExplosionsBeforeEnding)
			: m_numExplosionsBeforeEnding);
		AddTokenInt(tokens, "DamageToEnemiesOnExplode", string.Empty, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_damageToEnemiesOnExplodeMod.GetModifiedValue(m_damageToEnemiesOnExplode)
			: m_damageToEnemiesOnExplode);
		AddTokenInt(tokens, "HealingToAlliesOnExplode", string.Empty, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_healingToAlliesOnExplodeMod.GetModifiedValue(m_healingToAlliesOnExplode)
			: m_healingToAlliesOnExplode);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_effectToEnemiesOnExplodeMod.GetModifiedValue(m_effectToEnemiesOnExplode)
			: m_effectToEnemiesOnExplode, "EffectToEnemiesOnExplode", m_effectToEnemiesOnExplode);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManGeyser != null
			? abilityMod_FishManGeyser.m_effectToAlliesOnExplodeMod.GetModifiedValue(m_effectToAlliesOnExplode)
			: m_effectToAlliesOnExplode, "EffectToAlliesOnExplode", m_effectToAlliesOnExplode);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageToEnemiesOnCast());
		GetEffectToEnemiesOnCast().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealingToAlliesOnCast());
		GetEffectToAlliesOnCast().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealOnCasterPerEnemyHit());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null || ActorData == null)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (Targeter is AbilityUtil_Targeter_MultipleShapes targeter)
		{
			foreach (AbilityUtil_Targeter_MultipleShapes.HitActorContext context in targeter.GetHitActorContext())
			{
				if (context.m_actor == targetActor && targetActor.GetTeam() != ActorData.GetTeam())
				{
					dictionary[AbilityTooltipSymbol.Damage] = GetDamageForShapeIndex(context.m_hitShapeIndex);
					break;
				}
			}
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			dictionary[AbilityTooltipSymbol.Damage] = GetDamageToEnemiesOnCast();
		}
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			int enemyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			dictionary[AbilityTooltipSymbol.Healing] = enemyNum * GetHealOnCasterPerEnemyHit();
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_FishManGeyser))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		
		m_abilityMod = abilityMod as AbilityMod_FishManGeyser;
		Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
