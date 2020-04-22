using System.Collections.Generic;
using UnityEngine;

public class GremlinsMultiTargeterBasicAttack : Ability
{
	[Header("-- Targeting")]
	public AbilityAreaShape m_bombShape = AbilityAreaShape.Three_x_Three;

	public bool m_penetrateLos;

	public float m_minDistanceBetweenBombs = 1f;

	public bool m_useShapeForDeadzone;

	public AbilityAreaShape m_deadZoneShape;

	public float m_maxAngleWithFirst = 45f;

	[Header("-- Targeter Angle Indicator Config")]
	public bool m_useAngleIndicators = true;

	public float m_indicatorLineLength = 8f;

	[Header("-- Sequence -------------------------------")]
	public GameObject m_firstBombSequencePrefab;

	public GameObject m_subsequentBombSequencePrefab;

	private GremlinsLandMineInfoComponent m_bombInfoComp;

	private AbilityMod_GremlinsMultiTargeterBasicAttack m_abilityMod;

	public AbilityMod_GremlinsMultiTargeterBasicAttack GetMod()
	{
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "MultiTargeter Basic Attack";
		}
		m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		ResetTooltipAndTargetingNumbers();
		SetupTargeter();
	}

	public AbilityAreaShape GetBombShape()
	{
		AbilityAreaShape result;
		if (m_abilityMod == null)
		{
			result = m_bombShape;
		}
		else
		{
			result = m_abilityMod.m_bombShapeMod.GetModifiedValue(m_bombShape);
		}
		return result;
	}

	public float GetMinDistBetweenBombs()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_minDistBetweenBombsMod.GetModifiedValue(m_minDistanceBetweenBombs) : m_minDistanceBetweenBombs;
	}

	public bool UseShapeForDeadzone()
	{
		bool result;
		if (m_abilityMod == null)
		{
			result = m_useShapeForDeadzone;
		}
		else
		{
			result = m_abilityMod.m_useShapeForDeadzoneMod.GetModifiedValue(m_useShapeForDeadzone);
		}
		return result;
	}

	public AbilityAreaShape GetDeadzoneShape()
	{
		AbilityAreaShape result;
		if (m_abilityMod == null)
		{
			result = m_deadZoneShape;
		}
		else
		{
			result = m_abilityMod.m_deadzoneShapeMod.GetModifiedValue(m_deadZoneShape);
		}
		return result;
	}

	private int ModdedDirectHitDamagePerShot(int shotIndex)
	{
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_directHitDamagePerShot.Count > shotIndex)
			{
				if (shotIndex >= 0)
				{
					return m_abilityMod.m_directHitDamagePerShot[shotIndex].GetModifiedValue(m_bombInfoComp.m_directHitDamageAmount);
				}
			}
		}
		return m_bombInfoComp.m_directHitDamageAmount;
	}

	private float ModdedMaxAngleWithFirst()
	{
		if (m_abilityMod != null)
		{
			return m_abilityMod.m_maxAngleWithFirst.GetModifiedValue(m_maxAngleWithFirst);
		}
		return m_maxAngleWithFirst;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_GremlinsMultiTargeterBasicAttack))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_GremlinsMultiTargeterBasicAttack);
			SetupTargeter();
			ResetTargetingNumbersForMines();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
		ResetTargetingNumbersForMines();
	}

	private void SetupTargeter()
	{
		if (m_bombInfoComp == null)
		{
			m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		}
		if (GetExpectedNumberOfTargeters() > 1)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					ClearTargeters();
					float num = ModdedMaxAngleWithFirst();
					for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
					{
						AbilityUtil_Targeter_GremlinsBombInCone abilityUtil_Targeter_GremlinsBombInCone = new AbilityUtil_Targeter_GremlinsBombInCone(this, GetBombShape(), m_penetrateLos);
						abilityUtil_Targeter_GremlinsBombInCone.SetTooltipSubjectTypes();
						if (num < 180f)
						{
							if (m_useAngleIndicators)
							{
								abilityUtil_Targeter_GremlinsBombInCone.SetAngleIndicatorConfig(true, num, m_indicatorLineLength);
							}
						}
						abilityUtil_Targeter_GremlinsBombInCone.SetUseMultiTargetUpdate(true);
						base.Targeters.Add(abilityUtil_Targeter_GremlinsBombInCone);
					}
					while (true)
					{
						switch (7)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				}
			}
		}
		AbilityUtil_Targeter_GremlinsBombInCone abilityUtil_Targeter_GremlinsBombInCone2 = new AbilityUtil_Targeter_GremlinsBombInCone(this, GetBombShape(), m_penetrateLos);
		abilityUtil_Targeter_GremlinsBombInCone2.SetTooltipSubjectTypes();
		base.Targeter = abilityUtil_Targeter_GremlinsBombInCone2;
	}

	private void ResetTargetingNumbersForMines()
	{
		AbilityData component = GetComponent<AbilityData>();
		if (!(component != null))
		{
			return;
		}
		GremlinsDropMines gremlinsDropMines = component.GetAbilityOfType(typeof(GremlinsDropMines)) as GremlinsDropMines;
		if (!(gremlinsDropMines != null))
		{
			return;
		}
		while (true)
		{
			gremlinsDropMines.ResetNameplateTargetingNumbers();
			return;
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, GetNumTargets());
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < targets.Count; i++)
		{
			list.Add(targets[i].FreePos);
		}
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_bombInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_bombInfoComp.m_directHitDamageAmount);
			if (m_bombInfoComp.m_directHitDamageAmount != m_bombInfoComp.m_directHitSubsequentDamageAmount)
			{
				AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_bombInfoComp.m_directHitSubsequentDamageAmount);
			}
		}
		return numbers;
	}

	public override List<int> _001D()
	{
		List<int> list = new List<int>();
		GremlinsLandMineInfoComponent component = GetComponent<GremlinsLandMineInfoComponent>();
		if (component != null)
		{
			list.Add(component.m_directHitDamageAmount);
			list.Add(component.m_directHitSubsequentDamageAmount);
			list.Add(component.m_damageAmount);
		}
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeters[0].LastUpdatingGridPos);
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			if (i > 0)
			{
				BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(base.Targeters[i].LastUpdatingGridPos);
				if (boardSquareSafe2 == null)
				{
					continue;
				}
				if (boardSquareSafe2 == boardSquareSafe)
				{
					continue;
				}
			}
			Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeters[i], targetActor, currentTargeterIndex, ModdedDirectHitDamagePerShot(i), GetSubsequentHitDamage());
		}
		while (true)
		{
			return symbolToValue;
		}
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		GremlinsLandMineInfoComponent component = GetComponent<GremlinsLandMineInfoComponent>();
		if (component != null)
		{
			AddTokenInt(tokens, "Damage_DirectHit", string.Empty, component.m_directHitDamageAmount);
			AddTokenInt(tokens, "Damage_MoveOverHit", string.Empty, component.m_damageAmount);
			AddTokenInt(tokens, "MineDuration", string.Empty, component.m_mineDuration);
		}
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null) && boardSquareSafe.IsBaselineHeight())
		{
			if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
			{
				if (UseShapeForDeadzone())
				{
					if (AreaEffectUtils.IsSquareInShape(boardSquareSafe, GetDeadzoneShape(), caster.GetTravelBoardSquareWorldPosition(), caster.GetCurrentBoardSquare(), true, caster))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return false;
							}
						}
					}
				}
				if (targetIndex > 0)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
						{
							bool flag = true;
							Vector3 from = Board.Get().GetBoardSquareSafe(currentTargets[0].GridPos).ToVector3() - caster.GetTravelBoardSquareWorldPosition();
							Vector3 to = boardSquareSafe.ToVector3() - caster.GetTravelBoardSquareWorldPosition();
							if (Mathf.RoundToInt(Vector3.Angle(from, to)) > (int)ModdedMaxAngleWithFirst())
							{
								flag = false;
							}
							if (flag)
							{
								float num = GetMinDistBetweenBombs() * Board.Get().squareSize;
								for (int i = 0; i < targetIndex; i++)
								{
									BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[i].GridPos);
									if (boardSquareSafe2 == boardSquareSafe)
									{
										flag = false;
										break;
									}
									Vector3 vector = boardSquareSafe.ToVector3() - boardSquareSafe2.ToVector3();
									vector.y = 0f;
									float magnitude = vector.magnitude;
									if (magnitude < num)
									{
										flag = false;
										break;
									}
								}
							}
							return flag;
						}
						}
					}
				}
				return true;
			}
		}
		return false;
	}

	public int GetSubsequentHitDamage()
	{
		return Mathf.Max(0, m_bombInfoComp.m_directHitSubsequentDamageAmount);
	}
}
