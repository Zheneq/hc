using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BattleMonkBuffCharge_Prep : Ability
{
	[Separator("Targeting", true)]
	public AbilityAreaShape m_buffAlliesShape = AbilityAreaShape.Three_x_Three;

	[Header("-- How far can ally be to be considered valid target. Range in Target Data should be large")]
	public float m_allySelectRadius = 8.5f;

	public bool m_mustHitAllies = true;

	public bool m_buffAoePenetratesLoS;

	[Separator("Ally Hit Effect (for Enemy hit see Chain ability)", true)]
	public StandardEffectInfo m_allyBuff;

	public StandardEffectInfo m_selfBuff;

	[Separator("Sequence", true)]
	public float m_sequenceProjectileGroundOffset;

	public GameObject m_swordThrowSequencePrefab;

	public GameObject m_castOnSelfSequencePrefab;

	private AbilityMod_BattleMonkBuffCharge_Prep m_abilityMod;

	private BattleMonkBuffCharge_Dash m_dashAbility;

	private int m_cachedAbsorbOnSelf;

	private int m_cachedAbsorbOnAlly;

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.None;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_allyBuff.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		m_selfBuff.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		if (m_chainAbilities.Length > 0)
		{
			Ability ability = m_chainAbilities[0];
			if (ability is BattleMonkBuffCharge_Dash)
			{
				BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = ability as BattleMonkBuffCharge_Dash;
				battleMonkBuffCharge_Dash.SetTooltip();
				List<AbilityTooltipNumber> abilityTooltipNumbers = battleMonkBuffCharge_Dash.GetAbilityTooltipNumbers();
				using (List<AbilityTooltipNumber>.Enumerator enumerator = abilityTooltipNumbers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityTooltipNumber current = enumerator.Current;
						numbers.Add(current);
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							if (true)
							{
								return numbers;
							}
							/*OpCode not supported: LdMemberToken*/;
							return numbers;
						}
					}
				}
			}
		}
		return numbers;
	}

	private void Start()
	{
		if (m_chainAbilities.Length > 0)
		{
			Ability ability = m_chainAbilities[0];
			if (ability is BattleMonkBuffCharge_Dash)
			{
				m_dashAbility = (ability as BattleMonkBuffCharge_Dash);
			}
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_dashAbility == null)
		{
			if (m_chainAbilities.Length > 0)
			{
				Ability ability = m_chainAbilities[0];
				if (ability is BattleMonkBuffCharge_Dash)
				{
					m_dashAbility = (ability as BattleMonkBuffCharge_Dash);
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_BattleMonkUltimate(this, GetAllyHitShape(), m_buffAoePenetratesLoS, GetEnemyHitShape(), GetDamageAoePenetrateLos(), false);
		bool affectsEnemies = GetModdedDamage() > 0 || (m_dashAbility != null && m_dashAbility.m_enemyDebuff.m_applyEffect);
		base.Targeter.SetAffectedGroups(affectsEnemies, true, true);
		StandardEffectInfo selfBuffEffect = GetSelfBuffEffect();
		if (selfBuffEffect.m_applyEffect)
		{
			m_cachedAbsorbOnSelf = selfBuffEffect.m_effectData.m_absorbAmount;
		}
		StandardEffectInfo selfBuffEffect2 = GetSelfBuffEffect();
		if (selfBuffEffect2.m_applyEffect)
		{
			m_cachedAbsorbOnAlly = selfBuffEffect2.m_effectData.m_absorbAmount;
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return GetAllySelectRadius() < 20f;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetAllySelectRadius();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (GetRequireHitAlly())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					List<ActorData> actors = AreaEffectUtils.GetActorsInRadius(caster.GetTravelBoardSquareWorldPosition(), GetAllySelectRadius(), m_buffAoePenetratesLoS, caster, caster.GetTeam(), null);
					actors.Remove(caster);
					if (NetworkClient.active)
					{
						TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
					}
					TargeterUtils.RemoveActorsInvisibleToActor(ref actors, caster);
					return actors.Count > 0;
				}
				}
			}
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (!(boardSquareSafe == null) && boardSquareSafe.IsBaselineHeight())
		{
			if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
			{
				bool result = false;
				if (GetRequireHitAlly())
				{
					List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(caster.GetTravelBoardSquareWorldPosition(), GetAllySelectRadius(), m_buffAoePenetratesLoS, caster, caster.GetTeam(), null);
					actorsInRadius.Remove(caster);
					int num = 0;
					while (true)
					{
						if (num < actorsInRadius.Count)
						{
							ActorData actorData = actorsInRadius[num];
							bool num2;
							if (NetworkClient.active)
							{
								num2 = actorData.IsVisibleToClient();
							}
							else
							{
								num2 = actorData.IsActorVisibleToActor(caster);
							}
							if (num2)
							{
								BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
								if (currentBoardSquare != null)
								{
									if (AreaEffectUtils.IsSquareInShape(boardSquareSafe, m_buffAlliesShape, currentBoardSquare.ToVector3(), currentBoardSquare, m_buffAoePenetratesLoS, caster))
									{
										result = true;
										break;
									}
								}
							}
							num++;
							continue;
						}
						break;
					}
				}
				else
				{
					result = true;
				}
				return result;
			}
		}
		return false;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				dictionary[AbilityTooltipSymbol.Damage] = GetModdedDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				dictionary[AbilityTooltipSymbol.Absorb] = GetModdedAbsorbOnSelf();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
			{
				dictionary[AbilityTooltipSymbol.Absorb] = GetModdedAbsorbOnAlly();
			}
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkBuffCharge_Prep abilityMod_BattleMonkBuffCharge_Prep = modAsBase as AbilityMod_BattleMonkBuffCharge_Prep;
		if (m_chainAbilities.Length <= 0)
		{
			return;
		}
		while (true)
		{
			Ability ability = m_chainAbilities[0];
			if (!(ability is BattleMonkBuffCharge_Dash))
			{
				return;
			}
			while (true)
			{
				BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = ability as BattleMonkBuffCharge_Dash;
				string empty = string.Empty;
				int val;
				if ((bool)abilityMod_BattleMonkBuffCharge_Prep)
				{
					val = abilityMod_BattleMonkBuffCharge_Prep.m_damageMod.GetModifiedValue(battleMonkBuffCharge_Dash.m_damage);
				}
				else
				{
					val = battleMonkBuffCharge_Dash.m_damage;
				}
				AddTokenInt(tokens, "Damage", empty, val);
				StandardEffectInfo effectInfo;
				if ((bool)abilityMod_BattleMonkBuffCharge_Prep)
				{
					effectInfo = abilityMod_BattleMonkBuffCharge_Prep.m_selfEffectOverride.GetModifiedValue(m_selfBuff);
				}
				else
				{
					effectInfo = m_selfBuff;
				}
				AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "SelfEffect", m_selfBuff);
				StandardEffectInfo effectInfo2;
				if ((bool)abilityMod_BattleMonkBuffCharge_Prep)
				{
					effectInfo2 = abilityMod_BattleMonkBuffCharge_Prep.m_allyEffectOverride.GetModifiedValue(m_allyBuff);
				}
				else
				{
					effectInfo2 = m_allyBuff;
				}
				AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyEffect", m_allyBuff);
				return;
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BattleMonkBuffCharge_Prep))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_abilityMod = (abilityMod as AbilityMod_BattleMonkBuffCharge_Prep);
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

	public float GetAllySelectRadius()
	{
		return (!m_abilityMod) ? m_allySelectRadius : m_abilityMod.m_allySelectRadiusMod.GetModifiedValue(m_allySelectRadius);
	}

	public bool GetRequireHitAlly()
	{
		bool result;
		if (m_abilityMod == null)
		{
			result = m_mustHitAllies;
		}
		else
		{
			result = m_abilityMod.m_requireHitAlliesMod.GetModifiedValue(m_mustHitAllies);
		}
		return result;
	}

	public AbilityAreaShape GetAllyHitShape()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_allyShapeMod.GetModifiedValue(m_buffAlliesShape) : m_buffAlliesShape;
	}

	public bool GetDamageAoePenetrateLos()
	{
		int result;
		if (m_dashAbility == null)
		{
			result = 0;
		}
		else
		{
			result = (m_dashAbility.m_damageAoePenetratesLoS ? 1 : 0);
		}
		return (byte)result != 0;
	}

	public StandardEffectInfo GetSelfBuffEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
		{
			result = m_selfBuff;
		}
		else
		{
			result = m_abilityMod.m_selfEffectOverride.GetModifiedValue(m_selfBuff);
		}
		return result;
	}

	public StandardEffectInfo GetAllyBuffEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
		{
			result = m_allyBuff;
		}
		else
		{
			result = m_abilityMod.m_allyEffectOverride.GetModifiedValue(m_allyBuff);
		}
		return result;
	}

	public bool ShouldRemoveAllNegativeStatusFromAllies()
	{
		int result;
		if (m_abilityMod != null)
		{
			result = (m_abilityMod.m_removeAllNegativeStatusFromAllies ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public int GetModdedAbsorbOnSelf()
	{
		return m_cachedAbsorbOnSelf;
	}

	public int GetModdedAbsorbOnAlly()
	{
		return m_cachedAbsorbOnAlly;
	}

	public AbilityAreaShape GetEnemyHitShape()
	{
		AbilityAreaShape abilityAreaShape = m_buffAlliesShape;
		if (m_dashAbility != null)
		{
			abilityAreaShape = m_dashAbility.m_damageEnemiesShape;
		}
		if (m_abilityMod != null)
		{
			abilityAreaShape = m_abilityMod.m_enemyShapeMod.GetModifiedValue(abilityAreaShape);
		}
		return abilityAreaShape;
	}

	public int GetModdedDamage()
	{
		int num = 0;
		if (m_dashAbility != null)
		{
			num = m_dashAbility.m_damage;
		}
		int result = num;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(num);
		}
		return result;
	}
}
