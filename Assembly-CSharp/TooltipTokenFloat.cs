using System.Text;

public class TooltipTokenFloat : TooltipTokenEntry
{
	public float m_number;

	public TooltipTokenFloat(string name, string desc, float val)
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
		return new StringBuilder().Append("<color=#FFC000>").Append(m_number.ToString("n1")).Append("</color>").ToString();
	}

	public override string GetInEditorValuePreview()
	{
		return GetReplacementString();
	}
}
