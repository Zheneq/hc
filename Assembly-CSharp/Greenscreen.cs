using System;

public class Greenscreen : Singleton<Greenscreen>
{
	private void Start()
	{
		base.gameObject.SetActive(false);
	}
}
