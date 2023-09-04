using System.Collections.Generic;
using UnityEngine;

public class NekoEnlargeDisc : Ability
{
	[Separator("Targeting")]
	public float m_laserWidthOverride;
	public float m_aoeRadiusOverride;
	public float m_returnEndRadiusOverride;
	[Separator("On Hit Damage/Effect")]
	public int m_additionalDamageAmount;
	public StandardEffectInfo m_effectOnEnemies;
	[Separator("Ally Hits")]
	public int m_allyHeal;
	public StandardEffectInfo m_allyHitEffect;
	[Separator("Shielding for target hit on return (applied on start of next turn)")]
	public int m_shieldPerTargetHitOnReturn;
	public StandardActorEffectData m_shieldEffectData;
	[Separator("Cooldown Reduction")]
	public int m_cdrIfHitNoOne;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_discReturnOverrideSequencePrefab;
	public GameObject m_prepDiscReturnOverrideSequencePrefab;

	private AbilityMod_NekoEnlargeDisc m_abilityMod;
	private Neko_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedEffectOnEnemies;
	private StandardEffectInfo m_cachedAllyHitEffect;
	private StandardActorEffectData m_cachedShieldEffectData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Enlarge Disc";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		AbilityUtil_Targeter_AoE_Smooth targeter = new AbilityUtil_Targeter_AoE_Smooth(
			this, GetAoeRadius(), false, true, false, 0);
		if (Neko_SyncComponent.HomingDiscStartFromCaster())
		{
			targeter.m_adjustPosInConfirmedTargeting = true;
		}
		targeter.m_customCenterPosDelegate = GetCenterPosForTargeter;
		Targeters.Add(targeter);
		m_syncComp = GetComponent<Neko_SyncComponent>();
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnEnemies = m_abilityMod != null
			? m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies)
			: m_effectOnEnemies;
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
		m_cachedShieldEffectData = m_abilityMod != null
			? m_abilityMod.m_shieldEffectDataMod.GetModifiedValue(m_shieldEffectData)
			: m_shieldEffectData;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthOverrideMod.GetModifiedValue(m_laserWidthOverride)
			: m_laserWidthOverride;
	}

	public float GetAoeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeRadiusOverrideMod.GetModifiedValue(m_aoeRadiusOverride)
			: m_aoeRadiusOverride;
	}

	public float GetReturnEndAoeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_returnEndRadiusOverrideMod.GetModifiedValue(m_returnEndRadiusOverride)
			: m_returnEndRadiusOverride;
	}

	public int GetAdditionalDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_additionalDamageAmountMod.GetModifiedValue(m_additionalDamageAmount)
			: m_additionalDamageAmount;
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		return m_cachedEffectOnEnemies ?? m_effectOnEnemies;
	}

	public int GetAllyHeal()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHealMod.GetModifiedValue(m_allyHeal)
			: m_allyHeal;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public int GetShieldPerTargetHitOnReturn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldPerTargetHitOnReturnMod.GetModifiedValue(m_shieldPerTargetHitOnReturn)
			: m_shieldPerTargetHitOnReturn;
	}

	public StandardActorEffectData GetShieldEffectData()
	{
		return m_cachedShieldEffectData ?? m_shieldEffectData;
	}

	public int GetCdrIfHitNoOne()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfHitNoOneMod.GetModifiedValue(m_cdrIfHitNoOne)
			: m_cdrIfHitNoOne;
	}

	public bool CanIncludeAlliesOnReturn()
	{
		return GetAllyHeal() > 0
		       || GetAllyHitEffect() != null && GetAllyHitEffect().m_applyEffect;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "AdditionalDamageAmount", string.Empty, m_additionalDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnEnemies, "EffectOnEnemies", m_effectOnEnemies);
		AddTokenInt(tokens, "AllyHeal", string.Empty, m_allyHeal);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AddTokenInt(tokens, "ShieldPerTargetHitOnThrow", string.Empty, m_shieldPerTargetHitOnReturn);
		m_shieldEffectData.AddTooltipTokens(tokens, "ShieldEffectData");
		AddTokenInt(tokens, "CdrIfHitNoOne", string.Empty, m_cdrIfHitNoOne);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_syncComp != null && caster.GetCurrentBoardSquare() != null)
		{
			foreach (BoardSquare dest in m_syncComp.GetActiveDiscSquares())
			{
				bool isTargetInRange = caster.GetAbilityData().IsTargetSquareInRangeOfAbilityFromSquare(
					dest, caster.GetCurrentBoardSquare(), m_targetData[0].m_range, m_targetData[0].m_minRange);
				if (isTargetInRange
				    && (!m_targetData[0].m_checkLineOfSight || caster.GetCurrentBoardSquare().GetLOS(dest.x, dest.y)))
				{
					return true;
				}
			}
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return m_syncComp != null && m_syncComp.GetActiveDiscSquares().Contains(targetSquare);
	}

	public override bool AllowInvalidSquareForSquareBasedTarget()
	{
		return true;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_syncComp != null && m_syncComp.GetActiveDiscSquares().Count > 1
			? 1
			: 0;
	}

	public override TargetData[] GetTargetData()
	{
		return m_syncComp != null && m_syncComp.GetActiveDiscSquares().Count > 1
			? base.GetTargetData()
			: new TargetData[0];
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		AbilityTarget abilityTarget = base.CreateAbilityTargetForSimpleAction(caster);
		if (m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count == 1)
			{
				abilityTarget.SetValuesFromBoardSquare(activeDiscSquares[0], activeDiscSquares[0].GetOccupantLoSPos());
			}
		}
		return abilityTarget;
	}

	private Vector3 GetCenterPosForTargeter(ActorData caster, AbilityTarget currentTarget)
	{
		Vector3 result = ClampToSquareCenter(caster, currentTarget);
		if (Neko_SyncComponent.HomingDiscStartFromCaster()
		    && m_syncComp != null
		    && m_syncComp.m_homingActorIndex > 0
		    && caster.GetActorTargeting() != null)
		{
			BoardSquare evadeDestinationForTargeter = caster.GetActorTargeting().GetEvadeDestinationForTargeter();
			if (evadeDestinationForTargeter != null)
			{
				result = evadeDestinationForTargeter.ToVector3();
			}
		}
		return result;
	}

	public Vector3 ClampToSquareCenter(ActorData caster, AbilityTarget currentTarget)
	{
		return Board.Get().GetSquare(currentTarget.GridPos).GetOccupantLoSPos();
	}

	public override int GetTheatricsSortPriority(AbilityData.ActionType actionType)
	{
		return 1;
	}

	public override bool IgnoreCameraFraming()
	{
		return GetActionAnimType() == ActorModelData.ActionAnimationType.None;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NekoEnlargeDisc))
		{
			m_abilityMod = abilityMod as AbilityMod_NekoEnlargeDisc;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
