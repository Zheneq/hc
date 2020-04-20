using System;
using System.Collections.Generic;

public class ScampSideLaserOrCone : GenericAbility_Container
{
	[Separator("Target Select Component for when shield is down", true)]
	public GenericAbility_TargetSelectBase m_shieldDownTargetSelect;

	[Separator("On Hit Data for when shield is down", "yellow")]
	public OnHitAuthoredData m_shieldDownOnHitData;

	private Scamp_SyncComponent m_syncComp;

	public override string GetOnHitDataDesc()
	{
		return base.GetOnHitDataDesc() + "\n-- On Hit Data when shields are down --\n" + this.m_shieldDownOnHitData.GetInEditorDesc();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (this.m_shieldDownTargetSelect != null)
		{
			relevantTargetSelectCompForEditor.Add(this.m_shieldDownTargetSelect);
		}
		return relevantTargetSelectCompForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		this.m_syncComp = base.GetComponent<Scamp_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	public void ResetTargetersForShielding(bool hasShield)
	{
		base.ClearTargeters();
		List<AbilityUtil_Targeter> collection;
		if (!hasShield)
		{
			if (!(this.m_shieldDownTargetSelect == null))
			{
				collection = this.m_shieldDownTargetSelect.CreateTargeters(this);
				goto IL_56;
			}
		}
		collection = this.m_targetSelectComp.CreateTargeters(this);
		IL_56:
		base.Targeters.AddRange(collection);
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		if (this.m_syncComp != null)
		{
			if (this.m_syncComp.m_suitWasActiveOnTurnStart)
			{
				return this.m_onHitData;
			}
		}
		return this.m_shieldDownOnHitData;
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		if (this.m_syncComp != null)
		{
			if (this.m_syncComp.m_suitWasActiveOnTurnStart)
			{
				goto IL_54;
			}
		}
		if (!(this.m_shieldDownTargetSelect == null))
		{
			return this.m_shieldDownTargetSelect;
		}
		IL_54:
		return this.m_targetSelectComp;
	}
}
