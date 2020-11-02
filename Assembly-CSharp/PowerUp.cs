using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUp : NetworkBehaviour
{
	public enum PowerUpCategory
	{
		NoCategory,
		Healing,
		Might,
		Movement,
		Energy
	}

	public interface IPowerUpListener
	{
		void OnPowerUpDestroyed(PowerUp powerUp);
		PowerUp[] GetActivePowerUps();
		void SetSpawningEnabled(bool enabled);
		void OnTurnTick();
		bool IsPowerUpSpawnPoint(BoardSquare square);
		void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor);
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public int m_pickupTeamAsInt;
		public bool m_ignoreSpawnSpline;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			sbyte value = (sbyte)m_pickupTeamAsInt;
			stream.Serialize(ref value);
			stream.Serialize(ref m_ignoreSpawnSpline);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte value = (sbyte)m_pickupTeamAsInt;
			stream.Serialize(ref value);
			m_pickupTeamAsInt = value;
			stream.Serialize(ref m_ignoreSpawnSpline);
		}
	}

	private static int s_nextPowerupGuid;
	public Ability m_ability;
	public string m_powerUpName;
	public string m_powerUpToolTip;
	[AudioEvent(false)]
	public string m_audioEventPickUp = "ui_pickup_health";
	public GameObject m_sequencePrefab;
	public bool m_restrictPickupByTeam;
	public PowerUpCategory m_chatterCategory;

	[SyncVar]
	private Team m_pickupTeam = Team.Objects;
	[SyncVar(hook = "HookSetGuid")]
	private int m_guid = -1;
	[SyncVar]
	private uint m_sequenceSourceId;

	private List<int> m_clientSequenceIds = new List<int>();
	private BoardSquare m_boardSquare;

	[SyncVar]
	public bool m_isSpoil;
	[SyncVar]
	public bool m_ignoreSpawnSplineForSequence;

	public int m_duration;
	private int m_age;
	private bool m_pickedUp;
	private bool m_stolen;
	private ActorTag m_tags;
	private bool m_markedForRemoval;
	private SequenceSource _sequenceSource;

	private static int kRpcRpcOnPickedUp = -430057904;
	private static int kRpcRpcOnSteal = 1919536730;

	public int Guid
	{
		get
		{
			return m_guid;
		}
		private set
		{
		}
	}

	public IPowerUpListener powerUpListener { get; set; }

	public BoardSquare boardSquare => m_boardSquare;

	public Team PickupTeam
	{
		get
		{
			return m_pickupTeam;
		}
		set
		{
		}
	}

	internal SequenceSource SequenceSource
	{
		get
		{
			if (_sequenceSource == null)
			{
				_sequenceSource = new SequenceSource(null, null, false);
			}
			return _sequenceSource;
		}
	}

	public Team Networkm_pickupTeam
	{
		get
		{
			return m_pickupTeam;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_pickupTeam, 1u);
		}
	}

	public int Networkm_guid
	{
		get
		{
			return m_guid;
		}
		[param: In]
		set
		{
			if (NetworkServer.localClientActive && !syncVarHookGuard)
			{
				syncVarHookGuard = true;
				HookSetGuid(value);
				syncVarHookGuard = false;
			}
			SetSyncVar(value, ref m_guid, 2u);
		}
	}

	public uint Networkm_sequenceSourceId
	{
		get
		{
			return m_sequenceSourceId;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_sequenceSourceId, 4u);
		}
	}

	public bool Networkm_isSpoil
	{
		get
		{
			return m_isSpoil;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_isSpoil, 8u);
		}
	}

	public bool Networkm_ignoreSpawnSplineForSequence
	{
		get
		{
			return m_ignoreSpawnSplineForSequence;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_ignoreSpawnSplineForSequence, 16u);
		}
	}

	static PowerUp()
	{
		RegisterRpcDelegate(typeof(PowerUp), kRpcRpcOnPickedUp, InvokeRpcRpcOnPickedUp);
		RegisterRpcDelegate(typeof(PowerUp), kRpcRpcOnSteal, InvokeRpcRpcOnSteal);
		NetworkCRC.RegisterBehaviour("PowerUp", 0);
	}

	public void SetPickupTeam(Team value)
	{
		Networkm_pickupTeam = value;
	}

	public void AddTag(string powerupTag)
	{
		if (m_tags == null)
		{
			m_tags = gameObject.GetComponent<ActorTag>();
			if (m_tags == null)
			{
				m_tags = gameObject.AddComponent<ActorTag>();
			}
		}
		m_tags.AddTag(powerupTag);
	}

	public bool HasTag(string powerupTag)
	{
		return m_tags != null && m_tags.HasTag(powerupTag);
	}

	public void ClientSpawnSequences()
	{
		if (NetworkClient.active && m_clientSequenceIds.Count == 0)
		{
			BoardSquare squareFromPos = Board.Get().GetSquareFromPos(transform.position.x, transform.position.z);
			ExtraParams extraParams = new ExtraParams
			{
				m_pickupTeamAsInt = m_restrictPickupByTeam ? (int)m_pickupTeam : 2,
				m_ignoreSpawnSpline = m_ignoreSpawnSplineForSequence
			};
			Sequence.IExtraSequenceParams[] extraParams2 = new Sequence.IExtraSequenceParams[]
			{
				extraParams
			};
			if (_sequenceSource == null)
			{
				_sequenceSource = new SequenceSource(null, null, m_sequenceSourceId, false);
			}
			Sequence[] array = SequenceManager.Get().CreateClientSequences(m_sequencePrefab, squareFromPos, null, null, SequenceSource, extraParams2);
			bool flag = false;
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					array[i].RemoveAtTurnEnd = false;
					m_clientSequenceIds.Add(array[i].Id);
					if (!flag && PowerUpManager.Get() != null)
					{
						array[i].transform.parent = PowerUpManager.Get().GetSpawnedPersistentSequencesRoot().transform;
						flag = true;
					}
				}
			}
		}
	}

	public void CalculateBoardSquare()
	{
		m_boardSquare = Board.Get().GetSquareFromPos(transform.position.x, transform.position.z);
	}

	public void SetDuration(int duration)
	{
		m_duration = duration;
	}

	private void Awake()
	{
		if (NetworkServer.active)
		{
			Networkm_guid = s_nextPowerupGuid++;
			SequenceSource sequenceSource = SequenceSource;
			Networkm_sequenceSourceId = sequenceSource.RootID;
		}
	}

	private void HookSetGuid(int guid)
	{
		Networkm_guid = guid;
		PowerUpManager.Get().SetPowerUpGuid(this, guid);
	}

	private void Start()
	{
		if (m_ability == null)
		{
			Log.Error("PowerUp " + this + " needs a valid Ability assigned in the inspector for its prefab");
		}
		transform.parent = PowerUpManager.Get().GetSpawnedPowerupsRoot().transform;
		tag = "powerup";
		int layer = LayerMask.NameToLayer("PowerUp");
		foreach (Transform t in GetComponentsInChildren<Transform>())
		{
			t.gameObject.layer = layer;
		}
		if (m_boardSquare == null)
		{
			CalculateBoardSquare();
		}
		if (NetworkClient.active)
		{
			if (PowerUpManager.Get() != null)
			{
				PowerUpManager.Get().TrackClientPowerUp(this);
			}
			ClientSpawnSequences();
		}
	}

	[Server]
	public void CheckForPickupOnSpawn()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PowerUp::CheckForPickupOnSpawn()' called on client");
			return;
		}
	}

	public void CheckForPickupOnTurnStart()
	{
	}

	public void MarkSequencesForRemoval()
	{
		m_markedForRemoval = true;
		if (SequenceManager.Get() != null)
		{
			for (int i = 0; i < m_clientSequenceIds.Count; i++)
			{
				Sequence sequence = SequenceManager.Get().FindSequence(m_clientSequenceIds[i]);
				if (sequence != null)
				{
					sequence.MarkForRemoval();
				}
			}
		}
	}

	public bool WasMarkedForRemoval()
	{
		return m_markedForRemoval;
	}

	public void SetHideSequence(bool hide)
	{
		for (int i = 0; i < m_clientSequenceIds.Count; i++)
		{
			Sequence sequence = SequenceManager.Get().FindSequence(m_clientSequenceIds[i]);
			if (sequence != null)
			{
				sequence.gameObject.transform.localPosition = hide
					? new Vector3(0f, -100f, 0f)
					: Vector3.zero;
			}
		}
	}

	[ClientRpc]
	private void RpcOnPickedUp(int pickedUpByActorIndex)
	{
		Log.Info($"[JSON] {{\"RpcOnPickedUp\":{{\"pickedUpByActorIndex\":{DefaultJsonSerializer.Serialize(pickedUpByActorIndex)}}}}}");
		Client_OnPickedUp(pickedUpByActorIndex);
	}

	public void Client_OnPickedUp(int pickedUpByActorIndex)
	{
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(pickedUpByActorIndex);
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null && clientFog.IsVisible(boardSquare))
		{
			AudioManager.PostEvent(m_audioEventPickUp, (actorData == null) ? gameObject : actorData.gameObject);
		}
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.PowerUpActivated, new GameEventManager.PowerUpActivatedArgs
		{
			byActor = actorData,
			powerUp = this
		});
		MarkSequencesForRemoval();
	}

	[ClientRpc]
	private void RpcOnSteal(int actorIndexFor3DAudio)
	{
		Log.Info($"[JSON] {{\"RpcOnSteal\":{{\"actorIndexFor3DAudio\":{DefaultJsonSerializer.Serialize(actorIndexFor3DAudio)}}}}}");
		Client_OnSteal(actorIndexFor3DAudio);
	}

	public void Client_OnSteal(int actorIndexFor3DAudio)
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null && clientFog.IsVisible(boardSquare))
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndexFor3DAudio);
			AudioManager.PostEvent(m_audioEventPickUp, (actorData == null) ? gameObject : actorData.gameObject);
		}
		MarkSequencesForRemoval();
	}

	[Server]
	internal void Destroy()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PowerUp::Destroy()' called on client");
			return;
		}
		if (powerUpListener != null)
		{
			powerUpListener.OnPowerUpDestroyed(this);
		}
		NetworkServer.Destroy(gameObject);
	}

	private void OnDestroy()
	{
		if (NetworkClient.active)
		{
			if (PowerUpManager.Get() != null)
			{
				PowerUpManager.Get().UntrackClientPowerUp(this);
			}
			MarkSequencesForRemoval();
			m_clientSequenceIds.Clear();
		}
		if (PowerUpManager.Get() != null)
		{
			PowerUpManager.Get().OnPowerUpDestroy(this);
		}
	}

	public bool TeamAllowedForPickUp(Team team)
	{
		return !m_restrictPickupByTeam || team == PickupTeam;
	}

	internal void OnTurnTick()
	{
	}

	public bool CanBeStolen()
	{
		return true;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeRpcRpcOnPickedUp(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcOnPickedUp called on server.");
			return;
		}
		((PowerUp)obj).RpcOnPickedUp((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcOnSteal(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcOnSteal called on server.");
			return;
		}
		((PowerUp)obj).RpcOnSteal((int)reader.ReadPackedUInt32());
	}

	public void CallRpcOnPickedUp(int pickedUpByActorIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcOnPickedUp called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcOnPickedUp);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)pickedUpByActorIndex);
		SendRPCInternal(networkWriter, 0, "RpcOnPickedUp");
	}

	public void CallRpcOnSteal(int actorIndexFor3DAudio)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcOnSteal called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcOnSteal);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndexFor3DAudio);
		SendRPCInternal(networkWriter, 0, "RpcOnSteal");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write((int)m_pickupTeam);
			writer.WritePackedUInt32((uint)m_guid);
			writer.WritePackedUInt32(m_sequenceSourceId);
			writer.Write(m_isSpoil);
			writer.Write(m_ignoreSpawnSplineForSequence);
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
			writer.Write((int)m_pickupTeam);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_guid);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(m_sequenceSourceId);
		}
		if ((syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_isSpoil);
		}
		if ((syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_ignoreSpawnSplineForSequence);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_pickupTeam = (Team)reader.ReadInt32();
			m_guid = (int)reader.ReadPackedUInt32();
			m_sequenceSourceId = reader.ReadPackedUInt32();
			m_isSpoil = reader.ReadBoolean();
			m_ignoreSpawnSplineForSequence = reader.ReadBoolean();
			LogJson();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_pickupTeam = (Team)reader.ReadInt32();
		}
		if ((num & 2) != 0)
		{
			HookSetGuid((int)reader.ReadPackedUInt32());
		}
		if ((num & 4) != 0)
		{
			m_sequenceSourceId = reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			m_isSpoil = reader.ReadBoolean();
		}
		if ((num & 0x10) != 0)
		{
			m_ignoreSpawnSplineForSequence = reader.ReadBoolean();
		}
		LogJson(num);
	}

	private void LogJson(int mask = int.MaxValue)
	{
		var jsonLog = new List<string>();
		if ((mask & 1) != 0)
		{
			jsonLog.Add($"\"pickupTeam\":{DefaultJsonSerializer.Serialize(m_pickupTeam)}");
		}
		if ((mask & 2) != 0)
		{
			jsonLog.Add($"\"guid\":{DefaultJsonSerializer.Serialize(Networkm_guid)}");
		}
		if ((mask & 4) != 0)
		{
			jsonLog.Add($"\"sequenceSourceId\":{DefaultJsonSerializer.Serialize(m_sequenceSourceId)}");
		}
		if ((mask & 8) != 0)
		{
			jsonLog.Add($"\"isSpoil\":{DefaultJsonSerializer.Serialize(m_isSpoil)}");
		}
		if ((mask & 16) != 0)
		{
			jsonLog.Add($"\"ignoreSpawnSplineForSequence\":{DefaultJsonSerializer.Serialize(m_ignoreSpawnSplineForSequence)}");
		}

		Log.Info($"[JSON] {{\"powerUp\":{{{System.String.Join(",", jsonLog.ToArray())}}}}}");
	}
}
