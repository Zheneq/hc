using System.Collections.Generic;
using UnityEngine;

public class BazookaGirlDelayedBombDrops : Ability
{
	public enum TargetingType
	{
		Shape,
		Cone
	}

	[Header("-- Targeting")]
	public TargetingType m_targetingType;

	public bool m_penetrateLos;

	[Header("-- Targeting All?")]
	public bool m_targetAll;

	[Header("-- If using Shape")]
	public AbilityAreaShape m_targetingShape = AbilityAreaShape.Five_x_Five_NoCorners;

	[Header("-- If Using Cone")]
	public float m_coneWidthAngle = 270f;

	public float m_coneLength = 1.5f;

	public float m_coneBackwardOffset;

	[Header("-- Dropped Bomb Info")]
	public int m_bombDropDelay = 1;

	public AbilityPriority m_bombDropPhase = AbilityPriority.Combat_Damage;

	public int m_bombDropAnimIndexInEffect = 11;

	public BazookaGirlDroppedBombInfo m_bombInfo;

	[Header("-- Additional Damage from fewer hit areas, = extraDmgPeArea * Max(0, (maxNumAreas - numAreas))")]
	public int m_maxNumOfAreasForExtraDamage;

	public int m_extraDamagePerFewerArea;

	[Header("-- On Ability Hit Effect")]
	public StandardEffectInfo m_enemyOnAbilityHitEffect;

	[Header("-- Sequences ----------------------------------------")]
	public GameObject m_castSequencePrefab;

	public GameObject m_dropSiteMarkerSequencePrefab;

	public GameObject m_bombDropSequencePrefab;

	public GameObject m_targetMarkedSequencePrefab;

	[TextArea(1, 5)]
	public string m_notes;

