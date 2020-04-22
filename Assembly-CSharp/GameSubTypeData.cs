using System;
using UnityEngine;

public class GameSubTypeData : MonoBehaviour
{
	[Serializable]
	public class GameSubTypeInstructionDisplayInfo
	{
		public string HeaderString;

		public string[] IconResourceStrings;

		[TextArea(3, 10)]
		public string TooltipString;

		[HideInInspector]
		public int InstructionSetIndex;

		[HideInInspector]
		public int DisplayIndex;

		public string HeaderStringLocalized => StringUtil.TR_GetLoadingHeader(InstructionSetIndex, DisplayIndex);

		public string TooltipStringLocalized => StringUtil.TR_GetLoadingTooltip(InstructionSetIndex, DisplayIndex);
	}

	[Serializable]
	public class GameSubTypeInstructions
	{
		public GameSubType.GameLoadScreenInstructions InstructionType;

		public GameSubTypeInstructionDisplayInfo[] DisplayInfos;
	}

	public GameSubTypeInstructions[] Instructions;

	private static GameSubTypeData s_instance;

	public GameSubTypeInstructionDisplayInfo GetDisplayPanelInfo(int setIndex, int displayIndex)
	{
		GameSubTypeInstructions instructionSet = GetInstructionSet(setIndex);
		if (instructionSet != null && -1 < displayIndex)
		{
			if (displayIndex < instructionSet.DisplayInfos.Length)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return instructionSet.DisplayInfos[displayIndex];
					}
				}
			}
		}
		return null;
	}

	public GameSubTypeInstructions GetInstructionSet(int setIndex)
	{
		if (-1 < setIndex && setIndex < Instructions.Length)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return Instructions[setIndex];
				}
			}
		}
		return null;
	}

	public GameSubTypeInstructions GetInstructionSet(GameSubType.GameLoadScreenInstructions instructionType)
	{
		for (int i = 0; i < Instructions.Length; i++)
		{
			if (Instructions[i].InstructionType != instructionType)
			{
				continue;
			}
			while (true)
			{
				return Instructions[i];
			}
		}
		while (true)
		{
			return null;
		}
	}

	private void Awake()
	{
		s_instance = this;
		int num = 0;
		while (num < Instructions.Length)
		{
			GameSubTypeInstructions gameSubTypeInstructions = Instructions[num];
			for (int i = 0; i < gameSubTypeInstructions.DisplayInfos.Length; i++)
			{
				gameSubTypeInstructions.DisplayInfos[i].InstructionSetIndex = num;
				gameSubTypeInstructions.DisplayInfos[i].DisplayIndex = i;
			}
			while (true)
			{
				num++;
				goto IL_0059;
			}
			IL_0059:;
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public static GameSubTypeData Get()
	{
		return s_instance;
	}
}
