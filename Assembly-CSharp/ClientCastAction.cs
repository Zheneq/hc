using UnityEngine.Networking;

public class ClientCastAction
{
	public ActorData m_caster;

	public Ability m_ability;

	private int m_techPointGain;

	private int m_techPointLoss;

	public bool ReactedToCast
	{
		get;
		set;
	}

	public static ClientCastAction ClientCastAction_DeSerializeFromReader(ref NetworkReader reader)
	{
		ClientCastAction clientCastAction = new ClientCastAction();
		int actorIndex = reader.ReadInt32();
		sbyte b = reader.ReadSByte();
		int techPointGain = reader.ReadInt32();
		int techPointLoss = reader.ReadInt32();
		clientCastAction.m_caster = GameFlowData.Get().FindActorByActorIndex(actorIndex);
		if (clientCastAction.m_caster != null)
		{
			AbilityData.ActionType type = (AbilityData.ActionType)b;
			clientCastAction.m_ability = clientCastAction.m_caster.GetAbilityData().GetAbilityOfActionType(type);
		}
		clientCastAction.m_techPointGain = techPointGain;
		clientCastAction.m_techPointLoss = techPointLoss;
		clientCastAction.ReactedToCast = false;
		return clientCastAction;
	}

	public void OnCast()
	{
		bool flag = ClientResolutionManager.Get().IsInResolutionState();
		if (m_techPointGain > 0)
		{
			if (flag)
			{
				m_caster.ClientUnresolvedTechPointGain += m_techPointGain;
			}
			m_caster.AddCombatText(m_techPointGain.ToString(), string.Empty, CombatTextCategory.TP_Recovery, BuffIconToDisplay.None);
		}
		if (m_techPointLoss > 0)
		{
			if (flag)
			{
				m_caster.ClientUnresolvedTechPointLoss += m_techPointLoss;
			}
			m_caster.AddCombatText(m_techPointLoss.ToString(), string.Empty, CombatTextCategory.TP_Damage, BuffIconToDisplay.None);
		}
		ReactedToCast = true;
	}
}
