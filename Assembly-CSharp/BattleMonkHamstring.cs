// ROGUES
// SERVER
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class BattleMonkHamstring : Ability
{
	[Header("-- Laser")]
	public int m_laserDamageAmount = 5;
	public int m_damageAfterFirstHit;
	public LaserTargetingInfo m_laserInfo;
	public StandardEffectInfo m_laserHitEffect;
	[Header("-- Explosion")]
	public bool m_explodeOnActorHit;
	public AbilityAreaShape m_explodeShape = AbilityAreaShape.Three_x_Three;
	public int m_explosionDamageAmount;
	public StandardEffectInfo m_explosionHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSelfSequencePrefab;
	public GameObject m_projectileSequencePrefab;

	private AbilityMod_BattleMonkHamstring m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Hamstring";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (ShouldExplodeOnActorHit())
		{
			Targeter = new AbilityUtil_Targeter_LaserWithShape(
				this,
				GetExplodeShape(),
				GetLaserWidth(),
				GetLaserRange(),
				m_laserInfo.penetrateLos,
				GetMaxTargets(),
				m_laserInfo.affectsAllies,
				m_laserInfo.affectsCaster,
				m_laserInfo.affectsEnemies);
		}
		else if (GetMaxBounces() > 0)
		{
			Targeter = new AbilityUtil_Targeter_BounceLaser(
				this,
				GetLaserWidth(),
				GetDistancePerBounce(),
				GetLaserRange(),
				GetMaxBounces(),
				GetMaxTargets(),
				false);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_Laser(
				this,
				GetLaserWidth(),
				GetLaserRange(),
				m_laserInfo.penetrateLos,
				GetMaxTargets(),
				m_laserInfo.affectsAllies,
				m_laserInfo.affectsCaster);
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	public int GetLaserDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public int GetDamageAfterFirstHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAfterFirstHitMod.GetModifiedValue(m_damageAfterFirstHit)
			: m_damageAfterFirstHit;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_widthMod.GetModifiedValue(m_laserInfo.width)
			: m_laserInfo.width;
	}

	public float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_rangeMod.GetModifiedValue(m_laserInfo.range)
			: m_laserInfo.range;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetMod.GetModifiedValue(m_laserInfo.maxTargets)
			: m_laserInfo.maxTargets;
	}

	public int GetMaxBounces()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxBounces.GetModifiedValue(0)
			: 0;
	}

	public float GetDistancePerBounce()
	{
		return m_abilityMod != null
			? m_abilityMod.m_distancePerBounce.GetModifiedValue(0f)
			: 0f;
	}

	public GameObject GetProjectileSequence()
	{
		return m_abilityMod != null
			? m_abilityMod.m_projectileSequencePrefab.GetModifiedValue(m_projectileSequencePrefab)
			: m_projectileSequencePrefab;
	}

	public bool ShouldExplodeOnActorHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explodeOnActorHitMod.GetModifiedValue(m_explodeOnActorHit)
			: m_explodeOnActorHit;
	}

	public int GetExplosionDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount;
	}

	public AbilityAreaShape GetExplodeShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explodeShapeMod.GetModifiedValue(m_explodeShape)
			: m_explodeShape;
	}

	public int CalcDamageForOrderIndex(int hitOrder)
	{
		int damageAfterFirstHit = GetDamageAfterFirstHit();
		if (damageAfterFirstHit > 0 && hitOrder > 0)
		{
			return damageAfterFirstHit;
		}
		return GetLaserDamage();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		if (m_explodeOnActorHit)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_explosionDamageAmount);
			m_explosionHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		}
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetLaserDamage());
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		if (ShouldExplodeOnActorHit())
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetExplosionDamage());
			m_explosionHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		}
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) <= 0)
		{
			if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Secondary) > 0)
			{
				results.m_damage = GetExplosionDamage();
			}
		}
		else
		{
			if (Targeter is AbilityUtil_Targeter_LaserWithShape)
			{
				List<ActorData> lastLaserHitActors = (Targeter as AbilityUtil_Targeter_LaserWithShape).GetLastLaserHitActors();
				for (int i = 0; i < lastLaserHitActors.Count; i++)
				{
					if (targetActor == lastLaserHitActors[i])
					{
						results.m_damage = CalcDamageForOrderIndex(i);
						break;
					}
				}
			}
			else if (Targeter is AbilityUtil_Targeter_BounceLaser)
			{
				ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = (Targeter as AbilityUtil_Targeter_BounceLaser).GetHitActorContext();
				for (int i = 0; i < hitActorContext.Count; i++)
				{
					if (hitActorContext[i].actor == targetActor)
					{
						results.m_damage = CalcDamageForOrderIndex(i);
						break;
					}
				}
			}
			else if (Targeter is AbilityUtil_Targeter_Laser)
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
		}

		return true;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			dictionary[AbilityTooltipSymbol.Damage] = GetLaserDamage();
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
		{
			dictionary[AbilityTooltipSymbol.Damage] = GetExplosionDamage();
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkHamstring abilityMod_BattleMonkHamstring = modAsBase as AbilityMod_BattleMonkHamstring;
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, abilityMod_BattleMonkHamstring != null
			? abilityMod_BattleMonkHamstring.m_laserDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BattleMonkHamstring != null && abilityMod_BattleMonkHamstring.m_useLaserHitEffectOverride
			? abilityMod_BattleMonkHamstring.m_laserHitEffectOverride
			: m_laserHitEffect, "LaserHitEffect", m_laserHitEffect);
		AddTokenInt(tokens, "ExplosionDamageAmount", string.Empty, abilityMod_BattleMonkHamstring != null
			? abilityMod_BattleMonkHamstring.m_explosionDamageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BattleMonkHamstring != null && abilityMod_BattleMonkHamstring.m_useExplosionHitEffectOverride
			? abilityMod_BattleMonkHamstring.m_explosionHitEffectOverride
			: m_explosionHitEffect, "ExplosionHitEffect", m_explosionHitEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BattleMonkHamstring))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_BattleMonkHamstring;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		if (GetMaxBounces() > 0)
		{
			List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, m_laserInfo.affectsAllies, m_laserInfo.affectsEnemies);
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> laserTargets = FindBouncingLaserTargets(
				targets[0],
				caster,
				relevantTeams,
				out var segmentPts,
				out _,
				null);
			list.Add(new ServerClientUtils.SequenceStartData(
				GetProjectileSequence(),
				caster.GetCurrentBoardSquare(),
				laserTargets.Keys.ToArray(),
				caster,
				additionalData.m_sequenceSource,
				new BouncingShotSequence.ExtraParams
				{
					laserTargets = laserTargets,
					segmentPts = segmentPts
				}.ToArray()));
		}
		else
		{
			GetLaserHitActors(targets, caster, out VectorUtils.LaserCoords laserCoordinates, out _, null);
			if (GetMaxTargets() <= 0)
			{
				float maxDistanceInWorld = GetLaserRange() * Board.Get().squareSize;
				float widthInWorld = GetLaserWidth() * Board.Get().squareSize;
				laserCoordinates = VectorUtils.GetLaserCoordinates(
					caster.GetLoSCheckPos(),
					targets[0].AimDirection,
					maxDistanceInWorld,
					widthInWorld,
					m_laserInfo.penetrateLos,
					caster);
			}

			list.Add(new ServerClientUtils.SequenceStartData(
				m_projectileSequencePrefab,
				laserCoordinates.end,
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource));
		}
		if (m_castSelfSequencePrefab != null)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_castSelfSequencePrefab,
				caster.GetCurrentBoardSquare(),
				new ActorData[0],
				caster,
				additionalData.m_sequenceSource));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> laserHitActors = GetLaserHitActors(targets, caster, out _, out var bouncingLaserInfo, nonActorTargetInfo);
		for (int i = 0; i < laserHitActors.Count; i++)
		{
			Vector3 damageOrigin = caster.GetFreePos();
			
			// custom
			if (bouncingLaserInfo.TryGetValue(laserHitActors[i], out AreaEffectUtils.BouncingLaserInfo hitInfo))
			{
				damageOrigin = hitInfo.m_segmentOrigin;
			}
			// end custom
			
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(laserHitActors[i], damageOrigin));
			int addAmount = CalcDamageForOrderIndex(i);
			actorHitResults.AddBaseDamage(addAmount);
			if (m_abilityMod != null && m_abilityMod.m_useLaserHitEffectOverride)
			{
				actorHitResults.AddStandardEffectInfo(m_abilityMod.m_laserHitEffectOverride);
			}
			else
			{
				actorHitResults.AddStandardEffectInfo(m_laserHitEffect);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (laserHitActors.Count > 0 && ShouldExplodeOnActorHit())
		{
			ActorData laserHitActor = laserHitActors[laserHitActors.Count - 1];
			foreach (ActorData explosionHitActor in GetExplosionHitActors(targets, caster, nonActorTargetInfo))
			{
				if (!laserHitActors.Contains(explosionHitActor))
				{
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(explosionHitActor, laserHitActor.GetFreePos()));
					actorHitResults.SetBaseDamage(GetExplosionDamage());
					if (m_abilityMod != null && m_abilityMod.m_useExplosionHitEffectOverride)
					{
						actorHitResults.AddStandardEffectInfo(m_abilityMod.m_explosionHitEffectOverride);
					}
					else
					{
						actorHitResults.AddStandardEffectInfo(m_explosionHitEffect);
					}
					abilityResults.StoreActorHit(actorHitResults);
				}
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetLaserHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out VectorUtils.LaserCoords endPoints,
		out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bouncingLaserInfo, // custom
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, m_laserInfo.affectsAllies, m_laserInfo.affectsEnemies);
		if (GetMaxBounces() > 0)
		{
			List<List<NonActorTargetInfo>> nonActorTargetInfos = new List<List<NonActorTargetInfo>>();
			bouncingLaserInfo =  // custom
				FindBouncingLaserTargets(
				targets[0],
				caster,
				relevantTeams,
				out List<Vector3> laserEndPoints,
				out List<ActorData> laserHitActors,
				nonActorTargetInfos);
			if (nonActorTargetInfo != null)
			{
				foreach (List<NonActorTargetInfo> collection in nonActorTargetInfos)
				{
					nonActorTargetInfo.AddRange(collection);
				}
			}
			endPoints = default(VectorUtils.LaserCoords);
			endPoints.start = caster.GetFreePos();
			if (laserEndPoints.Count > 0)
			{
				endPoints.end = laserEndPoints[0];
			}
			return laserHitActors;
		}
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			GetLaserRange(),
			GetLaserWidth(),
			caster,
			relevantTeams,
			m_laserInfo.penetrateLos,
			GetMaxTargets(),
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		endPoints = laserCoords;
		bouncingLaserInfo = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>(); // custom
		return actorsInLaser;
	}

	// added in rogues
	private Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> FindBouncingLaserTargets(
		AbilityTarget targeter,
		ActorData caster,
		List<Team> affectedTeams,
		out List<Vector3> laserEndPoints,
		out List<ActorData> orderedHitActors,
		List<List<NonActorTargetInfo>> nonActorTargetInfoInSegments)
	{
		laserEndPoints = VectorUtils.CalculateBouncingLaserEndpoints(
			caster.GetLoSCheckPos(),
			targeter.AimDirection,
			GetDistancePerBounce(),
			GetLaserRange(),
			GetMaxBounces(),
			caster,
			GetLaserWidth(),
			GetMaxTargets(),
			true,
			affectedTeams,
			false,
			out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> result,
			out orderedHitActors,
			nonActorTargetInfoInSegments);
		return result;
	}

	// added in rogues
	private List<ActorData> GetExplosionHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> laserHitActors = GetLaserHitActors(targets, caster, out _, out _, null);
		List<ActorData> result;
		if (laserHitActors.Count > 0)
		{
			BoardSquare currentBoardSquare = laserHitActors[laserHitActors.Count - 1].GetCurrentBoardSquare();
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetExplodeShape(), currentBoardSquare.ToVector3(), currentBoardSquare);
			result = AreaEffectUtils.GetActorsInShape(
				GetExplodeShape(),
				centerOfShape,
				currentBoardSquare,
				false,
				caster,
				caster.GetOtherTeams(),
				nonActorTargetInfo);
		}
		else
		{
			result = new List<ActorData>();
		}
		return result;
	}

	// added in rogues
	public override void OnAbilityAssistedKill(ActorData caster, ActorData target)
	{
		caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.BattleMonkStats.AssistsWithRoot);
	}
#endif
}
