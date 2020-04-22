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
		return base.GetOnHitDataDesc() + "\n-- On Hit Data when shields are down --\n" + m_shieldDownOnHitData.GetInEditorDesc();
	}

	public override List<GenericAbility_TargetSelectBase> GetRelevantTargetSelectCompForEditor()
	{
		List<GenericAbility_TargetSelectBase> relevantTargetSelectCompForEditor = base.GetRelevantTargetSelectCompForEditor();
		if (m_shieldDownTargetSelect != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
		List<AbilityUtil_Targeter> collection;
		if (!hasShield)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(m_shieldDownTargetSelect == null))
			{
				collection = m_shieldDownTargetSelect.CreateTargeters(this);
				goto IL_0056;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		collection = m_targetSelectComp.CreateTargeters(this);
		goto IL_0056;
		IL_0056:
		base.Targeters.AddRange(collection);
	}

	public override OnHitAuthoredData GetOnHitAuthoredData()
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_syncComp.m_suitWasActiveOnTurnStart)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return m_onHitData;
					}
				}
			}
		}
		return m_shieldDownOnHitData;
	}

	public override GenericAbility_TargetSelectBase GetTargetSelectComp()
	{
		if (m_syncComp != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_syncComp.m_suitWasActiveOnTurnStart)
			{
				goto IL_0054;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (m_shieldDownTargetSelect == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			goto IL_0054;
		}
		return m_shieldDownTargetSelect;
		IL_0054:
		return m_targetSelectComp;
	}
}
