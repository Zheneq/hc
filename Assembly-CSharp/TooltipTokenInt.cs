using System.Text;

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
		return new StringBuilder().Append("[").Append(m_name).Append("]").ToString();
	}

	public override string GetReplacementString()
	{
		return new StringBuilder().Append("<color=#FFC000>").Append(m_number).Append("</color>").ToString();
	}

	public override string GetInEditorValuePreview()
	{
		return GetReplacementString();
	}
}
