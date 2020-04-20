using System;
using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlDelayedMissile : Ability
{
	[Header("-- On Cast Hit Effect")]
	public StandardEffectInfo m_onCastEnemyHitEffect;

	[Header("-- Bomb Impact")]
	public int m_damage;

	public StandardEffectInfo m_effectOnHit;

	public int m_turnsBeforeExploding = 1;

	[Header("-- Targeting")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;

	public bool m_penetrateLineOfSight;

	public List<BazookaGirlDelayedMissile.ShapeToHitInfo> m_additionalShapeToHitInfo = new List<BazookaGirlDelayedMissile.ShapeToHitInfo>();

	[Header("-- Fake Markers (when using multi-click version), valid when positive")]
	public int m_useFakeMarkerIndexStart = -1;

	[Header("-- Anim")]
	public int m_explosionAnimationIndex = 0xB;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_markerSequencePrefab;

	public GameObject m_impactSequencePrefab;

	public GameObject m_fakeMarkerSequencePrefab;

	private AbilityMod_BazookaGirlDelayedMissile m_abilityMod;

	private List<AbilityAreaShape> m_additionalShapes = new List<AbilityAreaShape>();

	private List<BazookaGirlDelayedMissile.ShapeToHitInfo> m_cachedShapeToHitInfo = new List<BazookaGirlDelayedMissile.ShapeToHitInfo>();

	private StandardEffectInfo m_cachedOnExplosionEffect;

	private void Start()
	{
		this.SetupTargeter();
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_damage;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
		}
		return result;
	}

	public StandardEffectInfo GetOnCastEnemyHitEffect()
	{
		if (this.m_abilityMod == null)
		{
			return this.m_onCastEnemyHitEffect;
		}
		return this.m_abilityMod.m_effectOnEnemyOnCastOverride.GetModifiedValue(this.m_onCastEnemyHitEffect);
	}

	public AbilityAreaShape GetShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_shapeMod.GetModifiedValue(this.m_shape);
		}
		else
		{
			result = this.m_shape;
		}
		return result;
	}

	public bool UseAdditionalShapes()
	{
		if (this.m_abilityMod != null && this.m_abilityMod.m_useAdditionalShapeToHitInfoOverride)
		{
			return this.m_abilityMod.m_additionalShapeToHitInfoMod.Count > 0;
		}
		return this.m_additionalShapeToHitInfo.Count > 0;
	}

	public List<BazookaGirlDelayedMissile.ShapeToHitInfo> GetShapeToHitInfo()
	{
		return this.m_cachedShapeToHitInfo;
	}

	public int GetUseFakeMarkerIndexStart()
	{
		return (!this.m_abilityMod) ? this.m_useFakeMarkerIndexStart : this.m_abilityMod.m_useFakeMarkerIndexStartMod.GetModifiedValue(this.m_useFakeMarkerIndexStart);
	}

	private void SetCachedFields()
	{
		this.m_cachedOnExplosionEffect = ((!this.m_abilityMod) ? this.m_effectOnHit : this.m_abilityMod.m_onExplosionEffectMod.GetModifiedValue(this.m_effectOnHit));
	}

	public StandardEffectInfo GetOnExplosionEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedOnExplosionEffect != null)
		{
			result = this.m_cachedOnExplosionEffect;
		}
		else
		{
			result = this.m_effectOnHit;
		}
		return result;
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		if (this.m_abilityMod != null && this.m_abilityMod.m_useAdditionalShapeToHitInfoOverride)
		{
			this.m_cachedShapeToHitInfo = new List<BazookaGirlDelayedMissile.ShapeToHitInfo>();
			using (List<AbilityMod_BazookaGirlDelayedMissile.ShapeToHitInfoMod>.Enumerator enumerator = this.m_abilityMod.m_additionalShapeToHitInfoMod.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityMod_BazookaGirlDelayedMissile.ShapeToHitInfoMod shapeToHitInfoMod = enumerator.Current;
					BazookaGirlDelayedMissile.ShapeToHitInfo shapeToHitInfo = new BazookaGirlDelayedMissile.ShapeToHitInfo();
					shapeToHitInfo.m_shape = shapeToHitInfoMod.m_shape;
					shapeToHitInfo.m_damage = shapeToHitInfoMod.m_damageMod.GetModifiedValue(this.m_damage);
					shapeToHitInfo.m_onExplosionEffect = shapeToHitInfoMod.m_onExplosionEffectInfo.GetModifiedValue(this.m_effectOnHit);
					this.m_cachedShapeToHitInfo.Add(shapeToHitInfo);
				}
			}
		}
		else
		{
			this.m_cachedShapeToHitInfo = new List<BazookaGirlDelayedMissile.ShapeToHitInfo>(this.m_additionalShapeToHitInfo);
		}
		this.m_cachedShapeToHitInfo.Sort();
		if (this.UseAdditionalShapes())
		{
			this.m_additionalShapes.Clear();
			List<BazookaGirlDelayedMissile.ShapeToHitInfo> shapeToHitInfo2 = this.GetShapeToHitInfo();
			using (List<BazookaGirlDelayedMissile.ShapeToHitInfo>.Enumerator enumerator2 = shapeToHitInfo2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					BazookaGirlDelayedMissile.ShapeToHitInfo shapeToHitInfo3 = enumerator2.Current;
					this.m_additionalShapes.Add(shapeToHitInfo3.m_shape);
				}
			}
		}
		base.ClearTargeters();
		if (this.GetExpectedNumberOfTargeters() < 2)
		{
			if (this.UseAdditionalShapes())
			{
				List<AbilityAreaShape> list = new List<AbilityAreaShape>();
				list.Add(this.GetShape());
				list.AddRange(this.m_additionalShapes);
				List<AbilityTooltipSubject> subjects = new List<AbilityTooltipSubject>
				{
					AbilityTooltipSubject.Primary
				};
				base.Targeter = new AbilityUtil_Targeter_MultipleShapes(this, list, subjects, this.m_penetrateLineOfSight, true, false, false);
			}
			else
			{
				base.Targeter = new AbilityUtil_Targeter_BazookaGirlDelayedMissile(this, this.GetShape(), this.m_penetrateLineOfSight, false, AbilityAreaShape.SingleSquare, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false);
			}
		}
		else
		{
			for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
			{
				AbilityUtil_Targeter_BazookaGirlDelayedMissile abilityUtil_Targeter_BazookaGirlDelayedMissile = new AbilityUtil_Targeter_BazookaGirlDelayedMissile(this, this.GetShape(), this.m_penetrateLineOfSight, false, AbilityAreaShape.SingleSquare, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false);
				if (this.GetUseFakeMarkerIndexStart() > 0)
				{
					if (i >= this.GetUseFakeMarkerIndexStart())
					{
						abilityUtil_Targeter_BazookaGirlDelayedMissile.SetTooltipSubjectTypes(AbilityTooltipSubject.Quaternary, AbilityTooltipSubject.Quaternary, AbilityTooltipSubject.None);
						abilityUtil_Targeter_BazookaGirlDelayedMissile.SetAffectedGroups(false, false, false);
					}
				}
				base.Targeters.Add(abilityUtil_Targeter_BazookaGirlDelayedMissile);
				base.Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result = 1;
		if (this.m_abilityMod != null && this.m_abilityMod.m_useTargetDataOverrides)
		{
			if (this.m_abilityMod.m_targetDataOverrides.Length > 1)
			{
				result = this.m_abilityMod.m_targetDataOverrides.Length;
			}
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_damage)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (base.Targeter is AbilityUtil_Targeter_MultipleShapes)
			{
				List<AbilityUtil_Targeter_MultipleShapes.HitActorContext> hitActorContext = (base.Targeter as AbilityUtil_Targeter_MultipleShapes).GetHitActorContext();
				using (List<AbilityUtil_Targeter_MultipleShapes.HitActorContext>.Enumerator enumerator = hitActorContext.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityUtil_Targeter_MultipleShapes.HitActorContext hitActorContext2 = enumerator.Current;
						if (hitActorContext2.m_actor == targetActor)
						{
							dictionary[AbilityTooltipSymbol.Damage] = this.GetDamageForShapeIndex(hitActorContext2.m_hitShapeIndex);
							goto IL_C7;
						}
					}
				}
				IL_C7:;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.GetDamageAmount();
			}
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlDelayedMissile abilityMod_BazookaGirlDelayedMissile = modAsBase as AbilityMod_BazookaGirlDelayedMissile;
		StandardEffectInfo effectInfo;
		if (abilityMod_BazookaGirlDelayedMissile)
		{
			effectInfo = abilityMod_BazookaGirlDelayedMissile.m_effectOnEnemyOnCastOverride.GetModifiedValue(this.m_onCastEnemyHitEffect);
		}
		else
		{
			effectInfo = this.m_onCastEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "OnCastEnemyHitEffect", this.m_onCastEnemyHitEffect, true);
		string name = "Damage";
		string empty = string.Empty;
		int val;
		if (abilityMod_BazookaGirlDelayedMissile)
		{
			val = abilityMod_BazookaGirlDelayedMissile.m_damageMod.GetModifiedValue(this.m_damage);
		}
		else
		{
			val = this.m_damage;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		StandardEffectInfo effectInfo2;
		if (abilityMod_BazookaGirlDelayedMissile)
		{
			effectInfo2 = abilityMod_BazookaGirlDelayedMissile.m_onExplosionEffectMod.GetModifiedValue(this.m_effectOnHit);
		}
		else
		{
			effectInfo2 = this.m_effectOnHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnHit", this.m_effectOnHit, true);
		for (int i = 0; i < this.m_additionalShapeToHitInfo.Count; i++)
		{
			base.AddTokenInt(tokens, "Damage_ExtraLayer_" + i, string.Empty, this.m_additionalShapeToHitInfo[i].m_damage, false);
		}
	}

	private int GetDamageForShapeIndex(int index)
	{
		int result = this.GetDamageAmount();
		List<BazookaGirlDelayedMissile.ShapeToHitInfo> shapeToHitInfo = this.GetShapeToHitInfo();
		if (index > 0)
		{
			if (this.UseAdditionalShapes())
			{
				if (index <= this.m_additionalShapes.Count)
				{
					result = shapeToHitInfo[index - 1].m_damage;
				}
			}
		}
		return result;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		bool result;
		if (animIndex != this.m_explosionAnimationIndex)
		{
			result = base.CanTriggerAnimAtIndexForTaunt(animIndex);
		}
		else
		{
			result = true;
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BazookaGirlDelayedMissile))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_BazookaGirlDelayedMissile);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		foreach (AbilityTarget target in targets)
		{
			list.AddRange(AreaEffectUtils.BuildShapeCornersList(this.GetShape(), target));
		}
		return list;
	}

	[Serializable]
	public class ShapeToHitInfo : ShapeToDataBase
	{
		public int m_damage;

		public StandardEffectInfo m_onExplosionEffect;
	}
}
