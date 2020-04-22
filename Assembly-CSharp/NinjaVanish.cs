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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "NinjaVanish";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (SkipEvade())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					AbilityAreaShape shape = AbilityAreaShape.SingleSquare;
					if (GetGroundEffectData() != null)
					{
						shape = GetGroundEffectData().shape;
					}
					AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
					bool applyEffect = GetEffectOnEnemy().m_applyEffect;
					AbilityUtil_Targeter_Shape abilityUtil_Targeter_Shape = new AbilityUtil_Targeter_Shape(this, shape, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, applyEffect, false, affectsCaster);
					abilityUtil_Targeter_Shape.m_customCenterPosDelegate = GetCenterPosForTargeter;
					base.Targeter = abilityUtil_Targeter_Shape;
					return;
				}
				}
			}
		}
		base.Targeter = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false);
		base.Targeter.SetShowArcToShape(false);
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\nIf using [Skip Evade] option, please set phase to one that is not Evasion.";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		int result;
		if (!SkipEvade())
		{
			result = (base.CanShowTargetableRadiusPreview() ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override TargetData[] GetTargetData()
	{
		if (SkipEvade())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_noEvadeTargetData;
				}
			}
		}
		return base.GetTargetData();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnSelf, "EffectOnSelf", m_effectOnSelf);
		AbilityMod.AddToken_EffectInfo(tokens, m_selfEffectOnNextTurn, "SelfEffectOnNextTurn", m_selfEffectOnNextTurn);
		AddTokenInt(tokens, "SelfHealOnTurnStartIfInField", string.Empty, m_selfHealOnTurnStartIfInField);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnEnemy, "EffectOnEnemy", m_effectOnEnemy);
		AddTokenInt(tokens, "SmokeFieldDuration", string.Empty, m_smokeFieldDuration);
		m_visionBlockBarrierData.AddTooltipTokens(tokens, "VisionBlockBarrierData");
		AddTokenInt(tokens, "CdrIfOnlyAbilityUsed", string.Empty, m_cdrIfOnlyAbilityUsed);
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEffectOnSelf;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnSelf = m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf);
		}
		else
		{
			cachedEffectOnSelf = m_effectOnSelf;
		}
		m_cachedEffectOnSelf = cachedEffectOnSelf;
		StandardEffectInfo cachedEffectOnEnemy;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnEnemy = m_abilityMod.m_effectOnEnemyMod.GetModifiedValue(m_effectOnEnemy);
		}
		else
		{
			cachedEffectOnEnemy = m_effectOnEnemy;
		}
		m_cachedEffectOnEnemy = cachedEffectOnEnemy;
		StandardEffectInfo cachedSelfEffectOnNextTurn;
		if ((bool)m_abilityMod)
		{
			cachedSelfEffectOnNextTurn = m_abilityMod.m_selfEffectOnNextTurnMod.GetModifiedValue(m_selfEffectOnNextTurn);
		}
		else
		{
			cachedSelfEffectOnNextTurn = m_selfEffectOnNextTurn;
		}
		m_cachedSelfEffectOnNextTurn = cachedSelfEffectOnNextTurn;
		StandardBarrierData cachedVisionBlockBarrierData;
		if ((bool)m_abilityMod)
		{
			cachedVisionBlockBarrierData = m_abilityMod.m_visionBlockBarrierDataMod.GetModifiedValue(m_visionBlockBarrierData);
		}
		else
		{
			cachedVisionBlockBarrierData = m_visionBlockBarrierData;
		}
		m_cachedVisionBlockBarrierData = cachedVisionBlockBarrierData;
		m_cachedGroundEffectData = ((!m_abilityMod) ? m_groundEffectData : m_abilityMod.m_groundEffectDataMod.GetModifiedValue(m_groundEffectData));
		int num = Mathf.Max(1, GetSmokeFieldDuration());
		m_cachedVisionBlockBarrierData.m_maxDuration = num;
		m_cachedGroundEffectData.duration = num;
	}

	public bool CanQueueMoveAfterEvade()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canQueueMoveAfterEvadeMod.GetModifiedValue(m_canQueueMoveAfterEvade);
		}
		else
		{
			result = m_canQueueMoveAfterEvade;
		}
		return result;
	}

	public bool SkipEvade()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_skipEvadeMod.GetModifiedValue(m_skipEvade);
		}
		else
		{
			result = m_skipEvade;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnSelf != null)
		{
			result = m_cachedEffectOnSelf;
		}
		else
		{
			result = m_effectOnSelf;
		}
		return result;
	}

	public StandardEffectInfo GetSelfEffectOnNextTurn()
	{
		return (m_cachedSelfEffectOnNextTurn == null) ? m_selfEffectOnNextTurn : m_cachedSelfEffectOnNextTurn;
	}

	public int GetSelfHealOnTurnStartIfInField()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_selfHealOnTurnStartIfInFieldMod.GetModifiedValue(m_selfHealOnTurnStartIfInField);
		}
		else
		{
			result = m_selfHealOnTurnStartIfInField;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnEnemy()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnEnemy != null)
		{
			result = m_cachedEffectOnEnemy;
		}
		else
		{
			result = m_effectOnEnemy;
		}
		return result;
	}

	public int GetSmokeFieldDuration()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_smokeFieldDurationMod.GetModifiedValue(m_smokeFieldDuration);
		}
		else
		{
			result = m_smokeFieldDuration;
		}
		return result;
	}

	public float GetBarrierWidth()
	{
		return (!m_abilityMod) ? m_barrierWidth : m_abilityMod.m_barrierWidthMod.GetModifiedValue(m_barrierWidth);
	}

	public StandardBarrierData GetVisionBlockBarrierData()
	{
		return (m_cachedVisionBlockBarrierData == null) ? m_visionBlockBarrierData : m_cachedVisionBlockBarrierData;
	}

	public GroundEffectField GetGroundEffectData()
	{
		GroundEffectField result;
		if (m_cachedGroundEffectData != null)
		{
			result = m_cachedGroundEffectData;
		}
		else
		{
			result = m_groundEffectData;
		}
		return result;
	}

	public int GetCdrIfOnlyAbilityUsed()
	{
		return (!m_abilityMod) ? m_cdrIfOnlyAbilityUsed : m_abilityMod.m_cdrIfOnlyAbilityUsedMod.GetModifiedValue(m_cdrIfOnlyAbilityUsed);
	}

	public bool CdrConsiderCatalyst()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrConsiderCatalystMod.GetModifiedValue(m_cdrConsiderCatalyst);
		}
		else
		{
			result = m_cdrConsiderCatalyst;
		}
		return result;
	}

	internal override bool IsStealthEvade()
	{
		return true;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		if (SkipEvade())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return ActorData.MovementType.None;
				}
			}
		}
		return ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		int result;
		if (CanQueueMoveAfterEvade())
		{
			result = ((!SkipEvade()) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (SkipEvade())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		bool result = false;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.IsBaselineHeight() && boardSquareSafe != caster.GetCurrentBoardSquare())
			{
				result = true;
			}
		}
		return result;
	}

	private Vector3 GetCenterPosForTargeter(ActorData caster, AbilityTarget currentTarget)
	{
		Vector3 result = caster.GetTravelBoardSquareWorldPosition();
		if (caster.GetActorTargeting() != null)
		{
			if (GetRunPriority() > AbilityPriority.Evasion)
			{
				BoardSquare evadeDestinationForTargeter = caster.GetActorTargeting().GetEvadeDestinationForTargeter();
				if (evadeDestinationForTargeter != null)
				{
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
		if (abilityMod.GetType() != typeof(AbilityMod_NinjaVanish))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_NinjaVanish);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
