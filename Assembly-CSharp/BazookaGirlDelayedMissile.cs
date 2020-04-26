using System;
using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlDelayedMissile : Ability
{
	[Serializable]
	public class ShapeToHitInfo : ShapeToDataBase
	{
		public int m_damage;

		public StandardEffectInfo m_onExplosionEffect;
	}

	[Header("-- On Cast Hit Effect")]
	public StandardEffectInfo m_onCastEnemyHitEffect;

	[Header("-- Bomb Impact")]
	public int m_damage;

	public StandardEffectInfo m_effectOnHit;

	public int m_turnsBeforeExploding = 1;

	[Header("-- Targeting")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;

	public bool m_penetrateLineOfSight;

	public List<ShapeToHitInfo> m_additionalShapeToHitInfo = new List<ShapeToHitInfo>();

	[Header("-- Fake Markers (when using multi-click version), valid when positive")]
	public int m_useFakeMarkerIndexStart = -1;

	[Header("-- Anim")]
	public int m_explosionAnimationIndex = 11;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_markerSequencePrefab;

	public GameObject m_impactSequencePrefab;

	public GameObject m_fakeMarkerSequencePrefab;

	private AbilityMod_BazookaGirlDelayedMissile m_abilityMod;

	private List<AbilityAreaShape> m_additionalShapes = new List<AbilityAreaShape>();

	private List<ShapeToHitInfo> m_cachedShapeToHitInfo = new List<ShapeToHitInfo>();

	private StandardEffectInfo m_cachedOnExplosionEffect;

	private void Start()
	{
		SetupTargeter();
	}

	public int GetDamageAmount()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_damage;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
		}
		return result;
	}

	public StandardEffectInfo GetOnCastEnemyHitEffect()
	{
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_onCastEnemyHitEffect;
				}
			}
		}
		return m_abilityMod.m_effectOnEnemyOnCastOverride.GetModifiedValue(m_onCastEnemyHitEffect);
	}

	public AbilityAreaShape GetShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_shapeMod.GetModifiedValue(m_shape);
		}
		else
		{
			result = m_shape;
		}
		return result;
	}

	public bool UseAdditionalShapes()
	{
		if (m_abilityMod != null && m_abilityMod.m_useAdditionalShapeToHitInfoOverride)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_additionalShapeToHitInfoMod.Count > 0;
				}
			}
		}
		return m_additionalShapeToHitInfo.Count > 0;
	}

	public List<ShapeToHitInfo> GetShapeToHitInfo()
	{
		return m_cachedShapeToHitInfo;
	}

	public int GetUseFakeMarkerIndexStart()
	{
		return (!m_abilityMod) ? m_useFakeMarkerIndexStart : m_abilityMod.m_useFakeMarkerIndexStartMod.GetModifiedValue(m_useFakeMarkerIndexStart);
	}

	private void SetCachedFields()
	{
		m_cachedOnExplosionEffect = ((!m_abilityMod) ? m_effectOnHit : m_abilityMod.m_onExplosionEffectMod.GetModifiedValue(m_effectOnHit));
	}

	public StandardEffectInfo GetOnExplosionEffect()
	{
		StandardEffectInfo result;
		if (m_cachedOnExplosionEffect != null)
		{
			result = m_cachedOnExplosionEffect;
		}
		else
		{
			result = m_effectOnHit;
		}
		return result;
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		if (m_abilityMod != null && m_abilityMod.m_useAdditionalShapeToHitInfoOverride)
		{
			m_cachedShapeToHitInfo = new List<ShapeToHitInfo>();
			using (List<AbilityMod_BazookaGirlDelayedMissile.ShapeToHitInfoMod>.Enumerator enumerator = m_abilityMod.m_additionalShapeToHitInfoMod.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityMod_BazookaGirlDelayedMissile.ShapeToHitInfoMod current = enumerator.Current;
					ShapeToHitInfo shapeToHitInfo = new ShapeToHitInfo();
					shapeToHitInfo.m_shape = current.m_shape;
					shapeToHitInfo.m_damage = current.m_damageMod.GetModifiedValue(m_damage);
					shapeToHitInfo.m_onExplosionEffect = current.m_onExplosionEffectInfo.GetModifiedValue(m_effectOnHit);
					m_cachedShapeToHitInfo.Add(shapeToHitInfo);
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						goto end_IL_0047;
					}
				}
				end_IL_0047:;
			}
		}
		else
		{
			m_cachedShapeToHitInfo = new List<ShapeToHitInfo>(m_additionalShapeToHitInfo);
		}
		m_cachedShapeToHitInfo.Sort();
		if (UseAdditionalShapes())
		{
			m_additionalShapes.Clear();
			List<ShapeToHitInfo> shapeToHitInfo2 = GetShapeToHitInfo();
			using (List<ShapeToHitInfo>.Enumerator enumerator2 = shapeToHitInfo2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ShapeToHitInfo current2 = enumerator2.Current;
					m_additionalShapes.Add(current2.m_shape);
				}
			}
		}
		ClearTargeters();
		if (GetExpectedNumberOfTargeters() < 2)
		{
			if (UseAdditionalShapes())
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						List<AbilityAreaShape> list = new List<AbilityAreaShape>();
						list.Add(GetShape());
						list.AddRange(m_additionalShapes);
						List<AbilityTooltipSubject> list2 = new List<AbilityTooltipSubject>();
						list2.Add(AbilityTooltipSubject.Primary);
						List<AbilityTooltipSubject> subjects = list2;
						base.Targeter = new AbilityUtil_Targeter_MultipleShapes(this, list, subjects, m_penetrateLineOfSight);
						return;
					}
					}
				}
			}
			base.Targeter = new AbilityUtil_Targeter_BazookaGirlDelayedMissile(this, GetShape(), m_penetrateLineOfSight, false, AbilityAreaShape.SingleSquare);
			return;
		}
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_BazookaGirlDelayedMissile abilityUtil_Targeter_BazookaGirlDelayedMissile = new AbilityUtil_Targeter_BazookaGirlDelayedMissile(this, GetShape(), m_penetrateLineOfSight, false, AbilityAreaShape.SingleSquare);
			if (GetUseFakeMarkerIndexStart() > 0)
			{
				if (i >= GetUseFakeMarkerIndexStart())
				{
					abilityUtil_Targeter_BazookaGirlDelayedMissile.SetTooltipSubjectTypes(AbilityTooltipSubject.Quaternary, AbilityTooltipSubject.Quaternary);
					abilityUtil_Targeter_BazookaGirlDelayedMissile.SetAffectedGroups(false, false, false);
				}
			}
			base.Targeters.Add(abilityUtil_Targeter_BazookaGirlDelayedMissile);
			base.Targeters[i].SetUseMultiTargetUpdate(true);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result = 1;
		if (m_abilityMod != null && m_abilityMod.m_useTargetDataOverrides)
		{
			if (m_abilityMod.m_targetDataOverrides.Length > 1)
			{
				result = m_abilityMod.m_targetDataOverrides.Length;
			}
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damage));
		return list;
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
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
					{
						List<AbilityUtil_Targeter_MultipleShapes.HitActorContext> hitActorContext = (base.Targeter as AbilityUtil_Targeter_MultipleShapes).GetHitActorContext();
						using (List<AbilityUtil_Targeter_MultipleShapes.HitActorContext>.Enumerator enumerator = hitActorContext.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								AbilityUtil_Targeter_MultipleShapes.HitActorContext current = enumerator.Current;
								if (current.m_actor == targetActor)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											break;
										default:
											dictionary[AbilityTooltipSymbol.Damage] = GetDamageForShapeIndex(current.m_hitShapeIndex);
											return dictionary;
										}
									}
								}
							}
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									return dictionary;
								}
							}
						}
					}
					}
				}
			}
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmount();
			}
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlDelayedMissile abilityMod_BazookaGirlDelayedMissile = modAsBase as AbilityMod_BazookaGirlDelayedMissile;
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_BazookaGirlDelayedMissile)
		{
			effectInfo = abilityMod_BazookaGirlDelayedMissile.m_effectOnEnemyOnCastOverride.GetModifiedValue(m_onCastEnemyHitEffect);
		}
		else
		{
			effectInfo = m_onCastEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "OnCastEnemyHitEffect", m_onCastEnemyHitEffect);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_BazookaGirlDelayedMissile)
		{
			val = abilityMod_BazookaGirlDelayedMissile.m_damageMod.GetModifiedValue(m_damage);
		}
		else
		{
			val = m_damage;
		}
		AddTokenInt(tokens, "Damage", empty, val);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_BazookaGirlDelayedMissile)
		{
			effectInfo2 = abilityMod_BazookaGirlDelayedMissile.m_onExplosionEffectMod.GetModifiedValue(m_effectOnHit);
		}
		else
		{
			effectInfo2 = m_effectOnHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnHit", m_effectOnHit);
		for (int i = 0; i < m_additionalShapeToHitInfo.Count; i++)
		{
			AddTokenInt(tokens, "Damage_ExtraLayer_" + i, string.Empty, m_additionalShapeToHitInfo[i].m_damage);
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private int GetDamageForShapeIndex(int index)
	{
		int result = GetDamageAmount();
		List<ShapeToHitInfo> shapeToHitInfo = GetShapeToHitInfo();
		if (index > 0)
		{
			if (UseAdditionalShapes())
			{
				if (index <= m_additionalShapes.Count)
				{
					result = shapeToHitInfo[index - 1].m_damage;
				}
			}
		}
		return result;
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		int result;
		if (animIndex != m_explosionAnimationIndex)
		{
			result = (base.CanTriggerAnimAtIndexForTaunt(animIndex) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BazookaGirlDelayedMissile))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_BazookaGirlDelayedMissile);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		foreach (AbilityTarget target in targets)
		{
			list.AddRange(AreaEffectUtils.BuildShapeCornersList(GetShape(), target));
		}
		return list;
	}
}
