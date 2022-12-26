// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MantaBasicAttack : Ability
{
	[Header("-- Targeting")]
	public float m_coneWidthAngle = 180f;
	public float m_coneBackwardOffset;
	public float m_coneLengthInner = 1.5f;
	public float m_coneLengthThroughWalls = 2.5f;
	[Header("-- Damage")]
	public int m_damageAmountInner = 28;
	public int m_damageAmountThroughWalls = 10;
	public StandardEffectInfo m_effectInner;
	public StandardEffectInfo m_effectOuter;
	[Header("-- Sequences")]
	public GameObject m_throughWallsConeSequence;

	private Manta_SyncComponent m_syncComp;
	private AbilityMod_MantaBasicAttack m_abilityMod;
	private int c_innerConeIdentifier = 1;
	private StandardEffectInfo m_cachedEffectInner;
	private StandardEffectInfo m_cachedEffectOuter;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Crush & Quake";
		}
		m_syncComp = GetComponent<Manta_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		float coneWidthAngle = GetConeWidthAngle();
		Targeter = new AbilityUtil_Targeter_MultipleCones(
			this, 
			new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>
			{
				new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneWidthAngle, GetConeLengthInner()),
				new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneWidthAngle, GetConeLengthThroughWalls())
			},
			m_coneBackwardOffset,
			true,
			true);
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Art --</color>\n" +
		       "On Sequence, for HitActorGroupOnAnimEventSequence components, use:\n"
		       + c_innerConeIdentifier + " for Inner cone group identifier\n";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeLengthThroughWalls();
	}

	private void SetCachedFields()
	{
		m_cachedEffectInner = m_abilityMod != null
			? m_abilityMod.m_effectInnerMod.GetModifiedValue(m_effectInner)
			: m_effectInner;
		m_cachedEffectOuter = m_abilityMod != null
			? m_abilityMod.m_effectOuterMod.GetModifiedValue(m_effectOuter)
			: m_effectOuter;
	}

	public float GetConeWidthAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset) 
			: m_coneBackwardOffset;
	}

	public float GetConeLengthInner()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_coneLengthInnerMod.GetModifiedValue(m_coneLengthInner) 
			: m_coneLengthInner;
	}

	public float GetConeLengthThroughWalls()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneLengthThroughWallsMod.GetModifiedValue(m_coneLengthThroughWalls)
			: m_coneLengthThroughWalls;
	}

	public int GetDamageAmountInner()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_damageAmountInnerMod.GetModifiedValue(m_damageAmountInner) 
			: m_damageAmountInner;
	}

	public int GetDamageAmountThroughWalls()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_damageAmountThroughWallsMod.GetModifiedValue(m_damageAmountThroughWalls) 
			: m_damageAmountThroughWalls;
	}

	public int GetExtraDamageNoLoS()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamageNoLoSMod.GetModifiedValue(0) 
			: 0;
	}

	public StandardEffectInfo GetEffectInner()
	{
		return m_cachedEffectInner ?? m_effectInner;
	}

	public StandardEffectInfo GetEffectOuter()
	{
		return m_cachedEffectOuter ?? m_effectOuter;
	}

	public StandardEffectInfo GetAdditionalDirtyFightingExplosionEffect()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_additionalDirtyFightingExplosionEffect.operation == AbilityModPropertyEffectInfo.ModOp.Override
				? m_abilityMod.m_additionalDirtyFightingExplosionEffect.effectInfo
				: null;
	}

	public bool ShouldDisruptBrushInCone()
	{
		return m_abilityMod != null && m_abilityMod.m_disruptBrushInConeMod.GetModifiedValue(false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaBasicAttack))
		{
			m_abilityMod = abilityMod as AbilityMod_MantaBasicAttack;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Near, m_damageAmountInner));
		m_effectInner.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Near);
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, m_damageAmountThroughWalls));
		m_effectOuter.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Far);
		return numbers;
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = base.Debug_GetExpectedNumbersInTooltip();
		int num = Mathf.Abs(m_damageAmountInner - m_damageAmountThroughWalls);
		if (num != 0)
		{
			list.Add(num);
		}
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmountInner", string.Empty, m_damageAmountInner);
		AddTokenInt(tokens, "DamageAmountThroughWalls", string.Empty, m_damageAmountThroughWalls);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectInner, "EffectInner", m_effectInner);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOuter, "EffectOuter", m_effectOuter);
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			int extraDamage = 0;
			if (!ActorData.CurrentBoardSquare.GetLOS(targetActor.CurrentBoardSquare.x, targetActor.CurrentBoardSquare.y))
			{
				extraDamage += GetExtraDamageNoLoS();
			}
