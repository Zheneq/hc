using System;

public class TooltipTokenPct : TooltipTokenEntry
{
	public int m_number;

	public TooltipTokenPct(string name, string desc, int val) : base(name, desc)
	{
		this.m_number = val;
	}

	public override string GetStringToReplace()
	{
		return "[" + this.m_name + "]";
	}

	public override string GetReplacementString()
	{
		return "<color=#FFC000>" + this.m_number + "%</color>";
	}

	public override string GetInEditorValuePreview()
	{
		return this.GetReplacementString();
	}
}
