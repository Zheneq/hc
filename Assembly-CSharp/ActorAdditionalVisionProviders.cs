using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class ActorAdditionalVisionProviders : NetworkBehaviour
{
	private SyncListVisionProviderInfo m_visionProviders = new SyncListVisionProviderInfo();
	private ActorData m_actorData;

	private static int kListm_visionProviders = -547880216;

	static ActorAdditionalVisionProviders()
	{		
		RegisterSyncListDelegate(typeof(ActorAdditionalVisionProviders), kListm_visionProviders, InvokeSyncListm_visionProviders);
		NetworkCRC.RegisterBehaviour("ActorAdditionalVisionProviders", 0);
	}

	private void Start()
	{
		m_actorData = GetComponent<ActorData>();
	}

	public override void OnStartClient()
	{
		m_visionProviders.Callback = SyncListCallbackVisionProviders;
	}

	public SyncListVisionProviderInfo GetVisionProviders()
	{
		return m_visionProviders;
	}

	private void SyncListCallbackVisionProviders(SyncList<VisionProviderInfo>.Operation op, int _incorrectIndexBugIn51And52)
	{
		if (m_actorData != null)
		{
			m_actorData.GetFogOfWar().MarkForRecalculateVisibility();
		}
	}

	[Server]
	public void AddVisionProviderOnGridPos(GridPos gridPos, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::AddVisionProviderOnGridPos(GridPos,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		m_visionProviders.Add(new VisionProviderInfo(gridPos, radius, useSraightLineDist, brushRevealType, ignoreLos, canFunctionInGlobalBlind, flag));
		m_actorData.GetFogOfWar().MarkForRecalculateVisibility();
	}

	[Server]
	public void RemoveVisionProviderOnGridPos(GridPos gridPos, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnGridPos(GridPos,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		int index = -1;		
		for (int i = 0; i < m_visionProviders.Count; i++)
		{
			if (m_visionProviders[i].IsEqual(gridPos, radius, useSraightLineDist, brushRevealType, ignoreLos, flag, canFunctionInGlobalBlind))
			{
				index = i;
				break;
			}
		}
		if (index >= 0)
		{
			m_visionProviders.RemoveAt(index);
		}
	}

	[Server]
	public void AddVisionProviderOnActor(int actorIndex, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::AddVisionProviderOnActor(System.Int32,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		m_visionProviders.Add(new VisionProviderInfo(actorIndex, radius, useSraightLineDist, brushRevealType, ignoreLos, canFunctionInGlobalBlind, flag));
		m_actorData.GetFogOfWar().MarkForRecalculateVisibility();
	}

	[Server]
	public void RemoveVisionProviderOnActor(int actorIndex, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnActor(System.Int32,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		int index = -1;
		
		for (int i = 0;  i < m_visionProviders.Count; i++)
		{
			if (m_visionProviders[i].IsEqual(actorIndex, radius, useSraightLineDist, brushRevealType, ignoreLos, flag, canFunctionInGlobalBlind))
			{
				index = i;
				break;
			}
		}
		if (index >= 0)
		{
			m_visionProviders.RemoveAt(index);
		}
	}

	[Server]
	public void RemoveVisionProviderOnActor(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnActor(System.Int32)' called on client");
			return;
		}
		for (int i = m_visionProviders.Count - 1; i >= 0; i--)
		{
			if (m_visionProviders[i].m_actorIndex == actorIndex
				&& m_visionProviders[i].m_satelliteIndex == -1)
			{
				m_visionProviders.RemoveAt(i);
			}
		}
	}

	[Server]
	public void AddVisionProviderOnSatellite(int actorIndex, int satelliteIndex, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::AddVisionProviderOnSatellite(System.Int32,System.Int32,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		m_visionProviders.Add(new VisionProviderInfo(actorIndex, satelliteIndex, radius, useSraightLineDist, brushRevealType, ignoreLos, canFunctionInGlobalBlind, flag));
		m_actorData.GetFogOfWar().MarkForRecalculateVisibility();
	}

	[Server]
	public void RemoveVisionProviderOnSatellite(int actorIndex, int satelliteIndex, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnSatellite(System.Int32,System.Int32,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		int index = -1;
		for (int i = 0; i < m_visionProviders.Count; i++)
		{
			if (m_visionProviders[i].IsEqual(actorIndex, satelliteIndex, radius, useSraightLineDist, brushRevealType, ignoreLos, flag, canFunctionInGlobalBlind))
			{
				index = i;
				break;
			}
		}
		if (index >= 0)
		{
			m_visionProviders.RemoveAt(index);
		}
	}

	[Server]
	public void RemoveVisionProviderOnSatellite(int actorIndex, int satelliteIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnSatellite(System.Int32,System.Int32)' called on client");
			return;
		}
		for (int i = m_visionProviders.Count - 1; i >= 0; i--)
		{
			if (m_visionProviders[i].m_actorIndex == actorIndex
				&& m_visionProviders[i].m_satelliteIndex == satelliteIndex)
			{
				m_visionProviders.RemoveAt(i);
			}
		}
	}

	public bool HasVisionProviderOnSatellite(int actorIndex, int satelliteIndex)
	{
		foreach (VisionProviderInfo visionProvider in m_visionProviders)
		{
			if (visionProvider.m_actorIndex == actorIndex
				&& visionProvider.m_satelliteIndex == satelliteIndex)
			{
				return true;
			}
		}
		return false;
	}

	public bool GetVisionProviderInfoOnSatellite(int actorIndex, int satelliteIndex, out VisionProviderInfo visionProviderInfo)
	{
		foreach (VisionProviderInfo visionProvider in m_visionProviders)
		{
			if (visionProvider.m_actorIndex == actorIndex
				&& visionProvider.m_satelliteIndex == satelliteIndex)
			{
				visionProviderInfo = visionProvider;
				return true;
			}
		}
		visionProviderInfo = default(VisionProviderInfo);
		return false;
	}

	[Server]
	public void ClearVisionProviders()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::ClearVisionProviders()' called on client");
			return;
		}
		m_visionProviders.Clear();
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_visionProviders(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_visionProviders called on server.");
			return;
		}
		((ActorAdditionalVisionProviders)obj).m_visionProviders.HandleMsg(reader);
	}

	private void Awake()
	{
		m_visionProviders.InitializeBehaviour(this, kListm_visionProviders);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			GeneratedNetworkCode._WriteStructSyncListVisionProviderInfo_None(writer, m_visionProviders);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			GeneratedNetworkCode._WriteStructSyncListVisionProviderInfo_None(writer, m_visionProviders);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			GeneratedNetworkCode._ReadStructSyncListVisionProviderInfo_None(reader, m_visionProviders);
			LogJson();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
        if ((num & 1) != 0)
        {
            GeneratedNetworkCode._ReadStructSyncListVisionProviderInfo_None(reader, m_visionProviders);
		}
		LogJson(num);
	}

	private void LogJson(int mask = System.Int32.MaxValue)
	{
		var jsonLog = new System.Collections.Generic.List<string>();
		if ((mask & 1) != 0)
		{
			jsonLog.Add($"\"visionProviders\":{DefaultJsonSerializer.Serialize(m_visionProviders)}");
		}
		Log.Info($"[JSON] {{\"actorStatus\":{{{System.String.Join(",", jsonLog.ToArray())}}}}}");
	}
}
