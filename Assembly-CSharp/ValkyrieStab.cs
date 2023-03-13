using System.Collections.Generic;
using UnityEngine;

public class ValkyrieStab : Ability
{
	[Header("-- Targeting")]
	public float m_coneWidthMinAngle = 10f;
	public float m_coneWidthMaxAngle = 70f;
	public float m_coneBackwardOffset;
	public float m_coneMinLength = 2.5f;
	public float m_coneMaxLength = 5f;
	public AreaEffectUtils.StretchConeStyle m_coneStretchStyle;
	public bool m_penetrateLineOfSight;
	public int m_maxTargets = 5;
	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 20;
	public int m_lessDamagePerTarget = 3;
	public StandardEffectInfo m_targetHitEffect;
	[Header("-- Sequences")]
	public GameObject m_centerProjectileSequencePrefab;
	public GameObject m_sideProjectileSequencePrefab;
	private Valkyrie_SyncComponent m_syncComp;
	private AbilityMod_ValkyrieStab m_abilityMod;
	private StandardEffectInfo m_cachedTargetHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Spear Poke";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_syncComp = GetComponent<Valkyrie_SyncComponent>();
		SetCachedFields();
		AbilityUtil_Targeter_ReverseStretchCone targeter = new AbilityUtil_Targeter_ReverseStretchCone(
			this, 
			GetConeMinLength(), 
			GetConeMaxLength(), 
			GetConeWidthMinAngle(), 
			GetConeWidthMaxAngle(), 
			m_coneStretchStyle, 
			GetConeBackwardOffset(), 
			PenetrateLineOfSight());
		base.Targeter = targeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeMaxLength() + GetConeBackwardOffset();
	}

	private void SetCachedFields()
	{
		m_cachedTargetHitEffect = (m_abilityMod ? m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect) : m_targetHitEffect);
	}

	public float GetConeWidthMinAngle()
	{
		return (!m_abilityMod) ? m_coneWidthMinAngle : m_abilityMod.m_coneWidthMinAngleMod.GetModifiedValue(m_coneWidthMinAngle);
	}

	public float GetConeWidthMaxAngle()
	{
		if (!m_abilityMod)
		{
			return m_coneWidthMaxAngle;
		}
		return m_abilityMod.m_coneWidthMaxAngleMod.GetModifiedValue(m_coneWidthMaxAngle);
	}

	public float GetConeBackwardOffset()
	{
		if (!m_abilityMod)
		{
			return m_coneBackwardOffset;
		}
		return m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
	}

	public float GetConeMinLength()
	{
		if (!m_abilityMod)
		{
			return m_coneMinLength;
		}
		return m_abilityMod.m_coneMinLengthMod.GetModifiedValue(m_coneMinLength);
	}

	public float GetConeMaxLength()
	{
		if (!m_abilityMod)
		{
			return m_coneMaxLength;
		}
		return m_abilityMod.m_coneMaxLengthMod.GetModifiedValue(m_coneMaxLength);
	}

	public bool PenetrateLineOfSight()
	{
		return (!m_abilityMod) ? m_penetrateLineOfSight : m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
	}

	public int GetMaxTargets()
	{
		if (!m_abilityMod)
		{
			return m_maxTargets;
		}
		return m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
	}

	public int GetDamageAmount()
	{
		if (!m_abilityMod)
		{
			return m_damageAmount;
		}
		return m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
	}

	public int GetLessDamagePerTarget()
	{
		if (!m_abilityMod)
		{
			return m_lessDamagePerTarget;
		}
		return m_abilityMod.m_lessDamagePerTargetMod.GetModifiedValue(m_lessDamagePerTarget);
	}

	public int GetExtraDamageOnSpearTip()
	{
		if (!m_abilityMod)
		{
			return 0;
		}
		return m_abilityMod.m_extraDamageOnSpearTip.GetModifiedValue(0);
	}

	public int GetExtraDamageFirstTarget()
	{
		if (!m_abilityMod)
		{
			return 0;
		}
		return m_abilityMod.m_extraDamageFirstTarget.GetModifiedValue(0);
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		return (m_cachedTargetHitEffect == null) ? m_targetHitEffect : m_cachedTargetHitEffect;
	}

	public int GetExtraAbsorbNextShieldBlockPerHit()
	{
		return m_abilityMod ? m_abilityMod.m_perHitExtraAbsorbNextShieldBlock.GetModifiedValue(0) : 0;
	}

	public int GetMaxExtraAbsorbNextShieldBlock()
	{
		if (!m_abilityMod)
		{
			return 0;
		}
		return m_abilityMod.m_maxExtraAbsorbNextShieldBlock.GetModifiedValue(0);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieStab))
		{
			m_abilityMod = (abilityMod as AbilityMod_ValkyrieStab);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetDamageAmount()));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeter.GetActorsInRange();
		List<ActorData> list = new List<ActorData>();
		int num = 0;
		foreach (AbilityUtil_Targeter.ActorTarget actorTarget in actorsInRange)
		{
			list.Add(actorTarget.m_actor);
			if (actorTarget.m_actor == targetActor && actorTarget.m_subjectTypes.Contains(AbilityTooltipSubject.Far))
			{
				num = GetExtraDamageOnSpearTip();
			}
		}
		int num2 = GetDamageAmount();
		bool flag = true;
		num2 += GetExtraDamageFirstTarget();
		using (List<ActorData>.Enumerator enumerator2 = list.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				if (enumerator2.Current == targetActor)
				{
					dictionary[AbilityTooltipSymbol.Damage] = num2 + num;
					break;
				}
				if (m_syncComp == null || !m_syncComp.m_skipDamageReductionForNextStab)
				{
					num2 = Mathf.Max(0, num2 - GetLessDamagePerTarget());
				}
				if (flag)
				{
					flag = false;
					num2 -= GetExtraDamageFirstTarget();
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage", "damage in the cone", GetDamageAmount());
		AddTokenInt(tokens, "LessDamagePerTarget", string.Empty, m_lessDamagePerTarget);
		AddTokenInt(tokens, "Cone_MinAngle", "smallest angle of the damage cone", (int)GetConeWidthMinAngle());
		AddTokenInt(tokens, "Cone_MaxAngle", "largest angle of the damage cone", (int)GetConeWidthMaxAngle());
		AddTokenInt(tokens, "Cone_MinLength", "shortest range of the damage cone", Mathf.RoundToInt(GetConeMinLength()));
		AddTokenInt(tokens, "Cone_MaxLength", "longest range of the damage cone", Mathf.RoundToInt(GetConeMaxLength()));
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffect, "TargetHitEffect", m_targetHitEffect);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = GetConeMinLength() * Board.Get().squareSize;
		max = GetConeMaxLength() * Board.Get().squareSize;
		return true;
	}

