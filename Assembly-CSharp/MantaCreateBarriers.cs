using System.Collections.Generic;
using UnityEngine;

public class MantaCreateBarriers : Ability
{
	[Header("-- Whether require Manta to be inside target area --")]
	public bool m_requireCasterInShape = true;

	public AbilityAreaShape m_targetAreaShape = AbilityAreaShape.Five_x_Five;

	[Header("-- Barriers")]
	[Separator("NOTE: you can also use MantaCreateBarriersChainFinal for damage stuff!", true)]
	public bool m_delayBarriersUntilStartOfNextTurn;

	public int m_prisonSides = 8;

	public float m_prisonRadius = 3.5f;

	public StandardBarrierData m_prisonBarrierData;

	public AbilityAreaShape m_shapeForTargeter = AbilityAreaShape.Seven_x_Seven;

	[Tooltip("WARNING: don't do this if it's a Blast phase ability unless the walls don't block abilities")]
	public bool m_createBarriersImmediately;

	[Header("-- Ground effect")]
	public StandardGroundEffectInfo m_groundEffectInfo;

	public int m_damageOnCast = 30;

	[Header("-- On Cast Ally Hit (applies to caster as well)")]
	public int m_allyHealOnCast;

	public StandardEffectInfo m_effectOnAlliesOnCast;

	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private Manta_SyncComponent m_syncComp;

	private AbilityMod_MantaCreateBarriers m_abilityMod;

	private MantaCreateBarriersChainFinal m_finalDamageChain;

	private StandardBarrierData m_cachedPrisonBarrierData;

