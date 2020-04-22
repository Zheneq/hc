using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalStealth : Ability
{
	public StandardActorEffectData m_selfEffect;

	public bool m_useCharge;

	[TextArea(1, 10)]
	public string m_notes;

	private AbilityMod_RobotAnimalStealth m_abilityMod;

	private void Start()
	{
		if (m_useCharge)
		{
			while (true)
			{
				switch (6)
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
			if (GetNumTargets() == 0)
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
				Debug.LogError("Robot Animal Stealth cannot use charge if there is no targeter targets specified");
				m_useCharge = false;
			}
		}
		SetupTargeter();
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (UseCharge())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return Mathf.Clamp(GetNumTargets(), 1, 2);
				}
			}
		}
		return 1;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetModdedStealthEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (UseCharge())
		{
			if (targetIndex == 0)
			{
				while (true)
				{
					int result;
					switch (6)
					{
					case 0:
						break;
					default:
						{
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
							if (boardSquareSafe != null)
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
								if (boardSquareSafe.IsBaselineHeight())
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									result = ((KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe) != null) ? 1 : 0);
									goto IL_0073;
								}
							}
							result = 0;
							goto IL_0073;
						}
						IL_0073:
						return (byte)result != 0;
					}
				}
			}
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[0].GridPos);
			BoardSquare boardSquareSafe3 = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (boardSquareSafe3 != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquareSafe2 != boardSquareSafe3)
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
					if (boardSquareSafe3.IsBaselineHeight() && KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe3, boardSquareSafe2, false) != null)
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
				}
			}
			return false;
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_selfEffect.AddTooltipTokens(tokens, "SelfEffect");
	}

	private void SetupTargeter()
	{
		if (UseCharge())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (GetExpectedNumberOfTargeters() < 2)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								base.Targeter = new AbilityUtil_Targeter_ChargeAoE(this, 0f, 0f, 0f, -1, false, false);
								return;
							}
						}
					}
					ClearTargeters();
					for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
					{
						base.Targeters.Add(new AbilityUtil_Targeter_ChargeAoE(this, 0f, 0f, 0f, -1, false, false));
						base.Targeters[i].SetUseMultiTargetUpdate(true);
					}
					return;
				}
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always);
		base.Targeter.ShowArcToShape = false;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RobotAnimalStealth))
		{
			m_abilityMod = (abilityMod as AbilityMod_RobotAnimalStealth);
			SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public StandardActorEffectData GetModdedStealthEffect()
	{
		StandardActorEffectData result;
		if (!(m_abilityMod == null))
		{
			if (m_abilityMod.m_selfEffectOverride.m_applyEffect)
			{
				result = m_abilityMod.m_selfEffectOverride.m_effectData;
				goto IL_004b;
			}
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
		}
		result = m_selfEffect;
		goto IL_004b;
		IL_004b:
		return result;
	}

	public bool ShouldApplyEffectOnNextDamageAttack()
	{
		return !(m_abilityMod == null) && m_abilityMod.m_effectOnNextDamageAttack.m_applyEffect;
	}

	public StandardEffectInfo GetEffectOnNextDamageAttack()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_effectOnNextDamageAttack : new StandardEffectInfo();
	}

	public int GetExtraDamageNextAttack()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (4)
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
			result = 0;
		}
		else
		{
			result = m_abilityMod.m_extraDamageNextAttack;
		}
		return result;
	}

	public bool UseCharge()
	{
		int result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (6)
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
			result = (m_useCharge ? 1 : 0);
		}
		else if (GetNumTargets() > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			result = ((m_abilityMod.m_useChainAbilityOverrides && m_abilityMod.m_chainAbilityOverrides.Length > 0) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}
}
