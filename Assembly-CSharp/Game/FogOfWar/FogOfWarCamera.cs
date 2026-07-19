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
					break;
				}
				num++;
				continue;
			}
			break;
		}
		return fogOfWarCamera;
	}
}
