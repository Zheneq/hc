using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BrushCoordinator : NetworkBehaviour, IGameEventListener
{
	private static BrushCoordinator s_instance;

	private bool m_setupBrush;

	private bool m_cameraCreated;

	private bool m_brushVisible;

	public BrushRegion[] m_regions;

	private SyncListInt m_regionsLastDisruptionTurn = new SyncListInt();

	private static int kListm_regionsLastDisruptionTurn;

	private static int kRpcRpcUpdateClientFog;

	static BrushCoordinator()
	{
		kRpcRpcUpdateClientFog = -2062592578;
		NetworkBehaviour.RegisterRpcDelegate(typeof(BrushCoordinator), kRpcRpcUpdateClientFog, InvokeRpcRpcUpdateClientFog);
		kListm_regionsLastDisruptionTurn = 1707345339;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(BrushCoordinator), kListm_regionsLastDisruptionTurn, InvokeSyncListm_regionsLastDisruptionTurn);
		NetworkCRC.RegisterBehaviour("BrushCoordinator", 0);
	}

	public static BrushCoordinator Get()
	{
		return s_instance;
	}

	private void OnEnable()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameCameraCreated);
	}

	private void OnDisable()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameCameraCreated);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.GameCameraCreated)
		{
			return;
		}
		if (!(VisualsLoader.Get() == null))
		{
			if (!VisualsLoader.Get().LevelLoaded())
			{
				return;
			}
		}
		m_cameraCreated = true;
	}

	private void Awake()
	{
		s_instance = this;
		m_regionsLastDisruptionTurn.InitializeBehaviour(this, kListm_regionsLastDisruptionTurn);
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public override void OnStartClient()
	{
		m_regionsLastDisruptionTurn.Callback = SyncListCallbackLastDisruptTurn;
	}

	public void EnableBrushVisibility()
	{
		m_brushVisible = true;
	}

	public bool DisableAllBrush()
	{
		int result;
		if (SinglePlayerManager.Get() != null)
		{
			if (!SinglePlayerManager.Get().EnableBrush())
			{
				result = 1;
				goto IL_0066;
			}
		}
		if (DebugParameters.Get() != null)
		{
			result = (DebugParameters.Get().GetParameterAsBool("DisableBrush") ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		goto IL_0066;
		IL_0066:
		return (byte)result != 0;
	}

	private void Start()
	{
		for (int i = 0; i < m_regions.Length; i++)
		{
			m_regions[i].Initialize();
			if (NetworkServer.active)
			{
				m_regionsLastDisruptionTurn.Add(-1);
			}
		}
		TrySetupBrushSquares();
		_000E();
	}

	private void TrySetupBrushSquares()
	{
		if (m_setupBrush)
		{
			return;
		}
		while (true)
		{
			if (!(Board.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (GameFlowData.Get() != null && m_cameraCreated)
				{
					while (true)
					{
						SetupBrushSquares();
						FogOfWar.CalculateFogOfWarForTeam(Team.TeamA);
						FogOfWar.CalculateFogOfWarForTeam(Team.TeamB);
						m_setupBrush = true;
						return;
					}
				}
				return;
			}
		}
	}

	private void _000E()
	{
		if (!Application.isEditor)
		{
			return;
		}
		for (int i = 0; i < m_regions.Length; i++)
		{
			BrushRegion brushRegion = m_regions[i];
			if (brushRegion == null)
			{
				Debug.LogError("Brush Region at index " + i + " is null");
				continue;
			}
			if (brushRegion.m_disruptedVFX != null)
			{
				if (brushRegion.m_disruptedVFX.GetComponent<PKFxFX>() == null)
				{
					Debug.LogError("Brush Region at index " + i + " has Disrupted VFX field set but it doesn't have a PKFxFX component");
				}
			}
			if (brushRegion.m_functioningVFX != null)
			{
				if (brushRegion.m_functioningVFX.GetComponent<PKFxFX>() == null)
				{
					Debug.LogError("Brush Region at index " + i + " has Functioning VFX field set but it doesn't have a PKFxFX component");
				}
			}
		}
	}

	private void SetupBrushSquares()
	{
		for (int i = 0; i < m_regions.Length; i++)
		{
			if (m_regions[i] == null)
			{
				Log.Error($"Null brush region ({i}); fix brush coordinator's data.");
			}
			else
			{
				List<BoardSquare> squaresInRegion = m_regions[i].GetSquaresInRegion();
				foreach (BoardSquare item in squaresInRegion)
				{
					if (item.BrushRegion == -1)
					{
						item.BrushRegion = i;
					}
					else
					{
						Log.Error($"Two brush regions ({item.BrushRegion} and {i}) are claiming the same boardSquare ({item.name})");
					}
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

	private void SyncListCallbackLastDisruptTurn(SyncList<int>.Operation op, int _incorrectIndexBugIn51And52)
	{
		if (!(FogOfWar.GetClientFog() != null))
		{
			return;
		}
		while (true)
		{
			FogOfWar.GetClientFog().MarkForRecalculateVisibility();
			return;
		}
	}

	[ClientRpc]
	public void RpcUpdateClientFog()
	{
		if (!(FogOfWar.GetClientFog() != null))
		{
			return;
		}
		while (true)
		{
			FogOfWar.GetClientFog().UpdateVisibilityOfSquares();
			return;
		}
	}

	private void Update()
	{
		TrySetupBrushSquares();
		bool flag = !DisableAllBrush();
		bool flag2 = m_brushVisible && (CameraManager.Get() == null || !CameraManager.Get().ShouldHideBrushVfx());
		if (!m_setupBrush)
		{
			return;
		}
		BrushRegion brushRegion;
		bool flag3;
		for (int i = 0; i < m_regions.Length; brushRegion.UpdateBorderVisibility(flag3), i++)
		{
			brushRegion = m_regions[i];
			flag3 = IsRegionFunctioning(i);
			if (brushRegion.m_disruptedVFX != null)
			{
				if (flag)
				{
					if (!flag3)
					{
						if (flag2)
						{
							brushRegion.m_disruptedVFX.GetComponent<PKFxFX>().StartEffect();
							goto IL_0144;
						}
					}
				}
				brushRegion.m_disruptedVFX.GetComponent<PKFxFX>().TerminateEffect();
			}
			else
			{
				for (int j = 0; j < brushRegion.m_perSquareDisruptedVFX.Count; j++)
				{
					GameObject gameObject = brushRegion.m_perSquareDisruptedVFX[j];
					if (flag && !flag3)
					{
						if (flag2)
						{
							gameObject.GetComponent<PKFxFX>().StartEffect();
							continue;
						}
					}
					gameObject.GetComponent<PKFxFX>().TerminateEffect();
				}
			}
			goto IL_0144;
			IL_0144:
			if (brushRegion.m_functioningVFX != null)
			{
				if (flag3)
				{
					if (flag2)
					{
						brushRegion.m_functioningVFX.GetComponent<PKFxFX>().StartEffect();
						continue;
					}
				}
				brushRegion.m_functioningVFX.GetComponent<PKFxFX>().TerminateEffect();
				continue;
			}
			for (int k = 0; k < brushRegion.m_perSquareFunctioningVFX.Count; k++)
			{
				GameObject gameObject2 = brushRegion.m_perSquareFunctioningVFX[k];
				if (flag && flag3 && flag2)
				{
					gameObject2.GetComponent<PKFxFX>().StartEffect();
				}
				else
				{
					gameObject2.GetComponent<PKFxFX>().TerminateEffect();
				}
			}
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public List<ActorData> GetActorsInBrushRegion(int regionIndex)
	{
		List<ActorData> result = null;
		if (DisableAllBrush())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return new List<ActorData>();
				}
			}
		}
		if (regionIndex >= 0)
		{
			if (regionIndex < m_regions.Length)
			{
				BrushRegion brushRegion = m_regions[regionIndex];
				result = brushRegion.GetOccupantActors();
			}
		}
		return result;
	}

	public bool IsRegionFunctioning(int regionIndex)
	{
		if (DisableAllBrush())
		{
			return false;
		}
		if (regionIndex >= 0 && regionIndex < m_regions.Length)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					int num = m_regionsLastDisruptionTurn[regionIndex];
					if (num <= 0)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
					int num2 = GameFlowData.Get().CurrentTurn - num;
					if (num2 >= GameplayData.Get().m_brushDisruptionTurns)
					{
						return true;
					}
					return false;
				}
				}
			}
		}
		return false;
	}

	public bool IsSquareHiddenFrom(BoardSquare subjectSquare, BoardSquare observerSquare)
	{
		if (DisableAllBrush())
		{
			while (true)
			{
				return false;
			}
		}
		bool flag;
		if (!(subjectSquare == null))
		{
			if (!(observerSquare == null))
			{
				if (subjectSquare.BrushRegion == observerSquare.BrushRegion)
				{
					flag = false;
				}
				else if (subjectSquare.IsInBrushRegion())
				{
					int brushRegion = subjectSquare.BrushRegion;
					if (IsRegionFunctioning(brushRegion))
					{
						float num = observerSquare.HorizontalDistanceOnBoardTo(subjectSquare);
						float distanceCanSeeIntoBrush = GameplayData.Get().m_distanceCanSeeIntoBrush;
						if (num > distanceCanSeeIntoBrush)
						{
							flag = true;
						}
						else
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
				goto IL_00d1;
			}
		}
		flag = false;
		goto IL_00d1;
		IL_0144:
		int result;
		bool flag2;
		if (flag)
		{
			result = ((!flag2) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
		IL_00d1:
		if (!flag)
		{
			return false;
		}
		if (!(subjectSquare == null))
		{
			if (!(subjectSquare.OccupantActor == null))
			{
				ActorStatus actorStatus = subjectSquare.OccupantActor.GetActorStatus();
				if (!actorStatus.HasStatus(StatusType.Revealed))
				{
					if (!actorStatus.HasStatus(StatusType.CantHideInBrush))
					{
						flag2 = false;
						goto IL_0144;
					}
				}
				flag2 = true;
				goto IL_0144;
			}
		}
		flag2 = false;
		goto IL_0144;
	}

	internal List<Plane> CalcIntersectingBrushSidePlanes(Bounds bounds)
	{
		List<Plane> list = new List<Plane>(4);
		Vector3 center = bounds.center;
		float x = center.x;
		Vector3 center2 = bounds.center;
		Bounds bounds2 = new Bounds(new Vector3(x, 0f, center2.z), bounds.size);
		List<BoardSquare> list2 = Board.Get()._000E(bounds2);
		for (int i = 0; i < list2.Count; i++)
		{
			BoardSquare boardSquare = list2[i];
			byte brushExteriorSideFlags = GetBrushExteriorSideFlags(boardSquare);
			for (byte b = 0; b < 4; b = (byte)(b + 1))
			{
				byte b2 = (byte)(1 << (int)b);
				if ((b2 & brushExteriorSideFlags) != 0)
				{
					if (boardSquare.CalcSideBounds((SideFlags)b2).Intersects(bounds))
					{
						list.Add(boardSquare.CalcSidePlane((SideFlags)b2));
					}
				}
			}
		}
		while (true)
		{
			return list;
		}
	}

	internal byte GetBrushExteriorSideFlags(BoardSquare square)
	{
		if (IsRegionFunctioning(square.BrushRegion))
		{
			BrushRegion brushRegion = m_regions[square.BrushRegion];
			return brushRegion.GetExteriorSideFlags(square);
		}
		return 0;
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera() || !m_setupBrush)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_regions.Length; i++)
			{
				BrushRegion brushRegion = m_regions[i];
				bool functioning = IsRegionFunctioning(i);
				brushRegion.DrawOutlineGizmos(functioning);
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_regionsLastDisruptionTurn(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_regionsLastDisruptionTurn called on server.");
					return;
				}
			}
		}
		((BrushCoordinator)obj).m_regionsLastDisruptionTurn.HandleMsg(reader);
	}

	protected static void InvokeRpcRpcUpdateClientFog(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC RpcUpdateClientFog called on server.");
					return;
				}
			}
		}
		((BrushCoordinator)obj).RpcUpdateClientFog();
	}

	public void CallRpcUpdateClientFog()
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcUpdateClientFog called on client.");
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcUpdateClientFog);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendRPCInternal(networkWriter, 0, "RpcUpdateClientFog");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					SyncListInt.WriteInstance(writer, m_regionsLastDisruptionTurn);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_regionsLastDisruptionTurn);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		int setBits = int.MaxValue;
		if (!initialState)
		{
			setBits = (int)reader.ReadPackedUInt32();
		}
        if ((setBits & 1) != 0)
        {
            SyncListInt.ReadReference(reader, m_regionsLastDisruptionTurn);
        }
    }
}
