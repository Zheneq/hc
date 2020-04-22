using System.Collections.Generic;
using UnityEngine;

public class SenseiYingYangDash : Ability
{
	[Separator("Custom colors for Ability Bar Icon", true)]
	public Color m_colorForAllyDash = new Color(0f, 1f, 1f);

	public Color m_colorForEnemyDash = new Color(1f, 0f, 1f);

	[Separator("Targeting Info", "cyan")]
	public bool m_chooseDestination;

	public AbilityAreaShape m_chooseDestShape = AbilityAreaShape.Three_x_Three;

	public bool m_useActorAtSquareBeforeEvadeIfMiss = true;

	[Separator("For Second Dash", "cyan")]
	public int m_secondCastTurns = 1;

	public bool m_secondDashAllowBothTeams;

	[Separator("On Enemy Hit", true)]
	public int m_damage = 10;

	public StandardEffectInfo m_enemyHitEffect;

	public int m_extraDamageForDiffTeamSecondDash;

	public int m_extraDamageForLowHealth;

	public float m_enemyLowHealthThresh;

	public bool m_reverseHealthThreshForEnemy;

	[Separator("On Ally Hit", true)]
	public int m_healOnAlly = 10;

	public StandardEffectInfo m_allyHitEffect;

	public int m_extraHealOnAllyForDiffTeamSecondDash;

	public int m_extraHealOnAllyForLowHealth;

	public float m_allyLowHealthThresh;

	public bool m_reverseHealthThreshForAlly;

	[Header("-- whether to heal allies who dashed away")]
	public bool m_healAllyWhoDashedAway;

	[Header("-- Cooldown reduction")]
	public int m_cdrIfNoSecondDash;

	[Header("-- Sequences --")]
	public GameObject m_castOnAllySequencePrefab;

	public GameObject m_castOnEnemySequencePrefab;

	private AbilityMod_SenseiYingYangDash m_abilityMod;

