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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		Gizmos.DrawIcon(base.transform.position, "icon_SpawnLocation.png");
	}
}
