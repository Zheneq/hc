using UnityEngine;

public class FogOfWarCameraSettings : MonoBehaviour
{
	public Color fogColor = new Color(0.4706f, 0.4706f, 0.4706f, 0f);

	public float minHeightOffset = -90f;

	public Camera m_postCamera;

	public Camera m_camera;

	private static FogOfWarCameraSettings s_instance;

	internal static FogOfWarCameraSettings Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}
}