	private Sensei_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedAllyHitEffect;

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
			m_abilityName = "SenseiYingYangDash";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_syncComp = GetComponent<Sensei_SyncComponent>();
		AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, true, true);
		abilityUtil_Targeter_Charge.m_affectCasterDelegate = IncludeCasterInTargeter;
		if (ChooseDestinaton())
		{
			while (true)
			{
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
					base.Targeters.Add(abilityUtil_Targeter_Charge);
					AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge2 = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, false);
					abilityUtil_Targeter_Charge2.SetUseMultiTargetUpdate(true);
					base.Targeters.Add(abilityUtil_Targeter_Charge2);
					return;
				}
				}
			}
		}
		base.Targeter = abilityUtil_Targeter_Charge;
	}

	private bool IncludeCasterInTargeter(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
	{
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		if (moddedEffectForSelf != null)
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
			if (moddedEffectForSelf.m_applyEffect)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		return false;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public bool ChooseDestinaton()
	{
		int result;
		if (m_chooseDestination)
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
			result = ((m_targetData.Length > 1) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int result;
		if (ChooseDestinaton())
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
			result = 2;
		}
		else
		{
			result = 1;
		}
		return result;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = ((!m_abilityMod) ? m_enemyHitEffect : m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect));
		StandardEffectInfo cachedAllyHitEffect;
		if ((bool)m_abilityMod)
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
			cachedAllyHitEffect = m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = m_allyHitEffect;
		}
		m_cachedAllyHitEffect = cachedAllyHitEffect;
	}

	public AbilityAreaShape GetChooseDestShape()
	{
		AbilityAreaShape result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_chooseDestShapeMod.GetModifiedValue(m_chooseDestShape);
		}
		else
		{
			result = m_chooseDestShape;
		}
		return result;
	}

	public int GetSecondCastTurns()
	{
		int result;
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
			result = m_abilityMod.m_secondCastTurnsMod.GetModifiedValue(m_secondCastTurns);
		}
		else
		{
			result = m_secondCastTurns;
		}
		return result;
	}

	public bool SecondDashAllowBothTeams()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_secondDashAllowBothTeamsMod.GetModifiedValue(m_secondDashAllowBothTeams);
		}
		else
		{
			result = m_secondDashAllowBothTeams;
		}
		return result;
	}

	public int GetDamage()
	{
		return (!m_abilityMod) ? m_damage : m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
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
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public int GetExtraDamageForDiffTeamSecondDash()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_extraDamageForDiffTeamSecondDashMod.GetModifiedValue(m_extraDamageForDiffTeamSecondDash);
		}
		else
		{
			result = m_extraDamageForDiffTeamSecondDash;
		}
		return result;
	}

	public int GetExtraDamageForLowHealth()
	{
		return (!m_abilityMod) ? m_extraDamageForLowHealth : m_abilityMod.m_extraDamageForLowHealthMod.GetModifiedValue(m_extraDamageForLowHealth);
	}

	public float GetEnemyLowHealthThresh()
	{
		float result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_enemyLowHealthThreshMod.GetModifiedValue(m_enemyLowHealthThresh);
		}
		else
		{
			result = m_enemyLowHealthThresh;
		}
		return result;
	}

	public bool ReverseHealthThreshForEnemy()
	{
		return (!m_abilityMod) ? m_reverseHealthThreshForEnemy : m_abilityMod.m_reverseHealthThreshForEnemyMod.GetModifiedValue(m_reverseHealthThreshForEnemy);
	}

	public int GetHealOnAlly()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_healOnAllyMod.GetModifiedValue(m_healOnAlly);
		}
		else
		{
			result = m_healOnAlly;
		}
		return result;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyHitEffect != null)
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
			result = m_cachedAllyHitEffect;
		}
		else
		{
			result = m_allyHitEffect;
		}
		return result;
	}

	public int GetExtraHealOnAllyForDiffTeamSecondDash()
	{
		return (!m_abilityMod) ? m_extraHealOnAllyForDiffTeamSecondDash : m_abilityMod.m_extraHealOnAllyForDiffTeamSecondDashMod.GetModifiedValue(m_extraHealOnAllyForDiffTeamSecondDash);
	}

	public int GetExtraHealOnAllyForLowHealth()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_extraHealOnAllyForLowHealthMod.GetModifiedValue(m_extraHealOnAllyForLowHealth);
		}
		else
		{
			result = m_extraHealOnAllyForLowHealth;
		}
		return result;
	}

	public float GetAllyLowHealthThresh()
	{
		float result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_allyLowHealthThreshMod.GetModifiedValue(m_allyLowHealthThresh);
		}
		else
		{
			result = m_allyLowHealthThresh;
		}
		return result;
	}

	public bool ReverseHealthThreshForAlly()
	{
		bool result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_reverseHealthThreshForAllyMod.GetModifiedValue(m_reverseHealthThreshForAlly);
		}
		else
		{
			result = m_reverseHealthThreshForAlly;
		}
		return result;
	}

	public int GetCdrIfNoSecondDash()
	{
		int result;
		if ((bool)m_abilityMod)
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
			result = m_abilityMod.m_cdrIfNoSecondDashMod.GetModifiedValue(m_cdrIfNoSecondDash);
		}
		else
		{
			result = m_cdrIfNoSecondDash;
		}
		return result;
	}

	public int GetCurrentAllyHeal(ActorData allyActor, ActorData caster)
	{
		int num = GetHealOnAlly();
		if (allyActor != null)
		{
			bool flag = allyActor.GetHitPointShareOfMax() < GetAllyLowHealthThresh();
			if (ReverseHealthThreshForAlly())
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
				flag = (allyActor.GetHitPointShareOfMax() > GetAllyLowHealthThresh());
			}
			if (GetExtraHealOnAllyForLowHealth() > 0 && GetAllyLowHealthThresh() > 0f)
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
				if (flag)
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
					num += GetExtraHealOnAllyForLowHealth();
				}
			}
			if (ShouldApplyBonusForDiffTeam(allyActor, caster) && GetExtraHealOnAllyForDiffTeamSecondDash() > 0)
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
				num += GetExtraHealOnAllyForDiffTeamSecondDash();
			}
		}
		return num;
	}

	public int GetCurrentDamage(ActorData enemyActor, ActorData caster)
	{
		int num = GetDamage();
		if (enemyActor != null)
		{
			bool flag = enemyActor.GetHitPointShareOfMax() < GetEnemyLowHealthThresh();
			if (ReverseHealthThreshForEnemy())
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
				flag = (enemyActor.GetHitPointShareOfMax() > GetEnemyLowHealthThresh());
			}
			if (GetExtraDamageForLowHealth() > 0)
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
				if (GetEnemyLowHealthThresh() > 0f)
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
					if (flag)
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
						num += GetExtraDamageForLowHealth();
					}
				}
			}
			if (ShouldApplyBonusForDiffTeam(enemyActor, caster) && GetExtraDamageForDiffTeamSecondDash() > 0)
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
				num += GetExtraDamageForDiffTeamSecondDash();
			}
		}
		return num;
	}

	public bool CanTargetEnemy()
	{
		int result;
		if (m_syncComp != null)
		{
			bool flag = IsForSecondDash();
			if (flag)
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
				if (!SecondDashAllowBothTeams())
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
					if (flag)
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
						result = (m_syncComp.m_syncLastYingYangDashedToAlly ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					goto IL_0061;
				}
			}
			result = 1;
			goto IL_0061;
		}
		return true;
		IL_0061:
		return (byte)result != 0;
	}

	public bool CanTargetAlly()
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				int result;
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
						bool flag = IsForSecondDash();
						if (flag)
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
							if (!SecondDashAllowBothTeams())
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
								if (flag)
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
									result = ((!m_syncComp.m_syncLastYingYangDashedToAlly) ? 1 : 0);
								}
								else
								{
									result = 0;
								}
								goto IL_006e;
							}
						}
						result = 1;
						goto IL_006e;
					}
					IL_006e:
					return (byte)result != 0;
				}
			}
		}
		return true;
	}

	private bool IsForSecondDash()
	{
		return m_syncComp != null && m_syncComp.m_syncTurnsForSecondYingYangDash > 0;
	}

	private bool ShouldApplyBonusForDiffTeam(ActorData targetActor, ActorData caster)
	{
		if (IsForSecondDash())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					bool flag = targetActor.GetTeam() == caster.GetTeam();
					return m_syncComp.m_syncLastYingYangDashedToAlly != flag;
				}
				}
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damage);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, m_healOnAlly);
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		AppendTooltipNumbersFromBaseModEffects(ref numbers);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		ActorData actorData = base.ActorData;
		if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) > 0)
		{
			results.m_healing = GetCurrentAllyHeal(targetActor, actorData);
		}
		else if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
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
			results.m_damage = GetCurrentDamage(targetActor, actorData);
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "SecondCastTurns", string.Empty, m_secondCastTurns);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "ExtraDamageForDiffTeamSecondDash", string.Empty, m_extraDamageForDiffTeamSecondDash);
		AddTokenInt(tokens, "ExtraDamageForLowHealth", string.Empty, m_extraDamageForLowHealth);
		AddTokenInt(tokens, "HealOnAlly", string.Empty, m_healOnAlly);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AddTokenInt(tokens, "ExtraHealOnAllyForDiffTeamSecondDash", string.Empty, m_extraHealOnAllyForDiffTeamSecondDash);
		AddTokenInt(tokens, "ExtraHealOnAllyForLowHealth", string.Empty, m_extraHealOnAllyForLowHealth);
		AddTokenInt(tokens, "CdrIfNoSecondDash", string.Empty, m_cdrIfNoSecondDash);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SenseiYingYangDash))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_SenseiYingYangDash);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(caster, CanTargetEnemy(), CanTargetAlly(), false, ValidateCheckPath.CanBuildPath, true, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		bool flag2 = false;
		if (targetIndex == 0)
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
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (boardSquareSafe != null)
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
				if (boardSquareSafe.OccupantActor != null)
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
					if (CanTargetActorInDecision(caster, boardSquareSafe.OccupantActor, CanTargetEnemy(), CanTargetAlly(), false, ValidateCheckPath.CanBuildPath, true, false))
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
						flag = true;
						flag2 = true;
					}
				}
			}
		}
		else
		{
			flag = true;
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 1].GridPos);
			BoardSquare boardSquareSafe3 = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (boardSquareSafe3 != null && boardSquareSafe3.IsBaselineHeight())
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
				if (boardSquareSafe3 != boardSquareSafe2)
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
					if (boardSquareSafe3 != caster.GetCurrentBoardSquare())
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
						if (AreaEffectUtils.IsSquareInShape(boardSquareSafe3, GetChooseDestShape(), target.FreePos, boardSquareSafe2, false, caster))
						{
							flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe3, boardSquareSafe2, false, out int _);
						}
					}
				}
			}
		}
		int result;
		if (flag2)
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
			result = (flag ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override bool UseCustomAbilityIconColor()
	{
		int result;
		if (m_syncComp != null)
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
			result = ((m_syncComp.m_syncTurnsForSecondYingYangDash > 0) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override Color GetCustomAbilityIconColor(ActorData actor)
	{
		if (m_syncComp != null)
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
			if (m_syncComp.m_syncTurnsForSecondYingYangDash > 0)
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
				bool flag = CanTargetAlly();
				bool flag2 = CanTargetEnemy();
				if (flag)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						return m_colorForAllyDash;
					}
				}
				if (flag2)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return m_colorForEnemyDash;
						}
					}
				}
			}
		}
		return Color.white;
	}
}
