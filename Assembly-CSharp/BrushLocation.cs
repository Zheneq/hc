using UnityEngine;

public class BrushLocation : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Gizmos.DrawIcon(base.transform.position, "icon_Brush.png");
	}
}
