using System.Collections.Generic;
using UnityEngine;

public class SenseiDash : Ability
{
	[Header("-- Targeting")]
	public bool m_canTargetAlly = true;

	public bool m_canTargetEnemy;

	public AbilityAreaShape m_chooseDestinationShape = AbilityAreaShape.Three_x_Three;

	[Header("-- On Hit")]
	public int m_damageAmount = 20;

	public int m_healAmount = 20;

	public StandardEffectInfo m_effectOnTargetEnemy;

	public StandardEffectInfo m_effectOnTargetAlly;

	[Header("-- If Hitting Targets In Between")]
	public bool m_hitActorsInBetween = true;

	public float m_chargeHitWidth = 1f;

	public float m_radiusAroundStartToHit;

	public float m_radiusAroundEndToHit = 1.5f;

	public bool m_chargeHitPenetrateLos;

	[Header("-- Sequences --")]
	public GameObject m_hitSequencePrefab;

	private StandardEffectInfo m_cachedEffectOnTargetEnemy;

	private StandardEffectInfo m_cachedEffectOnTargetAlly;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		ClearTargeters();
		AbilityUtil_Targeter abilityUtil_Targeter = null;
		if (ShouldHitActorsInBetween())
		{
			abilityUtil_Targeter = new AbilityUtil_Targeter_ChargeAoE(this, GetRadiusAroundStartToHit(), GetRadiusAroundEndToHit(), 0.5f * GetChargeHitWidth(), -1, false, ChargeHitPenetrateLos());
			abilityUtil_Targeter.SetAffectedGroups(CanHitEnemy(), CanHitAlly(), false);
			AbilityUtil_Targeter_ChargeAoE obj = abilityUtil_Targeter as AbilityUtil_Targeter_ChargeAoE;
			
			obj.m_shouldAddTargetDelegate = delegate(ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability)
				{
					bool result = false;
					SenseiDash senseiDash = ability as SenseiDash;
					if (senseiDash != null)
					{
						if (senseiDash.CanHitEnemy() && actorToConsider.GetTeam() != caster.GetTeam())
						{
							result = true;
						}
						if (senseiDash.CanHitAlly())
						{
							if (actorToConsider.GetTeam() == caster.GetTeam())
							{
								result = true;
							}
						}
					}
					else
					{
						result = true;
					}
					return result;
				};
		}
		else
		{
			abilityUtil_Targeter = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, CanHitEnemy(), CanHitAlly());
		}
		base.Targeter = abilityUtil_Targeter;
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnTargetEnemy = m_effectOnTargetEnemy;
		m_cachedEffectOnTargetAlly = m_effectOnTargetAlly;
	}

	public bool CanTargetAlly()
	{
		return m_canTargetAlly;
	}

	public bool CanTargetEnemy()
	{
		return m_canTargetEnemy;
	}

	public bool CanHitAlly()
	{
		int result;
		if (GetHealAmount() <= 0)
		{
			result = (GetEffectOnTargetAlly().m_applyEffect ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public bool CanHitEnemy()
	{
		return GetDamageAmount() > 0 || GetEffectOnTargetEnemy().m_applyEffect;
	}

	public AbilityAreaShape GetChooseDestinationShape()
	{
		return m_chooseDestinationShape;
	}

	public int GetDamageAmount()
	{
		return m_damageAmount;
	}

	public int GetHealAmount()
	{
		return m_healAmount;
	}

	public StandardEffectInfo GetEffectOnTargetEnemy()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnTargetEnemy != null)
		{
			result = m_cachedEffectOnTargetEnemy;
		}
		else
		{
			result = m_effectOnTargetEnemy;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnTargetAlly()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnTargetAlly != null)
		{
			result = m_cachedEffectOnTargetAlly;
		}
		else
		{
			result = m_effectOnTargetAlly;
		}
		return result;
	}

	public bool ShouldHitActorsInBetween()
	{
		return m_hitActorsInBetween;
	}

	public float GetChargeHitWidth()
	{
		return m_chargeHitWidth;
	}

	public float GetRadiusAroundStartToHit()
	{
		return m_radiusAroundStartToHit;
	}

	public float GetRadiusAroundEndToHit()
	{
		return m_radiusAroundEndToHit;
	}

	public bool ChargeHitPenetrateLos()
	{
		return m_chargeHitPenetrateLos;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "HealAmount", string.Empty, m_healAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTargetEnemy, "EffectOnTargetEnemy", m_effectOnTargetEnemy);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnTargetAlly, "EffectOnTargetAlly", m_effectOnTargetAlly);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageAmount());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealAmount());
		if (GetEffectOnTargetAlly() != null)
		{
			GetEffectOnTargetAlly().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			bool flag = true;
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				dictionary[AbilityTooltipSymbol.Damage] = (flag ? GetDamageAmount() : 0);
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
			{
				Dictionary<AbilityTooltipSymbol, int> dictionary2 = dictionary;
				int value;
				if (flag)
				{
					value = GetHealAmount();
				}
				else
				{
					value = 0;
				}
				dictionary2[AbilityTooltipSymbol.Healing] = value;
			}
		}
		return dictionary;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(caster, CanTargetEnemy(), CanTargetAlly(), false, ValidateCheckPath.CanBuildPath, true, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsValidForGameplay())
			{
				if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
				{
					bool flag = false;
					bool flag2 = false;
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(GetChooseDestinationShape(), target, false, caster, TargeterUtils.GetRelevantTeams(caster, CanTargetAlly(), CanTargetEnemy()), null);
					using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
					{
						while (true)
						{
							if (!enumerator.MoveNext())
							{
								break;
							}
							ActorData current = enumerator.Current;
							if (CanTargetActorInDecision(caster, current, CanTargetEnemy(), CanTargetAlly(), false, ValidateCheckPath.Ignore, true, false))
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										flag = true;
										goto end_IL_0093;
									}
								}
							}
						}
						end_IL_0093:;
					}
					BoardSquare boardSquareSafe2 = Board.Get().GetSquare(target.GridPos);
					if (boardSquareSafe2 != null)
					{
						if (boardSquareSafe2.IsValidForGameplay())
						{
							if (boardSquareSafe2 != caster.GetCurrentBoardSquare())
							{
								flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe2, caster.GetCurrentBoardSquare(), false, out int _);
							}
						}
					}
					return flag2 && flag;
				}
			}
		}
		return false;
	}
}
