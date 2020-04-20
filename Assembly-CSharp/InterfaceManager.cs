using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InterfaceManager : NetworkBehaviour
{
	public Texture2D m_backgroundTexture;

	public float m_combatTextLifetime = 2f;

	public float m_combatTextScrollSpeed = 0.5f;

	public int m_combatTextSize = 0x10;

	private const float LOW_TIME_MIN = 5.4f;

	public static Color s_techPointsTextColor = new Color(0.5f, 0.4f, 1f);

	public static Color s_coolDownTextColor = new Color(1f, 0.5f, 0.1f);

	private static InterfaceManager s_instance;

	public static InterfaceManager Get()
	{
		return InterfaceManager.s_instance;
	}

	private void Awake()
	{
		InterfaceManager.s_instance = this;
	}

	public override void OnStartClient()
	{
		ClientGameManager.Get().Client.RegisterHandler(0x31, new NetworkMessageDelegate(this.MsgDisplayAlert));
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InterfaceManager.OnDestroy()).MethodHandle;
			}
			NetworkClient client = ClientGameManager.Get().Client;
			if (client != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				client.UnregisterHandler(0x31);
			}
		}
		InterfaceManager.s_instance = null;
	}

	private void Update()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		ActorData actorData = null;
		bool flag = false;
		if (!InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.SwitchFreelancer))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InterfaceManager.Update()).MethodHandle;
			}
			if (!Application.isEditor)
			{
				goto IL_69;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!Input.GetKeyDown(KeyCode.F11))
			{
				goto IL_69;
			}
		}
		actorData = GameFlowData.Get().nextOwnedActorData;
		flag = true;
		IL_69:
		if (activeOwnedActorData != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			List<ActorData> ownedActorDatas = GameFlowData.Get().m_ownedActorDatas;
			Team team = activeOwnedActorData.GetTeam();
			for (int i = 0; i < ownedActorDatas.Count; i++)
			{
				if (ownedActorDatas[i].GetTeam() == team)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					KeyPreference keyPreference = KeyPreference.Freelancer1;
					keyPreference += i;
					if (InputManager.Get().IsKeyBindingNewlyHeld(keyPreference))
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (actorData != ownedActorDatas[i])
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
							actorData = ownedActorDatas[i];
							flag = true;
						}
						break;
					}
				}
			}
		}
		if (actorData != null)
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
			CameraManager.Get().SetTargetObject(actorData.gameObject, CameraManager.CameraTargetReason.UserFocusingOnActor);
			if (GameFlowData.Get().IsActorDataOwned(actorData) && actorData != GameFlowData.Get().activeOwnedActorData)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				GameFlowData.Get().activeOwnedActorData = actorData;
			}
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
				UIFrontEnd.PlaySound(FrontEndButtonSounds.HudLockIn);
			}
		}
	}

	public void OnTurnTick()
	{
		UISounds.GetUISounds().Play("ui_notification_turn_start");
		UILoadingScreenPanel.Get().SetVisible(false);
		if (UIFrontendLoadingScreen.Get().gameObject.activeSelf)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InterfaceManager.OnTurnTick()).MethodHandle;
			}
			UIFrontendLoadingScreen.Get().StartDisplayFadeOut();
		}
	}

	public bool ShouldHandleMouseClick()
	{
		bool flag = !UIUtils.IsMouseOnGUI();
		bool flag2 = GUIUtility.hotControl == 0;
		return flag && flag2;
	}

	[Server]
	public void SendDisplayAlert(string alertText, Color alertColor, NetworkConnection destinationConnection)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void InterfaceManager::SendDisplayAlert(System.String,UnityEngine.Color,UnityEngine.Networking.NetworkConnection)' called on client");
			return;
		}
		InterfaceManager.DisplayAlertMessage displayAlertMessage = new InterfaceManager.DisplayAlertMessage();
		displayAlertMessage.alertText = alertText;
		displayAlertMessage.r = alertColor.r;
		displayAlertMessage.g = alertColor.g;
		displayAlertMessage.b = alertColor.b;
		NetworkServer.SendToClient(destinationConnection.connectionId, 0x31, displayAlertMessage);
	}

	[Client]
	protected void MsgDisplayAlert(NetworkMessage netMsg)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InterfaceManager.MsgDisplayAlert(NetworkMessage)).MethodHandle;
			}
			Debug.LogWarning("[Client] function 'System.Void InterfaceManager::MsgDisplayAlert(UnityEngine.Networking.NetworkMessage)' called on server");
			return;
		}
		InterfaceManager.DisplayAlertMessage displayAlertMessage = netMsg.ReadMessage<InterfaceManager.DisplayAlertMessage>();
		this.DisplayAlert(displayAlertMessage.alertText, new Color(displayAlertMessage.r, displayAlertMessage.g, displayAlertMessage.b), 2f, false, 0);
	}

	internal void DisplayAlert(string alertText, Color color, float messageTimeSeconds = 2f, bool showBackground = false, int displayIndex = 0)
	{
		HUD_UI.Get().m_mainScreenPanel.m_alertDisplay.DisplayAlert(alertText, color, messageTimeSeconds, showBackground, displayIndex);
	}

	internal void CancelAlert(string alertText)
	{
		HUD_UI.Get().m_mainScreenPanel.m_alertDisplay.CancelAlert(alertText);
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result;
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}

	public class DisplayAlertMessage : MessageBase
	{
		public string alertText;

		public float r;

		public float g;

		public float b;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(this.alertText);
			writer.Write(this.r);
			writer.Write(this.g);
			writer.Write(this.b);
		}

		public override void Deserialize(NetworkReader reader)
		{
			this.alertText = reader.ReadString();
			this.r = reader.ReadSingle();
			this.g = reader.ReadSingle();
			this.b = reader.ReadSingle();
		}
	}
}
