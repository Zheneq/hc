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
}
