using System.Collections.Generic;
using UnityEngine;

public class ScoundrelEvasionRoll : Ability
{
	[Header("-- Energy gain per step")]
	public int m_extraEnergyPerStep;

	private AbilityMod_ScoundrelEvasionRoll m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Evasion Roll";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_ScoundrelEvasionRoll(this, ShouldCreateTrapWire(), GetTrapwirePattern());
		if (HasAdditionalEffectFromMod())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					(base.Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll).m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
					return;
				}
			}
		}
		if (EffectForLandingInBrush() == null)
		{
			return;
		}
		while (true)
		{
			(base.Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll).m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
			return;
		}
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.IsValidForGameplay())
			{
				if (boardSquareSafe != caster.GetCurrentBoardSquare())
				{
					return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe) != null;
				}
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		StandardEffectInfo standardEffectInfo = EffectForLandingInBrush();
		if (standardEffectInfo != null)
		{
			standardEffectInfo.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		if (HasAdditionalEffectFromMod())
		{
			m_abilityMod.m_additionalEffectOnStart.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		return numbers;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetExtraEnergyPerStep() > 0 && base.Targeter is AbilityUtil_Targeter_ScoundrelEvasionRoll)
		{
			AbilityUtil_Targeter_ScoundrelEvasionRoll abilityUtil_Targeter_ScoundrelEvasionRoll = base.Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll;
			int numNodesInPath = abilityUtil_Targeter_ScoundrelEvasionRoll.m_numNodesInPath;
			if (numNodesInPath > 1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return GetExtraEnergyPerStep() * (numNodesInPath - 1);
					}
				}
			}
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScoundrelEvasionRoll))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_ScoundrelEvasionRoll);
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

	public int GetExtraEnergyPerStep()
	{
		return (!m_abilityMod) ? m_extraEnergyPerStep : m_abilityMod.m_extraEnergyPerStepMod.GetModifiedValue(m_extraEnergyPerStep);
	}

	private bool ShouldCreateTrapWire()
	{
		int result;
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_dropTrapWireOnStart)
			{
				result = ((m_abilityMod.m_trapwirePattern != AbilityGridPattern.NoPattern) ? 1 : 0);
				goto IL_004c;
			}
		}
		result = 0;
		goto IL_004c;
		IL_004c:
		return (byte)result != 0;
	}

	private bool HasAdditionalEffectFromMod()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = (m_abilityMod.m_additionalEffectOnStart.m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private AbilityGridPattern GetTrapwirePattern()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_trapwirePattern : AbilityGridPattern.NoPattern;
	}

	private int TechPointGainPerAdjacentAlly()
	{
		return (m_abilityMod != null) ? m_abilityMod.m_techPointGainPerAdjacentAlly : 0;
	}

	private int TechPointGrantedToAdjacentAllies()
	{
		return (m_abilityMod != null) ? m_abilityMod.m_techPointGrantedToAdjacentAllies : 0;
	}

	private StandardEffectInfo EffectForLandingInBrush()
	{
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_effectToSelfForLandingInBrush != null)
			{
				if (m_abilityMod.m_effectToSelfForLandingInBrush.m_applyEffect)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return m_abilityMod.m_effectToSelfForLandingInBrush;
						}
					}
				}
			}
		}
		return null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ScoundrelEvasionRoll abilityMod_ScoundrelEvasionRoll = modAsBase as AbilityMod_ScoundrelEvasionRoll;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ScoundrelEvasionRoll)
		{
			val = abilityMod_ScoundrelEvasionRoll.m_extraEnergyPerStepMod.GetModifiedValue(m_extraEnergyPerStep);
		}
		else
		{
			val = m_extraEnergyPerStep;
		}
		AddTokenInt(tokens, "ExtraEnergyPerStep", empty, val);
		Passive_StickAndMove component = GetComponent<Passive_StickAndMove>();
		string empty2 = string.Empty;
		int val2;
		if (component != null)
		{
			if (component.m_damageToAdvanceCooldown > 0)
			{
				val2 = 1;
				goto IL_0088;
			}
		}
		val2 = 0;
		goto IL_0088;
		IL_0088:
		AddTokenInt(tokens, "CDR_OnDamageTaken", empty2, val2);
	}
}
