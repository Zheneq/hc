using System;

[Serializable]
public class KeyBindingCommand
{
	public string Name;

	public string DisplayName;

	public bool Settable;

	public KeyBindCategory Category;

	public string GetDisplayName()
	{
		return StringUtil.TR_KeyBindCommand(this.Name);
	}
}
