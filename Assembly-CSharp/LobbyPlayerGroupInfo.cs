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

	public LobbyCharacterInfo ChararacterInfo
	{
		get { return m_ChararacterInfo; }
	}

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
		bool switchedChars = m_ChararacterInfo == null
		                     || m_ChararacterInfo.CharacterType != newInfo.CharacterType;
		m_ChararacterInfo = newInfo;
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			ClientSelectedVisualInfo = m_ChararacterInfo.CharacterSkin
		});
		if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()
		    && UICharacterSelectScreenController.Get() != null)
		{
			UICharacterSelectScreenController.Get().QuickPlayUpdateCharacters(
				GameManager.Get().GameplayOverrides, switchedChars, isFromServerResponse);
		}
	}
}
