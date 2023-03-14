using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class ValkyrieThrowShield : Ability
{
	[Header("-- Targeting")]
	public float m_width = 1f;
	public float m_maxDistancePerBounce = 15f;
	public float m_maxTotalDistance = 50f;
	public int m_maxBounces = 1;
	public int m_maxTargetsHit = 1;
	[Header("-- Damage")]
	public int m_damageAmount = 20;
	public int m_bonusDamagePerBounce;
	[Header("-- Knockback")]
	public float m_knockbackDistance;
	public KnockbackType m_knockbackType;
	[Header("-- Sequences")]
	public GameObject m_projectileSequence;
	private Valkyrie_SyncComponent m_syncComp;
	private AbilityMod_ValkyrieThrowShield m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Ricoshield";
		}
		m_syncComp = GetComponent<Valkyrie_SyncComponent>();
		SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxDistancePerBounce();
	}

	public float GetLaserWidth()
	{
		if (!m_abilityMod)
		{
			return m_width;
		}
		return m_abilityMod.m_widthMod.GetModifiedValue(m_width);
	}

	public float GetMaxDistancePerBounce()
	{
		return (!m_abilityMod) ? m_maxDistancePerBounce : m_abilityMod.m_maxDistancePerBounceMod.GetModifiedValue(m_maxDistancePerBounce);
	}

	public float GetMaxTotalDistance()
	{
		if (!m_abilityMod)
		{
			return m_maxTotalDistance;
		}
		return m_abilityMod.m_maxTotalDistanceMod.GetModifiedValue(m_maxTotalDistance);
	}

	public int GetMaxBounces()
	{
		if (!m_abilityMod)
		{
			return m_maxBounces;
		}
		return m_abilityMod.m_maxBouncesMod.GetModifiedValue(m_maxBounces);
	}

	public int GetMaxTargetsHit()
	{
		if (!m_abilityMod)
		{
			return m_maxTargetsHit;
		}
		return m_abilityMod.m_maxTargetsHitMod.GetModifiedValue(m_maxTargetsHit);
	}

	public bool BounceOnHitActor()
	{
		return m_abilityMod && m_abilityMod.m_bounceOnHitActorMod.GetModifiedValue(false);
	}

	public int GetBaseDamage()
	{
		return (!m_abilityMod) ? m_damageAmount : m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
	}

	public int GetBonusDamagePerBounce()
	{
		if (!m_abilityMod)
		{
			return m_bonusDamagePerBounce;
		}
		return m_abilityMod.m_bonusDamagePerBounceMod.GetModifiedValue(m_bonusDamagePerBounce);
	}

	public int GetLessDamagePerTarget()
	{
		if (!m_abilityMod)
		{
			return 0;
		}
		return m_abilityMod.m_lessDamagePerTargetMod.GetModifiedValue(0);
	}

	public float GetKnockbackDistance()
	{
		if (!m_abilityMod)
		{
			return m_knockbackDistance;
		}
		return m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance);
	}

	public float GetBonusKnockbackPerBounce()
	{
		return (!m_abilityMod) ? 0f : m_abilityMod.m_bonusKnockbackDistancePerBounceMod.GetModifiedValue(0f);
	}

	public KnockbackType GetKnockbackType()
	{
		if (!m_abilityMod)
		{
			return m_knockbackType;
		}
		return m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType);
	}

	public int GetMaxKnockbackTargets()
	{
		if (!m_abilityMod)
		{
			return 0;
		}
		return m_abilityMod.m_maxKnockbackTargetsMod.GetModifiedValue(0);
	}

	public AbilityModCooldownReduction GetCooldownReductionOnLaserHitCaster()
	{
		return (!m_abilityMod) ? null : m_abilityMod.m_cooldownReductionOnLaserHitCaster;
	}

	public int GetExtraDamage()
	{
		if (m_syncComp != null)
		{
			return m_syncComp.m_extraDamageNextShieldThrow;
		}
		return 0;
	}

	public float GetExtraKnockbackDistance(ActorData hitActor)
	{
		AbilityUtil_Targeter_BounceLaser abilityUtil_Targeter_BounceLaser = base.Targeter as AbilityUtil_Targeter_BounceLaser;
		if (abilityUtil_Targeter_BounceLaser != null)
		{
			ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = abilityUtil_Targeter_BounceLaser.GetHitActorContext();
			if (!hitActorContext.IsNullOrEmpty<AbilityUtil_Targeter_BounceLaser.HitActorContext>())
			{
				foreach (AbilityUtil_Targeter_BounceLaser.HitActorContext hitActorContext2 in hitActorContext)
				{
					if (hitActorContext2.actor == hitActor)
					{
						return GetBonusKnockbackPerBounce() * (float)hitActorContext2.segmentIndex;
					}
				}
			}
		}
		return 0f;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieThrowShield))
		{
			m_abilityMod = (abilityMod as AbilityMod_ValkyrieThrowShield);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		AbilityUtil_Targeter_BounceLaser abilityUtil_Targeter_BounceLaser = new AbilityUtil_Targeter_BounceLaser(this, GetLaserWidth(), GetMaxDistancePerBounce(), GetMaxTotalDistance(), GetMaxBounces(), GetMaxTargetsHit(), BounceOnHitActor());
		abilityUtil_Targeter_BounceLaser.InitKnockbackData(GetKnockbackDistance(), GetKnockbackType(), GetMaxKnockbackTargets(), GetExtraKnockbackDistance);
		abilityUtil_Targeter_BounceLaser.m_penetrateTargetsAndHitCaster = (GetCooldownReductionOnLaserHitCaster() != null && GetCooldownReductionOnLaserHitCaster().HasCooldownReduction());
		base.Targeter = abilityUtil_Targeter_BounceLaser;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetBaseDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = (base.Targeters[currentTargeterIndex] as AbilityUtil_Targeter_BounceLaser).GetHitActorContext();
		for (int i = 0; i < hitActorContext.Count; i++)
		{
			if (hitActorContext[i].actor == targetActor)
			{
				int num = GetBonusDamagePerBounce() * hitActorContext[i].segmentIndex;
				int value = GetBaseDamage() + num + GetExtraDamage() - i * GetLessDamagePerTarget();
				dictionary[AbilityTooltipSymbol.Damage] = value;
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxBounces", string.Empty, m_maxBounces);
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, m_maxTargetsHit);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "BonusDamagePerBounce", string.Empty, m_bonusDamagePerBounce);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Normal;
	}

