using UnityEngine;
using UnityEngine.Networking;

public class CTF_Flag : NetworkBehaviour
{
	public byte m_flagGuid;

	public Team m_team;

	private ActorData m_serverHolderActor;

	private BoardSquare m_serverIdleSquare;

	private ActorData m_clientHolderActor;

	private BoardSquare m_clientIdleSquare;

	private Sequence m_flagBeingHeldSequenceInstance;

	private bool m_initializedOffscreenIndicator;

	private bool m_notifiedOfSpawn;

	private bool m_alreadyTurnedIn;

	private int m_lastClientUpdateFlagHolderEventGuid = -1;

	private ActorData m_gatheredHolderActor;

	private BoardSquare m_gatheredIdleSquare;

	private BoardSquarePathInfo m_gatheredPath;

	private int m_gatheredMovementDamageSincePickedUp;

	private int m_gatheredMovementDamageSinceTurnStart;

	private BoardSquare m_originalSquare;

	private int m_damageOnHolderSincePickedUp_Gross;

	private int m_damageOnHolderSinceTurnStart_Gross;

	private int m_clientUnresolvedDamageOnHolder;

	private int m_numFullTurnsSpentHeldInTurninRegion;

	private bool m_spentEntireTurnReadyToBeTurnedIn;

	public ActorData ServerHolderActor
	{
		get
		{
			return m_serverHolderActor;
		}
		set
		{
			if (!(value != m_serverHolderActor))
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_serverHolderActor = value;
				DamageOnHolderSincePickedUp_Gross = 0;
				DamageOnHolderSinceTurnStart_Gross = 0;
				return;
			}
		}
	}

	public BoardSquare ServerIdleSquare
	{
		get
		{
			return m_serverIdleSquare;
		}
		set
		{
			if (value != m_serverIdleSquare)
			{
				m_serverIdleSquare = value;
			}
		}
	}

	public ActorData ClientHolderActor
	{
		get
		{
			return m_clientHolderActor;
		}
		set
		{
			if (!(value != m_clientHolderActor))
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_clientHolderActor = value;
				ClientUnresolvedDamageOnHolder = 0;
				if (!(CaptureTheFlag.Get() != null) || !(CaptureTheFlag.Get().m_flagBeingHeldSequence != null))
				{
					return;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					if (m_flagBeingHeldSequenceInstance != null)
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
						if (!m_flagBeingHeldSequenceInstance.MarkedForRemoval)
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
							m_flagBeingHeldSequenceInstance.MarkForRemoval();
						}
						m_flagBeingHeldSequenceInstance = null;
					}
					if (!(m_clientHolderActor != null))
					{
						return;
					}
					GameObject flagBeingHeldSequence = CaptureTheFlag.Get().m_flagBeingHeldSequence;
					BoardSquare currentBoardSquare = m_clientHolderActor.CurrentBoardSquare;
					SequenceSource sequenceSource = CaptureTheFlag.Get().SequenceSource;
					Sequence[] array = SequenceManager.Get().CreateClientSequences(flagBeingHeldSequence, currentBoardSquare, m_clientHolderActor.AsArray(), m_clientHolderActor, sequenceSource, null);
					if (array != null && array.Length != 0)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (array.Length <= 1)
						{
							m_flagBeingHeldSequenceInstance = array[0];
							return;
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					Debug.LogError("CTF_Flag creating flag-being-held sequence, but had bad output.");
					return;
				}
			}
		}
	}

	public BoardSquare ClientIdleSquare
	{
		get
		{
			return m_clientIdleSquare;
		}
		set
		{
			if (value != m_clientIdleSquare)
			{
				m_clientIdleSquare = value;
			}
		}
	}

	public int LastClientUpdateFlagHolderEventGuid
	{
		get
		{
			return m_lastClientUpdateFlagHolderEventGuid;
		}
		set
		{
			if (m_lastClientUpdateFlagHolderEventGuid == value)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_lastClientUpdateFlagHolderEventGuid = value;
				return;
			}
		}
	}

	public ActorData GatheredHolderActor
	{
		get
		{
			return m_gatheredHolderActor;
		}
		set
		{
			if (!(value != m_gatheredHolderActor))
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_gatheredHolderActor = value;
				GatheredMovementDamageSincePickedUp = 0;
				GatheredMovementDamageSinceTurnStart = 0;
				return;
			}
		}
	}

	public BoardSquare GatheredIdleSquare
	{
		get
		{
			return m_gatheredIdleSquare;
		}
		set
		{
			if (!(value != m_gatheredIdleSquare))
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_gatheredIdleSquare = value;
				return;
			}
		}
	}

	public BoardSquarePathInfo GatheredPath
	{
		get
		{
			return m_gatheredPath;
		}
		set
		{
			if (value == m_gatheredPath)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_gatheredPath = value;
				return;
			}
		}
	}

	public int GatheredMovementDamageSincePickedUp
	{
		get
		{
			return m_gatheredMovementDamageSincePickedUp;
		}
		set
		{
			if (value == m_gatheredMovementDamageSincePickedUp)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_gatheredMovementDamageSincePickedUp = value;
				return;
			}
		}
	}

	public int GatheredMovementDamageSinceTurnStart
	{
		get
		{
			return m_gatheredMovementDamageSinceTurnStart;
		}
		set
		{
			if (m_gatheredMovementDamageSinceTurnStart == value)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_gatheredMovementDamageSinceTurnStart = value;
				return;
			}
		}
	}

	public int DamageOnHolderSincePickedUp_Gross
	{
		get
		{
			return m_damageOnHolderSincePickedUp_Gross;
		}
		set
		{
			if (value == m_damageOnHolderSincePickedUp_Gross)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_damageOnHolderSincePickedUp_Gross = value;
				ClientUnresolvedDamageOnHolder = 0;
				return;
			}
		}
	}

	public int DamageOnHolderSinceTurnStart_Gross
	{
		get
		{
			return m_damageOnHolderSinceTurnStart_Gross;
		}
		set
		{
			if (value != m_damageOnHolderSinceTurnStart_Gross)
			{
				m_damageOnHolderSinceTurnStart_Gross = value;
				ClientUnresolvedDamageOnHolder = 0;
			}
		}
	}

	public int ClientUnresolvedDamageOnHolder
	{
		get
		{
			return m_clientUnresolvedDamageOnHolder;
		}
		set
		{
			if (value != m_clientUnresolvedDamageOnHolder)
			{
				m_clientUnresolvedDamageOnHolder = value;
			}
		}
	}

	public int NumFullTurnsSpentHeldInTurninRegion
	{
		get
		{
			return m_numFullTurnsSpentHeldInTurninRegion;
		}
		private set
		{
			if (m_numFullTurnsSpentHeldInTurninRegion == value)
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
				m_numFullTurnsSpentHeldInTurninRegion = value;
				return;
			}
		}
	}

	public bool SpentEntireTurnReadyToBeTurnedIn
	{
		get
		{
			return m_spentEntireTurnReadyToBeTurnedIn;
		}
		private set
		{
			if (m_spentEntireTurnReadyToBeTurnedIn != value)
			{
				m_spentEntireTurnReadyToBeTurnedIn = value;
			}
		}
	}

	public void Initialize(BoardSquare square, Team team, byte flagGuid)
	{
		m_originalSquare = square;
		m_team = team;
		m_flagGuid = flagGuid;
		m_serverHolderActor = null;
		m_serverIdleSquare = square;
		m_clientHolderActor = null;
		m_clientIdleSquare = square;
		UpdatePosition();
	}

	public Sprite GetIcon()
	{
		if (CaptureTheFlag.Get() != null)
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
					return CaptureTheFlag.Get().m_flagIcon;
				}
			}
		}
		return null;
	}

	public bool ShouldShowIndicator()
	{
		return m_clientIdleSquare != null;
	}

	public void OnNotHeldInTurninRegion()
	{
		NumFullTurnsSpentHeldInTurninRegion = 0;
		SpentEntireTurnReadyToBeTurnedIn = false;
	}

	private void Start()
	{
		if (CaptureTheFlag.Get() != null)
		{
			CaptureTheFlag.Get().OnNewFlagStarted(this);
		}
	}

	private void OnDestroy()
	{
		if (CaptureTheFlag.Get() != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			CaptureTheFlag.Get().OnFlagDestroyed(this);
		}
		if (!(HUD_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveCtfFlag(this);
			return;
		}
	}

	public Vector3 GetPosition()
	{
		if (ClientHolderActor == null)
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
					if (ClientIdleSquare == null)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return base.transform.position;
							}
						}
					}
					return ClientIdleSquare.ToVector3();
				}
			}
		}
		if (ClientHolderActor.IsVisibleToClient())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return ClientHolderActor.transform.position;
				}
			}
		}
		return base.transform.position;
	}

	public Quaternion GetRotation()
	{
		if (ClientHolderActor == null)
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
					return Quaternion.identity;
				}
			}
		}
		return ClientHolderActor.transform.rotation;
	}

	public BoardSquare GetOriginalSquare()
	{
		return m_originalSquare;
	}

	public Team GetIntrinsicTeam()
	{
		return m_team;
	}

	public Team GetCapturingTeam_Client()
	{
		if (m_team == Team.TeamA)
		{
			return Team.TeamB;
		}
		if (m_team == Team.TeamB)
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
					return Team.TeamA;
				}
			}
		}
		if (ClientHolderActor != null)
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
			if (ClientHolderActor.GetTeam() == Team.TeamA)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return Team.TeamA;
					}
				}
			}
		}
		if (ClientHolderActor != null && ClientHolderActor.GetTeam() == Team.TeamB)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return Team.TeamB;
				}
			}
		}
		return Team.Objects;
	}

	public void OnPickedUp_Client(ActorData newHolder, int eventGuid)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Log.Error("Calling CTF_Flag.OnPickedUp_Client on a non-client.");
					return;
				}
			}
		}
		if (eventGuid != -1)
		{
			if (eventGuid <= LastClientUpdateFlagHolderEventGuid)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		LastClientUpdateFlagHolderEventGuid = eventGuid;
		ActorData clientHolderActor = ClientHolderActor;
		ClientHolderActor = newHolder;
		ClientIdleSquare = null;
		if (CaptureTheFlag.Get() != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			CaptureTheFlag.Get().Client_OnFlagHolderChanged(clientHolderActor, ClientHolderActor, false, m_alreadyTurnedIn);
		}
		GameEventManager.MatchObjectiveEventArgs matchObjectiveEventArgs = new GameEventManager.MatchObjectiveEventArgs();
		matchObjectiveEventArgs.objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.CasePickedUp_Client;
		matchObjectiveEventArgs.controlPoint = null;
		matchObjectiveEventArgs.activatingActor = newHolder;
		matchObjectiveEventArgs.team = newHolder.GetTeam();
		GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, matchObjectiveEventArgs);
	}

	public void OnReturned_Client(ActorData returner)
	{
		if (!NetworkClient.active)
		{
			Log.Error("Calling CTF_Flag.OnReturned_Client on a non-client.");
			return;
		}
		ClientHolderActor = null;
		ClientIdleSquare = m_originalSquare;
	}

	public void OnDropped_Client(BoardSquare newIdleSquare, int eventGuid)
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
					Log.Error("Calling CTF_Flag.OnDropped_Client on a non-client.");
					return;
				}
			}
		}
		if (eventGuid != -1)
		{
			if (eventGuid <= LastClientUpdateFlagHolderEventGuid)
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
				break;
			}
		}
		LastClientUpdateFlagHolderEventGuid = eventGuid;
		ActorData clientHolderActor = ClientHolderActor;
		ClientHolderActor = null;
		ClientIdleSquare = newIdleSquare;
		if (CaptureTheFlag.Get() != null)
		{
			CaptureTheFlag.Get().Client_OnFlagHolderChanged(clientHolderActor, ClientHolderActor, false, m_alreadyTurnedIn);
		}
	}

	public void OnTurnedIn_Client(ActorData capturingActor, int eventGuid)
	{
		if (!NetworkClient.active)
		{
			Log.Error("Calling CTF_Flag.OnTurnedIn_Client on a non-client.");
			return;
		}
		if (eventGuid != -1)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (eventGuid <= LastClientUpdateFlagHolderEventGuid)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		LastClientUpdateFlagHolderEventGuid = eventGuid;
		ActorData clientHolderActor = ClientHolderActor;
		ClientHolderActor = null;
		ClientIdleSquare = null;
		if (CaptureTheFlag.Get() != null)
		{
			CaptureTheFlag.Get().Client_OnFlagHolderChanged(clientHolderActor, ClientHolderActor, true, m_alreadyTurnedIn);
		}
		m_alreadyTurnedIn = true;
		GameEventManager.MatchObjectiveEventArgs matchObjectiveEventArgs = new GameEventManager.MatchObjectiveEventArgs();
		matchObjectiveEventArgs.objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.FlagTurnedIn_Client;
		matchObjectiveEventArgs.controlPoint = null;
		matchObjectiveEventArgs.activatingActor = capturingActor;
		matchObjectiveEventArgs.team = capturingActor.GetTeam();
		GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, matchObjectiveEventArgs);
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		int num;
		if (initialState)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = -1;
		}
		else
		{
			num = (int)base.syncVarDirtyBits;
		}
		uint num2 = (uint)num;
		byte flagGuid = m_flagGuid;
		byte value = (byte)m_team;
		sbyte value2;
		if (ServerHolderActor == null)
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
			value2 = (sbyte)ActorData.s_invalidActorIndex;
		}
		else
		{
			value2 = (sbyte)ServerHolderActor.ActorIndex;
		}
		sbyte value3;
		sbyte value4;
		if (ServerIdleSquare == null)
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
			value3 = -1;
			value4 = -1;
		}
		else
		{
			value3 = (sbyte)ServerIdleSquare.x;
			value4 = (sbyte)ServerIdleSquare.y;
		}
		int damageOnHolderSincePickedUp_Gross = DamageOnHolderSincePickedUp_Gross;
		int damageOnHolderSinceTurnStart_Gross = DamageOnHolderSinceTurnStart_Gross;
		writer.Write(flagGuid);
		writer.Write(value);
		writer.Write(value2);
		writer.Write(value3);
		writer.Write(value4);
		writer.Write(damageOnHolderSincePickedUp_Gross);
		writer.Write(damageOnHolderSinceTurnStart_Gross);
		return num2 != 0;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		byte flagGuid = reader.ReadByte();
		byte team = reader.ReadByte();
		sbyte b = reader.ReadSByte();
		sbyte b2 = reader.ReadSByte();
		sbyte b3 = reader.ReadSByte();
		int damageOnHolderSincePickedUp_Gross = reader.ReadInt32();
		int damageOnHolderSinceTurnStart_Gross = reader.ReadInt32();
		m_flagGuid = flagGuid;
		m_team = (Team)team;
		ActorData clientHolderActor = ClientHolderActor;
		if (b == (sbyte)ActorData.s_invalidActorIndex)
		{
			ClientHolderActor = null;
		}
		else
		{
			ClientHolderActor = GameFlowData.Get().FindActorByActorIndex(b);
		}
		if (b2 == -1 && b3 == -1)
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
			ClientIdleSquare = null;
		}
		else
		{
			ClientIdleSquare = Board.Get().GetBoardSquare(b2, b3);
		}
		if (clientHolderActor != ClientHolderActor)
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
			if (CaptureTheFlag.Get() != null)
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
				CaptureTheFlag.Get().Client_OnFlagHolderChanged(clientHolderActor, ClientHolderActor, false, m_alreadyTurnedIn);
			}
		}
		DamageOnHolderSincePickedUp_Gross = damageOnHolderSincePickedUp_Gross;
		DamageOnHolderSinceTurnStart_Gross = damageOnHolderSinceTurnStart_Gross;
	}

	private void Update()
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (HUD_UI.Get() != null && !m_initializedOffscreenIndicator)
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
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddCtfFlag(this);
			m_initializedOffscreenIndicator = true;
		}
		UpdatePosition();
		MeshRenderer[] components = GetComponents<MeshRenderer>();
		MeshRenderer[] array = components;
		foreach (MeshRenderer meshRenderer in array)
		{
			if (ClientHolderActor == null)
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
				meshRenderer.enabled = true;
			}
			else
			{
				meshRenderer.enabled = false;
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			MeshRenderer[] componentsInChildren = GetComponentsInChildren<MeshRenderer>();
			MeshRenderer[] array2 = componentsInChildren;
			foreach (MeshRenderer meshRenderer2 in array2)
			{
				if (ClientHolderActor == null)
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
					meshRenderer2.enabled = true;
				}
				else
				{
					meshRenderer2.enabled = false;
				}
			}
			if (m_notifiedOfSpawn || !(InterfaceManager.Get() != null))
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
				if (CaptureTheFlag.Get() != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						InterfaceManager.Get().DisplayAlert(StringUtil.TR("BriefcaseLocated", "CTF"), CaptureTheFlag.Get().m_textColor_neutral);
						m_notifiedOfSpawn = true;
						return;
					}
				}
				return;
			}
		}
	}

	public void UpdatePosition()
	{
		Vector3 position = GetPosition();
		base.transform.position = position;
		Quaternion rotation = GetRotation();
		base.transform.rotation = rotation;
	}

	private void UNetVersion()
	{
	}
}
