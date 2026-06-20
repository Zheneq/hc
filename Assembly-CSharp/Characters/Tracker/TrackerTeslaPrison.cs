// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class TrackerTeslaPrison : TrackerDrone
{
	public enum PrisonWallSegmentType
	{
		RegularPolygon,
		SquareMadeOfCornersAndMidsection
	}

	[Header("-- Wall Config and Barrier Data")]
	public PrisonWallSegmentType m_wallSegmentType = PrisonWallSegmentType.SquareMadeOfCornersAndMidsection;
	public bool m_targeterPenetrateLos = true;
	public StandardBarrierData m_prisonBarrierData;
	[Header("-- If Wall Segement Type is Square Made Of Corners and Midsection, Dimentions")]
	public int m_squareCornerLength = 1;
	public int m_squareMidsectionLength = 1;
	[Header("-- If Wall Segment Type is Regular Polygon")]
	public int m_prisonSides = 8;
	public float m_prisonRadius = 2f;
	[Header("-- Move Drone")]
	public bool m_moveDrone = true;
	[Header("-- Additional Effect to enemies in shape --")]
	public AbilityAreaShape m_additionalEffectShape = AbilityAreaShape.Three_x_Three;
	public StandardEffectInfo m_additionalEffectOnEnemiesInShape;
	[Header("-- Sequences -------------------------------------------------")]
	public bool m_createCastSequenceIfMovingDrone;
	public GameObject m_castSequencePrefab;

	private AbilityMod_TrackerTeslaPrison m_ultAbilityMod;
	private StandardBarrierData m_cachedBarrierData;
	private StandardEffectInfo m_cachedAdditionalEffectOnEnemiesInShape;

	protected override bool UseAltMovement()
	{
		return true;
	}

	public AbilityMod_TrackerTeslaPrison GetUltMod()
	{
		return m_ultAbilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Tesla Prison";
		}
		if (m_prisonSides < 3)
		{
			m_prisonSides = 4;
		}
		if (m_squareCornerLength <= 0)
		{
			m_squareCornerLength = 1;
		}
		if (m_squareMidsectionLength < 0)
		{
			m_squareMidsectionLength = 0;
		}
		m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		if (m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
			m_moveDrone = false;
		}
		m_visionProvider = GetComponent<ActorAdditionalVisionProviders>();
		if (m_visionProvider == null)
		{
			Debug.LogError("No additional vision provider component");
		}
		Setup();
		ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		SetCachedFields();
		if (m_droneInfoComp == null)
		{
			m_droneInfoComp = GetComponent<TrackerDroneInfoComponent>();
		}
		if (m_droneInfoComp == null)
		{
			Debug.LogError("No Drone Info component");
		}
		if (m_droneTracker == null)
		{
			m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		}
		if (m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
			m_moveDrone = false;
		}
		if (m_moveDrone)
		{
			bool hitUntrackedTargets = m_droneInfoComp.GetUntrackedHitEffect().m_applyEffect || m_droneInfoComp.GetDamageOnUntracked(true) > 0;
			Targeter = new AbilityUtil_Targeter_TeslaPrison(
				this, m_wallSegmentType, m_squareCornerLength, m_squareMidsectionLength, m_prisonSides, m_prisonRadius,
				m_droneTracker, m_droneInfoComp.m_travelTargeterEndRadius, m_droneInfoComp.m_travelTargeterEndRadius,
				m_droneInfoComp.m_travelTargeterLineRadius, -1, false, m_droneInfoComp.m_targetingIgnoreLos,
				m_droneInfoComp.m_droneTravelHitTargets, hitUntrackedTargets);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_TeslaPrison(this, m_wallSegmentType, m_squareCornerLength, m_squareMidsectionLength, m_prisonSides, m_prisonRadius);
		}
	}

	private void SetCachedFields()
	{
	// rogues?
//#if SERVER
//		if (m_prisonBarrierData == null)
//		{
//			m_prisonBarrierData = ScriptableObject.CreateInstance<StandardBarrierData>();
//		}
//#endif
		m_cachedBarrierData = m_ultAbilityMod != null
			? m_ultAbilityMod.m_barrierDataMod.GetModifiedValue(m_prisonBarrierData)
			: m_prisonBarrierData;
		m_cachedAdditionalEffectOnEnemiesInShape = m_ultAbilityMod != null
			? m_ultAbilityMod.m_additionalEffectOnEnemiesInShapeMod.GetModifiedValue(m_additionalEffectOnEnemiesInShape)
			: m_additionalEffectOnEnemiesInShape;
	}

	private StandardBarrierData GetPrisonBarrierData()
	{
		return m_ultAbilityMod == null
			? m_prisonBarrierData
			: m_cachedBarrierData;
	}

	private StandardEffectInfo GetAdditionalEffectOnEnemiesInShape()
	{
		return m_cachedAdditionalEffectOnEnemiesInShape != null
			? m_cachedAdditionalEffectOnEnemiesInShape
			: m_additionalEffectOnEnemiesInShape;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerTeslaPrison mod = modAsBase as AbilityMod_TrackerTeslaPrison;
		StandardBarrierData barrierData = mod != null
			? mod.m_barrierDataMod.GetModifiedValue(m_prisonBarrierData)
			: m_prisonBarrierData;
		barrierData.AddTooltipTokens(tokens, "Wall");
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_droneInfoComp != null)
		{
			if (m_moveDrone && m_droneInfoComp != null)
			{
				AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_droneInfoComp.GetDamageOnTracked(true));
				m_droneInfoComp.m_droneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
				AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_droneInfoComp.GetDamageOnUntracked(true));
				m_droneInfoComp.m_untrackedDroneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
			}
			m_prisonBarrierData.m_onEnemyMovedThrough.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Quaternary);
			m_prisonBarrierData.m_onAllyMovedThrough.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = CalculateAbilityTooltipNumbers();
		foreach (AbilityTooltipNumber abilityTooltipNumber in list)
		{
			if (abilityTooltipNumber.m_subject == AbilityTooltipSubject.Primary
				&& abilityTooltipNumber.m_symbol == AbilityTooltipSymbol.Damage)
			{
				abilityTooltipNumber.m_value = m_droneInfoComp.GetDamageOnTracked(true);
			}
			else if (abilityTooltipNumber.m_subject == AbilityTooltipSubject.Secondary
				&& abilityTooltipNumber.m_symbol == AbilityTooltipSymbol.Damage)
			{
				abilityTooltipNumber.m_value = m_droneInfoComp.GetDamageOnUntracked(true);
			}
		}
		return list;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!m_moveDrone)
		{
			return true;
		}
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null
			|| !targetSquare.IsValidForGameplay()
			|| caster.GetCurrentBoardSquare() == null)
		{
			return false;
		}
		float maxMoveDist = m_droneInfoComp.m_targeterMaxRangeFromDrone * Board.Get().squareSize;
		float maxDistFromCaster = m_droneInfoComp.GetTargeterMaxRangeFromCaster(true) * Board.Get().squareSize;
		Vector3 startPos = caster.GetFreePos();
		if (m_droneTracker.DroneIsActive())
		{
			BoardSquare dronePos = Board.Get().GetSquareFromIndex(m_droneTracker.BoardX(), m_droneTracker.BoardY());
			if (dronePos != null)
			{
				if (targetSquare == dronePos)
				{
					return false;
				}
				startPos = dronePos.ToVector3();
			}
		}
		Vector3 casterPos = caster.GetCurrentBoardSquare().ToVector3();
		return (maxMoveDist <= 0f || Vector3.Distance(targetSquare.ToVector3(), startPos) <= maxMoveDist)
			&& (maxDistFromCaster <= 0f || Vector3.Distance(targetSquare.ToVector3(), casterPos) <= maxDistFromCaster);
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = base.Debug_GetExpectedNumbersInTooltip();
		if (m_prisonBarrierData != null)
		{
			list.Add(m_prisonBarrierData.m_onEnemyMovedThrough.m_damage);
		}
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TrackerTeslaPrison))
		{
			m_ultAbilityMod = (abilityMod as AbilityMod_TrackerTeslaPrison);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_ultAbilityMod = null;
		Setup();
	}

