using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class MatchComponent : ICloneable
{
	public List<MatchComponent.Actor> Actors = new List<MatchComponent.Actor>();

	public DateTime MatchTime { get; set; }

	public PlayerGameResult Result { get; set; }

	public int Kills { get; set; }

	public CharacterType CharacterUsed { get; set; }

	public GameType GameType { get; set; }

	public string MapName { get; set; }

	public int NumOfTurns { get; set; }

	public string SubTypeLocTag { get; set; }

	public CharacterType GetFirstPlayerCharacter()
	{
		if (this.Actors == null)
		{
			return this.CharacterUsed;
		}
		for (int i = 0; i < this.Actors.Count; i++)
		{
			if (this.Actors[i].IsPlayer)
			{
				return this.Actors[i].Character;
			}
		}
		return this.CharacterUsed;
	}

	public string GetTimeDifferenceText()
	{
		TimeSpan difference = DateTime.UtcNow - this.MatchTime;
		return StringUtil.GetTimeDifferenceText(difference, false);
	}

	public string GetSubTypeNameTerm()
	{
		string text;
		if (this.SubTypeLocTag != null)
		{
			text = this.SubTypeLocTag.Split("@".ToCharArray()).First<string>();
		}
		else
		{
			text = "unknown";
		}
		string text2 = text;
		if (text2 == "unknown")
		{
			GameType gameType = this.GameType;
			switch (gameType)
			{
			case GameType.Custom:
				text2 = "GenericCustom";
				break;
			case GameType.Practice:
				text2 = "GenericPractice";
				break;
			case GameType.Tutorial:
				text2 = "GenericTutorial";
				break;
			default:
				switch (gameType)
				{
				case GameType.Ranked:
					text2 = "GenericRanked";
					break;
				case GameType.NewPlayerSolo:
					text2 = "GenericNewPlayerSolo";
					break;
				}
				break;
			}
		}
		if (text2 == "unknown")
		{
			if (this.MapName.EndsWith("CTF"))
			{
				text2 = "GenericBriefcase";
			}
		}
		return text2;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}

	[Serializable]
	public struct Actor
	{
		public CharacterType Character;

		public Team Team;

		public bool IsPlayer;
	}
}
