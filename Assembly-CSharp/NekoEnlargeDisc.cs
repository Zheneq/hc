using System;
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
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.Start()).MethodHandle;
			}
			this.m_abilityName = "Enlarge Disc";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		AbilityUtil_Targeter_AoE_Smooth abilityUtil_Targeter_AoE_Smooth = new AbilityUtil_Targeter_AoE_Smooth(this, this.GetAoeRadius(), false, true, false, 0);
		if (Neko_SyncComponent.HomingDiscStartFromCaster())
		{
			abilityUtil_Targeter_AoE_Smooth.m_adjustPosInConfirmedTargeting = true;
		}
		abilityUtil_Targeter_AoE_Smooth.m_customCenterPosDelegate = new AbilityUtil_Targeter_AoE_Smooth.CustomCenterPosDelegate(this.GetCenterPosForTargeter);
		base.Targeters.Add(abilityUtil_Targeter_AoE_Smooth);
		this.m_syncComp = base.GetComponent<Neko_SyncComponent>();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnEnemies;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.SetCachedFields()).MethodHandle;
			}
			cachedEffectOnEnemies = this.m_abilityMod.m_effectOnEnemiesMod.GetModifiedValue(this.m_effectOnEnemies);
		}
		else
		{
			cachedEffectOnEnemies = this.m_effectOnEnemies;
		}
		this.m_cachedEffectOnEnemies = cachedEffectOnEnemies;
		StandardEffectInfo cachedAllyHitEffect;
		if (this.m_abilityMod)
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
			cachedAllyHitEffect = this.m_abilityMod.m_allyHitEffectMod.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			cachedAllyHitEffect = this.m_allyHitEffect;
		}
		this.m_cachedAllyHitEffect = cachedAllyHitEffect;
		StandardActorEffectData cachedShieldEffectData;
		if (this.m_abilityMod)
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
			cachedShieldEffectData = this.m_abilityMod.m_shieldEffectDataMod.GetModifiedValue(this.m_shieldEffectData);
		}
		else
		{
			cachedShieldEffectData = this.m_shieldEffectData;
		}
		this.m_cachedShieldEffectData = cachedShieldEffectData;
	}

	public float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.GetLaserWidth()).MethodHandle;
			}
			result = this.m_abilityMod.m_laserWidthOverrideMod.GetModifiedValue(this.m_laserWidthOverride);
		}
		else
		{
			result = this.m_laserWidthOverride;
		}
		return result;
	}

	public float GetAoeRadius()
	{
		return (!this.m_abilityMod) ? this.m_aoeRadiusOverride : this.m_abilityMod.m_aoeRadiusOverrideMod.GetModifiedValue(this.m_aoeRadiusOverride);
	}

	public float GetReturnEndAoeRadius()
	{
		return (!this.m_abilityMod) ? this.m_returnEndRadiusOverride : this.m_abilityMod.m_returnEndRadiusOverrideMod.GetModifiedValue(this.m_returnEndRadiusOverride);
	}

	public int GetAdditionalDamageAmount()
	{
		return (!this.m_abilityMod) ? this.m_additionalDamageAmount : this.m_abilityMod.m_additionalDamageAmountMod.GetModifiedValue(this.m_additionalDamageAmount);
	}

	public StandardEffectInfo GetEffectOnEnemies()
	{
		return (this.m_cachedEffectOnEnemies == null) ? this.m_effectOnEnemies : this.m_cachedEffectOnEnemies;
	}

	public int GetAllyHeal()
	{
		return (!this.m_abilityMod) ? this.m_allyHeal : this.m_abilityMod.m_allyHealMod.GetModifiedValue(this.m_allyHeal);
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return (this.m_cachedAllyHitEffect == null) ? this.m_allyHitEffect : this.m_cachedAllyHitEffect;
	}

	public int GetShieldPerTargetHitOnReturn()
	{
		int result;
		if (this.m_abilityMod)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.GetShieldPerTargetHitOnReturn()).MethodHandle;
			}
			result = this.m_abilityMod.m_shieldPerTargetHitOnReturnMod.GetModifiedValue(this.m_shieldPerTargetHitOnReturn);
		}
		else
		{
			result = this.m_shieldPerTargetHitOnReturn;
		}
		return result;
	}

	public StandardActorEffectData GetShieldEffectData()
	{
		StandardActorEffectData result;
		if (this.m_cachedShieldEffectData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.GetShieldEffectData()).MethodHandle;
			}
			result = this.m_cachedShieldEffectData;
		}
		else
		{
			result = this.m_shieldEffectData;
		}
		return result;
	}

	public int GetCdrIfHitNoOne()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.GetCdrIfHitNoOne()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrIfHitNoOneMod.GetModifiedValue(this.m_cdrIfHitNoOne);
		}
		else
		{
			result = this.m_cdrIfHitNoOne;
		}
		return result;
	}

	public bool CanIncludeAlliesOnReturn()
	{
		bool result;
		if (this.GetAllyHeal() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.CanIncludeAlliesOnReturn()).MethodHandle;
			}
			result = (this.GetAllyHitEffect() != null && this.GetAllyHitEffect().m_applyEffect);
		}
		else
		{
			result = true;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "AdditionalDamageAmount", string.Empty, this.m_additionalDamageAmount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnEnemies, "EffectOnEnemies", this.m_effectOnEnemies, true);
		base.AddTokenInt(tokens, "AllyHeal", string.Empty, this.m_allyHeal, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyHitEffect, "AllyHitEffect", this.m_allyHitEffect, true);
		base.AddTokenInt(tokens, "ShieldPerTargetHitOnThrow", string.Empty, this.m_shieldPerTargetHitOnReturn, false);
		this.m_shieldEffectData.AddTooltipTokens(tokens, "ShieldEffectData", false, null);
		base.AddTokenInt(tokens, "CdrIfHitNoOne", string.Empty, this.m_cdrIfHitNoOne, false);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			if (caster.GetCurrentBoardSquare() != null)
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
				List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
				using (List<BoardSquare>.Enumerator enumerator = activeDiscSquares.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BoardSquare boardSquare = enumerator.Current;
						float minRange = this.m_targetData[0].m_minRange;
						float range = this.m_targetData[0].m_range;
						bool flag = caster.GetAbilityData().IsTargetSquareInRangeOfAbilityFromSquare(boardSquare, caster.GetCurrentBoardSquare(), range, minRange);
						bool flag2;
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
							if (this.m_targetData[0].m_checkLineOfSight)
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
								flag2 = caster.GetCurrentBoardSquare().\u0013(boardSquare.x, boardSquare.y);
							}
							else
							{
								flag2 = true;
							}
						}
						else
						{
							flag2 = false;
						}
						flag = flag2;
						if (flag)
						{
							return true;
						}
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				return false;
			}
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (this.m_syncComp != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Contains(boardSquareSafe))
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
				return true;
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
		if (this.m_syncComp != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.GetExpectedNumberOfTargeters()).MethodHandle;
			}
			List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count > 1)
			{
				return 1;
			}
		}
		return 0;
	}

	public override TargetData[] GetTargetData()
	{
		if (this.m_syncComp != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.GetTargetData()).MethodHandle;
			}
			List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
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
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.CreateAbilityTargetForSimpleAction(ActorData)).MethodHandle;
			}
			List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
			if (activeDiscSquares.Count == 1)
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
				abilityTarget.SetValuesFromBoardSquare(activeDiscSquares[0], activeDiscSquares[0].GetWorldPositionForLoS());
			}
		}
		return abilityTarget;
	}

	private Vector3 GetCenterPosForTargeter(ActorData caster, AbilityTarget currentTarget)
	{
		Vector3 result = this.ClampToSquareCenter(caster, currentTarget);
		if (Neko_SyncComponent.HomingDiscStartFromCaster())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.GetCenterPosForTargeter(ActorData, AbilityTarget)).MethodHandle;
			}
			if (this.m_syncComp != null)
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
				if (this.m_syncComp.m_homingActorIndex > 0)
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
					if (caster.GetActorTargeting() != null)
					{
						BoardSquare evadeDestinationForTargeter = caster.GetActorTargeting().GetEvadeDestinationForTargeter();
						if (evadeDestinationForTargeter != null)
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
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		return boardSquareSafe.GetWorldPositionForLoS();
	}

	public override int GetTheatricsSortPriority(AbilityData.ActionType actionType)
	{
		return 1;
	}

	public override bool IgnoreCameraFraming()
	{
		return this.GetActionAnimType() == ActorModelData.ActionAnimationType.None;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NekoEnlargeDisc))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NekoEnlargeDisc.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_NekoEnlargeDisc);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
