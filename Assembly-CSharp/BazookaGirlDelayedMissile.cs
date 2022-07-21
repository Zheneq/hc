// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// empty in rouges
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
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public StandardEffectInfo GetOnCastEnemyHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_effectOnEnemyOnCastOverride.GetModifiedValue(m_onCastEnemyHitEffect)
			: m_onCastEnemyHitEffect;
	}

	public AbilityAreaShape GetShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shapeMod.GetModifiedValue(m_shape)
			: m_shape;
	}

	public bool UseAdditionalShapes()
	{
		return m_abilityMod != null && m_abilityMod.m_useAdditionalShapeToHitInfoOverride
			? m_abilityMod.m_additionalShapeToHitInfoMod.Count > 0
			: m_additionalShapeToHitInfo.Count > 0;
	}

	public List<ShapeToHitInfo> GetShapeToHitInfo()
	{
		return m_cachedShapeToHitInfo;
	}

	public int GetUseFakeMarkerIndexStart()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useFakeMarkerIndexStartMod.GetModifiedValue(m_useFakeMarkerIndexStart)
			: m_useFakeMarkerIndexStart;
	}

	private void SetCachedFields()
	{
		m_cachedOnExplosionEffect = m_abilityMod != null
			? m_abilityMod.m_onExplosionEffectMod.GetModifiedValue(m_effectOnHit)
			: m_effectOnHit;
	}

	public StandardEffectInfo GetOnExplosionEffect()
	{
		return m_cachedOnExplosionEffect ?? m_effectOnHit;
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		if (m_abilityMod != null && m_abilityMod.m_useAdditionalShapeToHitInfoOverride)
		{
			m_cachedShapeToHitInfo = new List<ShapeToHitInfo>();
			foreach (AbilityMod_BazookaGirlDelayedMissile.ShapeToHitInfoMod mod in m_abilityMod.m_additionalShapeToHitInfoMod)
			{
				ShapeToHitInfo shapeToHitInfo = new ShapeToHitInfo
				{
					m_shape = mod.m_shape,
					m_damage = mod.m_damageMod.GetModifiedValue(m_damage),
					m_onExplosionEffect = mod.m_onExplosionEffectInfo.GetModifiedValue(m_effectOnHit)
				};
				m_cachedShapeToHitInfo.Add(shapeToHitInfo);
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
			foreach (ShapeToHitInfo shape in GetShapeToHitInfo())
			{
				m_additionalShapes.Add(shape.m_shape);
			}
		}
		ClearTargeters();
		if (GetExpectedNumberOfTargeters() >= 2)
		{
			for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
			{
				AbilityUtil_Targeter_BazookaGirlDelayedMissile targeter = new AbilityUtil_Targeter_BazookaGirlDelayedMissile(this, GetShape(), m_penetrateLineOfSight, false, AbilityAreaShape.SingleSquare);
				if (GetUseFakeMarkerIndexStart() > 0 && i >= GetUseFakeMarkerIndexStart())
				{
					targeter.SetTooltipSubjectTypes(AbilityTooltipSubject.Quaternary, AbilityTooltipSubject.Quaternary);
					targeter.SetAffectedGroups(false, false, false);
				}

				Targeters.Add(targeter);
				Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
		else if (UseAdditionalShapes())
		{
			List<AbilityAreaShape> shapes = new List<AbilityAreaShape> { GetShape() };
			shapes.AddRange(m_additionalShapes);
			List<AbilityTooltipSubject> subjects = new List<AbilityTooltipSubject> { AbilityTooltipSubject.Primary };
			Targeter = new AbilityUtil_Targeter_MultipleShapes(this, shapes, subjects, m_penetrateLineOfSight);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_BazookaGirlDelayedMissile(this, GetShape(), m_penetrateLineOfSight, false, AbilityAreaShape.SingleSquare);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_useTargetDataOverrides
		       && m_abilityMod.m_targetDataOverrides.Length > 1
			? m_abilityMod.m_targetDataOverrides.Length
			: 1;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damage)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (Targeter is AbilityUtil_Targeter_MultipleShapes)
		{
			AbilityUtil_Targeter_MultipleShapes targeter = Targeter as AbilityUtil_Targeter_MultipleShapes;
			List<AbilityUtil_Targeter_MultipleShapes.HitActorContext> hitActorContext = targeter.GetHitActorContext();
			foreach (AbilityUtil_Targeter_MultipleShapes.HitActorContext current in hitActorContext)
			{
				if (current.m_actor == targetActor)
				{
					dictionary[AbilityTooltipSymbol.Damage] = GetDamageForShapeIndex(current.m_hitShapeIndex);
					return dictionary;
				}
			}
		}
		else
		{
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmount();
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlDelayedMissile abilityMod_BazookaGirlDelayedMissile = modAsBase as AbilityMod_BazookaGirlDelayedMissile;
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BazookaGirlDelayedMissile != null
			? abilityMod_BazookaGirlDelayedMissile.m_effectOnEnemyOnCastOverride.GetModifiedValue(m_onCastEnemyHitEffect)
			: m_onCastEnemyHitEffect, "OnCastEnemyHitEffect", m_onCastEnemyHitEffect);
		AddTokenInt(tokens, "Damage", string.Empty, abilityMod_BazookaGirlDelayedMissile != null
			? abilityMod_BazookaGirlDelayedMissile.m_damageMod.GetModifiedValue(m_damage)
			: m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BazookaGirlDelayedMissile != null
			? abilityMod_BazookaGirlDelayedMissile.m_onExplosionEffectMod.GetModifiedValue(m_effectOnHit)
			: m_effectOnHit, "EffectOnHit", m_effectOnHit);
		for (int i = 0; i < m_additionalShapeToHitInfo.Count; i++)
		{
			AddTokenInt(tokens, "Damage_ExtraLayer_" + i, string.Empty, m_additionalShapeToHitInfo[i].m_damage);
		}
	}

	private int GetDamageForShapeIndex(int index)
	{
		return index > 0
		       && UseAdditionalShapes()
		       && index <= m_additionalShapes.Count
			? GetShapeToHitInfo()[index - 1].m_damage
			: GetDamageAmount();
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		return animIndex == m_explosionAnimationIndex || base.CanTriggerAnimAtIndexForTaunt(animIndex);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BazookaGirlDelayedMissile))
		{
			m_abilityMod = abilityMod as AbilityMod_BazookaGirlDelayedMissile;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> points = new List<Vector3>();
		foreach (AbilityTarget target in targets)
		{
			points.AddRange(AreaEffectUtils.BuildShapeCornersList(GetShape(), target));
		}
		return points;
	}
	
#if SERVER
	// custom
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			targets[0].FreePos,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}
	
	// custom
	public override void GatherAbilityResults(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		if (GetExpectedNumberOfTargeters() >= 2)
		{
			Log.Error($"Cannot gather ability results for multiple targeters!");
		}
		else if (UseAdditionalShapes())
		{
			// TODO ZUKI ability resolution
			Log.Error($"Cannot gather ability results for additional shapes!");
		}
		else
		{
			GatherAbilityResultsDirectionCone(targets, caster, ref abilityResults);
		}
	}
	
	// custom
	private void GatherAbilityResultsDirectionCone(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		AbilityTarget currentTarget = targets[0];
		BoardSquare targetSquare =  Board.Get().GetSquare(currentTarget.GridPos);
		if (targetSquare == null)
		{
			Log.Error("GatherAbilityResultsDirectionCone: Target square is null");
			return;
		}
		Vector3 freePos = currentTarget.FreePos;
		Vector3 damageOrigin = AreaEffectUtils.GetCenterOfShape(m_shape, freePos, targetSquare);
		List<ActorData> actors = AreaEffectUtils.GetActorsInShape(
			GetShape(),
			freePos,
			targetSquare,
			m_penetrateLineOfSight,
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		// actors.Remove(caster);
		// bool isCasterInShape = AreaEffectUtils.IsSquareInShape(
		// 	caster.GetCurrentBoardSquare(),
		// 	m_shape,
		// 	freePos, 
		// 	gameplayRefSquare,
		// 	m_penetrateLineOfSight,
		// 	caster);
		// if (isCasterInShape)
		// {
		// 	actors.Add(caster);
		// }
		List<ActorData> targetActors = actors.Where(target => target.GetTeam() != caster.GetTeam()).ToList();
		if (!targetActors.IsNullOrEmpty()) // TODO add effect if empty
		{
			foreach (ActorData targetActor in targetActors)
			{
				ActorHitParameters hitParams = new ActorHitParameters(targetActor, damageOrigin);
				ActorHitResults hitResults = new ActorHitResults(0, HitActionType.Damage, GetOnCastEnemyHitEffect(), hitParams);
				// TODO ZUKI ability resolution
				// hitResults.AddEffect(effect);
				abilityResults.StoreActorHit(hitResults);
			}
		}
		// TODO ZUKI ability resolution
		// abilityResults.StorePositionHit(positionHitResults);
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
