using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class ActorAdditionalVisionProviders : NetworkBehaviour
{
	private SyncListVisionProviderInfo m_visionProviders = new SyncListVisionProviderInfo();

	private ActorData m_actorData;

	private static int kListm_visionProviders;

	static ActorAdditionalVisionProviders()
	{
		kListm_visionProviders = -547880216;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorAdditionalVisionProviders), kListm_visionProviders, InvokeSyncListm_visionProviders);
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
		if (!(m_actorData != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_actorData.GetFogOfWar().MarkForRecalculateVisibility();
			return;
		}
	}

	[Server]
	public void AddVisionProviderOnGridPos(GridPos gridPos, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::AddVisionProviderOnGridPos(GridPos,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
					return;
				}
			}
		}
		m_visionProviders.Add(new VisionProviderInfo(gridPos, radius, useSraightLineDist, brushRevealType, ignoreLos, canFunctionInGlobalBlind, flag));
		m_actorData.GetFogOfWar().MarkForRecalculateVisibility();
	}

	[Server]
	public void RemoveVisionProviderOnGridPos(GridPos gridPos, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnGridPos(GridPos,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
					return;
				}
			}
		}
		int num = -1;
		int num2 = 0;
		while (true)
		{
			if (num2 < m_visionProviders.Count)
			{
				if (m_visionProviders[num2].IsEqual(gridPos, radius, useSraightLineDist, brushRevealType, ignoreLos, flag, canFunctionInGlobalBlind))
				{
					num = num2;
					break;
				}
				num2++;
				continue;
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
			break;
		}
		if (num < 0)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			m_visionProviders.RemoveAt(num);
			return;
		}
	}

	[Server]
	public void AddVisionProviderOnActor(int actorIndex, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::AddVisionProviderOnActor(System.Int32,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
					return;
				}
			}
		}
		m_visionProviders.Add(new VisionProviderInfo(actorIndex, radius, useSraightLineDist, brushRevealType, ignoreLos, canFunctionInGlobalBlind, flag));
		m_actorData.GetFogOfWar().MarkForRecalculateVisibility();
	}

	[Server]
	public void RemoveVisionProviderOnActor(int actorIndex, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnActor(System.Int32,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
					return;
				}
			}
		}
		int num = -1;
		int num2 = 0;
		while (true)
		{
			if (num2 < m_visionProviders.Count)
			{
				if (m_visionProviders[num2].IsEqual(actorIndex, radius, useSraightLineDist, brushRevealType, ignoreLos, flag, canFunctionInGlobalBlind))
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
					num = num2;
					break;
				}
				num2++;
				continue;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		if (num < 0)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			m_visionProviders.RemoveAt(num);
			return;
		}
	}

	[Server]
	public void RemoveVisionProviderOnActor(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnActor(System.Int32)' called on client");
					return;
				}
			}
		}
		for (int num = m_visionProviders.Count - 1; num >= 0; num--)
		{
			VisionProviderInfo visionProviderInfo = m_visionProviders[num];
			if (visionProviderInfo.m_actorIndex == actorIndex)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				VisionProviderInfo visionProviderInfo2 = m_visionProviders[num];
				if (visionProviderInfo2.m_satelliteIndex == -1)
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
					m_visionProviders.RemoveAt(num);
				}
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnSatellite(System.Int32,System.Int32,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
					return;
				}
			}
		}
		int num = -1;
		for (int i = 0; i < m_visionProviders.Count; i++)
		{
			if (m_visionProviders[i].IsEqual(actorIndex, satelliteIndex, radius, useSraightLineDist, brushRevealType, ignoreLos, flag, canFunctionInGlobalBlind))
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
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			m_visionProviders.RemoveAt(num);
		}
	}

	[Server]
	public void RemoveVisionProviderOnSatellite(int actorIndex, int satelliteIndex)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnSatellite(System.Int32,System.Int32)' called on client");
					return;
				}
			}
		}
		for (int num = m_visionProviders.Count - 1; num >= 0; num--)
		{
			VisionProviderInfo visionProviderInfo = m_visionProviders[num];
			if (visionProviderInfo.m_actorIndex == actorIndex)
			{
				VisionProviderInfo visionProviderInfo2 = m_visionProviders[num];
				if (visionProviderInfo2.m_satelliteIndex == satelliteIndex)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					m_visionProviders.RemoveAt(num);
				}
			}
		}
	}

	public bool HasVisionProviderOnSatellite(int actorIndex, int satelliteIndex)
	{
		for (int i = 0; i < m_visionProviders.Count; i++)
		{
			VisionProviderInfo visionProviderInfo = m_visionProviders[i];
			if (visionProviderInfo.m_actorIndex != actorIndex)
			{
				continue;
			}
			while (true)
			{
				switch (5)
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
			VisionProviderInfo visionProviderInfo2 = m_visionProviders[i];
			if (visionProviderInfo2.m_satelliteIndex != satelliteIndex)
			{
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				return true;
			}
		}
		return false;
	}

	public bool GetVisionProviderInfoOnSatellite(int actorIndex, int satelliteIndex, out VisionProviderInfo visionProviderInfo)
	{
		for (int i = 0; i < m_visionProviders.Count; i++)
		{
			VisionProviderInfo visionProviderInfo2 = m_visionProviders[i];
			if (visionProviderInfo2.m_actorIndex == actorIndex)
			{
				VisionProviderInfo visionProviderInfo3 = m_visionProviders[i];
				if (visionProviderInfo3.m_satelliteIndex == satelliteIndex)
				{
					visionProviderInfo = m_visionProviders[i];
					return true;
				}
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			visionProviderInfo = default(VisionProviderInfo);
			return false;
		}
	}

	[Server]
	public void ClearVisionProviders()
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::ClearVisionProviders()' called on client");
					return;
				}
			}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("SyncList m_visionProviders called on server.");
					return;
				}
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					GeneratedNetworkCode._WriteStructSyncListVisionProviderInfo_None(writer, m_visionProviders);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			GeneratedNetworkCode._WriteStructSyncListVisionProviderInfo_None(writer, m_visionProviders);
		}
		if (!flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					GeneratedNetworkCode._ReadStructSyncListVisionProviderInfo_None(reader, m_visionProviders);
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) == 0)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			GeneratedNetworkCode._ReadStructSyncListVisionProviderInfo_None(reader, m_visionProviders);
			return;
		}
	}
}
