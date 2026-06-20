// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class ExoChainGunSpray : Ability
{
	[Space(20f)]
	[Header("-- Cone to Sweep Across")]
	public float m_coneBackwardOffset;
	public float m_coneLength = 2.5f;
	public float m_minConeAngle = 10f;
	public float m_maxConeAngle = 90f;
	public int m_maxTargets;
	public float m_multiClickConeEdgeWidth = 0.2f;
	public bool m_penetrateLineOfSight;
	[Header("-- Damage and Effects")]
	public int m_minDamageAmount = 15;
	public int m_maxDamageAmount = 35;
	public StandardEffectInfo m_targetHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Old Painless";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (GetExpectedNumberOfTargeters() <= 1)
		{
			Log.Error("ExoChainGunSpray requires 2 targeters, please update the Target Data array in the character prefab.");
			return;
		}
		
		ClearTargeters();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_SweepMultiClickCone targeter = new AbilityUtil_Targeter_SweepMultiClickCone(
				this,
				GetMinConeAngle(),
				GetMaxConeAngle(),
				GetConeLength(),
				m_coneBackwardOffset,
				m_multiClickConeEdgeWidth,
				m_penetrateLineOfSight,
				GetMaxTargets());
			targeter.SetAffectedGroups(true, false, false);
			Targeters.Add(targeter);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, GetNumTargets());
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		foreach (AbilityTarget target in targets)
		{
			list.Add(target.FreePos);
		}
		return list;
	}

	private float GetMinConeAngle()
	{
		return m_minConeAngle;
	}

	private float GetMaxConeAngle()
	{
		return m_maxConeAngle;
	}

	private float GetConeLength()
	{
		return m_coneLength;
	}

	private int GetMaxTargets()
	{
		return m_maxTargets;
	}

	private int GetMinDamageAmount()
	{
		return m_minDamageAmount;
	}

	private int GetMaxDamageAmount()
	{
		return m_maxDamageAmount;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetMinDamageAmount());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (currentTargeterIndex > 0)
		{
			AbilityUtil_Targeter_SweepMultiClickCone abilityUtil_Targeter_SweepMultiClickCone = Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SweepMultiClickCone;
			int damageForSweepAngle = GetDamageForSweepAngle(abilityUtil_Targeter_SweepMultiClickCone.sweepAngle);
			AddNameplateValueForSingleHit(ref symbolToValue, abilityUtil_Targeter_SweepMultiClickCone, targetActor, damageForSweepAngle);
		}
		return symbolToValue;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Min_Damage", "damage dealt by the widest possible cone", GetMinDamageAmount());
		AddTokenInt(tokens, "Max_Damage", "damage dealt by the narrowest possible cone", GetMaxDamageAmount());
		AddTokenInt(tokens, "Min_Cone_Angle", "the narrowest cone", (int)GetMinConeAngle());
		AddTokenInt(tokens, "Max_Cone_Angle", "the widest cone", (int)GetMaxConeAngle());
		AddTokenInt(tokens, "Cone_Length", "range of the damage cone", Mathf.RoundToInt(GetConeLength()));
	}

	private Vector3 GetTargeterClampedAimDirection(Vector3 startAimDirection, Vector3 endAimDirection, out float sweepAngle, out float coneCenterDegrees)
	{
		float num = VectorUtils.HorizontalAngle_Deg(startAimDirection);
		sweepAngle = Vector3.Angle(startAimDirection, endAimDirection);
		float maxConeAngle = GetMaxConeAngle();
		float minConeAngle = GetMinConeAngle();
		if (maxConeAngle > 0f && sweepAngle > maxConeAngle)
		{
			endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, (float)Math.PI / 180f * (sweepAngle - maxConeAngle), 0f);
			sweepAngle = maxConeAngle;
		}
		else if (minConeAngle > 0f && sweepAngle < minConeAngle)
		{
			endAimDirection = Vector3.RotateTowards(endAimDirection, startAimDirection, (float)Math.PI / 180f * (sweepAngle - minConeAngle), 0f);
			sweepAngle = minConeAngle;
		}
		coneCenterDegrees = num;
		Vector3 vector = Vector3.Cross(startAimDirection, endAimDirection);
		if (vector.y > 0f)
		{
			coneCenterDegrees -= sweepAngle * 0.5f;
		}
		else
		{
			coneCenterDegrees += sweepAngle * 0.5f;
		}
		return endAimDirection;
	}

	private int GetDamageForSweepAngle(float sweepAngle)
	{
		float damageRange = GetMaxDamageAmount() - GetMinDamageAmount();
		float angleRange = GetMaxConeAngle() - GetMinConeAngle();
		float share = 1f - (sweepAngle - GetMinConeAngle()) / angleRange;
		share = Mathf.Clamp(share, 0f, 1f);
		return GetMinDamageAmount() + Mathf.RoundToInt(damageRange * share);
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		GetSweepHitActorsAndDamage(targets, caster, out _, out _, out List<ActorData> sequenceHitActors, null);
		BlasterStretchConeSequence.ExtraParams extraParams = new BlasterStretchConeSequence.ExtraParams();
		GetTargeterClampedAimDirection(
			targets[0].AimDirection,
			targets[targets.Count - 1].AimDirection,
			out extraParams.angleInDegrees,
			out extraParams.forwardAngle);
		extraParams.lengthInSquares = GetConeLength();
		return new ServerClientUtils.SequenceStartData(
			m_sequencePrefab,
			caster.GetCurrentBoardSquare(),
			sequenceHitActors.ToArray(),
			caster,
			additionalData.m_sequenceSource,
			new Sequence.IExtraSequenceParams[] { extraParams }
		);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Dictionary<ActorData, int> sweepHitActorsAndDamage = GetSweepHitActorsAndDamage(
			targets,
			caster,
			out Dictionary<ActorData, Vector3> dictionary,
			out _,
			out _,
			nonActorTargetInfo);
		foreach (ActorData actorData in sweepHitActorsAndDamage.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, dictionary[actorData]));
			actorHitResults.SetBaseDamage(sweepHitActorsAndDamage[actorData]);
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	public Dictionary<ActorData, int> GetSweepHitActorsAndDamage(
		List<AbilityTarget> targets,
		ActorData caster,
		out Dictionary<ActorData, Vector3> damageOrigins,
		out List<Vector3> sweepEndPoints,
		out List<ActorData> sequenceHitActors,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		damageOrigins = new Dictionary<ActorData, Vector3>();
		sweepEndPoints = new List<Vector3>();
		sequenceHitActors = new List<ActorData>();
		float sweepAngle = 5f;
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			sweepEndPoints.Add(targets[i].FreePos);
		}
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
		if (targets.Count > 1)
		{
			GetTargeterClampedAimDirection(
				targets[0].AimDirection,
				targets[targets.Count - 1].AimDirection,
				out sweepAngle,
				out coneCenterAngleDegrees);
		}
		List<Team> otherTeams = caster.GetOtherTeams();
		Vector3 aimDirection = targets[0].AimDirection;
		Vector3 aimDirection2 = targets[targets.Count - 1].AimDirection;
		List<NonActorTargetInfo> nonActorTargets = new List<NonActorTargetInfo>();
		List<NonActorTargetInfo> nonActorTargets2 = new List<NonActorTargetInfo>();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			caster.GetLoSCheckPos(),
			aimDirection,
			GetConeLength(),
			m_multiClickConeEdgeWidth,
			caster,
			otherTeams,
			m_penetrateLineOfSight,
			0,
			m_penetrateLineOfSight,
			true,
			out _,
			nonActorTargets);
		List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(
			caster.GetLoSCheckPos(),
			aimDirection2,
			GetConeLength(),
			m_multiClickConeEdgeWidth,
			caster,
			otherTeams,
			m_penetrateLineOfSight,
			0,
			m_penetrateLineOfSight,
			true,
			out _,
			nonActorTargets2);
		sequenceHitActors = AreaEffectUtils.GetActorsInCone(
			caster.GetFreePos(),
			coneCenterAngleDegrees,
			sweepAngle,
			GetConeLength(),
			m_coneBackwardOffset,
			m_penetrateLineOfSight,
			caster,
			otherTeams,
			nonActorTargetInfo);
		foreach (ActorData item in actorsInLaser)
		{
			if (!sequenceHitActors.Contains(item))
			{
				sequenceHitActors.Add(item);
			}
		}
		foreach (ActorData item2 in actorsInLaser2)
		{
			if (!sequenceHitActors.Contains(item2))
			{
				sequenceHitActors.Add(item2);
			}
		}
		int damageForSweepAngle = GetDamageForSweepAngle(sweepAngle);
		foreach (ActorData key in sequenceHitActors)
		{
			dictionary[key] = damageForSweepAngle;
			damageOrigins[key] = caster.GetFreePos();
		}
		return dictionary;
	}
#endif
}
