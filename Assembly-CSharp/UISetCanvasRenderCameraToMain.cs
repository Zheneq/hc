using UnityEngine;

public class UISetCanvasRenderCameraToMain : MonoBehaviour
{
	private void Update()
	{
		if (!(Camera.main != null))
		{
			return;
		}
		Canvas component = GetComponent<Canvas>();
		Camera worldCamera = Camera.main;
		PKFxRenderingPlugin componentInChildren = Camera.main.gameObject.GetComponentInChildren<PKFxRenderingPlugin>();
		if (componentInChildren != null)
		{
			Camera component2 = componentInChildren.gameObject.GetComponent<Camera>();
			if (component2 != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				worldCamera = component2;
			}
		}
		component.worldCamera = worldCamera;
		base.enabled = false;
	}
}
