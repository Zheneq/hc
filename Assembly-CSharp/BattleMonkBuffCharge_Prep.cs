using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BattleMonkBuffCharge_Prep : Ability
{
	[Separator("Targeting")]
	public AbilityAreaShape m_buffAlliesShape = AbilityAreaShape.Three_x_Three;
	[Header("-- How far can ally be to be considered valid target. Range in Target Data should be large")]
	public float m_allySelectRadius = 8.5f;
	public bool m_mustHitAllies = true;
	public bool m_buffAoePenetratesLoS;
	[Separator("Ally Hit Effect (for Enemy hit see Chain ability)")]
	public StandardEffectInfo m_allyBuff;
	public StandardEffectInfo m_selfBuff;
	[Separator("Sequence")]
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
			BattleMonkBuffCharge_Dash battleMonkBuffChargeDash = m_chainAbilities[0] as BattleMonkBuffCharge_Dash;
			if (!ReferenceEquals(battleMonkBuffChargeDash, null))
			{
				battleMonkBuffChargeDash.SetTooltip();
				foreach (AbilityTooltipNumber current in battleMonkBuffChargeDash.GetAbilityTooltipNumbers())
				{
					numbers.Add(current);
				}
			}
		}
		return numbers;
	}

	private void Start()
	{
		if (m_chainAbilities.Length > 0)
		{
			BattleMonkBuffCharge_Dash dash = m_chainAbilities[0] as BattleMonkBuffCharge_Dash;
			if (!ReferenceEquals(dash, null))
			{
				m_dashAbility = dash;
			}
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		BattleMonkBuffCharge_Dash dash = m_chainAbilities[0] as BattleMonkBuffCharge_Dash;
		if (m_dashAbility == null
		    && m_chainAbilities.Length > 0
		    && !ReferenceEquals(dash, null))
		{
			m_dashAbility = dash;
		}
		Targeter = new AbilityUtil_Targeter_BattleMonkUltimate(
			this,
			GetAllyHitShape(),
			m_buffAoePenetratesLoS,
			GetEnemyHitShape(),
			GetDamageAoePenetrateLos(),
			false);
		bool affectsEnemies = GetModdedDamage() > 0 || (m_dashAbility != null && m_dashAbility.m_enemyDebuff.m_applyEffect);
		Targeter.SetAffectedGroups(affectsEnemies, true, true);
		StandardEffectInfo selfBuffEffect = GetSelfBuffEffect();
		if (selfBuffEffect.m_applyEffect)
		{
			m_cachedAbsorbOnSelf = selfBuffEffect.m_effectData.m_absorbAmount;
		}
		StandardEffectInfo allyBuffEffect = GetSelfBuffEffect(); // TODO GetAllyBuffEffect()
		if (allyBuffEffect.m_applyEffect)
		{
			m_cachedAbsorbOnAlly = allyBuffEffect.m_effectData.m_absorbAmount;
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
		if (!GetRequireHitAlly())
		{
			return true;
		}
		List<ActorData> actors = AreaEffectUtils.GetActorsInRadius(
			caster.GetFreePos(),
			GetAllySelectRadius(),
			m_buffAoePenetratesLoS,
			caster,
			caster.GetTeam(),
			null);
		actors.Remove(caster);
		if (NetworkClient.active)
		{
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		}
		TargeterUtils.RemoveActorsInvisibleToActor(ref actors, caster);
		return actors.Count > 0;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null
		    || !targetSquare.IsValidForGameplay()
		    || targetSquare == caster.GetCurrentBoardSquare())
		{
			return false;
		}
		if (!GetRequireHitAlly())
		{
			return true;
		}
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
			caster.GetFreePos(),
			GetAllySelectRadius(),
			m_buffAoePenetratesLoS,
			caster,
			caster.GetTeam(),
			null);
		actorsInRadius.Remove(caster);
		foreach (ActorData actorData in actorsInRadius)
		{
			bool isVisible = NetworkClient.active
				? actorData.IsActorVisibleToClient()
				: actorData.IsActorVisibleToActor(caster);
			if (isVisible)
			{
				BoardSquare square = actorData.GetCurrentBoardSquare();
				if (square != null)
				{
					bool isSquareInShape = AreaEffectUtils.IsSquareInShape(
						targetSquare,
						m_buffAlliesShape,
						square.ToVector3(),
						square,
						m_buffAoePenetratesLoS,
						caster);
					if (isSquareInShape)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
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

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkBuffCharge_Prep abilityMod_BattleMonkBuffCharge_Prep = modAsBase as AbilityMod_BattleMonkBuffCharge_Prep;
		if (m_chainAbilities.Length > 0)
		{
			Ability ability = m_chainAbilities[0];
			BattleMonkBuffCharge_Dash battleMonkBuffChargeDash = ability as BattleMonkBuffCharge_Dash;
			if (!ReferenceEquals(battleMonkBuffChargeDash, null))
			{
				AddTokenInt(tokens, "Damage", string.Empty, abilityMod_BattleMonkBuffCharge_Prep != null
					? abilityMod_BattleMonkBuffCharge_Prep.m_damageMod.GetModifiedValue(battleMonkBuffChargeDash.m_damage)
					: battleMonkBuffChargeDash.m_damage);
				AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BattleMonkBuffCharge_Prep != null
					? abilityMod_BattleMonkBuffCharge_Prep.m_selfEffectOverride.GetModifiedValue(m_selfBuff)
					: m_selfBuff, "SelfEffect", m_selfBuff);
				AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BattleMonkBuffCharge_Prep != null
					? abilityMod_BattleMonkBuffCharge_Prep.m_allyEffectOverride.GetModifiedValue(m_allyBuff)
					: m_allyBuff, "AllyEffect", m_allyBuff);
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BattleMonkBuffCharge_Prep))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_BattleMonkBuffCharge_Prep;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public float GetAllySelectRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allySelectRadiusMod.GetModifiedValue(m_allySelectRadius)
			: m_allySelectRadius;
	}

	public bool GetRequireHitAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_requireHitAlliesMod.GetModifiedValue(m_mustHitAllies)
			: m_mustHitAllies;
	}

	public AbilityAreaShape GetAllyHitShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyShapeMod.GetModifiedValue(m_buffAlliesShape)
			: m_buffAlliesShape;
	}

	public bool GetDamageAoePenetrateLos()
	{
		return m_dashAbility != null && m_dashAbility.m_damageAoePenetratesLoS;
	}

	public StandardEffectInfo GetSelfBuffEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfEffectOverride.GetModifiedValue(m_selfBuff)
			: m_selfBuff;
	}

	public StandardEffectInfo GetAllyBuffEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyEffectOverride.GetModifiedValue(m_allyBuff)
			: m_allyBuff;
	}

	public bool ShouldRemoveAllNegativeStatusFromAllies()
	{
		return m_abilityMod != null && m_abilityMod.m_removeAllNegativeStatusFromAllies;
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
		int result = 0;
		if (m_dashAbility != null)
		{
			result = m_dashAbility.m_damage;
		}
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(result);
		}
		return result;
	}
}
