// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine.Networking;

// added in rogues
// TODO SERIALIZATION verify that server and client messages match
#if SERVER
public class RunResolutionActionsOutsideResolve : MessageBase
{
	public List<ResolutionAction> m_actionsServer;
	public List<ClientResolutionAction> m_actionsClient;

	public override void Serialize(NetworkWriter writer)
	{
		sbyte b = (sbyte)m_actionsServer.Count;
		writer.Write(b);
		for (int i = 0; i < b; i++)
		{
			m_actionsServer[i].ResolutionAction_SerializeToStream(writer);
		}
	}

	public override void Deserialize(NetworkReader reader)
	{
		sbyte b = reader.ReadSByte();
		m_actionsClient = new List<ClientResolutionAction>(b);
		for (int i = 0; i < b; i++)
		{
			ClientResolutionAction item = ClientResolutionAction.ClientResolutionAction_DeSerializeFromStream(reader);
			m_actionsClient.Add(item);
		}
	}
}
#endif
