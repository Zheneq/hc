using System;
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithBarrier : Ability
{
	[Header("-- Barrier Info")]
	public bool m_snapToGrid = true;

	public StandardBarrierData m_barrierData;

	[Separator("Sequences", true)]
	public GameObject m_castSeqPrefab;

	[TextArea(1, 0xA)]
	public string m_notes;

	private AbilityMod_NanoSmithBarrier m_abilityMod;

	private StandardBarrierData m_cachedBarrierData;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBarrier.Start()).MethodHandle;
			}
			this.m_abilityName = "Kinetic Barrier";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		StandardBarrierData barrierData = this.GetBarrierData();
		float width = barrierData.m_width;
		if (this.GetExpectedNumberOfTargeters() < 2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBarrier.SetupTargeter()).MethodHandle;
			}
			base.Targeter = new AbilityUtil_Targeter_Barrier(this, barrierData.m_width, this.m_snapToGrid, false, true);
		}
		else
		{
			base.ClearTargeters();
			base.Targeters.Add(new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible));
			AbilityUtil_Targeter_Barrier abilityUtil_Targeter_Barrier = new AbilityUtil_Targeter_Barrier(this, width, this.m_snapToGrid, false, false);
			abilityUtil_Targeter_Barrier.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_Barrier);
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.m_barrierData.ReportAbilityTooltipNumbers(ref result);
		return result;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Max(1, base.GetNumTargets());
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (this.GetExpectedNumberOfTargeters() > 1 && targetIndex > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBarrier.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			BoardSquare y = Board.\u000E().\u000E(target.GridPos);
			BoardSquare x = Board.\u000E().\u000E(currentTargets[0].GridPos);
			return x == y;
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_NanoSmithBarrier abilityMod_NanoSmithBarrier = modAsBase as AbilityMod_NanoSmithBarrier;
		StandardBarrierData standardBarrierData;
		if (abilityMod_NanoSmithBarrier)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBarrier.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			standardBarrierData = abilityMod_NanoSmithBarrier.m_barrierDataMod.GetModifiedValue(this.m_barrierData);
		}
		else
		{
			standardBarrierData = this.m_barrierData;
		}
		StandardBarrierData standardBarrierData2 = standardBarrierData;
		standardBarrierData2.AddTooltipTokens(tokens, "BarrierData", abilityMod_NanoSmithBarrier != null, this.m_barrierData);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NanoSmithBarrier))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBarrier.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_NanoSmithBarrier);
			this.m_cachedBarrierData = this.m_abilityMod.m_barrierDataMod.GetModifiedValue(this.m_barrierData);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.m_cachedBarrierData = null;
		this.SetupTargeter();
	}

	private StandardBarrierData GetBarrierData()
	{
		StandardBarrierData result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBarrier.GetBarrierData()).MethodHandle;
			}
			result = this.m_barrierData;
		}
		else
		{
			result = this.m_cachedBarrierData;
		}
		return result;
	}
}
