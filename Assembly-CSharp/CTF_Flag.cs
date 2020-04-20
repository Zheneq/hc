using System;
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

	public void Initialize(BoardSquare square, Team team, byte flagGuid)
	{
		this.m_originalSquare = square;
		this.m_team = team;
		this.m_flagGuid = flagGuid;
		this.m_serverHolderActor = null;
		this.m_serverIdleSquare = square;
		this.m_clientHolderActor = null;
		this.m_clientIdleSquare = square;
		this.UpdatePosition();
	}

	public ActorData ServerHolderActor
	{
		get
		{
			return this.m_serverHolderActor;
		}
		set
		{
			if (value != this.m_serverHolderActor)
			{
				this.m_serverHolderActor = value;
				this.DamageOnHolderSincePickedUp_Gross = 0;
				this.DamageOnHolderSinceTurnStart_Gross = 0;
			}
		}
	}

	public BoardSquare ServerIdleSquare
	{
		get
		{
			return this.m_serverIdleSquare;
		}
		set
		{
			if (value != this.m_serverIdleSquare)
			{
				this.m_serverIdleSquare = value;
			}
		}
	}

	public ActorData ClientHolderActor
	{
		get
		{
			return this.m_clientHolderActor;
		}
		set
		{
			if (value != this.m_clientHolderActor)
			{
				this.m_clientHolderActor = value;
				this.ClientUnresolvedDamageOnHolder = 0;
				if (CaptureTheFlag.Get() != null && CaptureTheFlag.Get().m_flagBeingHeldSequence != null)
				{
					if (this.m_flagBeingHeldSequenceInstance != null)
					{
						if (!this.m_flagBeingHeldSequenceInstance.MarkedForRemoval)
						{
							this.m_flagBeingHeldSequenceInstance.MarkForRemoval();
						}
						this.m_flagBeingHeldSequenceInstance = null;
					}
					if (this.m_clientHolderActor != null)
					{
						GameObject flagBeingHeldSequence = CaptureTheFlag.Get().m_flagBeingHeldSequence;
						BoardSquare currentBoardSquare = this.m_clientHolderActor.CurrentBoardSquare;
						SequenceSource sequenceSource = CaptureTheFlag.Get().SequenceSource;
						Sequence[] array = SequenceManager.Get().CreateClientSequences(flagBeingHeldSequence, currentBoardSquare, this.m_clientHolderActor.AsArray(), this.m_clientHolderActor, sequenceSource, null);
						if (array != null && array.Length != 0)
						{
							if (array.Length <= 1)
							{
								this.m_flagBeingHeldSequenceInstance = array[0];
								return;
							}
						}
						Debug.LogError("CTF_Flag creating flag-being-held sequence, but had bad output.");
					}
				}
			}
		}
	}

	public BoardSquare ClientIdleSquare
	{
		get
		{
			return this.m_clientIdleSquare;
		}
		set
		{
			if (value != this.m_clientIdleSquare)
			{
				this.m_clientIdleSquare = value;
			}
		}
	}

	public int LastClientUpdateFlagHolderEventGuid
	{
		get
		{
			return this.m_lastClientUpdateFlagHolderEventGuid;
		}
		set
		{
			if (this.m_lastClientUpdateFlagHolderEventGuid != value)
			{
				this.m_lastClientUpdateFlagHolderEventGuid = value;
			}
		}
	}

	public Sprite GetIcon()
	{
		if (CaptureTheFlag.Get() != null)
		{
			return CaptureTheFlag.Get().m_flagIcon;
		}
		return null;
	}

	public bool ShouldShowIndicator()
	{
		return this.m_clientIdleSquare != null;
	}

	public ActorData GatheredHolderActor
	{
		get
		{
			return this.m_gatheredHolderActor;
		}
		set
		{
			if (value != this.m_gatheredHolderActor)
			{
				this.m_gatheredHolderActor = value;
				this.GatheredMovementDamageSincePickedUp = 0;
				this.GatheredMovementDamageSinceTurnStart = 0;
			}
		}
	}

	public BoardSquare GatheredIdleSquare
	{
		get
		{
			return this.m_gatheredIdleSquare;
		}
		set
		{
			if (value != this.m_gatheredIdleSquare)
			{
				this.m_gatheredIdleSquare = value;
			}
		}
	}

	public BoardSquarePathInfo GatheredPath
	{
		get
		{
			return this.m_gatheredPath;
		}
		set
		{
			if (value != this.m_gatheredPath)
			{
				this.m_gatheredPath = value;
			}
		}
	}

	public int GatheredMovementDamageSincePickedUp
	{
		get
		{
			return this.m_gatheredMovementDamageSincePickedUp;
		}
		set
		{
			if (value != this.m_gatheredMovementDamageSincePickedUp)
			{
				this.m_gatheredMovementDamageSincePickedUp = value;
			}
		}
	}

	public int GatheredMovementDamageSinceTurnStart
	{
		get
		{
			return this.m_gatheredMovementDamageSinceTurnStart;
		}
		set
		{
			if (this.m_gatheredMovementDamageSinceTurnStart != value)
			{
				this.m_gatheredMovementDamageSinceTurnStart = value;
			}
		}
	}

	public int DamageOnHolderSincePickedUp_Gross
	{
		get
		{
			return this.m_damageOnHolderSincePickedUp_Gross;
		}
		set
		{
			if (value != this.m_damageOnHolderSincePickedUp_Gross)
			{
				this.m_damageOnHolderSincePickedUp_Gross = value;
				this.ClientUnresolvedDamageOnHolder = 0;
			}
		}
	}

	public int DamageOnHolderSinceTurnStart_Gross
	{
		get
		{
			return this.m_damageOnHolderSinceTurnStart_Gross;
		}
		set
		{
			if (value != this.m_damageOnHolderSinceTurnStart_Gross)
			{
				this.m_damageOnHolderSinceTurnStart_Gross = value;
				this.ClientUnresolvedDamageOnHolder = 0;
			}
		}
	}

	public int ClientUnresolvedDamageOnHolder
	{
		get
		{
			return this.m_clientUnresolvedDamageOnHolder;
		}
		set
		{
			if (value != this.m_clientUnresolvedDamageOnHolder)
			{
				this.m_clientUnresolvedDamageOnHolder = value;
			}
		}
	}

	public int NumFullTurnsSpentHeldInTurninRegion
	{
		get
		{
			return this.m_numFullTurnsSpentHeldInTurninRegion;
		}
		private set
		{
			if (this.m_numFullTurnsSpentHeldInTurninRegion != value)
			{
				this.m_numFullTurnsSpentHeldInTurninRegion = value;
			}
		}
	}

	public bool SpentEntireTurnReadyToBeTurnedIn
	{
		get
		{
			return this.m_spentEntireTurnReadyToBeTurnedIn;
		}
		private set
		{
			if (this.m_spentEntireTurnReadyToBeTurnedIn != value)
			{
				this.m_spentEntireTurnReadyToBeTurnedIn = value;
			}
		}
	}

	public void OnNotHeldInTurninRegion()
	{
		this.NumFullTurnsSpentHeldInTurninRegion = 0;
		this.SpentEntireTurnReadyToBeTurnedIn = false;
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
			CaptureTheFlag.Get().OnFlagDestroyed(this);
		}
		if (HUD_UI.Get() != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveCtfFlag(this);
		}
	}

	public Vector3 GetPosition()
	{
		Vector3 result;
		if (this.ClientHolderActor == null)
		{
			if (this.ClientIdleSquare == null)
			{
				result = base.transform.position;
			}
			else
			{
				result = this.ClientIdleSquare.ToVector3();
			}
		}
		else if (this.ClientHolderActor.IsVisibleToClient())
		{
			result = this.ClientHolderActor.transform.position;
		}
		else
		{
			result = base.transform.position;
		}
		return result;
	}

	public Quaternion GetRotation()
	{
		Quaternion result;
		if (this.ClientHolderActor == null)
		{
			result = Quaternion.identity;
		}
		else
		{
			result = this.ClientHolderActor.transform.rotation;
		}
		return result;
	}

	public BoardSquare GetOriginalSquare()
	{
		return this.m_originalSquare;
	}

	public Team GetIntrinsicTeam()
	{
		return this.m_team;
	}

	public Team GetCapturingTeam_Client()
	{
		if (this.m_team == Team.TeamA)
		{
			return Team.TeamB;
		}
		if (this.m_team == Team.TeamB)
		{
			return Team.TeamA;
		}
		if (this.ClientHolderActor != null)
		{
			if (this.ClientHolderActor.GetTeam() == Team.TeamA)
			{
				return Team.TeamA;
			}
		}
		if (this.ClientHolderActor != null && this.ClientHolderActor.GetTeam() == Team.TeamB)
		{
			return Team.TeamB;
		}
		return Team.Objects;
	}

	public void OnPickedUp_Client(ActorData newHolder, int eventGuid)
	{
		if (!NetworkClient.active)
		{
			Log.Error("Calling CTF_Flag.OnPickedUp_Client on a non-client.", new object[0]);
			return;
		}
		if (eventGuid != -1)
		{
			if (eventGuid <= this.LastClientUpdateFlagHolderEventGuid)
			{
				return;
			}
		}
		this.LastClientUpdateFlagHolderEventGuid = eventGuid;
		ActorData clientHolderActor = this.ClientHolderActor;
		this.ClientHolderActor = newHolder;
		this.ClientIdleSquare = null;
		if (CaptureTheFlag.Get() != null)
		{
			CaptureTheFlag.Get().Client_OnFlagHolderChanged(clientHolderActor, this.ClientHolderActor, false, this.m_alreadyTurnedIn);
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
			Log.Error("Calling CTF_Flag.OnReturned_Client on a non-client.", new object[0]);
			return;
		}
		this.ClientHolderActor = null;
		this.ClientIdleSquare = this.m_originalSquare;
	}

	public void OnDropped_Client(BoardSquare newIdleSquare, int eventGuid)
	{
		if (!NetworkClient.active)
		{
			Log.Error("Calling CTF_Flag.OnDropped_Client on a non-client.", new object[0]);
			return;
		}
		if (eventGuid != -1)
		{
			if (eventGuid <= this.LastClientUpdateFlagHolderEventGuid)
			{
				return;
			}
		}
		this.LastClientUpdateFlagHolderEventGuid = eventGuid;
		ActorData clientHolderActor = this.ClientHolderActor;
		this.ClientHolderActor = null;
		this.ClientIdleSquare = newIdleSquare;
		if (CaptureTheFlag.Get() != null)
		{
			CaptureTheFlag.Get().Client_OnFlagHolderChanged(clientHolderActor, this.ClientHolderActor, false, this.m_alreadyTurnedIn);
		}
	}

	public void OnTurnedIn_Client(ActorData capturingActor, int eventGuid)
	{
		if (!NetworkClient.active)
		{
			Log.Error("Calling CTF_Flag.OnTurnedIn_Client on a non-client.", new object[0]);
			return;
		}
		if (eventGuid != -1)
		{
			if (eventGuid <= this.LastClientUpdateFlagHolderEventGuid)
			{
				return;
			}
		}
		this.LastClientUpdateFlagHolderEventGuid = eventGuid;
		ActorData clientHolderActor = this.ClientHolderActor;
		this.ClientHolderActor = null;
		this.ClientIdleSquare = null;
		if (CaptureTheFlag.Get() != null)
		{
			CaptureTheFlag.Get().Client_OnFlagHolderChanged(clientHolderActor, this.ClientHolderActor, true, this.m_alreadyTurnedIn);
		}
		this.m_alreadyTurnedIn = true;
		GameEventManager.MatchObjectiveEventArgs matchObjectiveEventArgs = new GameEventManager.MatchObjectiveEventArgs();
		matchObjectiveEventArgs.objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.FlagTurnedIn_Client;
		matchObjectiveEventArgs.controlPoint = null;
		matchObjectiveEventArgs.activatingActor = capturingActor;
		matchObjectiveEventArgs.team = capturingActor.GetTeam();
		GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, matchObjectiveEventArgs);
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		uint num;
		if (initialState)
		{
			num = uint.MaxValue;
		}
		else
		{
			num = base.syncVarDirtyBits;
		}
		uint num2 = num;
		byte flagGuid = this.m_flagGuid;
		byte value = (byte)this.m_team;
		sbyte value2;
		if (this.ServerHolderActor == null)
		{
			value2 = (sbyte)ActorData.s_invalidActorIndex;
		}
		else
		{
			value2 = (sbyte)this.ServerHolderActor.ActorIndex;
		}
		sbyte value3;
		sbyte value4;
		if (this.ServerIdleSquare == null)
		{
			value3 = -1;
			value4 = -1;
		}
		else
		{
			value3 = (sbyte)this.ServerIdleSquare.x;
			value4 = (sbyte)this.ServerIdleSquare.y;
		}
		int damageOnHolderSincePickedUp_Gross = this.DamageOnHolderSincePickedUp_Gross;
		int damageOnHolderSinceTurnStart_Gross = this.DamageOnHolderSinceTurnStart_Gross;
		writer.Write(flagGuid);
		writer.Write(value);
		writer.Write(value2);
		writer.Write(value3);
		writer.Write(value4);
		writer.Write(damageOnHolderSincePickedUp_Gross);
		writer.Write(damageOnHolderSinceTurnStart_Gross);
		return num2 != 0U;
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
		this.m_flagGuid = flagGuid;
		this.m_team = (Team)team;
		ActorData clientHolderActor = this.ClientHolderActor;
		if ((int)b == (int)((sbyte)ActorData.s_invalidActorIndex))
		{
			this.ClientHolderActor = null;
		}
		else
		{
			this.ClientHolderActor = GameFlowData.Get().FindActorByActorIndex((int)b);
		}
		if ((int)b2 == -1 && (int)b3 == -1)
		{
			this.ClientIdleSquare = null;
		}
		else
		{
			this.ClientIdleSquare = Board.Get().GetBoardSquare((int)b2, (int)b3);
		}
		if (clientHolderActor != this.ClientHolderActor)
		{
			if (CaptureTheFlag.Get() != null)
			{
				CaptureTheFlag.Get().Client_OnFlagHolderChanged(clientHolderActor, this.ClientHolderActor, false, this.m_alreadyTurnedIn);
			}
		}
		this.DamageOnHolderSincePickedUp_Gross = damageOnHolderSincePickedUp_Gross;
		this.DamageOnHolderSinceTurnStart_Gross = damageOnHolderSinceTurnStart_Gross;
	}

	private void Update()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (HUD_UI.Get() != null && !this.m_initializedOffscreenIndicator)
		{
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddCtfFlag(this);
			this.m_initializedOffscreenIndicator = true;
		}
		this.UpdatePosition();
		MeshRenderer[] components = base.GetComponents<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in components)
		{
			if (this.ClientHolderActor == null)
			{
				meshRenderer.enabled = true;
			}
			else
			{
				meshRenderer.enabled = false;
			}
		}
		MeshRenderer[] componentsInChildren = base.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer2 in componentsInChildren)
		{
			if (this.ClientHolderActor == null)
			{
				meshRenderer2.enabled = true;
			}
			else
			{
				meshRenderer2.enabled = false;
			}
		}
		if (!this.m_notifiedOfSpawn && InterfaceManager.Get() != null)
		{
			if (CaptureTheFlag.Get() != null)
			{
				InterfaceManager.Get().DisplayAlert(StringUtil.TR("BriefcaseLocated", "CTF"), CaptureTheFlag.Get().m_textColor_neutral, 2f, false, 0);
				this.m_notifiedOfSpawn = true;
			}
		}
	}

	public void UpdatePosition()
	{
		Vector3 position = this.GetPosition();
		base.transform.position = position;
		Quaternion rotation = this.GetRotation();
		base.transform.rotation = rotation;
	}

	private void UNetVersion()
	{
	}
}