// #if SERVER
// 			// added in rogues
// 			if (m_syncComp != null)
// 			{
// 				extraDamage += m_syncComp.GetDirtyFightingExtraDamage(targetActor);
// 			}
// #endif
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Near))
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmountInner() + extraDamage;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Far))
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmountThroughWalls() + extraDamage;
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (m_syncComp == null)
		{
			return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
		}
		
		int num = 0;
		foreach (AbilityUtil_Targeter.ActorTarget actorTarget in Targeters[currentTargeterIndex].GetActorsInRange())
		{
			num += m_syncComp.GetDirtyFightingExtraTP(actorTarget.m_actor);
		}
		return num;
	}

	// removed in rogues
	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return symbolType == AbilityTooltipSymbol.Damage && m_syncComp != null
			? m_syncComp.GetAccessoryStringForDamage(targetActor, ActorData, this)
			: null;
	}

	public override bool ForceIgnoreCover(ActorData targetActor)
	{
		return targetActor != null
		       && DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject.Far, targetActor, ActorData.GetFreePos(), ActorData);
	}

	private bool InsideNearRadius(ActorData targetActor, Vector3 damageOrigin)
	{
		float radius = GetConeLengthInner() * Board.Get().squareSize;
		Vector3 vector = targetActor.GetFreePos() - damageOrigin;
		vector.y = 0f;
		float dist = vector.magnitude;
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			dist -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
		}
		return dist <= radius;
	}

	public override bool DoesTargetActorMatchTooltipSubject(
		AbilityTooltipSubject subjectType,
		ActorData targetActor,
		Vector3 damageOrigin,
		ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.Near && subjectType != AbilityTooltipSubject.Far)
		{
			return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
		}
		else if (targetingActor.CurrentBoardSquare.GetLOS(targetActor.CurrentBoardSquare.x, targetActor.CurrentBoardSquare.y))
		{
			return InsideNearRadius(targetActor, damageOrigin)
				? subjectType == AbilityTooltipSubject.Near
				: subjectType == AbilityTooltipSubject.Far;
		}
		else
		{
			return subjectType == AbilityTooltipSubject.Far;
		}
	}

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		BoardSquare squareFromVec = Board.Get().GetSquareFromVec3(caster.GetLoSCheckPos());
		List<ActorData> hitActors = FindHitActorsForConeLength(targets, caster, GetConeLengthInner(), false, null);
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_throughWallsConeSequence,
				squareFromVec,
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource,
				new Sequence.IExtraSequenceParams[]
				{
					new HitActorGroupOnAnimEventSequence.ActorParams
					{
						m_groupIdentifier = (sbyte)c_innerConeIdentifier,
						m_hitActors = hitActors
					}
				})
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActorsInner = FindHitActorsForConeLength(targets, caster, GetConeLengthInner(), false, nonActorTargetInfo);
		List<ActorData> hitActorsThroughWalls = FindHitActorsForConeLength(targets, caster, GetConeLengthThroughWalls(), true, null);
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		foreach (ActorData actorData in hitActorsThroughWalls)
		{
			int extraDamage = 0;
			bool los = caster.CurrentBoardSquare.GetLOS(actorData.CurrentBoardSquare.x, actorData.CurrentBoardSquare.y);
			if (!los)
			{
				extraDamage += GetExtraDamageNoLoS();
			}
			ActorHitResults actorHitResults;
			if (hitActorsInner.Contains(actorData))
			{
				actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, loSCheckPos));
				actorHitResults.SetBaseDamage(GetDamageAmountInner() + extraDamage);
				actorHitResults.AddStandardEffectInfo(GetEffectInner());
				if (!los)
				{
					actorHitResults.AddHitResultsTag(HitResultsTags.HittingThroughWalls);
				}
			}
			else
			{
				actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorData.GetLoSCheckPos()));
				actorHitResults.SetBaseDamage(GetDamageAmountThroughWalls() + extraDamage);
				actorHitResults.AddStandardEffectInfo(GetEffectOuter());
				if (!los)
				{
					actorHitResults.AddHitResultsTag(HitResultsTags.HittingThroughWalls);
				}
			}
			List<Effect> effectsOnTargetByCaster = ServerEffectManager.Get().GetEffectsOnTargetByCaster(actorData, caster, typeof(MantaDirtyFightingEffect));
			if (!effectsOnTargetByCaster.IsNullOrEmpty() && (effectsOnTargetByCaster[0] as MantaDirtyFightingEffect).IsActive())
			{
				StandardEffectInfo additionalDirtyFightingExplosionEffect = GetAdditionalDirtyFightingExplosionEffect();
				if (additionalDirtyFightingExplosionEffect != null)
				{
					actorHitResults.AddStandardEffectInfo(additionalDirtyFightingExplosionEffect);
				}
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	public override List<int> GetAdditionalBrushRegionsToDisrupt(ActorData caster, List<AbilityTarget> targets)
	{
		List<int> list = new List<int>();
		if (ShouldDisruptBrushInCone())
		{
			Vector3 aimDirection = targets[0].AimDirection;
			Vector3 loSCheckPos = caster.GetLoSCheckPos();
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
			List<BoardSquare> squaresInCone = AreaEffectUtils.GetSquaresInCone(
				loSCheckPos,
				coneCenterAngleDegrees,
				GetConeWidthAngle(),
				GetConeLengthThroughWalls(),
				GetConeBackwardOffset(),
				true,
				caster);
			foreach (BoardSquare boardSquare in squaresInCone)
			{
				if (boardSquare.IsInBrush() && !list.Contains(boardSquare.BrushRegion))
				{
					list.Add(boardSquare.BrushRegion);
				}
			}
		}
		return list;
	}

	// added in rogues
	private List<ActorData> FindHitActorsForConeLength(
		List<AbilityTarget> targets,
		ActorData caster,
		float coneLength,
		bool penetrateLoS,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 aimDirection = targets[0].AimDirection;
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
			loSCheckPos,
			coneCenterAngleDegrees,
			GetConeWidthAngle(),
			coneLength,
			m_coneBackwardOffset,
			penetrateLoS,
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		return actorsInCone ?? new List<ActorData>();
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.BaseDamage > 0 && results.HasHitResultsTag(HitResultsTags.HittingThroughWalls))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.MantaStats.NumHitsThroughWalls);
		}
	}
#endif
}
