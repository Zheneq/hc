using System.Collections.Generic;
using UnityEngine;

public class NanoSmithBlastShield : Ability
{
	[Header("-- Shield Effect")]
	public StandardActorEffectData m_shieldEffect;

	public int m_healOnEndIfHasRemainingAbsorb;

	public int m_energyGainOnShieldTarget;

	[Header("-- Extra Effect on Caster if targeting Ally")]
	public StandardEffectInfo m_extraEffectOnCasterIfTargetingAlly;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_shieldSequencePrefab;

	private bool m_allowOnEnemy;

	private AbilityMod_NanoSmithBlastShield m_abilityMod;

	private StandardEffectInfo m_cachedExtraEffectOnCasterIfTargetingAlly;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Blast Shield";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		int num;
		if (GetExtraEffectOnCasterIfTargetingAlly().m_applyEffect)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = 2;
		}
		else
		{
			num = 1;
		}
		AbilityUtil_Targeter.AffectsActor affectsCaster = (AbilityUtil_Targeter.AffectsActor)num;
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, m_allowOnEnemy, true, affectsCaster);
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedExtraEffectOnCasterIfTargetingAlly;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedExtraEffectOnCasterIfTargetingAlly = m_abilityMod.m_extraEffectOnCasterIfTargetingAllyMod.GetModifiedValue(m_extraEffectOnCasterIfTargetingAlly);
		}
		else
		{
			cachedExtraEffectOnCasterIfTargetingAlly = m_extraEffectOnCasterIfTargetingAlly;
		}
		m_cachedExtraEffectOnCasterIfTargetingAlly = cachedExtraEffectOnCasterIfTargetingAlly;
	}

	public StandardActorEffectData GetShieldEffectData()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_shieldEffectOverride.GetModifiedValue(m_shieldEffect) : m_shieldEffect;
	}

	public int GetHealOnEndIfHasRemainingAbsorb()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_healOnEndIfHasRemainingAbsorb;
		}
		else
		{
			result = m_abilityMod.m_healOnEndIfHasRemainingAbsorbMod.GetModifiedValue(m_healOnEndIfHasRemainingAbsorb);
		}
		return result;
	}

	public int GetEnergyGainOnShieldTarget()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_energyGainOnShieldTarget;
		}
		else
		{
			result = m_abilityMod.m_energyGainOnShieldTargetMod.GetModifiedValue(m_energyGainOnShieldTarget);
		}
		return result;
	}

	public StandardEffectInfo GetExtraEffectOnCasterIfTargetingAlly()
	{
		StandardEffectInfo result;
		if (m_cachedExtraEffectOnCasterIfTargetingAlly != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedExtraEffectOnCasterIfTargetingAlly;
		}
		else
		{
			result = m_extraEffectOnCasterIfTargetingAlly;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetShieldEffectData().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportEnergy(ref numbers, AbilityTooltipSubject.Primary, GetEnergyGainOnShieldTarget());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		if (GetExtraEffectOnCasterIfTargetingAlly().m_applyEffect)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ActorData actorData = base.ActorData;
			if (actorData != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (actorData == targetActor)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
					if (visibleActorsCountByTooltipSubject > 0)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						dictionary = new Dictionary<AbilityTooltipSymbol, int>();
						dictionary[AbilityTooltipSymbol.Absorb] = GetExtraEffectOnCasterIfTargetingAlly().m_effectData.m_absorbAmount;
					}
				}
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int energyGainOnShieldTarget = GetEnergyGainOnShieldTarget();
		if (energyGainOnShieldTarget > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(base.Targeter.LastUpdatingGridPos);
			if (boardSquareSafe != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquareSafe.OccupantActor == caster)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return energyGainOnShieldTarget;
						}
					}
				}
			}
		}
		return 0;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(caster, currentBestActorTarget, m_allowOnEnemy, true, true, ValidateCheckPath.Ignore, true, true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithBlastShield abilityMod_NanoSmithBlastShield = modAsBase as AbilityMod_NanoSmithBlastShield;
		StandardActorEffectData standardActorEffectData;
		if ((bool)abilityMod_NanoSmithBlastShield)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			standardActorEffectData = abilityMod_NanoSmithBlastShield.m_shieldEffectOverride.GetModifiedValue(m_shieldEffect);
		}
		else
		{
			standardActorEffectData = m_shieldEffect;
		}
		StandardActorEffectData standardActorEffectData2 = standardActorEffectData;
		standardActorEffectData2.AddTooltipTokens(tokens, "ShieldEffect", abilityMod_NanoSmithBlastShield != null, m_shieldEffect);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_NanoSmithBlastShield)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			val = abilityMod_NanoSmithBlastShield.m_healOnEndIfHasRemainingAbsorbMod.GetModifiedValue(m_healOnEndIfHasRemainingAbsorb);
		}
		else
		{
			val = m_healOnEndIfHasRemainingAbsorb;
		}
		AddTokenInt(tokens, "HealOnEndIfHasRemainingAbsorb", empty, val);
		AddTokenInt(tokens, "EnergyGainOnShieldTarget", string.Empty, (!abilityMod_NanoSmithBlastShield) ? m_energyGainOnShieldTarget : abilityMod_NanoSmithBlastShield.m_energyGainOnShieldTargetMod.GetModifiedValue(m_energyGainOnShieldTarget));
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_NanoSmithBlastShield) ? m_extraEffectOnCasterIfTargetingAlly : abilityMod_NanoSmithBlastShield.m_extraEffectOnCasterIfTargetingAllyMod.GetModifiedValue(m_extraEffectOnCasterIfTargetingAlly), "ExtraEffectOnCasterIfTargetingAlly", m_extraEffectOnCasterIfTargetingAlly);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_NanoSmithBlastShield))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_NanoSmithBlastShield);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
