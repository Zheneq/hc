using System;
using UnityEngine;

public class GameSubTypeData : MonoBehaviour
{
	public GameSubTypeData.GameSubTypeInstructions[] Instructions;

	private static GameSubTypeData s_instance;

	public GameSubTypeData.GameSubTypeInstructionDisplayInfo GetDisplayPanelInfo(int setIndex, int displayIndex)
	{
		GameSubTypeData.GameSubTypeInstructions instructionSet = this.GetInstructionSet(setIndex);
		if (instructionSet != null && -1 < displayIndex)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubTypeData.GetDisplayPanelInfo(int, int)).MethodHandle;
			}
			if (displayIndex < instructionSet.DisplayInfos.Length)
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
				return instructionSet.DisplayInfos[displayIndex];
			}
		}
		return null;
	}

	public GameSubTypeData.GameSubTypeInstructions GetInstructionSet(int setIndex)
	{
		if (-1 < setIndex && setIndex < this.Instructions.Length)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubTypeData.GetInstructionSet(int)).MethodHandle;
			}
			return this.Instructions[setIndex];
		}
		return null;
	}

	public GameSubTypeData.GameSubTypeInstructions GetInstructionSet(GameSubType.GameLoadScreenInstructions instructionType)
	{
		for (int i = 0; i < this.Instructions.Length; i++)
		{
			if (this.Instructions[i].InstructionType == instructionType)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubTypeData.GetInstructionSet(GameSubType.GameLoadScreenInstructions)).MethodHandle;
				}
				return this.Instructions[i];
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		return null;
	}

	private void Awake()
	{
		GameSubTypeData.s_instance = this;
		for (int i = 0; i < this.Instructions.Length; i++)
		{
			GameSubTypeData.GameSubTypeInstructions gameSubTypeInstructions = this.Instructions[i];
			for (int j = 0; j < gameSubTypeInstructions.DisplayInfos.Length; j++)
			{
				gameSubTypeInstructions.DisplayInfos[j].InstructionSetIndex = i;
				gameSubTypeInstructions.DisplayInfos[j].DisplayIndex = j;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubTypeData.Awake()).MethodHandle;
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public static GameSubTypeData Get()
	{
		return GameSubTypeData.s_instance;
	}

	[Serializable]
	public class GameSubTypeInstructionDisplayInfo
	{
		public string HeaderString;

		public string[] IconResourceStrings;

		[TextArea(3, 0xA)]
		public string TooltipString;

		[HideInInspector]
		public int InstructionSetIndex;

		[HideInInspector]
		public int DisplayIndex;

		public string HeaderStringLocalized
		{
			get
			{
				return StringUtil.TR_GetLoadingHeader(this.InstructionSetIndex, this.DisplayIndex);
			}
		}

		public string TooltipStringLocalized
		{
			get
			{
				return StringUtil.TR_GetLoadingTooltip(this.InstructionSetIndex, this.DisplayIndex);
			}
		}
	}

	[Serializable]
	public class GameSubTypeInstructions
	{
		public GameSubType.GameLoadScreenInstructions InstructionType;

		public GameSubTypeData.GameSubTypeInstructionDisplayInfo[] DisplayInfos;
	}
}
