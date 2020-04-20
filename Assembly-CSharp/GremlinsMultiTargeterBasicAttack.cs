using System;
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
		return this.m_abilityMod;
	}

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "MultiTargeter Basic Attack";
		}
		this.m_bombInfoComp = base.GetComponent<GremlinsLandMineInfoComponent>();
		base.ResetTooltipAndTargetingNumbers();
		this.SetupTargeter();
	}

	public AbilityAreaShape GetBombShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod == null)
		{
			result = this.m_bombShape;
		}
		else
		{
			result = this.m_abilityMod.m_bombShapeMod.GetModifiedValue(this.m_bombShape);
		}
		return result;
	}

	public float GetMinDistBetweenBombs()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_minDistBetweenBombsMod.GetModifiedValue(this.m_minDistanceBetweenBombs) : this.m_minDistanceBetweenBombs;
	}

	public bool UseShapeForDeadzone()
	{
		bool result;
		if (this.m_abilityMod == null)
		{
			result = this.m_useShapeForDeadzone;
		}
		else
		{
			result = this.m_abilityMod.m_useShapeForDeadzoneMod.GetModifiedValue(this.m_useShapeForDeadzone);
		}
		return result;
	}

	public AbilityAreaShape GetDeadzoneShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod == null)
		{
			result = this.m_deadZoneShape;
		}
		else
		{
			result = this.m_abilityMod.m_deadzoneShapeMod.GetModifiedValue(this.m_deadZoneShape);
		}
		return result;
	}

	private int ModdedDirectHitDamagePerShot(int shotIndex)
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_directHitDamagePerShot.Count > shotIndex)
			{
				if (shotIndex >= 0)
				{
					return this.m_abilityMod.m_directHitDamagePerShot[shotIndex].GetModifiedValue(this.m_bombInfoComp.m_directHitDamageAmount);
				}
			}
		}
		return this.m_bombInfoComp.m_directHitDamageAmount;
	}

	private float ModdedMaxAngleWithFirst()
	{
		if (this.m_abilityMod != null)
		{
			return this.m_abilityMod.m_maxAngleWithFirst.GetModifiedValue(this.m_maxAngleWithFirst);
		}
		return this.m_maxAngleWithFirst;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_GremlinsMultiTargeterBasicAttack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_GremlinsMultiTargeterBasicAttack);
			this.SetupTargeter();
			this.ResetTargetingNumbersForMines();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
		this.ResetTargetingNumbersForMines();
	}

	private void SetupTargeter()
	{
		if (this.m_bombInfoComp == null)
		{
			this.m_bombInfoComp = base.GetComponent<GremlinsLandMineInfoComponent>();
		}
		if (this.GetExpectedNumberOfTargeters() > 1)
		{
			base.ClearTargeters();
			float num = this.ModdedMaxAngleWithFirst();
			for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
			{
				AbilityUtil_Targeter_GremlinsBombInCone abilityUtil_Targeter_GremlinsBombInCone = new AbilityUtil_Targeter_GremlinsBombInCone(this, this.GetBombShape(), this.m_penetrateLos, AbilityUtil_Targeter_GremlinsBombInCone.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible);
				abilityUtil_Targeter_GremlinsBombInCone.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Primary);
				if (num < 180f)
				{
					if (this.m_useAngleIndicators)
					{
						abilityUtil_Targeter_GremlinsBombInCone.SetAngleIndicatorConfig(true, num, this.m_indicatorLineLength);
					}
				}
				abilityUtil_Targeter_GremlinsBombInCone.SetUseMultiTargetUpdate(true);
				base.Targeters.Add(abilityUtil_Targeter_GremlinsBombInCone);
			}
		}
		else
		{
			AbilityUtil_Targeter_GremlinsBombInCone abilityUtil_Targeter_GremlinsBombInCone2 = new AbilityUtil_Targeter_GremlinsBombInCone(this, this.GetBombShape(), this.m_penetrateLos, AbilityUtil_Targeter_GremlinsBombInCone.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible);
			abilityUtil_Targeter_GremlinsBombInCone2.SetTooltipSubjectTypes(AbilityTooltipSubject.Primary, AbilityTooltipSubject.Primary);
			base.Targeter = abilityUtil_Targeter_GremlinsBombInCone2;
		}
	}

	private void ResetTargetingNumbersForMines()
	{
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
		{
			GremlinsDropMines gremlinsDropMines = component.GetAbilityOfType(typeof(GremlinsDropMines)) as GremlinsDropMines;
			if (gremlinsDropMines != null)
			{
				gremlinsDropMines.ResetNameplateTargetingNumbers();
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, base.GetNumTargets());
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
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_bombInfoComp != null)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_bombInfoComp.m_directHitDamageAmount);
			if (this.m_bombInfoComp.m_directHitDamageAmount != this.m_bombInfoComp.m_directHitSubsequentDamageAmount)
			{
				AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_bombInfoComp.m_directHitSubsequentDamageAmount);
			}
		}
		return result;
	}

	public override List<int> symbol_001D()
	{
		List<int> list = new List<int>();
		GremlinsLandMineInfoComponent component = base.GetComponent<GremlinsLandMineInfoComponent>();
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
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeters[0].LastUpdatingGridPos);
		int i = 0;
		while (i <= currentTargeterIndex)
		{
			if (i <= 0)
			{
				goto IL_98;
			}
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(base.Targeters[i].LastUpdatingGridPos);
			if (!(boardSquareSafe2 == null))
			{
				if (!(boardSquareSafe2 == boardSquareSafe))
				{
					goto IL_98;
				}
			}
			IL_C4:
			i++;
			continue;
			IL_98:
			Ability.AddNameplateValueForOverlap(ref result, base.Targeters[i], targetActor, currentTargeterIndex, this.ModdedDirectHitDamagePerShot(i), this.GetSubsequentHitDamage(), AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
			goto IL_C4;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		GremlinsLandMineInfoComponent component = base.GetComponent<GremlinsLandMineInfoComponent>();
		if (component != null)
		{
			base.AddTokenInt(tokens, "Damage_DirectHit", string.Empty, component.m_directHitDamageAmount, false);
			base.AddTokenInt(tokens, "Damage_MoveOverHit", string.Empty, component.m_damageAmount, false);
			base.AddTokenInt(tokens, "MineDuration", string.Empty, component.m_mineDuration, false);
		}
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null) && boardSquareSafe.IsBaselineHeight())
		{
			if (boardSquareSafe == caster.GetCurrentBoardSquare())
			{
			}
			else
			{
				if (this.UseShapeForDeadzone())
				{
					if (AreaEffectUtils.IsSquareInShape(boardSquareSafe, this.GetDeadzoneShape(), caster.GetTravelBoardSquareWorldPosition(), caster.GetCurrentBoardSquare(), true, caster))
					{
						return false;
					}
				}
				if (targetIndex > 0)
				{
					bool flag = true;
					Vector3 from = Board.Get().GetBoardSquareSafe(currentTargets[0].GridPos).ToVector3() - caster.GetTravelBoardSquareWorldPosition();
					Vector3 to = boardSquareSafe.ToVector3() - caster.GetTravelBoardSquareWorldPosition();
					if (Mathf.RoundToInt(Vector3.Angle(from, to)) > (int)this.ModdedMaxAngleWithFirst())
					{
						flag = false;
					}
					if (flag)
					{
						float num = this.GetMinDistBetweenBombs() * Board.Get().squareSize;
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
				return true;
			}
		}
		return false;
	}

	public int GetSubsequentHitDamage()
	{
		return Mathf.Max(0, this.m_bombInfoComp.m_directHitSubsequentDamageAmount);
	}
}
