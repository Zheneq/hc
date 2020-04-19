using System;

public class StatMod
{
	public ModType mod;

	public float val;

	public void Setup(ModType mod, float val)
	{
		this.mod = mod;
		this.val = val;
	}
}
