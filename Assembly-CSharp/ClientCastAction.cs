using System;
using UnityEngine.Networking;

public class ClientCastAction
{
	public ActorData m_caster;

	public Ability m_ability;

	private int m_techPointGain;

	private int m_techPointLoss;

	public bool ReactedToCast { get; set; }

	public unsafe static ClientCastAction ClientCastAction_DeSerializeFromReader(ref NetworkReader reader)
	{
		ClientCastAction clientCastAction = new ClientCastAction();
		int actorIndex = reader.ReadInt32();
		sbyte b = reader.ReadSByte();
		int techPointGain = reader.ReadInt32();
		int techPointLoss = reader.ReadInt32();
		clientCastAction.m_caster = GameFlowData.Get().FindActorByActorIndex(actorIndex);
		if (clientCastAction.m_caster != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientCastAction.ClientCastAction_DeSerializeFromReader(NetworkReader*)).MethodHandle;
			}
			AbilityData.ActionType type = (AbilityData.ActionType)b;
			clientCastAction.m_ability = clientCastAction.m_caster.\u000E().GetAbilityOfActionType(type);
		}
		clientCastAction.m_techPointGain = techPointGain;
		clientCastAction.m_techPointLoss = techPointLoss;
		clientCastAction.ReactedToCast = false;
		return clientCastAction;
	}

	public void OnCast()
	{
		bool flag = ClientResolutionManager.Get().IsInResolutionState();
		if (this.m_techPointGain > 0)
		{
			if (flag)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientCastAction.OnCast()).MethodHandle;
				}
				this.m_caster.ClientUnresolvedTechPointGain += this.m_techPointGain;
			}
			this.m_caster.AddCombatText(this.m_techPointGain.ToString(), string.Empty, CombatTextCategory.TP_Recovery, BuffIconToDisplay.None);
		}
		if (this.m_techPointLoss > 0)
		{
			if (flag)
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
				this.m_caster.ClientUnresolvedTechPointLoss += this.m_techPointLoss;
			}
			this.m_caster.AddCombatText(this.m_techPointLoss.ToString(), string.Empty, CombatTextCategory.TP_Damage, BuffIconToDisplay.None);
		}
		this.ReactedToCast = true;
	}
}