	private StandardEffectInfo m_cachedEffectOnAlliesOnCast;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Lair";
		}
		if (m_prisonSides < 3)
		{
			m_prisonSides = 4;
		}
		Ability[] chainAbilities = GetChainAbilities();
		Ability[] array = chainAbilities;
		int num = 0;
		while (true)
		{
			if (num < array.Length)
			{
				Ability ability = array[num];
				if (ability != null && ability is MantaCreateBarriersChainFinal)
				{
					m_finalDamageChain = (ability as MantaCreateBarriersChainFinal);
					break;
				}
				num++;
				continue;
			}
			break;
		}
		m_syncComp = GetComponent<Manta_SyncComponent>();
		Setup();
		ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_TeslaPrison(this, TrackerTeslaPrison.PrisonWallSegmentType.RegularPolygon, 0, 0, GetPrisonSides(), GetPrisonRadius(), GetShapeForTargeter(), true);
		base.Targeter.SetAffectedGroups(true, IncludeAllies(), false);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetPrisonRadius();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_prisonBarrierData.AddTooltipTokens(tokens, "PrisonBarrierData");
		AddTokenInt(tokens, "DamageOnCast", string.Empty, m_damageOnCast);
		AddTokenInt(tokens, "AllyHealOnCast", string.Empty, m_allyHealOnCast);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnAlliesOnCast, "EffectOnAlliesOnCast", m_effectOnAlliesOnCast);
	}

	private void SetCachedFields()
	{
		StandardBarrierData cachedPrisonBarrierData;
		if ((bool)m_abilityMod)
		{
			cachedPrisonBarrierData = m_abilityMod.m_prisonBarrierDataMod.GetModifiedValue(m_prisonBarrierData);
		}
		else
		{
			cachedPrisonBarrierData = m_prisonBarrierData;
		}
		m_cachedPrisonBarrierData = cachedPrisonBarrierData;
		m_cachedEffectOnAlliesOnCast = ((!m_abilityMod) ? m_effectOnAlliesOnCast : m_abilityMod.m_effectOnAlliesOnCastMod.GetModifiedValue(m_effectOnAlliesOnCast));
	}

	public bool RequireCasterInShape()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_requireCasterInShapeMod.GetModifiedValue(m_requireCasterInShape);
		}
		else
		{
			result = m_requireCasterInShape;
		}
		return result;
	}

	public AbilityAreaShape GetTargetAreaShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_targetAreaShapeMod.GetModifiedValue(m_targetAreaShape);
		}
		else
		{
			result = m_targetAreaShape;
		}
		return result;
	}

	public bool DelayBarriersUntilStartOfNextTurn()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_delayBarriersUntilStartOfNextTurnMod.GetModifiedValue(m_delayBarriersUntilStartOfNextTurn);
		}
		else
		{
			result = m_delayBarriersUntilStartOfNextTurn;
		}
		return result;
	}

	public int GetPrisonSides()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_prisonSidesMod.GetModifiedValue(m_prisonSides);
		}
		else
		{
			result = m_prisonSides;
		}
		return result;
	}

	public float GetPrisonRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_prisonRadiusMod.GetModifiedValue(m_prisonRadius);
		}
		else
		{
			result = m_prisonRadius;
		}
		return result;
	}

	public AbilityAreaShape GetShapeForTargeter()
	{
		return (!m_abilityMod) ? m_shapeForTargeter : m_abilityMod.m_shapeForTargeterMod.GetModifiedValue(m_shapeForTargeter);
	}

	public bool CreateBarriersImmediately()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_createBarriersImmediatelyMod.GetModifiedValue(m_createBarriersImmediately);
		}
		else
		{
			result = m_createBarriersImmediately;
		}
		return result;
	}

	public StandardGroundEffectInfo GetGroundEffectInfo()
	{
		StandardGroundEffectInfo result;
		if ((bool)m_abilityMod && m_abilityMod.m_groundEffectInfoMod.m_applyGroundEffect)
		{
			result = m_abilityMod.m_groundEffectInfoMod;
		}
		else
		{
			result = m_groundEffectInfo;
		}
		return result;
	}

	public int GetDamageOnCast()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageOnCastMod.GetModifiedValue(m_damageOnCast);
		}
		else
		{
			result = m_damageOnCast;
		}
		return result;
	}

	public int GetAllyHealOnCast()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_allyHealOnCastMod.GetModifiedValue(m_allyHealOnCast);
		}
		else
		{
			result = m_allyHealOnCast;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnAlliesOnCast()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnAlliesOnCast != null)
		{
			result = m_cachedEffectOnAlliesOnCast;
		}
		else
		{
			result = m_effectOnAlliesOnCast;
		}
		return result;
	}

	private StandardBarrierData GetPrisonBarrierData()
	{
		StandardBarrierData result;
		if (m_cachedPrisonBarrierData == null)
		{
			result = m_prisonBarrierData;
		}
		else
		{
			result = m_cachedPrisonBarrierData;
		}
		return result;
	}

	private bool ShouldAddVisionProvider()
	{
		return (bool)m_abilityMod && m_abilityMod.m_addVisionProviderInsideBarriers.GetModifiedValue(false);
	}

	public bool IncludeAllies()
	{
		return GetAllyHealOnCast() > 0 || GetEffectOnAlliesOnCast().m_applyEffect;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_MantaCreateBarriers))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_MantaCreateBarriers);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_finalDamageChain != null)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_finalDamageChain.GetDamageOnCast()));
		}
		else
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, GetDamageOnCast()));
			m_groundEffectInfo.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
			GetEffectOnAlliesOnCast().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
			GetEffectOnAlliesOnCast().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
			AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHealOnCast());
			AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetAllyHealOnCast());
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = (!(m_finalDamageChain != null)) ? GetDamageOnCast() : m_finalDamageChain.GetDamageOnCast();
		if (GetGroundEffectInfo().m_applyGroundEffect && GetGroundEffectInfo().m_groundEffectData.damageAmount > 0)
		{
			num += GetGroundEffectInfo().m_groundEffectData.damageAmount;
		}
		dictionary[AbilityTooltipSymbol.Damage] = num;
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					int num = 0;
					List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
					using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							AbilityUtil_Targeter.ActorTarget current = enumerator.Current;
							num += m_syncComp.GetDirtyFightingExtraTP(current.m_actor);
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return num;
							}
						}
					}
				}
				}
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
		{
			if (m_syncComp != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return m_syncComp.GetAccessoryStringForDamage(targetActor, base.ActorData, this);
					}
				}
			}
		}
		return null;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (m_requireCasterInShape && caster.GetCurrentBoardSquare() != null)
		{
			BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
			if (boardSquareSafe != null)
			{
				return AreaEffectUtils.IsSquareInShape(caster.GetCurrentBoardSquare(), GetTargetAreaShape(), target.FreePos, boardSquareSafe, true, caster);
			}
			return false;
		}
		return true;
	}

	public override bool AllowInvalidSquareForSquareBasedTarget()
	{
		return true;
	}
}
