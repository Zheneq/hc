using System;
using UnityEngine;

public class PowerUpLocation : MonoBehaviour
{
	private BoardSquare m_boardSquare;

	public BoardSquare boardSquare
	{
		get
		{
			return this.m_boardSquare;
		}
	}

	public void Initialize()
	{
		this.m_boardSquare = Board.\u000E().\u0012(base.transform.position.x, base.transform.position.z);
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUpLocation.OnDrawGizmos()).MethodHandle;
			}
			return;
		}
		Gizmos.DrawIcon(base.transform.position, "icon_PowerUp.png");
	}
}
