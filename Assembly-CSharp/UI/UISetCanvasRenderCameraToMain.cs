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
				worldCamera = component2;
			}
		}
		component.worldCamera = worldCamera;
		base.enabled = false;
	}
}