#if SERVER
	//Added in rouges
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);
		int count = additionalData.m_abilityResults.HitActorList().Count;
		if (m_syncComp != null)
		{
			Valkyrie_SyncComponent syncComp = m_syncComp;
			syncComp.Networkm_extraAbsorbForGuard = syncComp.m_extraAbsorbForGuard + GetExtraAbsorbNextShieldBlockPerHit() * count;
			int maxExtraAbsorbNextShieldBlock = GetMaxExtraAbsorbNextShieldBlock();
			if (maxExtraAbsorbNextShieldBlock > 0)
			{
				m_syncComp.Networkm_extraAbsorbForGuard = Mathf.Min(m_syncComp.m_extraAbsorbForGuard, maxExtraAbsorbNextShieldBlock);
			}
		}
	}

	//Added in rouges
	private List<ActorData> GetHitTargets(List<AbilityTarget> targets, ActorData caster, Dictionary<ActorData, int> actorToDamage, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Vector3 vector = -1f * targets[0].AimDirection.normalized;
        AreaEffectUtils.GatherStretchConeDimensions(
            targets[0].FreePos, 
			caster.GetLoSCheckPos(), 
			GetConeMinLength(), 
			GetConeMaxLength(), 
			GetConeWidthMinAngle(), 
			GetConeWidthMaxAngle(), 
			m_coneStretchStyle, 
			out float num, 
			out float coneWidthDegrees, 
			false, 
			0, 
			-1f, 
			-1f);
        Vector3 vector2 = caster.GetLoSCheckPos() - num * Board.Get().squareSize * vector;
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vector);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
			vector2, 
			coneCenterAngleDegrees, 
			coneWidthDegrees, 
			num - GetConeBackwardOffset(), 
			GetConeBackwardOffset(), 
			true, 
			caster,
			caster.GetOtherTeams(), 
			nonActorTargetInfo);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInCone, caster.GetLoSCheckPos() - vector);
		List<ActorData> list = new List<ActorData>();
		float num2 = GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize * (GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize);
		Vector3 vector3 = vector2 - vector * GetConeBackwardOffset() * Board.Get().squareSize;
		int num3 = GetDamageAmount();
		bool flag = true;
		num3 += GetExtraDamageFirstTarget();
		foreach (ActorData actorData in actorsInCone)
		{
			if (!PenetrateLineOfSight())
			{
				BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
				if (caster.GetCurrentBoardSquare().GetLOS(
					currentBoardSquare.x, 
					currentBoardSquare.y) && 
					!BarrierManager.Get().AreAbilitiesBlocked(caster, caster.GetCurrentBoardSquare(), actorData.GetCurrentBoardSquare(), nonActorTargetInfo))
				{
					list.Add(actorData);
				}
			}
			else
			{
				list.Add(actorData);
			}
			int num4 = 0;
			if ((vector3 - actorData.GetLoSCheckPos()).sqrMagnitude <= num2)
			{
				num4 = GetExtraDamageOnSpearTip();
			}
			actorToDamage[actorData] = num3 + num4;
			if (m_syncComp == null || !m_syncComp.m_skipDamageReductionForNextStab)
			{
				num3 = Mathf.Max(0, num3 - GetLessDamagePerTarget());
			}
			if (flag)
			{
				flag = false;
				num3 -= GetExtraDamageFirstTarget();
			}
		}
		return list;
	}

	//Added in rouges
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 vector = -1f * targets[0].AimDirection.normalized;
        AreaEffectUtils.GatherStretchConeDimensions(
			targets[0].FreePos, 
			caster.GetLoSCheckPos(), 
			GetConeMinLength(), 
			GetConeMaxLength(), 
			GetConeWidthMinAngle(), 
			GetConeWidthMaxAngle(), 
			m_coneStretchStyle, 
			out float num, 
			out float num2, 
			false, 
			0, 
			-1f, 
			-1f);
		float num3 = num * Board.Get().squareSize;
		float num4 = GetConeBackwardOffset() * Board.Get().squareSize;
		Vector3 vector2 = caster.GetLoSCheckPos() - num3 * vector - vector * num4;
		Vector3 vector3 = Quaternion.AngleAxis(-0.5f * num2, Vector3.up) * vector;
		Vector3 vector4 = Quaternion.AngleAxis(0.5f * num2, Vector3.up) * vector;
		Vector3 item = caster.GetLoSCheckPos() - vector * num4;
		Vector3 item2 = vector2 + vector3 * num3;
		Vector3 item3 = vector2 + vector4 * num3;
		GameObject sideProjectileSequencePrefab = m_sideProjectileSequencePrefab;
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (sideProjectileSequencePrefab != null)
		{
			BouncingShotSequence.ExtraParams extraParams = new BouncingShotSequence.ExtraParams();
			extraParams.laserTargets = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
			extraParams.segmentPts = new List<Vector3>
			{
				item2,
				vector2
			};
			extraParams.useOriginalSegmentStartPos = true;
			ServerClientUtils.SequenceStartData item4 = new ServerClientUtils.SequenceStartData(
				sideProjectileSequencePrefab, 
				caster.GetCurrentBoardSquare(), 
				null, 
				caster, 
				additionalData.m_sequenceSource, 
				extraParams.ToArray());
			list.Add(item4);
			BouncingShotSequence.ExtraParams extraParams2 = new BouncingShotSequence.ExtraParams();
			extraParams2.laserTargets = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
			extraParams2.segmentPts = new List<Vector3>
			{
				item3,
				vector2
			};
			extraParams2.useOriginalSegmentStartPos = true;
			ServerClientUtils.SequenceStartData item5 = new ServerClientUtils.SequenceStartData(
				sideProjectileSequencePrefab, 
				caster.GetCurrentBoardSquare(), 
				null, 
				caster, 
				additionalData.m_sequenceSource, 
				extraParams2.ToArray());
			list.Add(item5);
		}
		List<Sequence.IExtraSequenceParams> list2 = new List<Sequence.IExtraSequenceParams>();
		list2.Add(new BouncingShotSequence.ExtraParams
		{
			laserTargets = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>(),
			segmentPts = new List<Vector3>
			{
				item,
				vector2
			},
			destinationHitTargets = additionalData.m_abilityResults.HitActorsArray()
		});
		list2.Add(new Sequence.FxAttributeParam
		{
			m_paramTarget = Sequence.FxAttributeParam.ParamTarget.MainVfx,
			m_paramNameCode = Sequence.FxAttributeParam.ParamNameCode.LengthInSquares,
			m_paramValue = num
		});
		float paramValue = 2f * Mathf.Tan(0.5f * num2 * 0.0174532924f) * num;
		list2.Add(new Sequence.FxAttributeParam
		{
			m_paramTarget = Sequence.FxAttributeParam.ParamTarget.MainVfx,
			m_paramNameCode = Sequence.FxAttributeParam.ParamNameCode.WidthInSquares,
			m_paramValue = paramValue
		});
		ServerClientUtils.SequenceStartData item6 = new ServerClientUtils.SequenceStartData(
			m_centerProjectileSequencePrefab, 
			vector2, 
			additionalData.m_abilityResults.HitActorsArray(), 
			caster, 
			additionalData.m_sequenceSource, 
			list2.ToArray());
		list.Add(item6);
		return list;
	}

	//Added in rouges
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		foreach (ActorData actorData in GetHitTargets(targets, caster, dictionary, nonActorTargetInfo))
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			actorHitResults.SetBaseDamage(dictionary[actorData]);
			actorHitResults.AddStandardEffectInfo(GetTargetHitEffect());
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
