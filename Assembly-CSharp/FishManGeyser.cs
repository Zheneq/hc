using System;
using System.Collections.Generic;
using UnityEngine;

public class FishManGeyser : Ability
{
	[Header("-- Shape on Cast, list from smaller to larger. Cast Shape is smallest")]
	public AbilityAreaShape m_castShape = AbilityAreaShape.Five_x_Five;

	public List<FishManGeyser.ShapeToDamage> m_additionalShapeToDamage = new List<FishManGeyser.ShapeToDamage>();

	[Header("-- Initial Cast")]
	public bool m_castPenetratesLoS;

	public int m_damageToEnemiesOnCast;

	public int m_healingToAlliesOnCast = 0x19;

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

	public int m_damageToEnemiesOnExplode = 0x1E;

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

	private List<FishManGeyser.ShapeToDamage> m_cachedShapeToDamage = new List<FishManGeyser.ShapeToDamage>();

	private void Start()
	{
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		if (this.ApplyKnockbackOnCast())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.Setup()).MethodHandle;
			}
			if (base.RunPriority != AbilityPriority.Combat_Knockback)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				Debug.LogError("Authoring error on FishManGeyser-- ability's run priority is " + base.RunPriority.ToString() + ", but it does knockback on cast.");
			}
		}
		List<AbilityUtil_Targeter_MultipleShapes.MultiShapeData> list = new List<AbilityUtil_Targeter_MultipleShapes.MultiShapeData>();
		for (int i = 0; i < this.m_cachedShapeToDamage.Count; i++)
		{
			list.Add(new AbilityUtil_Targeter_MultipleShapes.MultiShapeData
			{
				m_shape = this.m_cachedShapeToDamage[i].m_shape,
				m_penetrateLoS = this.CastPenetratesLoS(),
				m_knockbackEnemies = this.ApplyKnockbackOnCast(),
				m_knockbackDistance = this.GetKnockbackDistOnCast(),
				m_knockbackType = this.GetKnockbackTypeOnCast(),
				m_affectAllies = this.CastCanAffectAllies(),
				m_affectSelf = this.CastCanAffectAllies(),
				m_affectEnemies = this.CastCanAffectEnemies(),
				m_subjectEnemyInShape = AbilityTooltipSubject.Primary
			});
		}
		AbilityUtil_Targeter_MultipleShapes abilityUtil_Targeter_MultipleShapes = new AbilityUtil_Targeter_MultipleShapes(this, list);
		if (this.GetHealOnCasterPerEnemyHit() > 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			AbilityUtil_Targeter_MultipleShapes abilityUtil_Targeter_MultipleShapes2 = abilityUtil_Targeter_MultipleShapes;
			if (FishManGeyser.<>f__am$cache0 == null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				FishManGeyser.<>f__am$cache0 = delegate(ActorData caster, List<ActorData> actorsSoFar)
				{
					for (int j = 0; j < actorsSoFar.Count; j++)
					{
						if (caster.\u000E() != actorsSoFar[j].\u000E())
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!true)
							{
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(FishManGeyser.<Setup>m__0(ActorData, List<ActorData>)).MethodHandle;
							}
							return true;
						}
					}
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					return false;
				};
			}
			abilityUtil_Targeter_MultipleShapes2.m_affectCasterDelegate = FishManGeyser.<>f__am$cache0;
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
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.SetCachedFields()).MethodHandle;
			}
			cachedEffectToEnemiesOnCast = this.m_abilityMod.m_effectToEnemiesOnCastMod.GetModifiedValue(this.m_effectToEnemiesOnCast);
		}
		else
		{
			cachedEffectToEnemiesOnCast = this.m_effectToEnemiesOnCast;
		}
		this.m_cachedEffectToEnemiesOnCast = cachedEffectToEnemiesOnCast;
		StandardEffectInfo cachedEffectToAlliesOnCast;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEffectToAlliesOnCast = this.m_abilityMod.m_effectToAlliesOnCastMod.GetModifiedValue(this.m_effectToAlliesOnCast);
		}
		else
		{
			cachedEffectToAlliesOnCast = this.m_effectToAlliesOnCast;
		}
		this.m_cachedEffectToAlliesOnCast = cachedEffectToAlliesOnCast;
		StandardEffectInfo cachedEnemyEffectOnNextTurn;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEnemyEffectOnNextTurn = this.m_abilityMod.m_enemyEffectOnNextTurnMod.GetModifiedValue(this.m_enemyEffectOnNextTurn);
		}
		else
		{
			cachedEnemyEffectOnNextTurn = this.m_enemyEffectOnNextTurn;
		}
		this.m_cachedEnemyEffectOnNextTurn = cachedEnemyEffectOnNextTurn;
		StandardEffectInfo cachedEelEffectOnEnemies;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEelEffectOnEnemies = this.m_abilityMod.m_eelEffectOnEnemiesMod.GetModifiedValue(this.m_eelEffectOnEnemies);
		}
		else
		{
			cachedEelEffectOnEnemies = this.m_eelEffectOnEnemies;
		}
		this.m_cachedEelEffectOnEnemies = cachedEelEffectOnEnemies;
		StandardEffectInfo cachedEffectToEnemiesOnExplode;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEffectToEnemiesOnExplode = this.m_abilityMod.m_effectToEnemiesOnExplodeMod.GetModifiedValue(this.m_effectToEnemiesOnExplode);
		}
		else
		{
			cachedEffectToEnemiesOnExplode = this.m_effectToEnemiesOnExplode;
		}
		this.m_cachedEffectToEnemiesOnExplode = cachedEffectToEnemiesOnExplode;
		StandardEffectInfo cachedEffectToAlliesOnExplode;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEffectToAlliesOnExplode = this.m_abilityMod.m_effectToAlliesOnExplodeMod.GetModifiedValue(this.m_effectToAlliesOnExplode);
		}
		else
		{
			cachedEffectToAlliesOnExplode = this.m_effectToAlliesOnExplode;
		}
		this.m_cachedEffectToAlliesOnExplode = cachedEffectToAlliesOnExplode;
		this.m_cachedShapeToDamage.Clear();
		this.m_cachedShapeToDamage.Add(new FishManGeyser.ShapeToDamage(this.GetCastShape(), this.GetDamageToEnemiesOnCast()));
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_abilityMod.m_useAdditionalShapeOverride)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int i = 0; i < this.m_abilityMod.m_additionalShapeToDamageOverride.Count; i++)
				{
					this.m_cachedShapeToDamage.Add(new FishManGeyser.ShapeToDamage(this.m_abilityMod.m_additionalShapeToDamageOverride[i].m_shape, this.m_abilityMod.m_additionalShapeToDamageOverride[i].m_damage));
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				goto IL_29F;
			}
		}
		for (int j = 0; j < this.m_additionalShapeToDamage.Count; j++)
		{
			this.m_cachedShapeToDamage.Add(new FishManGeyser.ShapeToDamage(this.m_additionalShapeToDamage[j].m_shape, this.m_additionalShapeToDamage[j].m_damage));
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		IL_29F:
		this.m_cachedShapeToDamage.Sort();
	}

	public int GetDamageForShapeIndex(int shapeIndex)
	{
		if (this.m_cachedShapeToDamage != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetDamageForShapeIndex(int)).MethodHandle;
			}
			if (shapeIndex < this.m_cachedShapeToDamage.Count)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.m_cachedShapeToDamage[shapeIndex].m_damage;
			}
		}
		return this.GetDamageToEnemiesOnCast();
	}

	public AbilityAreaShape GetCastShape()
	{
		return (!this.m_abilityMod) ? this.m_castShape : this.m_abilityMod.m_castShapeMod.GetModifiedValue(this.m_castShape);
	}

	public bool CastPenetratesLoS()
	{
		return (!this.m_abilityMod) ? this.m_castPenetratesLoS : this.m_abilityMod.m_castPenetratesLoSMod.GetModifiedValue(this.m_castPenetratesLoS);
	}

	public int GetDamageToEnemiesOnCast()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetDamageToEnemiesOnCast()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageToEnemiesOnCastMod.GetModifiedValue(this.m_damageToEnemiesOnCast);
		}
		else
		{
			result = this.m_damageToEnemiesOnCast;
		}
		return result;
	}

	public int GetHealingToAlliesOnCast()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetHealingToAlliesOnCast()).MethodHandle;
			}
			result = this.m_abilityMod.m_healingToAlliesOnCastMod.GetModifiedValue(this.m_healingToAlliesOnCast);
		}
		else
		{
			result = this.m_healingToAlliesOnCast;
		}
		return result;
	}

	public int GetHealOnCasterPerEnemyHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetHealOnCasterPerEnemyHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_healOnCasterPerEnemyHitMod.GetModifiedValue(this.m_healOnCasterPerEnemyHit);
		}
		else
		{
			result = this.m_healOnCasterPerEnemyHit;
		}
		return result;
	}

	public bool ApplyKnockbackOnCast()
	{
		bool result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.ApplyKnockbackOnCast()).MethodHandle;
			}
			result = this.m_abilityMod.m_applyKnockbackOnCastMod.GetModifiedValue(this.m_applyKnockbackOnCast);
		}
		else
		{
			result = this.m_applyKnockbackOnCast;
		}
		return result;
	}

	public float GetKnockbackDistOnCast()
	{
		return (!this.m_abilityMod) ? this.m_knockbackDistOnCast : this.m_abilityMod.m_knockbackDistOnCastMod.GetModifiedValue(this.m_knockbackDistOnCast);
	}

	public KnockbackType GetKnockbackTypeOnCast()
	{
		KnockbackType result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetKnockbackTypeOnCast()).MethodHandle;
			}
			result = this.m_abilityMod.m_knockbackTypeOnCastMod.GetModifiedValue(this.m_knockbackTypeOnCast);
		}
		else
		{
			result = this.m_knockbackTypeOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetEffectToEnemiesOnCast()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectToEnemiesOnCast != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetEffectToEnemiesOnCast()).MethodHandle;
			}
			result = this.m_cachedEffectToEnemiesOnCast;
		}
		else
		{
			result = this.m_effectToEnemiesOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetEffectToAlliesOnCast()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectToAlliesOnCast != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetEffectToAlliesOnCast()).MethodHandle;
			}
			result = this.m_cachedEffectToAlliesOnCast;
		}
		else
		{
			result = this.m_effectToAlliesOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyEffectOnNextTurn()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyEffectOnNextTurn != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetEnemyEffectOnNextTurn()).MethodHandle;
			}
			result = this.m_cachedEnemyEffectOnNextTurn;
		}
		else
		{
			result = this.m_enemyEffectOnNextTurn;
		}
		return result;
	}

	public bool ApplyEelEffectOnEnemies()
	{
		bool result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.ApplyEelEffectOnEnemies()).MethodHandle;
			}
			result = this.m_abilityMod.m_applyEelEffectOnEnemiesMod.GetModifiedValue(this.m_applyEelEffectOnEnemies);
		}
		else
		{
			result = this.m_applyEelEffectOnEnemies;
		}
		return result;
	}

	public int GetEelDamage()
	{
		return (!this.m_abilityMod) ? this.m_eelDamage : this.m_abilityMod.m_eelDamageMod.GetModifiedValue(this.m_eelDamage);
	}

	public StandardEffectInfo GetEelEffectOnEnemies()
	{
		return (this.m_cachedEelEffectOnEnemies == null) ? this.m_eelEffectOnEnemies : this.m_cachedEelEffectOnEnemies;
	}

	public float GetEelRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetEelRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_eelRadiusMod.GetModifiedValue(this.m_eelRadius);
		}
		else
		{
			result = this.m_eelRadius;
		}
		return result;
	}

	public int GetTurnsTillFirstExplosion()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetTurnsTillFirstExplosion()).MethodHandle;
			}
			result = this.m_abilityMod.m_turnsTillFirstExplosionMod.GetModifiedValue(this.m_turnsTillFirstExplosion);
		}
		else
		{
			result = this.m_turnsTillFirstExplosion;
		}
		return result;
	}

	public int GetNumExplosionsBeforeEnding()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetNumExplosionsBeforeEnding()).MethodHandle;
			}
			result = this.m_abilityMod.m_numExplosionsBeforeEndingMod.GetModifiedValue(this.m_numExplosionsBeforeEnding);
		}
		else
		{
			result = this.m_numExplosionsBeforeEnding;
		}
		return result;
	}

	public AbilityAreaShape GetExplodeShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetExplodeShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_explodeShapeMod.GetModifiedValue(this.m_explodeShape);
		}
		else
		{
			result = this.m_explodeShape;
		}
		return result;
	}

	public bool ExplodePenetratesLoS()
	{
		return (!this.m_abilityMod) ? this.m_explodePenetratesLoS : this.m_abilityMod.m_explodePenetratesLoSMod.GetModifiedValue(this.m_explodePenetratesLoS);
	}

	public int GetDamageToEnemiesOnExplode()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetDamageToEnemiesOnExplode()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageToEnemiesOnExplodeMod.GetModifiedValue(this.m_damageToEnemiesOnExplode);
		}
		else
		{
			result = this.m_damageToEnemiesOnExplode;
		}
		return result;
	}

	public int GetHealingToAlliesOnExplode()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetHealingToAlliesOnExplode()).MethodHandle;
			}
			result = this.m_abilityMod.m_healingToAlliesOnExplodeMod.GetModifiedValue(this.m_healingToAlliesOnExplode);
		}
		else
		{
			result = this.m_healingToAlliesOnExplode;
		}
		return result;
	}

	public bool ApplyKnockbackOnExplode()
	{
		return (!this.m_abilityMod) ? this.m_applyKnockbackOnExplode : this.m_abilityMod.m_applyKnockbackOnExplodeMod.GetModifiedValue(this.m_applyKnockbackOnExplode);
	}

	public float GetKnockbackDistOnExplode()
	{
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetKnockbackDistOnExplode()).MethodHandle;
			}
			result = this.m_abilityMod.m_knockbackDistOnExplodeMod.GetModifiedValue(this.m_knockbackDistOnExplode);
		}
		else
		{
			result = this.m_knockbackDistOnExplode;
		}
		return result;
	}

	public StandardEffectInfo GetEffectToEnemiesOnExplode()
	{
		return (this.m_cachedEffectToEnemiesOnExplode == null) ? this.m_effectToEnemiesOnExplode : this.m_cachedEffectToEnemiesOnExplode;
	}

	public StandardEffectInfo GetEffectToAlliesOnExplode()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectToAlliesOnExplode != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetEffectToAlliesOnExplode()).MethodHandle;
			}
			result = this.m_cachedEffectToAlliesOnExplode;
		}
		else
		{
			result = this.m_effectToAlliesOnExplode;
		}
		return result;
	}

	private bool CastCanAffectEnemies()
	{
		if (this.GetDamageToEnemiesOnCast() <= 0)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.CastCanAffectEnemies()).MethodHandle;
			}
			if (!this.GetEffectToEnemiesOnCast().m_applyEffect)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.ApplyKnockbackOnCast();
			}
		}
		return true;
	}

	private bool CastCanAffectAllies()
	{
		bool result;
		if (this.GetHealingToAlliesOnCast() <= 0)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.CastCanAffectAllies()).MethodHandle;
			}
			result = this.GetEffectToAlliesOnCast().m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool ExplosionCanAffectEnemies()
	{
		bool result;
		if (this.GetDamageToEnemiesOnExplode() <= 0 && !this.GetEffectToEnemiesOnExplode().m_applyEffect)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.ExplosionCanAffectEnemies()).MethodHandle;
			}
			result = this.ApplyKnockbackOnExplode();
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool ExplosionCanAffectAllies()
	{
		bool result;
		if (this.GetHealingToAlliesOnExplode() <= 0)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.ExplosionCanAffectAllies()).MethodHandle;
			}
			result = this.GetEffectToAlliesOnExplode().m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManGeyser abilityMod_FishManGeyser = modAsBase as AbilityMod_FishManGeyser;
		string name = "DamageToEnemiesOnCast";
		string empty = string.Empty;
		int val;
		if (abilityMod_FishManGeyser)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_FishManGeyser.m_damageToEnemiesOnCastMod.GetModifiedValue(this.m_damageToEnemiesOnCast);
		}
		else
		{
			val = this.m_damageToEnemiesOnCast;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		base.AddTokenInt(tokens, "HealingToAlliesOnCast", string.Empty, (!abilityMod_FishManGeyser) ? this.m_healingToAlliesOnCast : abilityMod_FishManGeyser.m_healingToAlliesOnCastMod.GetModifiedValue(this.m_healingToAlliesOnCast), false);
		string name2 = "HealOnCasterPerEnemyHit";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_FishManGeyser)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_FishManGeyser.m_healOnCasterPerEnemyHitMod.GetModifiedValue(this.m_healOnCasterPerEnemyHit);
		}
		else
		{
			val2 = this.m_healOnCasterPerEnemyHit;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_FishManGeyser)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo = abilityMod_FishManGeyser.m_effectToEnemiesOnCastMod.GetModifiedValue(this.m_effectToEnemiesOnCast);
		}
		else
		{
			effectInfo = this.m_effectToEnemiesOnCast;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectToEnemiesOnCast", this.m_effectToEnemiesOnCast, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_FishManGeyser)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo2 = abilityMod_FishManGeyser.m_effectToAlliesOnCastMod.GetModifiedValue(this.m_effectToAlliesOnCast);
		}
		else
		{
			effectInfo2 = this.m_effectToAlliesOnCast;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectToAlliesOnCast", this.m_effectToAlliesOnCast, true);
		StandardEffectInfo effectInfo3;
		if (abilityMod_FishManGeyser)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo3 = abilityMod_FishManGeyser.m_enemyEffectOnNextTurnMod.GetModifiedValue(this.m_enemyEffectOnNextTurn);
		}
		else
		{
			effectInfo3 = this.m_enemyEffectOnNextTurn;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "EnemyEffectOnNextTurn", this.m_enemyEffectOnNextTurn, true);
		base.AddTokenInt(tokens, "EelDamage", string.Empty, (!abilityMod_FishManGeyser) ? this.m_eelDamage : abilityMod_FishManGeyser.m_eelDamageMod.GetModifiedValue(this.m_eelDamage), false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_FishManGeyser) ? this.m_eelEffectOnEnemies : abilityMod_FishManGeyser.m_eelEffectOnEnemiesMod.GetModifiedValue(this.m_eelEffectOnEnemies), "EelEffectOnEnemies", this.m_eelEffectOnEnemies, true);
		string name3 = "TurnsTillFirstExplosion";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_FishManGeyser)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			val3 = abilityMod_FishManGeyser.m_turnsTillFirstExplosionMod.GetModifiedValue(this.m_turnsTillFirstExplosion);
		}
		else
		{
			val3 = this.m_turnsTillFirstExplosion;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "NumExplosionsBeforeEnding";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_FishManGeyser)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			val4 = abilityMod_FishManGeyser.m_numExplosionsBeforeEndingMod.GetModifiedValue(this.m_numExplosionsBeforeEnding);
		}
		else
		{
			val4 = this.m_numExplosionsBeforeEnding;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		string name5 = "DamageToEnemiesOnExplode";
		string empty5 = string.Empty;
		int val5;
		if (abilityMod_FishManGeyser)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			val5 = abilityMod_FishManGeyser.m_damageToEnemiesOnExplodeMod.GetModifiedValue(this.m_damageToEnemiesOnExplode);
		}
		else
		{
			val5 = this.m_damageToEnemiesOnExplode;
		}
		base.AddTokenInt(tokens, name5, empty5, val5, false);
		string name6 = "HealingToAlliesOnExplode";
		string empty6 = string.Empty;
		int val6;
		if (abilityMod_FishManGeyser)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			val6 = abilityMod_FishManGeyser.m_healingToAlliesOnExplodeMod.GetModifiedValue(this.m_healingToAlliesOnExplode);
		}
		else
		{
			val6 = this.m_healingToAlliesOnExplode;
		}
		base.AddTokenInt(tokens, name6, empty6, val6, false);
		StandardEffectInfo effectInfo4;
		if (abilityMod_FishManGeyser)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo4 = abilityMod_FishManGeyser.m_effectToEnemiesOnExplodeMod.GetModifiedValue(this.m_effectToEnemiesOnExplode);
		}
		else
		{
			effectInfo4 = this.m_effectToEnemiesOnExplode;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo4, "EffectToEnemiesOnExplode", this.m_effectToEnemiesOnExplode, true);
		StandardEffectInfo effectInfo5;
		if (abilityMod_FishManGeyser)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo5 = abilityMod_FishManGeyser.m_effectToAlliesOnExplodeMod.GetModifiedValue(this.m_effectToAlliesOnExplode);
		}
		else
		{
			effectInfo5 = this.m_effectToAlliesOnExplode;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo5, "EffectToAlliesOnExplode", this.m_effectToAlliesOnExplode, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageToEnemiesOnCast());
		this.GetEffectToEnemiesOnCast().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetHealingToAlliesOnCast());
		this.GetEffectToAlliesOnCast().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetHealOnCasterPerEnemyHit());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		ActorData actorData = base.ActorData;
		if (tooltipSubjectTypes != null && actorData != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (base.Targeter is AbilityUtil_Targeter_MultipleShapes)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				List<AbilityUtil_Targeter_MultipleShapes.HitActorContext> hitActorContext = (base.Targeter as AbilityUtil_Targeter_MultipleShapes).GetHitActorContext();
				using (List<AbilityUtil_Targeter_MultipleShapes.HitActorContext>.Enumerator enumerator = hitActorContext.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityUtil_Targeter_MultipleShapes.HitActorContext hitActorContext2 = enumerator.Current;
						if (hitActorContext2.m_actor == targetActor && targetActor.\u000E() != actorData.\u000E())
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							dictionary[AbilityTooltipSymbol.Damage] = this.GetDamageForShapeIndex(hitActorContext2.m_hitShapeIndex);
							goto IL_FF;
						}
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				IL_FF:;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.GetDamageToEnemiesOnCast();
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				dictionary[AbilityTooltipSymbol.Healing] = visibleActorsCountByTooltipSubject * this.GetHealOnCasterPerEnemyHit();
			}
			return dictionary;
		}
		return null;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManGeyser))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FishManGeyser.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_FishManGeyser);
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

	[Serializable]
	public class ShapeToDamage : ShapeToDataBase
	{
		public int m_damage;

		public ShapeToDamage(AbilityAreaShape shape, int damage)
		{
			this.m_shape = shape;
			this.m_damage = damage;
		}
	}
}
