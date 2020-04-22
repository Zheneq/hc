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

	public LobbyCharacterInfo ChararacterInfo => m_ChararacterInfo;

	public LobbyPlayerGroupInfo()
	{
		SelectedQueueType = GameType.PvP;
	}

	public LobbyPlayerGroupInfo Clone()
	{
		return (LobbyPlayerGroupInfo)MemberwiseClone();
	}

	public void SetCharacterInfo(LobbyCharacterInfo newInfo, bool isFromServerResponse = false)
	{
		int num;
		if (m_ChararacterInfo != null)
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
			num = ((m_ChararacterInfo.CharacterType != newInfo.CharacterType) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool switchedChars = (byte)num != 0;
		m_ChararacterInfo = newInfo;
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			ClientSelectedVisualInfo = m_ChararacterInfo.CharacterSkin
		});
		if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
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
			if (UICharacterSelectScreenController.Get() != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					UICharacterSelectScreenController.Get().QuickPlayUpdateCharacters(GameManager.Get().GameplayOverrides, switchedChars, isFromServerResponse);
					return;
				}
			}
			return;
		}
	}
}
