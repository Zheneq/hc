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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		Gizmos.DrawIcon(base.transform.position, "icon_Brush.png");
	}
}
