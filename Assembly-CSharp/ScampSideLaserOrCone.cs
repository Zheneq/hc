using System.Collections.Generic;
using System.Text;

public class ScampSideLaserOrCone : GenericAbility_Container
{
	[Separator("Target Select Component for when shield is down")]
	public GenericAbility_TargetSelectBase m_shieldDownTargetSelect;
	[Separator("On Hit Data for when shield is down", "yellow")]
	public OnHitAuthoredData m_shieldDownOnHitData;

	private Scamp_SyncComponent m_syncComp;

	public override string GetOnHitDataDesc()
	{
		return new StringBuilder().Append(base.GetOnHitDataDesc()).Append("\n-- On Hit Data when shields are down --\n").Append(m_shieldDownOnHitData.GetInEditorDesc()).ToString();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (m_shieldDownTargetSelect != null)
		{
			relevantTargetSelectCompForEditor.Add(m_shieldDownTargetSelect);
		}
		return relevantTargetSelectCompForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		base.SetupTargetersAndCachedVars();
	}

	public void ResetTargetersForShielding(bool hasShield)
	{
		ClearTargeters();
		Targeters.AddRange(!hasShield && m_shieldDownTargetSelect != null
			? m_shieldDownTargetSelect.CreateTargeters(this)
			: m_targetSelectComp.CreateTargeters(this));
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart
			? m_onHitData
			: m_shieldDownOnHitData;
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart
		       || m_shieldDownTargetSelect == null
			? m_targetSelectComp
			: m_shieldDownTargetSelect;
	}
}