#if SERVER
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> abilityRunSequenceStartDataList = base.GetAbilityRunSequenceStartDataList(targets, caster, additionalData);
		bool flag = m_ultAbilityMod != null && m_ultAbilityMod.m_groundEffectInfoInCage.m_applyGroundEffect;
		ActorData[] targetActorArray = null;
		if (flag)
		{
			targetActorArray = new ActorData[]
			{
				caster
			};
		}
		if (!m_moveDrone || m_createCastSequenceIfMovingDrone || flag)
		{
			abilityRunSequenceStartDataList.Add(new ServerClientUtils.SequenceStartData(m_castSequencePrefab, caster.GetCurrentBoardSquare(), targetActorArray, caster, additionalData.m_sequenceSource, null));
		}
		return abilityRunSequenceStartDataList;
	}
#endif

#if SERVER
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_moveDrone)
		{
			caster.GetAbilityData().ReinitAbilityInteractionData(caster.GetAbilityData().GetAbilityOfType(typeof(TrackerDrone)));
			base.Run(targets, caster, additionalData);
		}
	}
#endif

#if SERVER
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		if (m_moveDrone)
		{
			base.GatherAbilityResults(targets, caster, ref abilityResults);
		}
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		Vector3 vector = (square != null) ? square.ToVector3() : targets[0].FreePos;
		float squareSize = Board.Get().squareSize;
		StandardEffectInfo additionalEffectOnEnemiesInShape = GetAdditionalEffectOnEnemiesInShape();
		if (additionalEffectOnEnemiesInShape != null && additionalEffectOnEnemiesInShape.m_applyEffect)
		{
			foreach (ActorData actorData in abilityResults.m_actorToHitResults.Keys)
			{
				if (actorData.GetTeam() != caster.GetTeam() && AreaEffectUtils.IsSquareInShape(actorData.GetCurrentBoardSquare(), m_additionalEffectShape, vector, square, true, caster))
				{
					StandardActorEffect effect = new StandardActorEffect(AsEffectSource(), actorData.GetCurrentBoardSquare(), actorData, caster, additionalEffectOnEnemiesInShape.m_effectData);
					abilityResults.m_actorToHitResults[actorData].AddEffect(effect);
				}
			}
		}
		PositionHitResults positionHitResults = abilityResults.GetStoredPositionHit(vector);
		bool flag = false;
		if (positionHitResults == null)
		{
			positionHitResults = new PositionHitResults(new PositionHitParameters(vector));
			flag = true;
		}
		StandardBarrierData prisonBarrierData = GetPrisonBarrierData();
		List<Barrier> list = new List<Barrier>();
		if (m_wallSegmentType == PrisonWallSegmentType.SquareMadeOfCornersAndMidsection)
		{
			float num = 0.05f;
			List<List<BarrierPoseInfo>> list2;
			List<BarrierPoseInfo> list3;
			BarrierPoseInfo.GetBarrierPosesForSquaresMadeOfCornerAndMidsection(square, (float)m_squareCornerLength - 0.05f, (float)m_squareMidsectionLength, -num, out list2, out list3);
			float width = prisonBarrierData.m_width;
			foreach (List<BarrierPoseInfo> list4 in list2)
			{
				List<Barrier> list5 = new List<Barrier>();
				foreach (BarrierPoseInfo barrierPoseInfo in list4)
				{
					prisonBarrierData.m_width = barrierPoseInfo.widthInWorld / squareSize;
					Barrier barrier = new Barrier(m_abilityName, barrierPoseInfo.midpoint, barrierPoseInfo.facingDirection, caster, prisonBarrierData, true, null, Team.Invalid, null);
					barrier.SetSourceAbility(this);
					positionHitResults.AddBarrier(barrier);
					list5.Add(barrier);
					list.Add(barrier);
				}
				LinkedBarrierData linkData = new LinkedBarrierData();
				BarrierManager.Get().LinkBarriers(list5, linkData);
			}
			foreach (BarrierPoseInfo barrierPoseInfo2 in list3)
			{
				prisonBarrierData.m_width = barrierPoseInfo2.widthInWorld / squareSize;
				Barrier barrier2 = new Barrier(m_abilityName, barrierPoseInfo2.midpoint, barrierPoseInfo2.facingDirection, caster, prisonBarrierData, true, null, Team.Invalid, null);
				barrier2.SetSourceAbility(this);
				positionHitResults.AddBarrier(barrier2);
				list.Add(barrier2);
			}
			prisonBarrierData.m_width = width;
		}
		else
		{
			List<BarrierPoseInfo> barrierPosesForRegularPolygon = BarrierPoseInfo.GetBarrierPosesForRegularPolygon(vector, m_prisonSides, m_prisonRadius * squareSize, 0f);
			if (barrierPosesForRegularPolygon != null)
			{
				float width2 = prisonBarrierData.m_width;
				prisonBarrierData.m_width = barrierPosesForRegularPolygon[0].widthInWorld / squareSize;
				for (int i = 0; i < barrierPosesForRegularPolygon.Count; i++)
				{
					Barrier barrier3 = new Barrier(m_abilityName, barrierPosesForRegularPolygon[i].midpoint, barrierPosesForRegularPolygon[i].facingDirection, caster, prisonBarrierData, true, null, Team.Invalid, null);
					barrier3.SetSourceAbility(this);
					positionHitResults.AddBarrier(barrier3);
					list.Add(barrier3);
				}
				prisonBarrierData.m_width = width2;
			}
		}
		if (flag)
		{
			abilityResults.StorePositionHit(positionHitResults);
		}
		TrackerTeslaPrison.BarrierSet_TrackerTeslaPrison barrierSetHandler = new TrackerTeslaPrison.BarrierSet_TrackerTeslaPrison(list);
		for (int j = 0; j < list.Count; j++)
		{
			list[j].SetBarrierSetHandler(barrierSetHandler);
		}
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		if (m_ultAbilityMod != null && m_ultAbilityMod.m_groundEffectInfoInCage.m_applyGroundEffect)
		{
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			GroundEffectField groundEffectData = m_ultAbilityMod.m_groundEffectInfoInCage.m_groundEffectData;
			StandardGroundEffect standardGroundEffect = new StandardGroundEffect(AsEffectSource(), square, targets[0].FreePos, null, caster, groundEffectData);
			List<ActorData> affectableActorsInField = m_ultAbilityMod.m_groundEffectInfoInCage.GetAffectableActorsInField(targets[0], caster, nonActorTargetInfo);
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(groundEffectData.shape, targets[0]);
			foreach (ActorData actorData2 in affectableActorsInField)
			{
				ActorHitResults hitResults = new ActorHitResults(new ActorHitParameters(actorData2, centerOfShape));
				m_ultAbilityMod.m_groundEffectInfoInCage.SetupActorHitResult(ref hitResults, caster, actorData2.GetCurrentBoardSquare(), 1);
				abilityResults.StoreActorHit(hitResults);
			}
			standardGroundEffect.AddToActorsHitThisTurn(affectableActorsInField);
			actorHitResults.AddEffect(standardGroundEffect);
			abilityResults.StoreActorHit(actorHitResults);
			abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}
#endif

#if SERVER
	public class BarrierSet_TrackerTeslaPrison : BarrierSet
	{
		private List<Barrier> m_barriers;

		public BarrierSet_TrackerTeslaPrison(List<Barrier> barriers)
		{
			m_barriers = new List<Barrier>(barriers);
		}

		public override bool ShouldAddGameplayHit(Barrier barrier, ActorData mover)
		{
			if (m_barriers != null && m_barriers.Contains(barrier))
			{
				for (int i = 0; i < m_barriers.Count; i++)
				{
					if (barrier != m_barriers[i] && m_barriers[i].ActorMovedThroughThisTurn(mover))
					{
						return false;
					}
				}
			}
			return true;
		}

		public override void OnBarrierEnd(Barrier endingBarrier)
		{
			if (m_barriers != null && m_barriers.Contains(endingBarrier))
			{
				m_barriers.Remove(endingBarrier);
			}
		}
	}
#endif
}
