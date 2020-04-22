using UnityEngine;

public class FogOfWarCamera : MonoBehaviour
{
	private void Awake()
	{
	}

	private void Start()
	{
	}

	public static FogOfWarCamera GetFirstFogOfWarCamera()
	{
		FogOfWarCamera fogOfWarCamera = null;
		Camera[] allCameras = Camera.allCameras;
		int num = 0;
		while (true)
		{
			if (num < allCameras.Length)
			{
				Camera camera = allCameras[num];
				fogOfWarCamera = camera.GetComponent<FogOfWarCamera>();
				if (fogOfWarCamera != null)
				{
					while (true)
					{
						switch (3)
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
					break;
				}
				num++;
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		return fogOfWarCamera;
	}
}
