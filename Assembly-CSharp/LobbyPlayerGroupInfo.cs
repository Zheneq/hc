using System;
using System.Collections.Generic;

[Serializable]
public class LobbyPlayerGroupInfo
{
	public bool InAGroup;

	public bool IsLeader;

	public GameType SelectedQueueType;

	public ushort SubTypeMask;

	private LobbyCharacterInfo m_ChararacterInfo;

	public string MemberDisplayName;

	public List<UpdateGroupMemberData> Members;

	public LobbyPlayerGroupInfo()
	{
		this.SelectedQueueType = GameType.PvP;
	}

	public LobbyPlayerGroupInfo Clone()
	{
		return (LobbyPlayerGroupInfo)base.MemberwiseClone();
	}

	public LobbyCharacterInfo ChararacterInfo
	{
		get
		{
			return this.m_ChararacterInfo;
		}
	}

	public void SetCharacterInfo(LobbyCharacterInfo newInfo, bool isFromServerResponse = false)
	{
		bool flag;
		if (this.m_ChararacterInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(LobbyPlayerGroupInfo.SetCharacterInfo(LobbyCharacterInfo, bool)).MethodHandle;
			}
			flag = (this.m_ChararacterInfo.CharacterType != newInfo.CharacterType);
		}
		else
		{
			flag = true;
		}
		bool switchedChars = flag;
		this.m_ChararacterInfo = newInfo;
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			ClientSelectedVisualInfo = new CharacterVisualInfo?(this.m_ChararacterInfo.CharacterSkin)
		});
		if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
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
			if (UICharacterSelectScreenController.Get() != null)
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
				UICharacterSelectScreenController.Get().QuickPlayUpdateCharacters(GameManager.Get().GameplayOverrides, switchedChars, isFromServerResponse);
			}
		}
	}
}
