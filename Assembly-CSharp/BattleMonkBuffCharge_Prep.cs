using System;
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
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		this.m_allyBuff.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Ally);
		this.m_selfBuff.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Self);
		if (this.m_chainAbilities.Length > 0)
		{
			Ability ability = this.m_chainAbilities[0];
			if (ability is BattleMonkBuffCharge_Dash)
			{
				BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = ability as BattleMonkBuffCharge_Dash;
				battleMonkBuffCharge_Dash.SetTooltip();
				List<AbilityTooltipNumber> abilityTooltipNumbers = battleMonkBuffCharge_Dash.GetAbilityTooltipNumbers();
				using (List<AbilityTooltipNumber>.Enumerator enumerator = abilityTooltipNumbers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityTooltipNumber item = enumerator.Current;
						list.Add(item);
					}
				}
			}
		}
		return list;
	}

	private void Start()
	{
		if (this.m_chainAbilities.Length > 0)
		{
			Ability ability = this.m_chainAbilities[0];
			if (ability is BattleMonkBuffCharge_Dash)
			{
				this.m_dashAbility = (ability as BattleMonkBuffCharge_Dash);
			}
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.m_dashAbility == null)
		{
			if (this.m_chainAbilities.Length > 0)
			{
				Ability ability = this.m_chainAbilities[0];
				if (ability is BattleMonkBuffCharge_Dash)
				{
					this.m_dashAbility = (ability as BattleMonkBuffCharge_Dash);
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_BattleMonkUltimate(this, this.GetAllyHitShape(), this.m_buffAoePenetratesLoS, this.GetEnemyHitShape(), this.GetDamageAoePenetrateLos(), false);
		bool affectsEnemies = this.GetModdedDamage() > 0 || (this.m_dashAbility != null && this.m_dashAbility.m_enemyDebuff.m_applyEffect);
		base.Targeter.SetAffectedGroups(affectsEnemies, true, true);
		StandardEffectInfo selfBuffEffect = this.GetSelfBuffEffect();
		if (selfBuffEffect.m_applyEffect)
		{
			this.m_cachedAbsorbOnSelf = selfBuffEffect.m_effectData.m_absorbAmount;
		}
		StandardEffectInfo selfBuffEffect2 = this.GetSelfBuffEffect();
		if (selfBuffEffect2.m_applyEffect)
		{
			this.m_cachedAbsorbOnAlly = selfBuffEffect2.m_effectData.m_absorbAmount;
		}
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return this.GetAllySelectRadius() < 20f;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetAllySelectRadius();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.GetRequireHitAlly())
		{
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(caster.GetTravelBoardSquareWorldPosition(), this.GetAllySelectRadius(), this.m_buffAoePenetratesLoS, caster, caster.GetTeam(), null, false, default(Vector3));
			actorsInRadius.Remove(caster);
			if (NetworkClient.active)
			{
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadius);
			}
			TargeterUtils.RemoveActorsInvisibleToActor(ref actorsInRadius, caster);
			return actorsInRadius.Count > 0;
		}
		return true;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null) && boardSquareSafe.IsBaselineHeight())
		{
			if (!(boardSquareSafe == caster.GetCurrentBoardSquare()))
			{
				bool result = false;
				if (this.GetRequireHitAlly())
				{
					List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(caster.GetTravelBoardSquareWorldPosition(), this.GetAllySelectRadius(), this.m_buffAoePenetratesLoS, caster, caster.GetTeam(), null, false, default(Vector3));
					actorsInRadius.Remove(caster);
					for (int i = 0; i < actorsInRadius.Count; i++)
					{
						ActorData actorData = actorsInRadius[i];
						bool flag;
						if (NetworkClient.active)
						{
							flag = actorData.IsVisibleToClient();
						}
						else
						{
							flag = actorData.IsActorVisibleToActor(caster, false);
						}
						bool flag2 = flag;
						if (flag2)
						{
							BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
							if (currentBoardSquare != null)
							{
								if (AreaEffectUtils.IsSquareInShape(boardSquareSafe, this.m_buffAlliesShape, currentBoardSquare.ToVector3(), currentBoardSquare, this.m_buffAoePenetratesLoS, caster))
								{
									return true;
								}
							}
						}
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
				dictionary[AbilityTooltipSymbol.Damage] = this.GetModdedDamage();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
			{
				dictionary[AbilityTooltipSymbol.Absorb] = this.GetModdedAbsorbOnSelf();
			}
			else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
			{
				dictionary[AbilityTooltipSymbol.Absorb] = this.GetModdedAbsorbOnAlly();
			}
			return dictionary;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkBuffCharge_Prep abilityMod_BattleMonkBuffCharge_Prep = modAsBase as AbilityMod_BattleMonkBuffCharge_Prep;
		if (this.m_chainAbilities.Length > 0)
		{
			Ability ability = this.m_chainAbilities[0];
			if (ability is BattleMonkBuffCharge_Dash)
			{
				BattleMonkBuffCharge_Dash battleMonkBuffCharge_Dash = ability as BattleMonkBuffCharge_Dash;
				string name = "Damage";
				string empty = string.Empty;
				int val;
				if (abilityMod_BattleMonkBuffCharge_Prep)
				{
					val = abilityMod_BattleMonkBuffCharge_Prep.m_damageMod.GetModifiedValue(battleMonkBuffCharge_Dash.m_damage);
				}
				else
				{
					val = battleMonkBuffCharge_Dash.m_damage;
				}
				base.AddTokenInt(tokens, name, empty, val, false);
				StandardEffectInfo effectInfo;
				if (abilityMod_BattleMonkBuffCharge_Prep)
				{
					effectInfo = abilityMod_BattleMonkBuffCharge_Prep.m_selfEffectOverride.GetModifiedValue(this.m_selfBuff);
				}
				else
				{
					effectInfo = this.m_selfBuff;
				}
				AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "SelfEffect", this.m_selfBuff, true);
				StandardEffectInfo effectInfo2;
				if (abilityMod_BattleMonkBuffCharge_Prep)
				{
					effectInfo2 = abilityMod_BattleMonkBuffCharge_Prep.m_allyEffectOverride.GetModifiedValue(this.m_allyBuff);
				}
				else
				{
					effectInfo2 = this.m_allyBuff;
				}
				AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyEffect", this.m_allyBuff, true);
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BattleMonkBuffCharge_Prep))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_BattleMonkBuffCharge_Prep);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public float GetAllySelectRadius()
	{
		return (!this.m_abilityMod) ? this.m_allySelectRadius : this.m_abilityMod.m_allySelectRadiusMod.GetModifiedValue(this.m_allySelectRadius);
	}

	public bool GetRequireHitAlly()
	{
		bool result;
		if (this.m_abilityMod == null)
		{
			result = this.m_mustHitAllies;
		}
		else
		{
			result = this.m_abilityMod.m_requireHitAlliesMod.GetModifiedValue(this.m_mustHitAllies);
		}
		return result;
	}

	public AbilityAreaShape GetAllyHitShape()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_allyShapeMod.GetModifiedValue(this.m_buffAlliesShape) : this.m_buffAlliesShape;
	}

	public bool GetDamageAoePenetrateLos()
	{
		bool result;
		if (this.m_dashAbility == null)
		{
			result = false;
		}
		else
		{
			result = this.m_dashAbility.m_damageAoePenetratesLoS;
		}
		return result;
	}

	public StandardEffectInfo GetSelfBuffEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			result = this.m_selfBuff;
		}
		else
		{
			result = this.m_abilityMod.m_selfEffectOverride.GetModifiedValue(this.m_selfBuff);
		}
		return result;
	}

	public StandardEffectInfo GetAllyBuffEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			result = this.m_allyBuff;
		}
		else
		{
			result = this.m_abilityMod.m_allyEffectOverride.GetModifiedValue(this.m_allyBuff);
		}
		return result;
	}

	public bool ShouldRemoveAllNegativeStatusFromAllies()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_removeAllNegativeStatusFromAllies;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public int GetModdedAbsorbOnSelf()
	{
		return this.m_cachedAbsorbOnSelf;
	}

	public int GetModdedAbsorbOnAlly()
	{
		return this.m_cachedAbsorbOnAlly;
	}

	public AbilityAreaShape GetEnemyHitShape()
	{
		AbilityAreaShape abilityAreaShape = this.m_buffAlliesShape;
		if (this.m_dashAbility != null)
		{
			abilityAreaShape = this.m_dashAbility.m_damageEnemiesShape;
		}
		if (this.m_abilityMod != null)
		{
			abilityAreaShape = this.m_abilityMod.m_enemyShapeMod.GetModifiedValue(abilityAreaShape);
		}
		return abilityAreaShape;
	}

	public int GetModdedDamage()
	{
		int num = 0;
		if (this.m_dashAbility != null)
		{
			num = this.m_dashAbility.m_damage;
		}
		int result = num;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(num);
		}
		return result;
	}
}
