using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InterfaceManager : NetworkBehaviour
{
	public class DisplayAlertMessage : MessageBase
	{
		public string alertText;

		public float r;

		public float g;

		public float b;

		public override void Serialize(NetworkWriter writer)
		{
			writer.Write(alertText);
			writer.Write(r);
			writer.Write(g);
			writer.Write(b);
		}

		public override void Deserialize(NetworkReader reader)
		{
			alertText = reader.ReadString();
			r = reader.ReadSingle();
			g = reader.ReadSingle();
			b = reader.ReadSingle();
		}
	}

	public Texture2D m_backgroundTexture;

	public float m_combatTextLifetime = 2f;

	public float m_combatTextScrollSpeed = 0.5f;

	public int m_combatTextSize = 16;

	private const float LOW_TIME_MIN = 5.4f;

	public static Color s_techPointsTextColor = new Color(0.5f, 0.4f, 1f);

	public static Color s_coolDownTextColor = new Color(1f, 0.5f, 0.1f);

	private static InterfaceManager s_instance;

	public static InterfaceManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	public override void OnStartClient()
	{
		ClientGameManager.Get().Client.RegisterHandler(49, MsgDisplayAlert);
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			NetworkClient client = ClientGameManager.Get().Client;
			if (client != null)
			{
				client.UnregisterHandler(49);
			}
		}
		s_instance = null;
	}

	private void Update()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		ActorData actorData = null;
		bool flag = false;
		if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.SwitchFreelancer))
		{
			goto IL_005a;
		}
		if (Application.isEditor)
		{
			if (Input.GetKeyDown(KeyCode.F11))
			{
				goto IL_005a;
			}
		}
		goto IL_0069;
		IL_0069:
		if (activeOwnedActorData != null)
		{
			List<ActorData> ownedActorDatas = GameFlowData.Get().m_ownedActorDatas;
			Team team = activeOwnedActorData.GetTeam();
			for (int i = 0; i < ownedActorDatas.Count; i++)
			{
				if (ownedActorDatas[i].GetTeam() != team)
				{
					continue;
				}
				KeyPreference keyPreference = KeyPreference.Freelancer1;
				keyPreference += i;
				if (!InputManager.Get().IsKeyBindingNewlyHeld(keyPreference))
				{
					continue;
				}
				if (actorData != ownedActorDatas[i])
				{
					actorData = ownedActorDatas[i];
					flag = true;
				}
				break;
			}
		}
		if (!(actorData != null))
		{
			return;
		}
		while (true)
		{
			CameraManager.Get().SetTargetObject(actorData.gameObject, CameraManager.CameraTargetReason.UserFocusingOnActor);
			if (GameFlowData.Get().IsActorDataOwned(actorData) && actorData != GameFlowData.Get().activeOwnedActorData)
			{
				GameFlowData.Get().activeOwnedActorData = actorData;
			}
			if (flag)
			{
				while (true)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.HudLockIn);
					return;
				}
			}
			return;
		}
		IL_005a:
		actorData = GameFlowData.Get().nextOwnedActorData;
		flag = true;
		goto IL_0069;
	}

	public void OnTurnTick()
	{
		UISounds.GetUISounds().Play("ui_notification_turn_start");
		UILoadingScreenPanel.Get().SetVisible(false);
		if (!UIFrontendLoadingScreen.Get().gameObject.activeSelf)
		{
			return;
		}
		while (true)
		{
			UIFrontendLoadingScreen.Get().StartDisplayFadeOut();
			return;
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
		DisplayAlertMessage displayAlertMessage = new DisplayAlertMessage();
		displayAlertMessage.alertText = alertText;
		displayAlertMessage.r = alertColor.r;
		displayAlertMessage.g = alertColor.g;
		displayAlertMessage.b = alertColor.b;
		NetworkServer.SendToClient(destinationConnection.connectionId, 49, displayAlertMessage);
	}

	[Client]
	protected void MsgDisplayAlert(NetworkMessage netMsg)
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
					Debug.LogWarning("[Client] function 'System.Void InterfaceManager::MsgDisplayAlert(UnityEngine.Networking.NetworkMessage)' called on server");
					return;
				}
			}
		}
		DisplayAlertMessage displayAlertMessage = netMsg.ReadMessage<DisplayAlertMessage>();
		Log.Info($"[JSON] {{\"msgDisplayAlert\":{DefaultJsonSerializer.Serialize(displayAlertMessage)}}}");
		DisplayAlert(displayAlertMessage.alertText, new Color(displayAlertMessage.r, displayAlertMessage.g, displayAlertMessage.b));
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
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
