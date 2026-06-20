// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RampartGrab : Ability
{
	[Header("-- On Hit Damage and Effect")]
	public int m_damageAmount = 10;
	public int m_damageAfterFirstHit;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Knockback Targeting")]
	public bool m_chooseEndPosition = true;
	public int m_maxTargets = 1;
	public float m_laserRange = 3f;
	public float m_laserWidth = 2f;
	public bool m_penetrateLos;
	[Header("-- Targeting Ranges")]
	public float m_destinationSelectRange = 1f;
	public int m_destinationAngleDegWithBack = 90;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private float m_knockbackDistance = 100f;
	private AbilityMod_RampartGrab m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Grab";
		}
		if (GetNumTargets() != 2)
		{
			Debug.LogError("Need 2 entries in Target Data");
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		ClearTargeters();
		if (ChooseEndPosition())
		{
			ClearTargeters();
			AbilityUtil_Targeter_Laser targeter1 = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserRange(), PenetrateLos(), GetMaxTargets());
			Targeters.Add(targeter1);
			AbilityUtil_Targeter_RampartGrab targeter2 = new AbilityUtil_Targeter_RampartGrab(this, AbilityAreaShape.SingleSquare, m_knockbackDistance, KnockbackType.PullToSource, GetLaserRange(), GetLaserWidth(), PenetrateLos(), GetMaxTargets());
			targeter2.SetUseMultiTargetUpdate(true);
			Targeters.Add(targeter2);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_KnockbackLaser(this, GetLaserWidth(), GetLaserRange(), PenetrateLos(), GetMaxTargets(), m_knockbackDistance, m_knockbackDistance, KnockbackType.PullToSourceActor, false);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return ChooseEndPosition() ? 2 : 1;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetDamageAfterFirstHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAfterFirstHitMod.GetModifiedValue(m_damageAfterFirstHit)
			: m_damageAfterFirstHit;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public bool ChooseEndPosition()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chooseEndPositionMod.GetModifiedValue(m_chooseEndPosition)
			: m_chooseEndPosition;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange)
			: m_laserRange;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public float GetDestinationSelectRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_destinationSelectRangeMod.GetModifiedValue(m_destinationSelectRange)
			: m_destinationSelectRange;
	}

	public int GetDestinationAngleDegWithBack()
	{
		return m_abilityMod != null
			? m_abilityMod.m_destinationAngleDegWithBackMod.GetModifiedValue(m_destinationAngleDegWithBack)
			: m_destinationAngleDegWithBack;
	}

	public int CalcDamageForOrderIndex(int hitOrder)
	{
		int damageAfterFirstHit = GetDamageAfterFirstHit();
		if (damageAfterFirstHit > 0 && hitOrder > 0)
		{
			return damageAfterFirstHit;
		}
		return GetDamageAmount();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_RampartGrab mod = modAsBase as AbilityMod_RampartGrab;
		AddTokenInt(tokens, "DamageAmount", "", mod != null
			? mod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "DamageAfterFirstHit", "", m_damageAfterFirstHit);
		AbilityMod.AddToken_EffectInfo(tokens, mod != null
			? mod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "MaxTargets", "", mod != null
			? mod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets);
		AddTokenInt(tokens, "DestinationAngleDegWithBack", "", mod != null
			? mod.m_destinationAngleDegWithBackMod.GetModifiedValue(m_destinationAngleDegWithBack)
			: m_destinationAngleDegWithBack);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0
			&& Targeter is AbilityUtil_Targeter_Laser)
		{
			List<AbilityUtil_Targeter_Laser.HitActorContext> hitActorContext = (Targeter as AbilityUtil_Targeter_Laser).GetHitActorContext();

			for (int i = 0; i < hitActorContext.Count; i++)
			{
				if (hitActorContext[i].actor == targetActor)
				{
					results.m_damage = CalcDamageForOrderIndex(i);
					break;
				}
			}
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex == 0)
		{
			return true;
		}
		BoardSquare targetPos = Board.Get().GetSquare(target.GridPos);
		if (targetPos == null || !targetPos.IsValidForGameplay())
		{
			return false;
		}
		bool result = false;
		if (targetPos != caster.GetCurrentBoardSquare())
		{
			float dist = VectorUtils.HorizontalPlaneDistInSquares(targetPos.ToVector3(), caster.GetFreePos());
			if (dist <= GetDestinationSelectRange())
			{
				Vector3 from = -1f * currentTargets[0].AimDirection;
				Vector3 to = targetPos.ToVector3() - caster.GetFreePos();
				from.y = 0f;
				to.y = 0f;
				int angle = Mathf.RoundToInt(Vector3.Angle(from, to));
				if (angle <= GetDestinationAngleDegWithBack())
				{
					result = true;
				}
			}
		}
		return result;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RampartGrab))
		{
			m_abilityMod = abilityMod as AbilityMod_RampartGrab;
		}
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out Vector3 vector, null);
		TargeterUtils.SortActorsByDistanceToPos(ref hitActors, vector);
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_castSequencePrefab, vector, hitActors.ToArray(), caster, additionalData.m_sequenceSource);
		list.Add(item);
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		KnockbackType type = KnockbackType.PullToSourceActor;
		BoardSquare boardSquare;
		if (ChooseEndPosition() && targets.Count >= 2)
		{
			boardSquare = Board.Get().GetSquare(targets[1].GridPos);
			type = KnockbackType.PullToSource;
		}
		else
		{
			boardSquare = caster.GetCurrentBoardSquare();
		}
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out Vector3 vector, nonActorTargetInfo);
		for (int i = 0; i < hitActors.Count; i++)
		{
			ActorData target = hitActors[i];
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, caster.GetFreePos()));
			int baseDamage = CalcDamageForOrderIndex(i);
			actorHitResults.SetBaseDamage(baseDamage);
			actorHitResults.AddStandardEffectInfo(GetEnemyHitEffect());
			KnockbackHitData knockbackData = new KnockbackHitData(target, caster, type, targets[0].AimDirection, boardSquare.ToVector3(), m_knockbackDistance);
			actorHitResults.AddKnockbackData(knockbackData);
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, out Vector3 endPos, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, targets[0].AimDirection, GetLaserRange(), GetLaserWidth(), caster, caster.GetOtherTeams(), PenetrateLos(), GetMaxTargets(), false, true, out laserCoords.end, nonActorTargetInfo);
		endPos = laserCoords.end;
		return actorsInLaser;
	}

	// added in rogues
	public override void OnAbilityAssistedKill(ActorData caster, ActorData target)
	{
		caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.RampartStats.GrabAssists);
	}
#endif
}