	private AbilityMod_BazookaGirlDelayedBombDrops m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Delayed Bomb Drops";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (TargetAllEnemies())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					base.Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, 30f, true);
					return;
				}
			}
		}
		if (m_targetingType == TargetingType.Shape)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					base.Targeter = new AbilityUtil_Targeter_Shape(this, m_targetingShape, PenetrateLos());
					return;
				}
			}
		}
		if (m_targetingType != TargetingType.Cone)
		{
			return;
		}
		while (true)
		{
			base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, GetConeAngle(), GetConeLength(), m_coneBackwardOffset, PenetrateLos(), true);
			return;
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeLength();
	}

	public int GetDamageAmount()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = m_bombInfo.m_damageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_bombInfo.m_damageAmount);
		}
		return result;
	}

	public float GetConeLength()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_coneLength;
		}
		else
		{
			result = m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength);
		}
		return result;
	}

	public float GetConeAngle()
	{
		float result;
		if (m_abilityMod == null)
		{
			result = m_coneWidthAngle;
		}
		else
		{
			result = m_abilityMod.m_coneAngleMod.GetModifiedValue(m_coneWidthAngle);
		}
		return result;
	}

	public bool TargetAllEnemies()
	{
		bool result;
		if (m_abilityMod == null)
		{
			result = m_targetAll;
		}
		else
		{
			result = m_abilityMod.m_targetAllMod.GetModifiedValue(m_targetAll);
		}
		return result;
	}

	public bool PenetrateLos()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_penetrateLosMod.GetModifiedValue(m_penetrateLos);
		}
		else
		{
			result = m_penetrateLos;
		}
		return result;
	}

	public int GetMaxNumOfAreasForExtraDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxNumOfAreasForExtraDamageMod.GetModifiedValue(m_maxNumOfAreasForExtraDamage);
		}
		else
		{
			result = m_maxNumOfAreasForExtraDamage;
		}
		return result;
	}

	public int GetExtraDamagePerFewerArea()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamagePerFewerAreaMod.GetModifiedValue(m_extraDamagePerFewerArea);
		}
		else
		{
			result = m_extraDamagePerFewerArea;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_enemyOnAbilityHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		int num;
		if (m_bombDropDelay <= 0)
		{
			num = 1;
		}
		else
		{
			num = 3;
		}
		AbilityTooltipSubject subject = (AbilityTooltipSubject)num;
		AbilityTooltipHelper.ReportDamage(ref numbers, subject, m_bombInfo.m_damageAmount);
		if (m_bombInfo.m_damageAmount != m_bombInfo.m_subsequentDamageAmount)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Quaternary, m_bombInfo.m_subsequentDamageAmount);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (m_bombDropDelay <= 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
					ActorData actorData = base.ActorData;
					List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
					List<BoardSquare> list = new List<BoardSquare>();
					using (List<ActorData>.Enumerator enumerator = visibleActorsInRangeByTooltipSubject.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							list.Add(current.GetCurrentBoardSquare());
						}
					}
					int num = 0;
					if (GetExtraDamagePerFewerArea() > 0)
					{
						if (GetMaxNumOfAreasForExtraDamage() > 0)
						{
							int num2 = GetMaxNumOfAreasForExtraDamage() - visibleActorsInRangeByTooltipSubject.Count;
							if (num2 > 0)
							{
								num = num2 * GetExtraDamagePerFewerArea();
							}
						}
					}
					int num3 = 0;
					bool flag = false;
					foreach (BoardSquare item in list)
					{
						Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_bombInfo.m_shape, item.ToVector3(), item);
						List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_bombInfo.m_shape, centerOfShape, item, PenetrateLos(), actorData, actorData.GetOpposingTeam(), null);
						foreach (ActorData item2 in actorsInShape)
						{
							if (item2 == targetActor)
							{
								if (flag)
								{
									num3 += m_bombInfo.m_subsequentDamageAmount;
								}
								else
								{
									num3 += GetDamageAmount();
									flag = true;
								}
							}
						}
					}
					dictionary[AbilityTooltipSymbol.Damage] = num3 + num;
					return dictionary;
				}
				}
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BazookaGirlDelayedBombDrops abilityMod_BazookaGirlDelayedBombDrops = modAsBase as AbilityMod_BazookaGirlDelayedBombDrops;
		AddTokenInt(tokens, "Damage", string.Empty, m_bombInfo.m_damageAmount);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_BazookaGirlDelayedBombDrops)
		{
			val = abilityMod_BazookaGirlDelayedBombDrops.m_maxNumOfAreasForExtraDamageMod.GetModifiedValue(m_maxNumOfAreasForExtraDamage);
		}
		else
		{
			val = m_maxNumOfAreasForExtraDamage;
		}
		AddTokenInt(tokens, "MaxNumOfAreasForExtraDamage", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_BazookaGirlDelayedBombDrops)
		{
			val2 = abilityMod_BazookaGirlDelayedBombDrops.m_extraDamagePerFewerAreaMod.GetModifiedValue(m_extraDamagePerFewerArea);
		}
		else
		{
			val2 = m_extraDamagePerFewerArea;
		}
		AddTokenInt(tokens, "ExtraDamagePerFewerArea", empty2, val2);
	}

	public override bool CanTriggerAnimAtIndexForTaunt(int animIndex)
	{
		int result;
		if (animIndex != m_bombDropAnimIndexInEffect)
		{
			result = (base.CanTriggerAnimAtIndexForTaunt(animIndex) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BazookaGirlDelayedBombDrops))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_BazookaGirlDelayedBombDrops);
					SetupTargeter();
					return;
				}
			}
		}
		Debug.LogError("Trying to apply wrong type of ability mod");
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> points = new List<Vector3>();
		Vector3 travelBoardSquareWorldPosition = caster.GetTravelBoardSquareWorldPosition();
		if (TargetAllEnemies())
		{
			AreaEffectUtils.AddRadiusExtremaToList(ref points, travelBoardSquareWorldPosition, 5f);
		}
		else if (m_targetingType == TargetingType.Shape)
		{
			AreaEffectUtils.AddShapeCornersToList(ref points, m_targetingShape, targets[0]);
		}
		else if (m_targetingType == TargetingType.Cone)
		{
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
			AreaEffectUtils.AddConeExtremaToList(ref points, travelBoardSquareWorldPosition, coneCenterAngleDegrees, GetConeAngle(), GetConeLength(), m_coneBackwardOffset);
		}
		return points;
	}
}
