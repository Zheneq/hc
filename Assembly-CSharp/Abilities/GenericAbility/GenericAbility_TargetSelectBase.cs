// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public abstract class GenericAbility_TargetSelectBase : MonoBehaviour
{
	[TextArea(1, 5)]
	public string m_notes;
	[Separator("Target Data Override, override ability's base Target Data (before mod)", true)]
	public bool m_useTargetDataOverride;
	public TargetData[] m_targetDataOverride;
	[Separator("Targeting - Team Filters, LOS. (Overrides fields in ConeTargetingInfo and LaserTargetingInfo)", true)]
	public bool m_includeEnemies = true;
	public bool m_includeAllies;
	public bool m_includeCaster;
	public bool m_ignoreLos;
	internal ContextCalcData m_contextCalcData;
	internal ContextVars m_commonProperties;
	protected ActorData m_owner;
	private TargetSelectModBase m_currentTargetSelectMod;

	// added in rogues
#if SERVER
	protected List<ServerEvadeUtils.ChargeSegment> m_chargeSegments = new List<ServerEvadeUtils.ChargeSegment>();
#endif

	private void Awake()
	{
		BaseInit();
		m_owner = GetComponent<ActorData>();
	}

#if SERVER
	public void BaseInit()  // public in rogues
#else
	private void BaseInit()
#endif
	{
		m_contextCalcData = new ContextCalcData();
		m_commonProperties = new ContextVars();
	}

	public virtual void Initialize()
	{
	}

	public virtual List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
	{
		return null;
	}

	public virtual bool HandleCanCastValidation(Ability ability, ActorData caster)
	{
		return true;
	}

	public virtual bool HandleCustomTargetValidation(Ability ability, ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return true;
	}

	public virtual string GetUsageForEditor()
	{
		return "";
	}

	public string GetContextUsageStr(string contextName, string usage, bool actorSpecific = true)  // not actorSpecific param in rogues
	{
		return ContextVars.GetContextUsageStr(contextName, usage, actorSpecific);
	}

	public virtual void ListContextNamesForEditor(List<string> names)
	{
	}

	public virtual ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.None;
	}

	public virtual bool CanShowTargeterRangePreview(TargetData[] targetData)
	{
		if (targetData.Length > 0)
		{
			float num = Mathf.Max(0f, targetData[0].m_range - 0.5f);
			if (num > 0f
				&& num < 15f
				&& targetData[0].m_targetingParadigm != Ability.TargetingParadigm.Direction)
			{
				return true;
			}
		}
		return false;
	}

	public virtual float GetTargeterRangePreviewRadius(Ability ability, ActorData caster)
	{
		return AbilityUtils.GetCurrentRangeInSquares(ability, caster, 0);
	}

	// rogues
//#if SERVER
//	public void CreateKnockbackPreviewLines(ActorData targetingActor, AbilityUtil_Targeter targeter, ActorData targetActor, OnHitKnockbackField knockbackField, TargetData targetData, ContextVars abilityContext)
//	{
//		int num = 0;
//		targeter.EnableAllMovementArrows();
//		AbilityTarget abilityTargetForTargeterUpdate = AbilityTarget.GetAbilityTargetForTargeterUpdate();
//		if (abilityTargetForTargeterUpdate != null)
//		{
//			Vector3 vector = abilityTargetForTargeterUpdate.FreePos;
//			vector = TargeterUtils.GetClampedFreePos(vector, targetingActor, targetData.m_minRange, targetData.m_range);
//			if (abilityContext.HasVarVec3(ContextKeys.s_KnockbackOrigin.GetKey()))
//			{
//				vector = abilityContext.GetValueVec3(ContextKeys.s_KnockbackOrigin.GetKey());
//				Debug.Log(string.Format("KnockbackOrigin={0}", vector));
//			}
//			Vector3 aimDir = targetActor.GetFreePos() - vector;
//			BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(targetActor, knockbackField.m_knockbackType, aimDir, vector, knockbackField.m_distance);
//			num = targeter.AddMovementArrowWithPrevious(targetActor, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
//			targeter.SetMovementArrowEnabledFromIndex(num, false);
//		}
//	}
//#endif

	public void SetTargetSelectMod(TargetSelectModBase modBase)
	{
		if (modBase == null)
		{
			Log.Error("Trying to apply null for target select mod");
			return;
		}
		m_currentTargetSelectMod = modBase;
		OnTargetSelModApplied(modBase);
	}

	public void ClearTargetSelectMod()
	{
		if (m_currentTargetSelectMod != null)
		{
			m_currentTargetSelectMod = null;
			OnTargetSelModRemoved();
		}
	}

	protected virtual void OnTargetSelModApplied(TargetSelectModBase modBase)
	{
		Log.Error("Please implement OnTargetSelModApplied in derived class " + GetType());
	}

	protected virtual void OnTargetSelModRemoved()
	{
		Log.Error("Please implement OnTargetSelModRemoved in derived class " + GetType());
	}

	public bool IncludeEnemies()
	{
		if (m_currentTargetSelectMod != null)
		{
			return m_currentTargetSelectMod.m_includeEnemiesMod.GetModifiedValue(m_includeEnemies);
		}
		return m_includeEnemies;
	}

	public bool IncludeAllies()
	{
		if (m_currentTargetSelectMod != null)
		{
			return m_currentTargetSelectMod.m_includeAlliesMod.GetModifiedValue(m_includeAllies);
		}
		return m_includeAllies;
	}

	public bool IncludeCaster()
	{
		return (m_currentTargetSelectMod == null) ? m_includeCaster : m_currentTargetSelectMod.m_includeCasterMod.GetModifiedValue(m_includeCaster);
	}

	public bool IgnoreLos()
	{
		if (m_currentTargetSelectMod != null)
		{
			return m_currentTargetSelectMod.m_ignoreLosMod.GetModifiedValue(m_ignoreLos);
		}
		return m_ignoreLos;
	}

	public TargetData[] GetTargetDataOverride()
	{
		if (m_currentTargetSelectMod != null && m_currentTargetSelectMod.m_overrideTargetDataOnTargetSelect)
		{
			return m_currentTargetSelectMod.m_targetDataOverride;
		}
		return m_targetDataOverride;
	}

	public bool GetPropFloat(int key, out float value)
	{
		return m_commonProperties.m_floatVars.TryGetValue(key, out value);
	}

	public bool GetPropInt(int key, out int value)
	{
		return m_commonProperties.m_intVars.TryGetValue(key, out value);
	}

	public bool GetPropVec3(int key, out Vector3 value)
	{
		return m_commonProperties.m_vec3Vars.TryGetValue(key, out value);
	}

