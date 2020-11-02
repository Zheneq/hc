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

	private static int kListm_regionsLastDisruptionTurn = 1707345339;
	private static int kRpcRpcUpdateClientFog = -2062592578;

	static BrushCoordinator()
	{		
		RegisterRpcDelegate(typeof(BrushCoordinator), kRpcRpcUpdateClientFog, InvokeRpcRpcUpdateClientFog);
		RegisterSyncListDelegate(typeof(BrushCoordinator), kListm_regionsLastDisruptionTurn, InvokeSyncListm_regionsLastDisruptionTurn);
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
		if (eventType == GameEventManager.EventType.GameCameraCreated
			&& (VisualsLoader.Get() == null || VisualsLoader.Get().LevelLoaded()))
		{
			m_cameraCreated = true;
		}
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
		return (SinglePlayerManager.Get() != null && !SinglePlayerManager.Get().EnableBrush())
			|| (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("DisableBrush"));
	}

	private void Start()
	{
		foreach (BrushRegion region in m_regions)
		{
			region.Initialize();
			if (NetworkServer.active)
			{
				m_regionsLastDisruptionTurn.Add(-1);
			}
		}
		TrySetupBrushSquares();
		DebugCheckRegions();
	}

	private void TrySetupBrushSquares()
	{
		if (!m_setupBrush
			&& Board.Get() != null
			&& GameFlowData.Get() != null
			&& m_cameraCreated)
		{
			SetupBrushSquares();
			FogOfWar.CalculateFogOfWarForTeam(Team.TeamA);
			FogOfWar.CalculateFogOfWarForTeam(Team.TeamB);
			m_setupBrush = true;
		}
	}

	private void DebugCheckRegions()
	{
		if (Application.isEditor)
		{
			for (int i = 0; i < m_regions.Length; i++)
			{
				BrushRegion brushRegion = m_regions[i];
				if (brushRegion != null)
				{
					if (brushRegion.m_disruptedVFX != null && brushRegion.m_disruptedVFX.GetComponent<PKFxFX>() == null)
					{
						Debug.LogError("Brush Region at index " + i + " has Disrupted VFX field set but it doesn't have a PKFxFX component");
					}
					if (brushRegion.m_functioningVFX != null && brushRegion.m_functioningVFX.GetComponent<PKFxFX>() == null)
					{
						Debug.LogError("Brush Region at index " + i + " has Functioning VFX field set but it doesn't have a PKFxFX component");
					}
				}
				else
				{
					Debug.LogError("Brush Region at index " + i + " is null");
				}
			}
		}
	}

	private void SetupBrushSquares()
	{
		for (int i = 0; i < m_regions.Length; i++)
		{
			if (m_regions[i] != null)
			{
				foreach (BoardSquare item in m_regions[i].GetSquaresInRegion())
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
			else
			{
				Log.Error($"Null brush region ({i}); fix brush coordinator's data.");
			}
		}
	}

	private void SyncListCallbackLastDisruptTurn(SyncList<int>.Operation op, int _incorrectIndexBugIn51And52)
	{
		if (FogOfWar.GetClientFog() != null)
		{
			FogOfWar.GetClientFog().MarkForRecalculateVisibility();
		}
	}

	[ClientRpc]
	public void RpcUpdateClientFog()
	{
		Log.Info($"[JSON] {{\"RpcUpdateClientFog\":{{}}}}");
		if (FogOfWar.GetClientFog() != null)
		{
			FogOfWar.GetClientFog().UpdateVisibilityOfSquares();
		}
	}

	private void Update()
	{
		TrySetupBrushSquares();
		bool areBrushesEnabled = !DisableAllBrush();
		bool areVfxVisible = m_brushVisible
			&& (CameraManager.Get() == null || !CameraManager.Get().ShouldHideBrushVfx());
		if (!m_setupBrush)
		{
			return;
		}
		for (int i = 0; i < m_regions.Length; i++)
		{
			BrushRegion brushRegion = m_regions[i];
			bool isRegionFunctioning = IsRegionFunctioning(i);
			if (brushRegion.m_disruptedVFX != null)
			{
				if (areBrushesEnabled && !isRegionFunctioning && areVfxVisible)
				{
					brushRegion.m_disruptedVFX.GetComponent<PKFxFX>().StartEffect();
				}
				else
				{
					brushRegion.m_disruptedVFX.GetComponent<PKFxFX>().TerminateEffect();
				}
			}
			else
			{
				foreach (GameObject vfx in brushRegion.m_perSquareDisruptedVFX)
				{
					if (areBrushesEnabled && !isRegionFunctioning && areVfxVisible)
					{
						vfx.GetComponent<PKFxFX>().StartEffect();
					}
					else
					{
						vfx.GetComponent<PKFxFX>().TerminateEffect();
					}
				}
			}
			if (brushRegion.m_functioningVFX != null)
			{
				if (isRegionFunctioning && areVfxVisible)
				{
					brushRegion.m_functioningVFX.GetComponent<PKFxFX>().StartEffect();
				}
				else
				{
					brushRegion.m_functioningVFX.GetComponent<PKFxFX>().TerminateEffect();
				}
			}
			else
			{
				foreach (GameObject vfx in brushRegion.m_perSquareFunctioningVFX)
				{
					if (areBrushesEnabled && isRegionFunctioning && areVfxVisible)
					{
						vfx.GetComponent<PKFxFX>().StartEffect();
					}
					else
					{
						vfx.GetComponent<PKFxFX>().TerminateEffect();
					}
				}
			}
			brushRegion.UpdateBorderVisibility(isRegionFunctioning);
		}
	}

	public List<ActorData> GetActorsInBrushRegion(int regionIndex)
	{
		if (DisableAllBrush())
		{
			return new List<ActorData>();
		}
		if (regionIndex >= 0 && regionIndex < m_regions.Length)
		{
			return m_regions[regionIndex].GetActorsInRegion();
		}
		return null;
	}

	public bool IsRegionFunctioning(int regionIndex)
	{
		if (DisableAllBrush())
		{
			return false;
		}
		if (regionIndex >= 0 && regionIndex < m_regions.Length)
		{
			int lastDisruptionTurn = m_regionsLastDisruptionTurn[regionIndex];
			if (lastDisruptionTurn <= 0)
			{
				return true;
			}
			int turnsPassed = GameFlowData.Get().CurrentTurn - lastDisruptionTurn;
			if (turnsPassed >= GameplayData.Get().m_brushDisruptionTurns)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsSquareHiddenFrom(BoardSquare subjectSquare, BoardSquare observerSquare)
	{
		if (DisableAllBrush())
		{
			return false;
		}
		bool isHidden = false;
		if (subjectSquare != null
			&& observerSquare != null
			&& subjectSquare.BrushRegion != observerSquare.BrushRegion
			&& subjectSquare.IsInBrush()
			&& IsRegionFunctioning(subjectSquare.BrushRegion))
		{
			float distance = observerSquare.HorizontalDistanceOnBoardTo(subjectSquare);
			float distanceCanSeeIntoBrush = GameplayData.Get().m_distanceCanSeeIntoBrush;
			isHidden = distance > distanceCanSeeIntoBrush;
		}
		if (!isHidden)
		{
			return false;
		}
		bool isRevealed = false;
		if (subjectSquare != null && subjectSquare.OccupantActor != null)
		{
			ActorStatus actorStatus = subjectSquare.OccupantActor.GetActorStatus();
			isRevealed = actorStatus.HasStatus(StatusType.Revealed)
				|| actorStatus.HasStatus(StatusType.CantHideInBrush);
		}
		return isHidden && !isRevealed;
	}

	internal List<Plane> CalcIntersectingBrushSidePlanes(Bounds bounds)
	{
		List<Plane> result = new List<Plane>(4);
		Bounds bounds2 = new Bounds(new Vector3(bounds.center.x, 0f, bounds.center.z), bounds.size);
		foreach (BoardSquare boardSquare in Board.Get().GetSquaresInBox(bounds2))
		{
			byte brushExteriorSideFlags = GetBrushExteriorSideFlags(boardSquare);
			for (byte b = 0; b < 4; b++)
			{
				byte mask = (byte)(1 << b);
				if ((mask & brushExteriorSideFlags) != 0
					&& boardSquare.CalcSideBounds((SideFlags)mask).Intersects(bounds))
				{
					result.Add(boardSquare.CalcSidePlane((SideFlags)mask));
				}
			}
		}
		return result;
	}

	internal byte GetBrushExteriorSideFlags(BoardSquare square)
	{
		if (IsRegionFunctioning(square.BrushRegion))
		{
			return m_regions[square.BrushRegion].GetExteriorSideFlags(square);
		}
		return 0;
	}

	private void OnDrawGizmos()
	{
		if (CameraManager.ShouldDrawGizmosForCurrentCamera() && m_setupBrush)
		{
			for (int i = 0; i < m_regions.Length; i++)
			{
				m_regions[i].DrawOutlineGizmos(IsRegionFunctioning(i));
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
			Debug.LogError("SyncList m_regionsLastDisruptionTurn called on server.");
			return;
		}
		((BrushCoordinator)obj).m_regionsLastDisruptionTurn.HandleMsg(reader);
		Log.Info($"[JSON] {{\"regionsLastDisruptionTurn\":{DefaultJsonSerializer.Serialize(((BrushCoordinator)obj).m_regionsLastDisruptionTurn)}}}");
	}

	protected static void InvokeRpcRpcUpdateClientFog(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcUpdateClientFog called on server.");
			return;
		}
		((BrushCoordinator)obj).RpcUpdateClientFog();
	}

	public void CallRpcUpdateClientFog()
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcUpdateClientFog called on client.");
			return;
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
			SyncListInt.WriteInstance(writer, m_regionsLastDisruptionTurn);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_regionsLastDisruptionTurn);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
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
		LogJson(setBits);
	}

	private void LogJson(int setBits = int.MaxValue)
	{
		var jsonLog = new List<string>();
		if ((setBits & 1) != 0)
		{
			jsonLog.Add($"\"regionsLastDisruptionTurn\":{DefaultJsonSerializer.Serialize(m_regionsLastDisruptionTurn)}");
		}

		Log.Info($"[JSON] {{\"brushCoordinator\":{{{System.String.Join(",", jsonLog.ToArray())}}}}}");
	}
}
