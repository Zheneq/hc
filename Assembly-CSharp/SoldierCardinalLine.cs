// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SoldierCardinalLine : Ability
{
	[Header("-- Targeting (shape for position targeter, line width for strafe hit area --")]
	public bool m_useBothCardinalDir;
	public AbilityAreaShape m_positionShape = AbilityAreaShape.Two_x_Two;
	public float m_lineWidth = 2f;
	public bool m_penetrateLos = true;
	[Header("-- On Hit Stuff --")]
	public int m_damageAmount = 10;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Extra Damage for near center")]
	public float m_nearCenterDistThreshold;
	public int m_extraDamageForNearCenterTargets;
	[Header("-- AoE around targets --")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Three_x_Three;
	public int m_aoeDamage;
	[Header("-- Subsequent Turn Hits --")]
	public int m_numSubsequentTurns;
	public int m_damageOnSubsequentTurns;
	public StandardEffectInfo m_enemyEffectOnSubsequentTurns;
	[Header("-- Sequences --")]
	public GameObject m_projectileSequencePrefab;
	
	private AbilityMod_SoldierCardinalLine m_abilityMod;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedEnemyEffectOnSubsequentTurns;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Cardinal Line";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		ClearTargeters();
		AbilityUtil_Targeter_Shape item = new AbilityUtil_Targeter_Shape(
			this,
			GetPositionShape(),
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false,
			false,
			AbilityUtil_Targeter.AffectsActor.Never);
		Targeters.Add(item);
		AbilityUtil_Targeter_SoldierCardinalLines abilityUtil_Targeter_SoldierCardinalLines = new AbilityUtil_Targeter_SoldierCardinalLines(
			this,
			GetPositionShape(),
			GetLineWidth(),
			PenetrateLos(),
			UseBothCardinalDir(),
			GetAoeDamage() > 0,
			GetAoeShape());
		abilityUtil_Targeter_SoldierCardinalLines.SetUseMultiTargetUpdate(true);
		abilityUtil_Targeter_SoldierCardinalLines.SetAffectedGroups(true, false, false);
		Targeters.Add(abilityUtil_Targeter_SoldierCardinalLines);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	public override TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		if (targetIndex == 1)
		{
			return TargetingParadigm.Direction;
		}
		return base.GetControlpadTargetingParadigm(targetIndex);
	}

	public override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			overridePos = AreaEffectUtils.GetCenterOfShape(GetPositionShape(), targetsSoFar[0]);
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			min = 1f;
			max = 1f;
			return true;
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedEnemyEffectOnSubsequentTurns = m_abilityMod != null
			? m_abilityMod.m_enemyEffectOnSubsequentTurnsMod.GetModifiedValue(m_enemyEffectOnSubsequentTurns)
			: m_enemyEffectOnSubsequentTurns;
	}

	public bool UseBothCardinalDir()
	{
		return m_abilityMod != null
			? m_abilityMod.m_useBothCardinalDirMod.GetModifiedValue(m_useBothCardinalDir)
			: m_useBothCardinalDir;
	}

	public AbilityAreaShape GetPositionShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_positionShapeMod.GetModifiedValue(m_positionShape)
			: m_positionShape;
	}

	public float GetLineWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lineWidthMod.GetModifiedValue(m_lineWidth)
			: m_lineWidth;
	}

	public bool PenetrateLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos)
			: m_penetrateLos;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public float GetNearCenterDistThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_nearCenterDistThresholdMod.GetModifiedValue(m_nearCenterDistThreshold)
			: m_nearCenterDistThreshold;
	}

	public int GetExtraDamageForNearCenterTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForNearCenterTargetsMod.GetModifiedValue(m_extraDamageForNearCenterTargets)
			: m_extraDamageForNearCenterTargets;
	}

	public AbilityAreaShape GetAoeShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeShapeMod.GetModifiedValue(m_aoeShape)
			: m_aoeShape;
	}

	public int GetAoeDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeDamageMod.GetModifiedValue(m_aoeDamage)
			: m_aoeDamage;
	}

	public int GetNumSubsequentTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_numSubsequentTurnsMod.GetModifiedValue(m_numSubsequentTurns)
			: m_numSubsequentTurns;
	}

	public int GetDamageOnSubsequentTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageOnSubsequentTurnsMod.GetModifiedValue(m_damageOnSubsequentTurns)
			: m_damageOnSubsequentTurns;
	}

	public StandardEffectInfo GetEnemyEffectOnSubsequentTurns()
	{
		return m_cachedEnemyEffectOnSubsequentTurns ?? m_enemyEffectOnSubsequentTurns;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (currentTargeterIndex > 0
		    && currentTargeterIndex < Targeters.Count
		    && Targeters[currentTargeterIndex] is AbilityUtil_Targeter_SoldierCardinalLines targeter)
		{
			List<AbilityTooltipSubject> tooltipSubjectTypes = targeter.GetTooltipSubjectTypes(targetActor);
			if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				dictionary = new Dictionary<AbilityTooltipSymbol, int>();
				int damage = 0;
				if (targeter.m_directHitActorToCenterDist.ContainsKey(targetActor))
				{
					damage += GetDamageAmount();
					if (GetExtraDamageForNearCenterTargets() > 0
					    && targeter.m_directHitActorToCenterDist[targetActor] <= GetNearCenterDistThreshold() * Board.Get().squareSize)
					{
						damage += GetExtraDamageForNearCenterTargets();
					}
				}
				if (targeter.m_aoeHitActors.Contains(targetActor))
				{
					damage += GetAoeDamage();
				}
				dictionary[AbilityTooltipSymbol.Damage] = damage;
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SoldierCardinalLine abilityMod_SoldierCardinalLine = modAsBase as AbilityMod_SoldierCardinalLine;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "AoeDamage", string.Empty, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_aoeDamageMod.GetModifiedValue(m_aoeDamage)
			: m_aoeDamage);
		AddTokenInt(tokens, "ExtraDamageForNearCenterTargets", string.Empty, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_extraDamageForNearCenterTargetsMod.GetModifiedValue(m_extraDamageForNearCenterTargets)
			: m_extraDamageForNearCenterTargets);
		AddTokenInt(tokens, "NumSubsequentTurns", string.Empty, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_numSubsequentTurnsMod.GetModifiedValue(m_numSubsequentTurns)
			: m_numSubsequentTurns);
		AddTokenInt(tokens, "DamageOnSubsequentTurns", string.Empty, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_damageOnSubsequentTurnsMod.GetModifiedValue(m_damageOnSubsequentTurns)
			: m_damageOnSubsequentTurns);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SoldierCardinalLine != null
			? abilityMod_SoldierCardinalLine.m_enemyEffectOnSubsequentTurnsMod.GetModifiedValue(m_enemyEffectOnSubsequentTurns)
			: m_enemyEffectOnSubsequentTurns, "EnemyEffectOnSubsequentTurns", m_enemyEffectOnSubsequentTurns);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierCardinalLine))
		{
			m_abilityMod = abilityMod as AbilityMod_SoldierCardinalLine;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		GetHitActors(targets, caster, out _, out List<List<ActorData>> actorsInDirs, out List<Vector3> dirStartPosList, out List<Vector3> dirEndPosList, nonActorTargetInfo);
		for (int i = 0; i < actorsInDirs.Count; i++)
		{
			SoldierProjectilesInLineSequence.HitAreaExtraParams hitAreaExtraParams = new SoldierProjectilesInLineSequence.HitAreaExtraParams
				{
					fromPos = dirStartPosList[i],
					toPos = dirEndPosList[i],
					areaWidthInSquares = GetLineWidth()
				};
			if (i == 0
			    && additionalData.m_abilityResults.HitActorList().Contains(caster)
			    && !actorsInDirs[i].Contains(caster))
			{
				actorsInDirs[i].Add(caster);
			}
			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
				m_projectileSequencePrefab,
				caster.GetFreePos(),
				actorsInDirs[i].ToArray(),
				caster,
				additionalData.m_sequenceSource,
				hitAreaExtraParams.ToArray());
			list.Add(item);
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Dictionary<ActorData, int> hitActors = GetHitActors(
			targets, caster, out Dictionary<ActorData, Vector3> actorToHitOrigin, out _, out _, out _, nonActorTargetInfo);
		foreach (ActorData actorData in hitActors.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorToHitOrigin[actorData]));
			actorHitResults.SetBaseDamage(hitActors[actorData]);
			actorHitResults.AddStandardEffectInfo(GetEnemyHitEffect());
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (GetNumSubsequentTurns() > 0)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetPositionShape(), targets[0]);
			List<Vector3> lineDirections = GetLineDirections(targets);
			SoldierCardinalLineEffect effect = new SoldierCardinalLineEffect(
				AsEffectSource(),
				caster,
				GetNumSubsequentTurns(),
				GetLineWidth(),
				PenetrateLos(),
				centerOfShape,
				lineDirections,
				GetDamageOnSubsequentTurns(),
				GetEnemyEffectOnSubsequentTurns(),
				m_projectileSequencePrefab);
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			casterHitResults.AddEffect(effect);
			abilityResults.StoreActorHit(casterHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private Dictionary<ActorData, int> GetHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out Dictionary<ActorData, Vector3> actorToHitOrigin,
		out List<List<ActorData>> actorsInDirs,
		out List<Vector3> dirStartPosList,
		out List<Vector3> dirEndPosList,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		return GetActorToDamageStatic(
			AreaEffectUtils.GetCenterOfShape(GetPositionShape(), targets[0]),
			GetLineDirections(targets),
			caster,
			out actorToHitOrigin,
			out actorsInDirs,
			out dirStartPosList,
			out dirEndPosList,
			nonActorTargetInfo,
			GetLineWidth(),
			PenetrateLos(),
			GetDamageAmount(),
			GetNearCenterDistThreshold(),
			GetExtraDamageForNearCenterTargets(),
			GetAoeShape(),
			GetAoeDamage());
	}

	// added in rogues
	public static Dictionary<ActorData, int> GetActorToDamageStatic(
		Vector3 shapeCenter,
		List<Vector3> lineDirs,
		ActorData caster,
		out Dictionary<ActorData, Vector3> actorToHitOrigin,
		out List<List<ActorData>> actorsInDirs,
		out List<Vector3> dirStartPosList,
		out List<Vector3> dirEndPosList,
		List<NonActorTargetInfo> nonActorTargetInfo,
		float lineWidth,
		bool ignoreLos,
		int baseDamage,
		float nearCenterDist,
		int extraDamageNearCenter,
		AbilityAreaShape aoeShape,
		int aoeDamage)
	{
		actorToHitOrigin = new Dictionary<ActorData, Vector3>();
		actorsInDirs = new List<List<ActorData>>();
		dirStartPosList = new List<Vector3>();
		dirEndPosList = new List<Vector3>();
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		HashSet<ActorData> actorsInAoE = new HashSet<ActorData>();
		int maxX = Board.Get().GetMaxX();
		int maxY = Board.Get().GetMaxY();
		float num = Mathf.Max(maxX, maxY) + 10f;
		float squareSize = Board.Get().squareSize;
		float num2 = num * squareSize;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, false, true);
		for (int i = 0; i < lineDirs.Count; i++)
		{
			Vector3 vector = lineDirs[i];
			Vector3 vector2 = 0.5f * num2 * vector;
			Vector3 vector3 = shapeCenter - vector2;
			vector3.x = Mathf.Clamp(vector3.x, 0f, maxX * squareSize);
			vector3.z = Mathf.Clamp(vector3.z, 0f, maxY * squareSize);
			Vector3 vector4 = shapeCenter + vector2;
			vector4.x = Mathf.Clamp(vector4.x, -1f, maxX * squareSize + 1f);
			vector4.z = Mathf.Clamp(vector4.z, -1f, maxY * squareSize + 1f);
			List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(
				vector3, vector4, 0f, 0f, 0.5f * lineWidth, ignoreLos, caster, relevantTeams, nonActorTargetInfo);
			actorsInDirs.Add(new List<ActorData>(actorsInRadiusOfLine));
			dirStartPosList.Add(vector3);
			dirEndPosList.Add(vector4);
			foreach (ActorData actor in actorsInRadiusOfLine)
			{
				int num3 = baseDamage;
				if (extraDamageNearCenter > 0 && AreaEffectUtils.PointToLineDistance2D(
					    actor.GetFreePos(), vector3, vector4) <= nearCenterDist * Board.Get().squareSize)
				{
					num3 += extraDamageNearCenter;
				}
				if (!dictionary.ContainsKey(actor) || dictionary[actor] < num3)
				{
					dictionary[actor] = num3;
				}
				if (!actorToHitOrigin.ContainsKey(actor))
				{
					actorToHitOrigin[actor] = vector3;
				}
				else if (actor.GetActorCover().IsInCoverWrt(actorToHitOrigin[actor]))  // , out HitChanceBracketType hitChanceBracketType in rogues
				{
					actorToHitOrigin[actor] = vector3;
				}
			}
			if (aoeDamage > 0)
			{
				foreach (ActorData actor in actorsInRadiusOfLine)
				{
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
						aoeShape, actor.GetFreePos(), actor.GetCurrentBoardSquare(), ignoreLos, caster, relevantTeams, null);
					actorsInShape.Remove(actor);
					foreach (ActorData actorInShape in actorsInShape)
					{
						actorsInAoE.Add(actorInShape);
						actorsInDirs[i].Add(actorInShape);
						if (!actorToHitOrigin.ContainsKey(actorInShape))
						{
							actorToHitOrigin[actorInShape] = vector3;
						}
					}
				}
			}
		}
		foreach (ActorData actor in actorsInAoE)
		{
			if (!dictionary.ContainsKey(actor))
			{
				dictionary[actor] = aoeDamage;
			}
			else
			{
				dictionary[actor] += aoeDamage;
			}
		}
		return dictionary;
	}

	// added in rogues
	private List<Vector3> GetLineDirections(List<AbilityTarget> targets)
	{
		AbilityTarget target = targets[0];
		AbilityTarget abilityTarget = targets[1];
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetPositionShape(), target);
		Vector3 vector = abilityTarget.FreePos - centerOfShape;
		vector.y = 0f;
		Vector3 vector2 = Vector3.forward;
		if (vector.magnitude > 0.1f)
		{
			vector2 = vector.normalized;
		}
		List<Vector3> list = new List<Vector3>();
		if (UseBothCardinalDir())
		{
			float angle = 0f;
			if (vector2.x < 0f)
			{
				angle = 180f;
			}
			list.Add(VectorUtils.AngleDegreesToVector(angle));
			float angle2 = 90f;
			if (vector2.z < 0f)
			{
				angle2 = 270f;
			}
			list.Add(VectorUtils.AngleDegreesToVector(angle2));
		}
		else
		{
			Vector3 item = VectorUtils.HorizontalAngleToClosestCardinalDirection(Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector2)));
			list.Add(item);
		}
		return list;
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = base.CalcPointsOfInterestForCamera(targets, caster);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetPositionShape(), targets[0]);
		List<Vector3> lineDirections = GetLineDirections(targets);
		int maxX = Board.Get().GetMaxX();
		int maxY = Board.Get().GetMaxY();
		float squareSize = Board.Get().squareSize;
		float num = 10f * squareSize;
		for (int i = 0; i < lineDirections.Count; i++)
		{
			Vector3 vector = lineDirections[i];
			Vector3 vector2 = 0.5f * num * vector;
			Vector3 vector3 = centerOfShape - vector2;
			vector3.x = Mathf.Clamp(vector3.x, 0f, maxX * squareSize);
			vector3.z = Mathf.Clamp(vector3.z, 0f, maxY * squareSize);
			Vector3 vector4 = centerOfShape + vector2;
			vector4.x = Mathf.Clamp(vector4.x, -1f, maxX * squareSize + 1f);
			vector4.z = Mathf.Clamp(vector4.z, -1f, maxY * squareSize + 1f);
			list.Add(vector3);
			list.Add(vector4);
		}
		return list;
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster.GetTeam() != target.GetTeam())
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.SoldierStats.TargetsHitByUlt);
		}
	}
#endif
}
