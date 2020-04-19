using System;
using System.Collections.Generic;
using UnityEngine;

public class SenseiDash : Ability
{
	[Header("-- Targeting")]
	public bool m_canTargetAlly = true;

	public bool m_canTargetEnemy;

	public AbilityAreaShape m_chooseDestinationShape = AbilityAreaShape.Three_x_Three;

	[Header("-- On Hit")]
	public int m_damageAmount = 0x14;

	public int m_healAmount = 0x14;

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
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.ClearTargeters();
		AbilityUtil_Targeter abilityUtil_Targeter;
		if (this.ShouldHitActorsInBetween())
		{
			abilityUtil_Targeter = new AbilityUtil_Targeter_ChargeAoE(this, this.GetRadiusAroundStartToHit(), this.GetRadiusAroundEndToHit(), 0.5f * this.GetChargeHitWidth(), -1, false, this.ChargeHitPenetrateLos());
			abilityUtil_Targeter.SetAffectedGroups(this.CanHitEnemy(), this.CanHitAlly(), false);
			AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = abilityUtil_Targeter as AbilityUtil_Targeter_ChargeAoE;
			if (SenseiDash.<>f__am$cache0 == null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiDash.SetupTargeter()).MethodHandle;
				}
				SenseiDash.<>f__am$cache0 = delegate(ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability)
				{
					bool result = false;
					SenseiDash senseiDash = ability as SenseiDash;
					if (senseiDash != null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle2 = methodof(SenseiDash.<SetupTargeter>m__0(ActorData, AbilityTarget, List<ActorData>, ActorData, Ability)).MethodHandle;
						}
						if (senseiDash.CanHitEnemy() && actorToConsider.\u000E() != caster.\u000E())
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							result = true;
						}
						if (senseiDash.CanHitAlly())
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (actorToConsider.\u000E() == caster.\u000E())
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
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
			abilityUtil_Targeter_ChargeAoE.m_shouldAddTargetDelegate = SenseiDash.<>f__am$cache0;
		}
		else
		{
			abilityUtil_Targeter = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, this.CanHitEnemy(), this.CanHitAlly());
		}
		base.Targeter = abilityUtil_Targeter;
	}

	private void SetCachedFields()
	{
		this.m_cachedEffectOnTargetEnemy = this.m_effectOnTargetEnemy;
		this.m_cachedEffectOnTargetAlly = this.m_effectOnTargetAlly;
	}

	public bool CanTargetAlly()
	{
		return this.m_canTargetAlly;
	}

	public bool CanTargetEnemy()
	{
		return this.m_canTargetEnemy;
	}

	public bool CanHitAlly()
	{
		bool result;
		if (this.GetHealAmount() <= 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiDash.CanHitAlly()).MethodHandle;
			}
			result = this.GetEffectOnTargetAlly().m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool CanHitEnemy()
	{
		return this.GetDamageAmount() > 0 || this.GetEffectOnTargetEnemy().m_applyEffect;
	}

	public AbilityAreaShape GetChooseDestinationShape()
	{
		return this.m_chooseDestinationShape;
	}

	public int GetDamageAmount()
	{
		return this.m_damageAmount;
	}

	public int GetHealAmount()
	{
		return this.m_healAmount;
	}

	public StandardEffectInfo GetEffectOnTargetEnemy()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnTargetEnemy != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiDash.GetEffectOnTargetEnemy()).MethodHandle;
			}
			result = this.m_cachedEffectOnTargetEnemy;
		}
		else
		{
			result = this.m_effectOnTargetEnemy;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnTargetAlly()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnTargetAlly != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiDash.GetEffectOnTargetAlly()).MethodHandle;
			}
			result = this.m_cachedEffectOnTargetAlly;
		}
		else
		{
			result = this.m_effectOnTargetAlly;
		}
		return result;
	}

	public bool ShouldHitActorsInBetween()
	{
		return this.m_hitActorsInBetween;
	}

	public float GetChargeHitWidth()
	{
		return this.m_chargeHitWidth;
	}

	public float GetRadiusAroundStartToHit()
	{
		return this.m_radiusAroundStartToHit;
	}

	public float GetRadiusAroundEndToHit()
	{
		return this.m_radiusAroundEndToHit;
	}

	public bool ChargeHitPenetrateLos()
	{
		return this.m_chargeHitPenetrateLos;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		base.AddTokenInt(tokens, "HealAmount", string.Empty, this.m_healAmount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnTargetEnemy, "EffectOnTargetEnemy", this.m_effectOnTargetEnemy, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnTargetAlly, "EffectOnTargetAlly", this.m_effectOnTargetAlly, true);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageAmount());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Ally, this.GetHealAmount());
		if (this.GetEffectOnTargetAlly() != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiDash.CalculateNameplateTargetingNumbers()).MethodHandle;
			}
			this.GetEffectOnTargetAlly().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		}
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiDash.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			bool flag = true;
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				dictionary[AbilityTooltipSymbol.Damage] = ((!flag) ? 0 : this.GetDamageAmount());
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
			{
				Dictionary<AbilityTooltipSymbol, int> dictionary2 = dictionary;
				AbilityTooltipSymbol key = AbilityTooltipSymbol.Healing;
				int value;
				if (flag)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					value = this.GetHealAmount();
				}
				else
				{
					value = 0;
				}
				dictionary2[key] = value;
			}
		}
		return dictionary;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return base.HasTargetableActorsInDecision(caster, this.CanTargetEnemy(), this.CanTargetAlly(), false, Ability.ValidateCheckPath.CanBuildPath, true, false, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (!(boardSquare == null))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SenseiDash.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquare.\u0016())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!(boardSquare == caster.\u0012()))
				{
					bool flag = false;
					bool flag2 = false;
					List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.GetChooseDestinationShape(), target, false, caster, TargeterUtils.GetRelevantTeams(caster, this.CanTargetAlly(), this.CanTargetEnemy()), null);
					using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData targetActor = enumerator.Current;
							if (base.CanTargetActorInDecision(caster, targetActor, this.CanTargetEnemy(), this.CanTargetAlly(), false, Ability.ValidateCheckPath.Ignore, true, false, false))
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								flag = true;
								goto IL_F1;
							}
						}
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					IL_F1:
					BoardSquare boardSquare2 = Board.\u000E().\u000E(target.GridPos);
					if (boardSquare2 != null)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (boardSquare2.\u0016())
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (boardSquare2 != caster.\u0012())
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								int num;
								flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquare2, caster.\u0012(), false, out num);
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
