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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					int num;
					if (!m_droneInfoComp.GetUntrackedHitEffect().m_applyEffect)
					{
						num = ((m_droneInfoComp.GetDamageOnUntracked(true) > 0) ? 1 : 0);
					}
					else
					{
						num = 1;
					}
					bool hitUntrackedTargets = (byte)num != 0;
					base.Targeter = new AbilityUtil_Targeter_TeslaPrison(this, m_wallSegmentType, m_squareCornerLength, m_squareMidsectionLength, m_prisonSides, m_prisonRadius, m_droneTracker, m_droneInfoComp.m_travelTargeterEndRadius, m_droneInfoComp.m_travelTargeterEndRadius, m_droneInfoComp.m_travelTargeterLineRadius, -1, false, m_droneInfoComp.m_targetingIgnoreLos, m_droneInfoComp.m_droneTravelHitTargets, hitUntrackedTargets);
					return;
				}
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_TeslaPrison(this, m_wallSegmentType, m_squareCornerLength, m_squareMidsectionLength, m_prisonSides, m_prisonRadius);
	}

	private void SetCachedFields()
	{
		StandardBarrierData cachedBarrierData;
		if ((bool)m_ultAbilityMod)
		{
			cachedBarrierData = m_ultAbilityMod.m_barrierDataMod.GetModifiedValue(m_prisonBarrierData);
		}
		else
		{
			cachedBarrierData = m_prisonBarrierData;
		}
		m_cachedBarrierData = cachedBarrierData;
		StandardEffectInfo cachedAdditionalEffectOnEnemiesInShape;
		if ((bool)m_ultAbilityMod)
		{
			cachedAdditionalEffectOnEnemiesInShape = m_ultAbilityMod.m_additionalEffectOnEnemiesInShapeMod.GetModifiedValue(m_additionalEffectOnEnemiesInShape);
		}
		else
		{
			cachedAdditionalEffectOnEnemiesInShape = m_additionalEffectOnEnemiesInShape;
		}
		m_cachedAdditionalEffectOnEnemiesInShape = cachedAdditionalEffectOnEnemiesInShape;
	}

	private StandardBarrierData GetPrisonBarrierData()
	{
		StandardBarrierData result;
		if (m_ultAbilityMod == null)
		{
			result = m_prisonBarrierData;
		}
		else
		{
			result = m_cachedBarrierData;
		}
		return result;
	}

	private StandardEffectInfo GetAdditionalEffectOnEnemiesInShape()
	{
		return (m_cachedAdditionalEffectOnEnemiesInShape == null) ? m_additionalEffectOnEnemiesInShape : m_cachedAdditionalEffectOnEnemiesInShape;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerTeslaPrison abilityMod_TrackerTeslaPrison = modAsBase as AbilityMod_TrackerTeslaPrison;
		StandardBarrierData standardBarrierData;
		if ((bool)abilityMod_TrackerTeslaPrison)
		{
			standardBarrierData = abilityMod_TrackerTeslaPrison.m_barrierDataMod.GetModifiedValue(m_prisonBarrierData);
		}
		else
		{
			standardBarrierData = m_prisonBarrierData;
		}
		StandardBarrierData standardBarrierData2 = standardBarrierData;
		standardBarrierData2.AddTooltipTokens(tokens, "Wall");
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_droneInfoComp != null)
		{
			if (m_moveDrone)
			{
				if (m_droneInfoComp != null)
				{
					AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_droneInfoComp.GetDamageOnTracked(true));
					m_droneInfoComp.m_droneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
					AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_droneInfoComp.GetDamageOnUntracked(true));
					m_droneInfoComp.m_untrackedDroneHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
				}
			}
			m_prisonBarrierData.m_onEnemyMovedThrough.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Quaternary);
			m_prisonBarrierData.m_onAllyMovedThrough.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = CalculateAbilityTooltipNumbers();
		for (int i = 0; i < list.Count; i++)
		{
			AbilityTooltipNumber abilityTooltipNumber = list[i];
			if (abilityTooltipNumber.m_subject == AbilityTooltipSubject.Primary)
			{
				if (abilityTooltipNumber.m_symbol == AbilityTooltipSymbol.Damage)
				{
					list[i].m_value = m_droneInfoComp.GetDamageOnTracked(true);
					continue;
				}
			}
			if (abilityTooltipNumber.m_subject == AbilityTooltipSubject.Secondary && abilityTooltipNumber.m_symbol == AbilityTooltipSymbol.Damage)
			{
				list[i].m_value = m_droneInfoComp.GetDamageOnUntracked(true);
			}
		}
		while (true)
		{
			return list;
		}
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!m_moveDrone)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		int result;
		if (boardSquareSafe != null && boardSquareSafe.IsBaselineHeight())
		{
			if (caster.GetCurrentBoardSquare() != null)
			{
				float num = m_droneInfoComp.m_targeterMaxRangeFromDrone * Board.Get().squareSize;
				float num2 = m_droneInfoComp.GetTargeterMaxRangeFromCaster(true) * Board.Get().squareSize;
				Vector3 b = caster.GetTravelBoardSquareWorldPosition();
				if (m_droneTracker.DroneIsActive())
				{
					BoardSquare boardSquare = Board.Get().GetSquare(m_droneTracker.BoardX(), m_droneTracker.BoardY());
					if (boardSquare != null)
					{
						if (boardSquare == boardSquareSafe)
						{
							return false;
						}
						b = boardSquare.ToVector3();
					}
				}
				if (!(num <= 0f))
				{
					if (!(Vector3.Distance(boardSquareSafe.ToVector3(), b) <= num))
					{
						result = 0;
						goto IL_016c;
					}
				}
				if (!(num2 <= 0f))
				{
					result = ((Vector3.Distance(boardSquareSafe.ToVector3(), caster.GetCurrentBoardSquare().ToVector3()) <= num2) ? 1 : 0);
				}
				else
				{
					result = 1;
				}
				goto IL_016c;
			}
		}
		return false;
		IL_016c:
		return (byte)result != 0;
	}

	public override List<int> _001D()
	{
		List<int> list = base._001D();
		if (m_prisonBarrierData != null)
		{
			list.Add(m_prisonBarrierData.m_onEnemyMovedThrough.m_damage);
		}
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_TrackerTeslaPrison))
		{
			return;
		}
		while (true)
		{
			m_ultAbilityMod = (abilityMod as AbilityMod_TrackerTeslaPrison);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_ultAbilityMod = null;
		Setup();
	}
}
