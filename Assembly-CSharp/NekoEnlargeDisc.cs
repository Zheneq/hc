using System.Collections.Generic;
using UnityEngine;

public class NekoEnlargeDisc : Ability
{
	[Separator("Targeting", true)]
	public float m_laserWidthOverride;

	public float m_aoeRadiusOverride;

	public float m_returnEndRadiusOverride;

	[Separator("On Hit Damage/Effect", true)]
	public int m_additionalDamageAmount;

	public StandardEffectInfo m_effectOnEnemies;

	[Separator("Ally Hits", true)]
	public int m_allyHeal;

	public StandardEffectInfo m_allyHitEffect;

	[Separator("Shielding for target hit on return (applied on start of next turn)", true)]
	public int m_shieldPerTargetHitOnReturn;

	public StandardActorEffectData m_shieldEffectData;

	[Separator("Cooldown Reduction", true)]
	public int m_cdrIfHitNoOne;

	[Separator("Sequences", true)]
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
		AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = new AbilityUtil_Targeter_AoE_Smooth(this, GetAoeRadius(), false, true, false, 0);
		if (Neko_SyncComponent.HomingDiscStartFromCaster())
		{
			abilityUtil_Targeter_AoE_Smooth.m_adjustPosInConfirmedTargeting = true;
		}
		abilityUtil_Targeter_AoE_Smooth.m_customCenterPosDelegate = GetCenterPosForTargeter;
		base.Targeters.Add(abilityUtil_Targeter_AoE_Smooth);
		m_syncComp = GetComponent<Neko_SyncComponent>();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnEnemies;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnEnemies = m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(m_effectOnEnemies);
		}
		else
		{
			cachedEffectOnEnemies = m_effectOnEnemies;
		}
		m_cachedEffectOnEnemies = cachedEffectOnEnemies;
		StandardEffectInfo cachedAllyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedAllyHitEffect = m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = m_allyHitEffect;
		}
		m_cachedAllyHitEffect = cachedAllyHitEffect;
		StandardActorEffectData cachedShieldEffectData;
		if ((bool)m_abilityMod)
		{
			cachedShieldEffectData = m_abilityMod.m_shieldEffectDataMod.GetModifiedValue(m_shieldEffectData);
		}
		else
		{
			cachedShieldEffectData = m_shieldEffectData;
		}
		m_cachedShieldEffectData = cachedShieldEffectData;
	}

	public float GetLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserWidthOverrideMod.GetModifiedValue(m_laserWidthOverride);
		}
		else
		{
			result = m_laserWidthOverride;
		}
		return result;
	}

	public float GetAoeRadius()
	{
		return (!m_abilityMod) ? m_aoeRadiusOverride : m_abilityMod.m_aoeRadiusOverrideMod.GetModifiedValue(m_aoeRadiusOverride);
	}

	public float GetReturnEndAoeRadius()
	{
		return (!m_abilityMod) ? m_returnEndRadiusOverride : m_abilityMod.m_returnEndRadiusOverrideMod.GetModifiedValue(m_returnEndRadiusOverride);
	}

	public int GetAdditionalDamageAmount()
	{
		return (!m_abilityMod) ? m_additionalDamageAmount : m_abilityMod.m_additionalDamageAmountMod.GetModifiedValue(m_additionalDamageAmount);
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		return (m_cachedEffectOnEnemies == null) ? m_effectOnEnemies : m_cachedEffectOnEnemies;
	}

	public int GetAllyHeal()
	{
		return (!m_abilityMod) ? m_allyHeal : m_abilityMod.m_allyHealMod.GetModifiedValue(m_allyHeal);
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return (m_cachedAllyHitEffect == null) ? m_allyHitEffect : m_cachedAllyHitEffect;
	}

	public int GetShieldPerTargetHitOnReturn()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_shieldPerTargetHitOnReturnMod.GetModifiedValue(m_shieldPerTargetHitOnReturn);
		}
		else
		{
			result = m_shieldPerTargetHitOnReturn;
		}
		return result;
	}

	public StandardActorEffectData GetShieldEffectData()
	{
		StandardActorEffectData result;
		if (m_cachedShieldEffectData != null)
		{
			result = m_cachedShieldEffectData;
		}
		else
		{
			result = m_shieldEffectData;
		}
		return result;
	}

	public int GetCdrIfHitNoOne()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrIfHitNoOneMod.GetModifiedValue(m_cdrIfHitNoOne);
		}
		else
		{
			result = m_cdrIfHitNoOne;
		}
		return result;
	}

	public bool CanIncludeAlliesOnReturn()
	{
		int result;
		if (GetAllyHeal() <= 0)
		{
			result = ((GetAllyHitEffect() != null && GetAllyHitEffect().m_applyEffect) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
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
		if (m_syncComp != null)
		{
			if (caster.GetCurrentBoardSquare() != null)
			{
				List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
				using (List<BoardSquare>.Enumerator enumerator = activeDiscSquares.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BoardSquare current = enumerator.Current;
						float minRange = m_targetData[0].m_minRange;
						float range = m_targetData[0].m_range;
						int num;
						if (caster.GetAbilityData().IsTargetSquareInRangeOfAbilityFromSquare(current, caster.GetCurrentBoardSquare(), range, minRange))
						{
							if (m_targetData[0].m_checkLineOfSight)
							{
								num = (caster.GetCurrentBoardSquare()._0013(current.x, current.y) ? 1 : 0);
							}
							else
							{
								num = 1;
							}
						}
						else
						{
							num = 0;
						}
						if (num != 0)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Contains(boardSquareSafe))
			{
				while (true)
				{
					switch (1)
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

	public override bool AllowInvalidSquareForSquareBasedTarget()
	{
		return true;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count > 1)
			{
				return 1;
			}
		}
		return 0;
	}

	public override TargetData[] GetTargetData()
	{
		if (m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count > 1)
			{
				return base.GetTargetData();
			}
		}
		return new TargetData[0];
	}

	public override AbilityTarget CreateAbilityTargetForSimpleAction(ActorData caster)
	{
		AbilityTarget abilityTarget = base.CreateAbilityTargetForSimpleAction(caster);
		if (m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count == 1)
			{
				abilityTarget.SetValuesFromBoardSquare(activeDiscSquares[0], activeDiscSquares[0].GetWorldPositionForLoS());
			}
		}
		return abilityTarget;
	}

	private Vector3 GetCenterPosForTargeter(ActorData caster, AbilityTarget currentTarget)
	{
		Vector3 result = ClampToSquareCenter(caster, currentTarget);
		if (Neko_SyncComponent.HomingDiscStartFromCaster())
		{
			if (m_syncComp != null)
			{
				if (m_syncComp.m_homingActorIndex > 0)
				{
					if (caster.GetActorTargeting() != null)
					{
						BoardSquare evadeDestinationForTargeter = caster.GetActorTargeting().GetEvadeDestinationForTargeter();
						if (evadeDestinationForTargeter != null)
						{
							result = evadeDestinationForTargeter.ToVector3();
						}
					}
				}
			}
		}
		return result;
	}

	public Vector3 ClampToSquareCenter(ActorData caster, AbilityTarget currentTarget)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		return boardSquareSafe.GetWorldPositionForLoS();
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
		if (abilityMod.GetType() != typeof(AbilityMod_NekoEnlargeDisc))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_NekoEnlargeDisc);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
