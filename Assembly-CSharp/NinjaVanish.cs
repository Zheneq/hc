using System;
using System.Collections.Generic;
using UnityEngine;

public class NinjaVanish : Ability
{
	[Separator("Evade/Move settings", true)]
	public bool m_canQueueMoveAfterEvade;

	[Header("-- Whether to skip dash (if want to be a combat ability, etc) --")]
	public bool m_skipEvade;

	[Separator("Self Hit - Effects / Heal on Turn Start", true)]
	public StandardEffectInfo m_effectOnSelf;

	public StandardEffectInfo m_selfEffectOnNextTurn;

	[Header("-- Heal on Self on next turn start if inside field --")]
	public int m_selfHealOnTurnStartIfInField;

	[Separator("Initial Cast Hit On Enemy", true)]
	public StandardEffectInfo m_effectOnEnemy;

	[Separator("Duration for barrier and ground effect", true)]
	public int m_smokeFieldDuration = 2;

	[Separator("Vision Blocking Barrier", true)]
	public float m_barrierWidth = 3f;

	public StandardBarrierData m_visionBlockBarrierData;

	[Separator("Ground Effect", true)]
	public GroundEffectField m_groundEffectData;

	[Separator("Cooldown Reduction if only ability used in turn", true)]
	public int m_cdrIfOnlyAbilityUsed;

	public bool m_cdrConsiderCatalyst;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_NinjaVanish m_abilityMod;

	private TargetData[] m_noEvadeTargetData = new TargetData[0];

	private StandardEffectInfo m_cachedEffectOnSelf;

	private StandardEffectInfo m_cachedEffectOnEnemy;

	private StandardEffectInfo m_cachedSelfEffectOnNextTurn;

	private StandardBarrierData m_cachedVisionBlockBarrierData;

