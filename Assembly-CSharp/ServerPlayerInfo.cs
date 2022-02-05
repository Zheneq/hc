// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;

// server-only
#if SERVER
public class ServerPlayerInfo
{
	public int PlayerId;
	public string Handle;
	public bool IsGameOwner;
	public BotDifficulty Difficulty;
	public Team TeamId;
	public CharacterType CharacterType;
	public CharacterVisualInfo CharacterSkin = new CharacterVisualInfo();
	public CharacterCardInfo CharacterCards = new CharacterCardInfo();

	// rogues
	//public CharacterGearInfo CharacterGear = new CharacterGearInfo();

	public CharacterAbilityVfxSwapInfo CharacterAbilityVfxSwaps = new CharacterAbilityVfxSwapInfo();
	public List<PlayerTauntData> CharacterTaunts = new List<PlayerTauntData>();
	public LoadedCharacterSelection SelectedCharacter;
	public LobbyServerPlayerInfo LobbyPlayerInfo;
	public List<ServerPlayerInfo> ProxyPlayerInfos = new List<ServerPlayerInfo>();

	public ServerPlayerInfo(LobbyServerPlayerInfo lobbyPlayerInfo, List<LobbyServerPlayerInfo> proxyPlayerInfos)
	{
		PlayerId = lobbyPlayerInfo.PlayerId;
		Handle = lobbyPlayerInfo.Handle;
		IsGameOwner = lobbyPlayerInfo.IsGameOwner;
		Difficulty = lobbyPlayerInfo.Difficulty;
		TeamId = lobbyPlayerInfo.TeamId;
		CharacterType = lobbyPlayerInfo.CharacterInfo.CharacterType;
		CharacterSkin = lobbyPlayerInfo.CharacterInfo.CharacterSkin;
		CharacterCards = lobbyPlayerInfo.CharacterInfo.CharacterCards;
		//CharacterGear = lobbyPlayerInfo.CharacterInfo.CharacterGear;  // rogues
		CharacterTaunts = lobbyPlayerInfo.CharacterInfo.CharacterTaunts;
		CharacterAbilityVfxSwaps = lobbyPlayerInfo.CharacterInfo.CharacterAbilityVfxSwaps;
		LobbyPlayerInfo = lobbyPlayerInfo;
		foreach (LobbyServerPlayerInfo lobbyPlayerInfo2 in proxyPlayerInfos)
		{
			ProxyPlayerInfos.Add(new ServerPlayerInfo(lobbyPlayerInfo2, new List<LobbyServerPlayerInfo>()));
		}
	}

	public bool IsNPCBot
	{
		get
		{
			return LobbyPlayerInfo.IsNPCBot;
		}
	}

	public bool IsLoadTestBot
	{
		get
		{
			return LobbyPlayerInfo.IsLoadTestBot;
		}
	}

	public bool IsAIControlled
	{
		get
		{
			return LobbyPlayerInfo.IsAIControlled;
		}
	}

	public IEnumerable<LoadedCharacterSelection> SelectedCharacters
	{
		get
		{
			return new LoadedCharacterSelection[]
			{
				SelectedCharacter
			}.Concat(from p in ProxyPlayerInfos
			select p.SelectedCharacter);
		}
	}

	public bool ReplacedWithBots
	{
		get
		{
			return LobbyPlayerInfo.ReplacedWithBots;
		}
		set
		{
			LobbyPlayerInfo.ReplacedWithBots = value;
		}
	}

	public override string ToString()
	{
		return string.Format("({0}) {1} {2} {3} (AIControlled: {4})", new object[]
		{
			PlayerId,
			Handle,
			TeamId,
			CharacterType,
			IsAIControlled
		});
	}
}
#endif
