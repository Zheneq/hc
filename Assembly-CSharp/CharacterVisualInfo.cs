using System;

[Serializable]
public struct CharacterVisualInfo
{
	public int skinIndex;

	public int patternIndex;

	public int colorIndex;

	public CharacterVisualInfo(int skin, int pattern, int color)
	{
		this.skinIndex = skin;
		this.patternIndex = pattern;
		this.colorIndex = color;
	}

	public override string ToString()
	{
		return string.Format("skin: {0}, pattern: {1}, color: {2}", this.skinIndex, this.patternIndex, this.colorIndex);
	}

	public override bool Equals(object obj)
	{
		if (obj is CharacterVisualInfo)
		{
			CharacterVisualInfo characterVisualInfo = (CharacterVisualInfo)obj;
			bool result;
			if (this.skinIndex == characterVisualInfo.skinIndex && this.patternIndex == characterVisualInfo.patternIndex)
			{
				result = (this.colorIndex == characterVisualInfo.colorIndex);
			}
			else
			{
				result = false;
			}
			return result;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public void ResetToDefault()
	{
		this.skinIndex = 0;
		this.patternIndex = 0;
		this.colorIndex = 0;
	}

	public bool IsDefaultSelection()
	{
		if (this.skinIndex == 0)
		{
			if (this.patternIndex == 0)
			{
				return this.colorIndex == 0;
			}
		}
		return false;
	}
}
