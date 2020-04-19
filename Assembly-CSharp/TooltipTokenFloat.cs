using System;

public class TooltipTokenFloat : TooltipTokenEntry
{
	public float m_number;

	public TooltipTokenFloat(string name, string desc, float val) : base(name, desc)
	{
		this.m_number = val;
	}

	public override string GetStringToReplace()
	{
		return "[" + this.m_name + "]";
	}

	public override string GetReplacementString()
	{
		return "<color=#FFC000>" + this.m_number.ToString("n1") + "</color>";
	}

	public override string GetInEditorValuePreview()
	{
		return this.GetReplacementString();
	}
}
