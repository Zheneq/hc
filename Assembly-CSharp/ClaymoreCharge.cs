using System.Collections.Generic;
using UnityEngine;

public class ClaymoreCharge : Ability
{
	[Header("-- Charge Targeting")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	public float m_width = 1f;

	public float m_maxRange = 10f;

	[Header("-- Normal On Hit Damage, Effect, etc")]
	public int m_directHitDamage = 20;

	public StandardEffectInfo m_directEnemyHitEffect;

	public bool m_directHitIgnoreCover = true;

	[Space(10f)]
	public int m_aoeDamage = 10;

	public StandardEffectInfo m_aoeEnemyHitEffect;

	[Header("-- Extra Damage from Charge Path Length")]
	public int m_extraDirectHitDamagePerSquare;

	[Header("-- Heal On Self")]
	public int m_healOnSelfPerTargetHit;

	[Header("-- Other On Hit Config")]
	public int m_cooldownOnHit = -1;

	public bool m_chaseHitActor;

	[Header("-- Charge Anim")]
	[Tooltip("Whether to set up charge like battlemonk charge with pivots and recovery")]
	public bool m_chargeWithPivotAndRecovery;

	[Tooltip("Only relevant if using pivot and recovery charge setup")]
	public float m_recoveryTime = 0.5f;

	[Header("-- Sequences")]
	public GameObject m_chargeSequencePrefab;

	public GameObject m_aoeHitSequencePrefab;

	private const int c_maxBounces = 0;

	private const int c_maxTargetsHit = 1;

	private const bool c_penetrateLoS = false;

	private AbilityMod_ClaymoreCharge m_abilityMod;

	private Claymore_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedDirectEnemyHitEffect;

	private StandardEffectInfo m_cachedAoeEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Berserker Charge";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_syncComp = GetComponent<Claymore_SyncComponent>();
		SetCachedFields();
		AbilityUtil_Targeter_ClaymoreCharge abilityUtil_Targeter_ClaymoreCharge = new AbilityUtil_Targeter_ClaymoreCharge(this, GetChargeWidth(), GetChargeRange(), GetAoeShape(), DirectHitIgnoreCover());
		if (GetHealOnSelfPerTargetHit() > 0)
		{
			if (_003C_003Ef__am_0024cache0 == null)
			{
				_003C_003Ef__am_0024cache0 = ((ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0);
			}
			abilityUtil_Targeter_ClaymoreCharge.m_affectCasterDelegate = _003C_003Ef__am_0024cache0;
		}
		base.Targeter = abilityUtil_Targeter_ClaymoreCharge;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetChargeRange();
	}

	private void SetCachedFields()
	{
		m_cachedDirectEnemyHitEffect = ((!m_abilityMod) ? m_directEnemyHitEffect : m_abilityMod.m_directEnemyHitEffectMod.GetModifiedValue(m_directEnemyHitEffect));
		m_cachedAoeEnemyHitEffect = ((!m_abilityMod) ? m_aoeEnemyHitEffect : m_abilityMod.m_aoeEnemyHitEffectMod.GetModifiedValue(m_aoeEnemyHitEffect));
	}

	public AbilityAreaShape GetAoeShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_aoeShapeMod.GetModifiedValue(m_aoeShape);
		}
		else
		{
			result = m_aoeShape;
		}
		return result;
	}

	public float GetChargeWidth()
	{
		return (!m_abilityMod) ? m_width : m_abilityMod.m_widthMod.GetModifiedValue(m_width);
	}

	public float GetChargeRange()
	{
		return (!m_abilityMod) ? m_maxRange : m_abilityMod.m_maxRangeMod.GetModifiedValue(m_maxRange);
	}

	public bool DirectHitIgnoreCover()
	{
		return (!m_abilityMod) ? m_directHitIgnoreCover : m_abilityMod.m_directHitIgnoreCoverMod.GetModifiedValue(m_directHitIgnoreCover);
	}

	public int GetDirectHitDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_directHitDamageMod.GetModifiedValue(m_directHitDamage);
		}
		else
		{
			result = m_directHitDamage;
		}
		return result;
	}

	public StandardEffectInfo GetDirectEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedDirectEnemyHitEffect != null)
		{
			result = m_cachedDirectEnemyHitEffect;
		}
		else
		{
			result = m_directEnemyHitEffect;
		}
		return result;
	}

	public int GetAoeDamage()
	{
		return (!m_abilityMod) ? m_aoeDamage : m_abilityMod.m_aoeDamageMod.GetModifiedValue(m_aoeDamage);
	}

	public StandardEffectInfo GetAoeEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAoeEnemyHitEffect != null)
		{
			result = m_cachedAoeEnemyHitEffect;
		}
		else
		{
			result = m_aoeEnemyHitEffect;
		}
		return result;
	}

	public int GetExtraDirectHitDamagePerSquare()
	{
		return (!m_abilityMod) ? m_extraDirectHitDamagePerSquare : m_abilityMod.m_extraDirectHitDamagePerSquareMod.GetModifiedValue(m_extraDirectHitDamagePerSquare);
	}

	public int GetHealOnSelfPerTargetHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healOnSelfPerTargetHitMod.GetModifiedValue(m_healOnSelfPerTargetHit);
		}
		else
		{
			result = m_healOnSelfPerTargetHit;
		}
		return result;
	}

	public int GetCooldownOnHit()
	{
		return (!m_abilityMod) ? m_cooldownOnHit : m_abilityMod.m_cooldownOnHitMod.GetModifiedValue(m_cooldownOnHit);
	}

	public bool GetChaseHitActor()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_chaseHitActorMod.GetModifiedValue(m_chaseHitActor);
		}
		else
		{
			result = m_chaseHitActor;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		return 1;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ClaymoreCharge abilityMod_ClaymoreCharge = modAsBase as AbilityMod_ClaymoreCharge;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ClaymoreCharge)
		{
			val = abilityMod_ClaymoreCharge.m_directHitDamageMod.GetModifiedValue(m_directHitDamage);
		}
		else
		{
			val = m_directHitDamage;
		}
		AddTokenInt(tokens, "DirectHitDamage", empty, val);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_ClaymoreCharge) ? m_directEnemyHitEffect : abilityMod_ClaymoreCharge.m_directEnemyHitEffectMod.GetModifiedValue(m_directEnemyHitEffect), "DirectEnemyHitEffect", m_directEnemyHitEffect);
		AddTokenInt(tokens, "AoeDamage", string.Empty, (!abilityMod_ClaymoreCharge) ? m_aoeDamage : abilityMod_ClaymoreCharge.m_aoeDamageMod.GetModifiedValue(m_aoeDamage));
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_ClaymoreCharge)
		{
			effectInfo = abilityMod_ClaymoreCharge.m_aoeEnemyHitEffectMod.GetModifiedValue(m_aoeEnemyHitEffect);
		}
		else
		{
			effectInfo = m_aoeEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "AoeEnemyHitEffect", m_aoeEnemyHitEffect);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ClaymoreCharge)
		{
			val2 = abilityMod_ClaymoreCharge.m_extraDirectHitDamagePerSquareMod.GetModifiedValue(m_extraDirectHitDamagePerSquare);
		}
		else
		{
			val2 = m_extraDirectHitDamagePerSquare;
		}
		AddTokenInt(tokens, "ExtraDirectHitDamagePerSquare", empty2, val2);
		AddTokenInt(tokens, "HealOnSelfPerTargetHit", string.Empty, (!abilityMod_ClaymoreCharge) ? m_healOnSelfPerTargetHit : abilityMod_ClaymoreCharge.m_healOnSelfPerTargetHitMod.GetModifiedValue(m_healOnSelfPerTargetHit));
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_ClaymoreCharge)
		{
			val3 = abilityMod_ClaymoreCharge.m_cooldownOnHitMod.GetModifiedValue(m_cooldownOnHit);
		}
		else
		{
			val3 = m_cooldownOnHit;
		}
		AddTokenInt(tokens, "CooldownOnHit", empty3, val3);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDirectHitDamage());
		GetDirectEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetAoeDamage());
		GetAoeEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealOnSelfPerTargetHit());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				int num = GetDirectHitDamage();
				if (GetExtraDirectHitDamagePerSquare() > 0)
				{
					if (base.Targeter is AbilityUtil_Targeter_ClaymoreCharge)
					{
						AbilityUtil_Targeter_ClaymoreCharge abilityUtil_Targeter_ClaymoreCharge = base.Targeter as AbilityUtil_Targeter_ClaymoreCharge;
						int num2 = Mathf.Max(0, abilityUtil_Targeter_ClaymoreCharge.LastUpdatePathSquareCount - 1);
						num += num2 * GetExtraDirectHitDamagePerSquare();
					}
				}
				dictionary[AbilityTooltipSymbol.Damage] = num;
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetAoeDamage();
			}
			else if (GetHealOnSelfPerTargetHit() > 0)
			{
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
				{
					int num4 = dictionary[AbilityTooltipSymbol.Healing] = GetHealOnSelfPerTargetHit() * Mathf.Max(0, base.Targeter.GetActorsInRange().Count - 1);
				}
			}
		}
		return dictionary;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		object result;
		if (m_syncComp != null)
		{
			result = m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, base.ActorData);
		}
		else
		{
			result = null;
		}
		return (string)result;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public static List<ActorData> GetActorsOnPath(BoardSquarePathInfo path, List<Team> relevantTeams, ActorData caster)
	{
		List<ActorData> list = new List<ActorData>();
		if (path != null)
		{
			for (BoardSquarePathInfo boardSquarePathInfo = path; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
			{
				ActorData occupantActor = boardSquarePathInfo.square.OccupantActor;
				if (!(occupantActor != null) || !AreaEffectUtils.IsActorTargetable(occupantActor, relevantTeams))
				{
					continue;
				}
				if (relevantTeams != null)
				{
					if (!relevantTeams.Contains(occupantActor.GetTeam()))
					{
						continue;
					}
				}
				list.Add(occupantActor);
			}
		}
		return list;
	}

	public static float GetMaxPotentialChargeDistance(Vector3 startPos, Vector3 endPos, Vector3 aimDir, float laserMaxDistInWorld, ActorData mover, out BoardSquare pathEndSquare)
	{
		float result = laserMaxDistInWorld;
		pathEndSquare = KnockbackUtils.GetLastValidBoardSquareInLine(startPos, endPos, false, false, laserMaxDistInWorld + 0.5f);
		BoardSquare boardSquare = Board.Get().GetBoardSquare(startPos);
		if (pathEndSquare != null)
		{
			if (pathEndSquare != boardSquare)
			{
				Vector3 pointToProject = pathEndSquare.ToVector3();
				Vector3 projectionPoint = VectorUtils.GetProjectionPoint(aimDir, startPos, pointToProject);
				float num = (projectionPoint - startPos).magnitude + 0.5f;
				if (num < laserMaxDistInWorld)
				{
					result = num;
				}
				goto IL_00a8;
			}
		}
		result = 0.5f;
		goto IL_00a8;
		IL_00a8:
		return result;
	}

	public static BoardSquare GetTrimmedDestinationInPath(BoardSquarePathInfo chargePath, out bool differentFromInputDest)
	{
		differentFromInputDest = false;
		BoardSquare result = null;
		if (chargePath != null)
		{
			BoardSquarePathInfo boardSquarePathInfo = chargePath;
			result = boardSquarePathInfo.square;
			int num = 0;
			while (boardSquarePathInfo.next != null)
			{
				BoardSquare square = boardSquarePathInfo.next.square;
				if (boardSquarePathInfo.square._0015())
				{
					if (!square._0015())
					{
						if (num > 0)
						{
							result = boardSquarePathInfo.square;
							differentFromInputDest = true;
							break;
						}
					}
				}
				result = square;
				boardSquarePathInfo = boardSquarePathInfo.next;
				num++;
			}
		}
		return result;
	}

	public static BoardSquare GetChargeDestinationSquare(Vector3 startPos, Vector3 chargeDestPos, ActorData lastChargeHitActor, BoardSquare initialPathEndSquare, ActorData caster, bool trimBeforeFirstInvalid)
	{
		BoardSquare boardSquare = null;
		if (lastChargeHitActor != null)
		{
			boardSquare = lastChargeHitActor.GetCurrentBoardSquare();
		}
		else
		{
			if (initialPathEndSquare != null)
			{
				boardSquare = initialPathEndSquare;
			}
			else
			{
				boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(startPos, chargeDestPos, true);
			}
			BoardSquare boardSquare2 = Board.Get().GetBoardSquare(startPos);
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(caster, boardSquare, boardSquare2, true);
			if (boardSquarePathInfo != null && trimBeforeFirstInvalid)
			{
				boardSquare = GetTrimmedDestinationInPath(boardSquarePathInfo, out bool _);
			}
		}
		return boardSquare;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClaymoreCharge))
		{
			m_abilityMod = (abilityMod as AbilityMod_ClaymoreCharge);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
