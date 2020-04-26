public class TooltipTokenInt : TooltipTokenEntry
{
	public int m_number;

	public TooltipTokenInt(string name, string desc, int val)
		: base(name, desc)
	{
		m_number = val;
	}

	public override string GetStringToReplace()
	{
		return "[" + m_name + "]";
	}

	public override string GetReplacementString()
	{
		return "<color=#FFC000>" + m_number + "</color>";
	}

	public override string GetInEditorValuePreview()
	{
		return GetReplacementString();
	}
}
