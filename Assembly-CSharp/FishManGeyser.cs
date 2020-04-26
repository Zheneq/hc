using System;
using System.Collections.Generic;
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
		if (ApplyKnockbackOnCast())
		{
			if (base.RunPriority != AbilityPriority.Combat_Knockback)
			{
				Debug.LogError("Authoring error on FishManGeyser-- ability's run priority is " + base.RunPriority.ToString() + ", but it does knockback on cast.");
			}
		}
		List<AbilityUtil_Targeter_MultipleShapes.MultiShapeData> list = new List<AbilityUtil_Targeter_MultipleShapes.MultiShapeData>();
		for (int i = 0; i < m_cachedShapeToDamage.Count; i++)
		{
			AbilityUtil_Targeter_MultipleShapes.MultiShapeData multiShapeData = new AbilityUtil_Targeter_MultipleShapes.MultiShapeData();
			multiShapeData.m_shape = m_cachedShapeToDamage[i].m_shape;
			multiShapeData.m_penetrateLoS = CastPenetratesLoS();
			multiShapeData.m_knockbackEnemies = ApplyKnockbackOnCast();
			multiShapeData.m_knockbackDistance = GetKnockbackDistOnCast();
			multiShapeData.m_knockbackType = GetKnockbackTypeOnCast();
			multiShapeData.m_affectAllies = CastCanAffectAllies();
			multiShapeData.m_affectSelf = CastCanAffectAllies();
			multiShapeData.m_affectEnemies = CastCanAffectEnemies();
			multiShapeData.m_subjectEnemyInShape = AbilityTooltipSubject.Primary;
			list.Add(multiShapeData);
		}
		AbilityUtil_Targeter_MultipleShapes abilityUtil_Targeter_MultipleShapes = new AbilityUtil_Targeter_MultipleShapes(this, list);
		if (GetHealOnCasterPerEnemyHit() > 0)
		{
			
			abilityUtil_Targeter_MultipleShapes.m_affectCasterDelegate = delegate(ActorData caster, List<ActorData> actorsSoFar)
				{
					for (int j = 0; j < actorsSoFar.Count; j++)
					{
						if (caster.GetTeam() != actorsSoFar[j].GetTeam())
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
						}
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				};
		}
		else
		{
			abilityUtil_Targeter_MultipleShapes.m_affectCasterDelegate = null;
		}
		base.Targeter = abilityUtil_Targeter_MultipleShapes;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectToEnemiesOnCast;
		if ((bool)m_abilityMod)
		{
			cachedEffectToEnemiesOnCast = m_abilityMod.m_effectToEnemiesOnCastMod.GetModifiedValue(m_effectToEnemiesOnCast);
		}
		else
		{
			cachedEffectToEnemiesOnCast = m_effectToEnemiesOnCast;
		}
		m_cachedEffectToEnemiesOnCast = cachedEffectToEnemiesOnCast;
		StandardEffectInfo cachedEffectToAlliesOnCast;
		if ((bool)m_abilityMod)
		{
			cachedEffectToAlliesOnCast = m_abilityMod.m_effectToAlliesOnCastMod.GetModifiedValue(m_effectToAlliesOnCast);
		}
		else
		{
			cachedEffectToAlliesOnCast = m_effectToAlliesOnCast;
		}
		m_cachedEffectToAlliesOnCast = cachedEffectToAlliesOnCast;
		StandardEffectInfo cachedEnemyEffectOnNextTurn;
		if ((bool)m_abilityMod)
		{
			cachedEnemyEffectOnNextTurn = m_abilityMod.m_enemyEffectOnNextTurnMod.GetModifiedValue(m_enemyEffectOnNextTurn);
		}
		else
		{
			cachedEnemyEffectOnNextTurn = m_enemyEffectOnNextTurn;
		}
		m_cachedEnemyEffectOnNextTurn = cachedEnemyEffectOnNextTurn;
		StandardEffectInfo cachedEelEffectOnEnemies;
		if ((bool)m_abilityMod)
		{
			cachedEelEffectOnEnemies = m_abilityMod.m_eelEffectOnEnemiesMod.GetModifiedValue(m_eelEffectOnEnemies);
		}
		else
		{
			cachedEelEffectOnEnemies = m_eelEffectOnEnemies;
		}
		m_cachedEelEffectOnEnemies = cachedEelEffectOnEnemies;
		StandardEffectInfo cachedEffectToEnemiesOnExplode;
		if ((bool)m_abilityMod)
		{
			cachedEffectToEnemiesOnExplode = m_abilityMod.m_effectToEnemiesOnExplodeMod.GetModifiedValue(m_effectToEnemiesOnExplode);
		}
		else
		{
			cachedEffectToEnemiesOnExplode = m_effectToEnemiesOnExplode;
		}
		m_cachedEffectToEnemiesOnExplode = cachedEffectToEnemiesOnExplode;
		StandardEffectInfo cachedEffectToAlliesOnExplode;
		if ((bool)m_abilityMod)
		{
			cachedEffectToAlliesOnExplode = m_abilityMod.m_effectToAlliesOnExplodeMod.GetModifiedValue(m_effectToAlliesOnExplode);
		}
		else
		{
			cachedEffectToAlliesOnExplode = m_effectToAlliesOnExplode;
		}
		m_cachedEffectToAlliesOnExplode = cachedEffectToAlliesOnExplode;
		m_cachedShapeToDamage.Clear();
		m_cachedShapeToDamage.Add(new ShapeToDamage(GetCastShape(), GetDamageToEnemiesOnCast()));
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_useAdditionalShapeOverride)
			{
				for (int i = 0; i < m_abilityMod.m_additionalShapeToDamageOverride.Count; i++)
				{
					m_cachedShapeToDamage.Add(new ShapeToDamage(m_abilityMod.m_additionalShapeToDamageOverride[i].m_shape, m_abilityMod.m_additionalShapeToDamageOverride[i].m_damage));
				}
				goto IL_029f;
			}
		}
		for (int j = 0; j < m_additionalShapeToDamage.Count; j++)
		{
			m_cachedShapeToDamage.Add(new ShapeToDamage(m_additionalShapeToDamage[j].m_shape, m_additionalShapeToDamage[j].m_damage));
		}
		goto IL_029f;
		IL_029f:
		m_cachedShapeToDamage.Sort();
	}

	public int GetDamageForShapeIndex(int shapeIndex)
	{
		if (m_cachedShapeToDamage != null)
		{
			if (shapeIndex < m_cachedShapeToDamage.Count)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return m_cachedShapeToDamage[shapeIndex].m_damage;
					}
				}
			}
		}
		return GetDamageToEnemiesOnCast();
	}

	public AbilityAreaShape GetCastShape()
	{
		return (!m_abilityMod) ? m_castShape : m_abilityMod.m_castShapeMod.GetModifiedValue(m_castShape);
	}

	public bool CastPenetratesLoS()
	{
		return (!m_abilityMod) ? m_castPenetratesLoS : m_abilityMod.m_castPenetratesLoSMod.GetModifiedValue(m_castPenetratesLoS);
	}

	public int GetDamageToEnemiesOnCast()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageToEnemiesOnCastMod.GetModifiedValue(m_damageToEnemiesOnCast);
		}
		else
		{
			result = m_damageToEnemiesOnCast;
		}
		return result;
	}

	public int GetHealingToAlliesOnCast()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healingToAlliesOnCastMod.GetModifiedValue(m_healingToAlliesOnCast);
		}
		else
		{
			result = m_healingToAlliesOnCast;
		}
		return result;
	}

	public int GetHealOnCasterPerEnemyHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healOnCasterPerEnemyHitMod.GetModifiedValue(m_healOnCasterPerEnemyHit);
		}
		else
		{
			result = m_healOnCasterPerEnemyHit;
		}
		return result;
	}

	public bool ApplyKnockbackOnCast()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_applyKnockbackOnCastMod.GetModifiedValue(m_applyKnockbackOnCast);
		}
		else
		{
			result = m_applyKnockbackOnCast;
		}
		return result;
	}

	public float GetKnockbackDistOnCast()
	{
		return (!m_abilityMod) ? m_knockbackDistOnCast : m_abilityMod.m_knockbackDistOnCastMod.GetModifiedValue(m_knockbackDistOnCast);
	}

	public KnockbackType GetKnockbackTypeOnCast()
	{
		KnockbackType result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackTypeOnCastMod.GetModifiedValue(m_knockbackTypeOnCast);
		}
		else
		{
			result = m_knockbackTypeOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetEffectToEnemiesOnCast()
	{
		StandardEffectInfo result;
		if (m_cachedEffectToEnemiesOnCast != null)
		{
			result = m_cachedEffectToEnemiesOnCast;
		}
		else
		{
			result = m_effectToEnemiesOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetEffectToAlliesOnCast()
	{
		StandardEffectInfo result;
		if (m_cachedEffectToAlliesOnCast != null)
		{
			result = m_cachedEffectToAlliesOnCast;
		}
		else
		{
			result = m_effectToAlliesOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyEffectOnNextTurn()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyEffectOnNextTurn != null)
		{
			result = m_cachedEnemyEffectOnNextTurn;
		}
		else
		{
			result = m_enemyEffectOnNextTurn;
		}
		return result;
	}

	public bool ApplyEelEffectOnEnemies()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_applyEelEffectOnEnemiesMod.GetModifiedValue(m_applyEelEffectOnEnemies);
		}
		else
		{
			result = m_applyEelEffectOnEnemies;
		}
		return result;
	}

	public int GetEelDamage()
	{
		return (!m_abilityMod) ? m_eelDamage : m_abilityMod.m_eelDamageMod.GetModifiedValue(m_eelDamage);
	}

	public StandardEffectInfo GetEelEffectOnEnemies()
	{
		return (m_cachedEelEffectOnEnemies == null) ? m_eelEffectOnEnemies : m_cachedEelEffectOnEnemies;
	}

	public float GetEelRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_eelRadiusMod.GetModifiedValue(m_eelRadius);
		}
		else
		{
			result = m_eelRadius;
		}
		return result;
	}

	public int GetTurnsTillFirstExplosion()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_turnsTillFirstExplosionMod.GetModifiedValue(m_turnsTillFirstExplosion);
		}
		else
		{
			result = m_turnsTillFirstExplosion;
		}
		return result;
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

	public AbilityAreaShape GetExplodeShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_explodeShapeMod.GetModifiedValue(m_explodeShape);
		}
		else
		{
			result = m_explodeShape;
		}
		return result;
	}

	public bool ExplodePenetratesLoS()
	{
		return (!m_abilityMod) ? m_explodePenetratesLoS : m_abilityMod.m_explodePenetratesLoSMod.GetModifiedValue(m_explodePenetratesLoS);
	}

	public int GetDamageToEnemiesOnExplode()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageToEnemiesOnExplodeMod.GetModifiedValue(m_damageToEnemiesOnExplode);
		}
		else
		{
			result = m_damageToEnemiesOnExplode;
		}
		return result;
	}

	public int GetHealingToAlliesOnExplode()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healingToAlliesOnExplodeMod.GetModifiedValue(m_healingToAlliesOnExplode);
		}
		else
		{
			result = m_healingToAlliesOnExplode;
		}
		return result;
	}

	public bool ApplyKnockbackOnExplode()
	{
		return (!m_abilityMod) ? m_applyKnockbackOnExplode : m_abilityMod.m_applyKnockbackOnExplodeMod.GetModifiedValue(m_applyKnockbackOnExplode);
	}

	public float GetKnockbackDistOnExplode()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackDistOnExplodeMod.GetModifiedValue(m_knockbackDistOnExplode);
		}
		else
		{
			result = m_knockbackDistOnExplode;
		}
		return result;
	}

	public StandardEffectInfo GetEffectToEnemiesOnExplode()
	{
		return (m_cachedEffectToEnemiesOnExplode == null) ? m_effectToEnemiesOnExplode : m_cachedEffectToEnemiesOnExplode;
	}

	public StandardEffectInfo GetEffectToAlliesOnExplode()
	{
		StandardEffectInfo result;
		if (m_cachedEffectToAlliesOnExplode != null)
		{
			result = m_cachedEffectToAlliesOnExplode;
		}
		else
		{
			result = m_effectToAlliesOnExplode;
		}
		return result;
	}

	private bool CastCanAffectEnemies()
	{
		int result;
		if (GetDamageToEnemiesOnCast() <= 0)
		{
			if (!GetEffectToEnemiesOnCast().m_applyEffect)
			{
				result = (ApplyKnockbackOnCast() ? 1 : 0);
				goto IL_0040;
			}
		}
		result = 1;
		goto IL_0040;
		IL_0040:
		return (byte)result != 0;
	}

	private bool CastCanAffectAllies()
	{
		int result;
		if (GetHealingToAlliesOnCast() <= 0)
		{
			result = (GetEffectToAlliesOnCast().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private bool ExplosionCanAffectEnemies()
	{
		int result;
		if (GetDamageToEnemiesOnExplode() <= 0 && !GetEffectToEnemiesOnExplode().m_applyEffect)
		{
			result = (ApplyKnockbackOnExplode() ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private bool ExplosionCanAffectAllies()
	{
		int result;
		if (GetHealingToAlliesOnExplode() <= 0)
		{
			result = (GetEffectToAlliesOnExplode().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManGeyser abilityMod_FishManGeyser = modAsBase as AbilityMod_FishManGeyser;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_FishManGeyser)
		{
			val = abilityMod_FishManGeyser.m_damageToEnemiesOnCastMod.GetModifiedValue(m_damageToEnemiesOnCast);
		}
		else
		{
			val = m_damageToEnemiesOnCast;
		}
		AddTokenInt(tokens, "DamageToEnemiesOnCast", empty, val);
		AddTokenInt(tokens, "HealingToAlliesOnCast", string.Empty, (!abilityMod_FishManGeyser) ? m_healingToAlliesOnCast : abilityMod_FishManGeyser.m_healingToAlliesOnCastMod.GetModifiedValue(m_healingToAlliesOnCast));
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_FishManGeyser)
		{
			val2 = abilityMod_FishManGeyser.m_healOnCasterPerEnemyHitMod.GetModifiedValue(m_healOnCasterPerEnemyHit);
		}
		else
		{
			val2 = m_healOnCasterPerEnemyHit;
		}
		AddTokenInt(tokens, "HealOnCasterPerEnemyHit", empty2, val2);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_FishManGeyser)
		{
			effectInfo = abilityMod_FishManGeyser.m_effectToEnemiesOnCastMod.GetModifiedValue(m_effectToEnemiesOnCast);
		}
		else
		{
			effectInfo = m_effectToEnemiesOnCast;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectToEnemiesOnCast", m_effectToEnemiesOnCast);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_FishManGeyser)
		{
			effectInfo2 = abilityMod_FishManGeyser.m_effectToAlliesOnCastMod.GetModifiedValue(m_effectToAlliesOnCast);
		}
		else
		{
			effectInfo2 = m_effectToAlliesOnCast;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectToAlliesOnCast", m_effectToAlliesOnCast);
		StandardEffectInfo effectInfo3;
		if ((bool)abilityMod_FishManGeyser)
		{
			effectInfo3 = abilityMod_FishManGeyser.m_enemyEffectOnNextTurnMod.GetModifiedValue(m_enemyEffectOnNextTurn);
		}
		else
		{
			effectInfo3 = m_enemyEffectOnNextTurn;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "EnemyEffectOnNextTurn", m_enemyEffectOnNextTurn);
		AddTokenInt(tokens, "EelDamage", string.Empty, (!abilityMod_FishManGeyser) ? m_eelDamage : abilityMod_FishManGeyser.m_eelDamageMod.GetModifiedValue(m_eelDamage));
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManGeyser) ? m_eelEffectOnEnemies : abilityMod_FishManGeyser.m_eelEffectOnEnemiesMod.GetModifiedValue(m_eelEffectOnEnemies), "EelEffectOnEnemies", m_eelEffectOnEnemies);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_FishManGeyser)
		{
			val3 = abilityMod_FishManGeyser.m_turnsTillFirstExplosionMod.GetModifiedValue(m_turnsTillFirstExplosion);
		}
		else
		{
			val3 = m_turnsTillFirstExplosion;
		}
		AddTokenInt(tokens, "TurnsTillFirstExplosion", empty3, val3);
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_FishManGeyser)
		{
			val4 = abilityMod_FishManGeyser.m_numExplosionsBeforeEndingMod.GetModifiedValue(m_numExplosionsBeforeEnding);
		}
		else
		{
			val4 = m_numExplosionsBeforeEnding;
		}
		AddTokenInt(tokens, "NumExplosionsBeforeEnding", empty4, val4);
		string empty5 = string.Empty;
		int val5;
		if ((bool)abilityMod_FishManGeyser)
		{
			val5 = abilityMod_FishManGeyser.m_damageToEnemiesOnExplodeMod.GetModifiedValue(m_damageToEnemiesOnExplode);
		}
		else
		{
			val5 = m_damageToEnemiesOnExplode;
		}
		AddTokenInt(tokens, "DamageToEnemiesOnExplode", empty5, val5);
		string empty6 = string.Empty;
		int val6;
		if ((bool)abilityMod_FishManGeyser)
		{
			val6 = abilityMod_FishManGeyser.m_healingToAlliesOnExplodeMod.GetModifiedValue(m_healingToAlliesOnExplode);
		}
		else
		{
			val6 = m_healingToAlliesOnExplode;
		}
		AddTokenInt(tokens, "HealingToAlliesOnExplode", empty6, val6);
		StandardEffectInfo effectInfo4;
		if ((bool)abilityMod_FishManGeyser)
		{
			effectInfo4 = abilityMod_FishManGeyser.m_effectToEnemiesOnExplodeMod.GetModifiedValue(m_effectToEnemiesOnExplode);
		}
		else
		{
			effectInfo4 = m_effectToEnemiesOnExplode;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo4, "EffectToEnemiesOnExplode", m_effectToEnemiesOnExplode);
		StandardEffectInfo effectInfo5;
		if ((bool)abilityMod_FishManGeyser)
		{
			effectInfo5 = abilityMod_FishManGeyser.m_effectToAlliesOnExplodeMod.GetModifiedValue(m_effectToAlliesOnExplode);
		}
		else
		{
			effectInfo5 = m_effectToAlliesOnExplode;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo5, "EffectToAlliesOnExplode", m_effectToAlliesOnExplode);
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
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		ActorData actorData = base.ActorData;
		if (tooltipSubjectTypes != null && actorData != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					if (base.Targeter is AbilityUtil_Targeter_MultipleShapes)
					{
						List<AbilityUtil_Targeter_MultipleShapes.HitActorContext> hitActorContext = (base.Targeter as AbilityUtil_Targeter_MultipleShapes).GetHitActorContext();
						using (List<AbilityUtil_Targeter_MultipleShapes.HitActorContext>.Enumerator enumerator = hitActorContext.GetEnumerator())
						{
							while (true)
							{
								if (!enumerator.MoveNext())
								{
									break;
								}
								AbilityUtil_Targeter_MultipleShapes.HitActorContext current = enumerator.Current;
								if (current.m_actor == targetActor && targetActor.GetTeam() != actorData.GetTeam())
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
											dictionary[AbilityTooltipSymbol.Damage] = GetDamageForShapeIndex(current.m_hitShapeIndex);
											goto end_IL_0088;
										}
									}
								}
							}
							end_IL_0088:;
						}
					}
					else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
					{
						dictionary[AbilityTooltipSymbol.Damage] = GetDamageToEnemiesOnCast();
					}
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
					{
						int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
						dictionary[AbilityTooltipSymbol.Healing] = visibleActorsCountByTooltipSubject * GetHealOnCasterPerEnemyHit();
					}
					return dictionary;
				}
			}
		}
		return null;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManGeyser))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_FishManGeyser);
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
}
