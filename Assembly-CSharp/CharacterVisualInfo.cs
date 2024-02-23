using System;
using System.Text;

[Serializable]
public struct CharacterVisualInfo
{
	public int skinIndex;

	public int patternIndex;

	public int colorIndex;

	public CharacterVisualInfo(int skin, int pattern, int color)
	{
		skinIndex = skin;
		patternIndex = pattern;
		colorIndex = color;
	}

	public override string ToString()
	{
		return new StringBuilder().Append("skin: ").Append(skinIndex).Append(", pattern: ").Append(patternIndex).Append(", color: ").Append(colorIndex).ToString();
	}

	public override bool Equals(object obj)
	{
		if (obj is CharacterVisualInfo)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					CharacterVisualInfo characterVisualInfo = (CharacterVisualInfo)obj;
					int result;
					if (skinIndex == characterVisualInfo.skinIndex && patternIndex == characterVisualInfo.patternIndex)
					{
						result = ((colorIndex == characterVisualInfo.colorIndex) ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				}
				}
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public void ResetToDefault()
	{
		skinIndex = 0;
		patternIndex = 0;
		colorIndex = 0;
	}

	public bool IsDefaultSelection()
	{
		int result;
		if (skinIndex == 0)
		{
			if (patternIndex == 0)
			{
				result = ((colorIndex == 0) ? 1 : 0);
				goto IL_002f;
			}
		}
		result = 0;
		goto IL_002f;
		IL_002f:
		return (byte)result != 0;
	}
}