	private GroundEffectField m_cachedGroundEffectData;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.Start()).MethodHandle;
			}
			this.m_abilityName = "NinjaVanish";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		if (this.SkipEvade())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.Setup()).MethodHandle;
			}
			AbilityAreaShape shape = AbilityAreaShape.SingleSquare;
			if (this.GetGroundEffectData() != null)
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
				shape = this.GetGroundEffectData().shape;
			}
			AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
			bool applyEffect = this.GetEffectOnEnemy().m_applyEffect;
			base.Targeter = new AbilityUtil_Targeter_Shape(this, shape, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, applyEffect, false, affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible)
			{
				m_customCenterPosDelegate = new AbilityUtil_Targeter_Shape.CustomCenterPosDelegate(this.GetCenterPosForTargeter)
			};
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false);
			base.Targeter.SetShowArcToShape(false);
		}
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\nIf using [Skip Evade] option, please set phase to one that is not Evasion.";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		bool result;
		if (!this.SkipEvade())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.CanShowTargetableRadiusPreview()).MethodHandle;
			}
			result = base.CanShowTargetableRadiusPreview();
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override TargetData[] GetTargetData()
	{
		if (this.SkipEvade())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.GetTargetData()).MethodHandle;
			}
			return this.m_noEvadeTargetData;
		}
		return base.GetTargetData();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnSelf, "EffectOnSelf", this.m_effectOnSelf, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_selfEffectOnNextTurn, "SelfEffectOnNextTurn", this.m_selfEffectOnNextTurn, true);
		base.AddTokenInt(tokens, "SelfHealOnTurnStartIfInField", string.Empty, this.m_selfHealOnTurnStartIfInField, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnEnemy, "EffectOnEnemy", this.m_effectOnEnemy, true);
		base.AddTokenInt(tokens, "SmokeFieldDuration", string.Empty, this.m_smokeFieldDuration, false);
		this.m_visionBlockBarrierData.AddTooltipTokens(tokens, "VisionBlockBarrierData", false, null);
		base.AddTokenInt(tokens, "CdrIfOnlyAbilityUsed", string.Empty, this.m_cdrIfOnlyAbilityUsed, false);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnSelf;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.SetCachedFields()).MethodHandle;
			}
			cachedEffectOnSelf = this.m_abilityMod.m_effectOnSelfMod.GetModifiedValue(this.m_effectOnSelf);
		}
		else
		{
			cachedEffectOnSelf = this.m_effectOnSelf;
		}
		this.m_cachedEffectOnSelf = cachedEffectOnSelf;
		StandardEffectInfo cachedEffectOnEnemy;
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
			cachedEffectOnEnemy = this.m_abilityMod.m_effectOnEnemyMod.GetModifiedValue(this.m_effectOnEnemy);
		}
		else
		{
			cachedEffectOnEnemy = this.m_effectOnEnemy;
		}
		this.m_cachedEffectOnEnemy = cachedEffectOnEnemy;
		StandardEffectInfo cachedSelfEffectOnNextTurn;
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
			cachedSelfEffectOnNextTurn = this.m_abilityMod.m_selfEffectOnNextTurnMod.GetModifiedValue(this.m_selfEffectOnNextTurn);
		}
		else
		{
			cachedSelfEffectOnNextTurn = this.m_selfEffectOnNextTurn;
		}
		this.m_cachedSelfEffectOnNextTurn = cachedSelfEffectOnNextTurn;
		StandardBarrierData cachedVisionBlockBarrierData;
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
			cachedVisionBlockBarrierData = this.m_abilityMod.m_visionBlockBarrierDataMod.GetModifiedValue(this.m_visionBlockBarrierData);
		}
		else
		{
			cachedVisionBlockBarrierData = this.m_visionBlockBarrierData;
		}
		this.m_cachedVisionBlockBarrierData = cachedVisionBlockBarrierData;
		this.m_cachedGroundEffectData = ((!this.m_abilityMod) ? this.m_groundEffectData : this.m_abilityMod.m_groundEffectDataMod.GetModifiedValue(this.m_groundEffectData));
		int num = Mathf.Max(1, this.GetSmokeFieldDuration());
		this.m_cachedVisionBlockBarrierData.m_maxDuration = num;
		this.m_cachedGroundEffectData.duration = num;
	}

	public bool CanQueueMoveAfterEvade()
	{
		bool result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.CanQueueMoveAfterEvade()).MethodHandle;
			}
			result = this.m_abilityMod.m_canQueueMoveAfterEvadeMod.GetModifiedValue(this.m_canQueueMoveAfterEvade);
		}
		else
		{
			result = this.m_canQueueMoveAfterEvade;
		}
		return result;
	}

	public bool SkipEvade()
	{
		bool result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.SkipEvade()).MethodHandle;
			}
			result = this.m_abilityMod.m_skipEvadeMod.GetModifiedValue(this.m_skipEvade);
		}
		else
		{
			result = this.m_skipEvade;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnSelf != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.GetEffectOnSelf()).MethodHandle;
			}
			result = this.m_cachedEffectOnSelf;
		}
		else
		{
			result = this.m_effectOnSelf;
		}
		return result;
	}

	public StandardEffectInfo GetSelfEffectOnNextTurn()
	{
		return (this.m_cachedSelfEffectOnNextTurn == null) ? this.m_selfEffectOnNextTurn : this.m_cachedSelfEffectOnNextTurn;
	}

	public int GetSelfHealOnTurnStartIfInField()
	{
		int result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.GetSelfHealOnTurnStartIfInField()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealOnTurnStartIfInFieldMod.GetModifiedValue(this.m_selfHealOnTurnStartIfInField);
		}
		else
		{
			result = this.m_selfHealOnTurnStartIfInField;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnEnemy()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnEnemy != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.GetEffectOnEnemy()).MethodHandle;
			}
			result = this.m_cachedEffectOnEnemy;
		}
		else
		{
			result = this.m_effectOnEnemy;
		}
		return result;
	}

	public int GetSmokeFieldDuration()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.GetSmokeFieldDuration()).MethodHandle;
			}
			result = this.m_abilityMod.m_smokeFieldDurationMod.GetModifiedValue(this.m_smokeFieldDuration);
		}
		else
		{
			result = this.m_smokeFieldDuration;
		}
		return result;
	}

	public float GetBarrierWidth()
	{
		return (!this.m_abilityMod) ? this.m_barrierWidth : this.m_abilityMod.m_barrierWidthMod.GetModifiedValue(this.m_barrierWidth);
	}

	public StandardBarrierData GetVisionBlockBarrierData()
	{
		return (this.m_cachedVisionBlockBarrierData == null) ? this.m_visionBlockBarrierData : this.m_cachedVisionBlockBarrierData;
	}

	public GroundEffectField GetGroundEffectData()
	{
		GroundEffectField result;
		if (this.m_cachedGroundEffectData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.GetGroundEffectData()).MethodHandle;
			}
			result = this.m_cachedGroundEffectData;
		}
		else
		{
			result = this.m_groundEffectData;
		}
		return result;
	}

	public int GetCdrIfOnlyAbilityUsed()
	{
		return (!this.m_abilityMod) ? this.m_cdrIfOnlyAbilityUsed : this.m_abilityMod.m_cdrIfOnlyAbilityUsedMod.GetModifiedValue(this.m_cdrIfOnlyAbilityUsed);
	}

	public bool CdrConsiderCatalyst()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.CdrConsiderCatalyst()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrConsiderCatalystMod.GetModifiedValue(this.m_cdrConsiderCatalyst);
		}
		else
		{
			result = this.m_cdrConsiderCatalyst;
		}
		return result;
	}

	internal override bool IsStealthEvade()
	{
		return true;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		if (this.SkipEvade())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.GetMovementType()).MethodHandle;
			}
			return ActorData.MovementType.None;
		}
		return ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		bool result;
		if (this.CanQueueMoveAfterEvade())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.CanOverrideMoveStartSquare()).MethodHandle;
			}
			result = !this.SkipEvade();
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (this.SkipEvade())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			return true;
		}
		bool result = false;
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (boardSquare != null)
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
			if (boardSquare.\u0016() && boardSquare != caster.\u0012())
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
				result = true;
			}
		}
		return result;
	}

	private Vector3 GetCenterPosForTargeter(ActorData caster, AbilityTarget currentTarget)
	{
		Vector3 result = caster.\u0016();
		if (caster.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.GetCenterPosForTargeter(ActorData, AbilityTarget)).MethodHandle;
			}
			if (this.GetRunPriority() > AbilityPriority.Evasion)
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
				BoardSquare evadeDestinationForTargeter = caster.\u000E().GetEvadeDestinationForTargeter();
				if (evadeDestinationForTargeter != null)
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
					result = evadeDestinationForTargeter.ToVector3();
				}
			}
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NinjaVanish))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaVanish.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_NinjaVanish);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