#if SERVER
	//Added in rouges
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);
		if (m_syncComp != null)
		{
			m_syncComp.Networkm_extraDamageNextShieldThrow = 0;
		}
	}

	//Added in rouges
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		bool flag = GetCooldownReductionOnLaserHitCaster() != null && GetCooldownReductionOnLaserHitCaster().HasCooldownReduction();
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary = FindLaserTargets(targets[0], caster, flag, out List<Vector3> segmentPts, out List<ActorData> list2, null, out bool flag2);
		BouncingShotSequence.ExtraParams extraParams = new BouncingShotSequence.ExtraParams();
		extraParams.laserTargets = dictionary;
		extraParams.segmentPts = segmentPts;
		extraParams.segmentPts.Add(caster.GetLoSCheckPos());
		extraParams.doPositionHitOnBounce = true;
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
			m_projectileSequence,
			caster.GetCurrentBoardSquare(), 
			dictionary.Keys.ToArray<ActorData>(), 
			caster, 
			additionalData.m_sequenceSource,
			extraParams.ToArray());
		list.Add(item);
		if (flag2 && flag)
		{
			ServerClientUtils.SequenceStartData item2 = new ServerClientUtils.SequenceStartData(null, caster.GetCurrentBoardSquare(), new ActorData[]
			{
				caster
			}, caster, additionalData.m_sequenceSource, null);
			list.Add(item2);
		}
		return list;
	}

	//Added in rouges
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Dictionary<ActorData, ActorHitResults> dictionary = new Dictionary<ActorData, ActorHitResults>();
		List<Barrier> list = new List<Barrier>();
		List<List<NonActorTargetInfo>> list2 = new List<List<NonActorTargetInfo>>();
		bool flag = GetCooldownReductionOnLaserHitCaster() != null && GetCooldownReductionOnLaserHitCaster().HasCooldownReduction();
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary2 = FindLaserTargets(targets[0], caster, flag, out List<Vector3> list3, out List<ActorData> list4, list2, out bool flag2);
		float knockbackDistance = GetKnockbackDistance();
		int maxKnockbackTargets = GetMaxKnockbackTargets();
		for (int i = 0; i < list4.Count; i++)
		{
			ActorData actorData = list4[i];
			Vector3 segmentOrigin = dictionary2[actorData].m_segmentOrigin;
			int endpointIndex = dictionary2[actorData].m_endpointIndex;
			int num = GetBaseDamage();
			int num2 = GetBonusDamagePerBounce() * endpointIndex;
			num += num2 + GetExtraDamage() - i * GetLessDamagePerTarget();
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, segmentOrigin));
			actorHitResults.SetBaseDamage(num);
			actorHitResults.SetBounceCount(endpointIndex);
			if (endpointIndex > 0)
			{
				actorHitResults.SetIgnoreCoverMinDist(true);
			}
			float num3 = GetBonusKnockbackPerBounce() * (float)endpointIndex;
			if ((knockbackDistance > 0f || num3 > 0f) && (maxKnockbackTargets <= 0 || i < maxKnockbackTargets))
			{
				Vector3 aimDir = list3[endpointIndex] - segmentOrigin;
				KnockbackHitData knockbackData = new KnockbackHitData(actorData, caster, GetKnockbackType(), aimDir, segmentOrigin, knockbackDistance + num3);
				actorHitResults.AddKnockbackData(knockbackData);
			}
			dictionary[actorData] = actorHitResults;
		}
		for (int j = 0; j < list3.Count; j++)
		{
			Vector3 pos = list3[j];
			List<NonActorTargetInfo> list5 = list2[j];
			for (int k = list5.Count - 1; k >= 0; k--)
			{
				NonActorTargetInfo nonActorTargetInfo = list5[k];
				if (nonActorTargetInfo is NonActorTargetInfo_BarrierBlock)
				{
					NonActorTargetInfo_BarrierBlock nonActorTargetInfo_BarrierBlock = nonActorTargetInfo as NonActorTargetInfo_BarrierBlock;
					if (nonActorTargetInfo_BarrierBlock.m_barrier != null && !list.Contains(nonActorTargetInfo_BarrierBlock.m_barrier))
					{
						PositionHitResults posHitRes = new PositionHitResults(new PositionHitParameters(pos));
						nonActorTargetInfo_BarrierBlock.AddPositionReactionHitToAbilityResults(caster, posHitRes, abilityResults, true);
						list.Add(nonActorTargetInfo_BarrierBlock.m_barrier);
					}
					list5.RemoveAt(k);
				}
			}
		}
		foreach (List<NonActorTargetInfo> nonActorTargetInfo2 in list2)
		{
			abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo2);
		}
		foreach (ActorHitResults hitResults in dictionary.Values)
		{
			abilityResults.StoreActorHit(hitResults);
		}
		if (flag2 && flag)
		{
			ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(caster, caster.GetLoSCheckPos()));
			GetCooldownReductionOnLaserHitCaster().AppendCooldownMiscEvents(actorHitResults2, true, 0, 0);
			abilityResults.StoreActorHit(actorHitResults2);
		}
	}

	//Added in rouges
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		FindLaserTargets(targets[0], caster, false, out List<Vector3> collection, out List<ActorData> list2, null, out bool flag);
		list.AddRange(collection);
		for (int i = 0; i < list2.Count; i++)
		{
			list.Add(list2[i].GetFreePos());
		}
		for (int j = 0; j < targets.Count; j++)
		{
			list.Add(targets[j].FreePos);
		}
		return list;
	}

	//Added in rouges
	private Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> FindLaserTargets(
		AbilityTarget targeter, 
		ActorData caster, 
		bool pierceTargetsToHitCaster, 
		out List<Vector3> laserEndPoints, 
		out List<ActorData> orderedHitActors, 
		List<List<NonActorTargetInfo>> nonActorTargetInfoInSegment, 
		out bool hitCaster)
	{
		hitCaster = false;
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
        laserEndPoints = VectorUtils.CalculateBouncingLaserEndpoints(
			loSCheckPos, 
			targeter.AimDirection, 
			GetMaxDistancePerBounce(), 
			GetMaxTotalDistance(), 
			GetMaxBounces(), 
			caster, 
			GetLaserWidth(), 
			GetMaxTargetsHit(), 
			true, 
			caster.GetOtherTeams(), 
			BounceOnHitActor(), 
			out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> result, 
			out orderedHitActors, 
			nonActorTargetInfoInSegment, 
			pierceTargetsToHitCaster, 
			true);
        if (pierceTargetsToHitCaster && laserEndPoints.Count > 1)
		{
			float totalMaxDistanceInSquares = GetMaxTotalDistance() - (laserEndPoints[0] - loSCheckPos).magnitude / Board.Get().squareSize;
			Vector3 normalized = (laserEndPoints[1] - laserEndPoints[0]).normalized;
			VectorUtils.CalculateBouncingLaserEndpoints(
				laserEndPoints[0],
				normalized,
				GetMaxDistancePerBounce(),
				totalMaxDistanceInSquares,
				GetMaxBounces(),
				caster,
				m_width,
				0,
				false,
				caster.GetTeamAsList(),
				BounceOnHitActor(),
				out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> dictionary,
				out List<ActorData> list,
				null,
				false,
				false);
			hitCaster = list.Contains(caster);
		}
		return result;
	}

	//Added in rouges
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.ValkyrieStats.DamageDoneByThrowShieldAndKnockback, results.FinalDamage);
		}
	}
#endif
}