#if SERVER
	// added in rogues
	public virtual BoardSquare GetValidChargeTestSourceSquare(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		return chargeSegments[0].m_pos;
	}

	// added in rogues
	public virtual Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		// custom
		Vector3 result = chargeSegments[0].m_pos.ToVector3() - chargeSegments[1].m_pos.ToVector3();
		result.y = 0f;
		result.Normalize();
		return result;
		// rogues
		// return Vector3.zero;
	}

	// added in rogues
	public virtual bool GetChargeThroughInvalidSquares()
	{
		return false;
	}

	// added in rogues
	public virtual ServerEvadeUtils.ChargeSegment[] GetChargePath(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return null;
	}

	// added in rogues
	public virtual void AddChargeSegment(ActorData caster, BoardSquare targetSquare, BoardSquarePathInfo.ChargeCycleType targetCycle, float moveSpeed, bool finalSegment)
	{
	}

	// added in rogues
	public virtual void ApplyMovementSpeed(ServerEvadeUtils.ChargeSegment[] targetSelectCharge, float movementSpeed)
	{
		for (int i = 0; i < targetSelectCharge.Length; i++)
		{
			targetSelectCharge[i].m_segmentMovementSpeed = movementSpeed;
		}
	}

	// added in rogues
	public virtual BoardSquare GetIdealDestination(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return null;
	}

	// added in rogues
	public virtual List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		return null;
	}

	// added in rogues
	public virtual List<ServerClientUtils.SequenceStartData> CreateSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData, Sequence.IExtraSequenceParams[] extraSequenceParams = null)
	{
		return null;
	}

	// added in rogues
	public virtual void CalcHitTargets(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		if (m_includeCaster)
		{
			AddHitActor(caster, caster.GetLoSCheckPos(), false);
		}
		if (!targets.IsNullOrEmpty<AbilityTarget>())
		{
			GetNonActorSpecificContext().SetValue(ContextKeys.s_TargetHitPos.GetKey(), targets[0].FreePos);
		}
	}

	// added in rogues
	public virtual Dictionary<ActorData, ActorHitContext> GetActorHitContextMap()
	{
		return m_contextCalcData.m_actorToHitContext;
	}

	// added in rogues
	public virtual ContextVars GetNonActorSpecificContext()
	{
		return m_contextCalcData.m_nonActorSpecificContext;
	}

	// added in rogues
	public virtual void ResetContextData()
	{
		m_contextCalcData.ResetContextData();
	}

	// added in rogues
	protected virtual void AddHitActor(ActorData actor, Vector3 hitOrigin, bool ignoreMinCoverDist = false)
	{
		if (actor == null)
		{
			Log.Error("Trying to add null actor");
		}
		m_contextCalcData.AddHitActor(actor, hitOrigin, ignoreMinCoverDist);
	}

	// added in rogues
	public bool HasContextForActor(ActorData actor)
	{
		return actor != null && m_contextCalcData.m_actorToHitContext.ContainsKey(actor);
	}

	// added in rogues
	protected void SetActorContext(ActorData actor, int contextKey, int value)
	{
		m_contextCalcData.SetActorContext(actor, contextKey, value);
	}

	// added in rogues
	protected void SetActorContext(ActorData actor, int contextKey, float value)
	{
		m_contextCalcData.SetActorContext(actor, contextKey, value);
	}

	// added in rogues
	protected void SetActorContext(ActorData actor, int contextKey, Vector3 value)
	{
		m_contextCalcData.SetActorContext(actor, contextKey, value);
	}
#endif
}
