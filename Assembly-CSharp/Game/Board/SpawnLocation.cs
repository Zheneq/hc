using UnityEngine;

public class SpawnLocation : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Gizmos.DrawIcon(base.transform.position, "icon_SpawnLocation.png");
	}
}
