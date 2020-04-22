using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class FrontendUIEnvironmentCamera : MonoBehaviour
{
	public Color m_ambientColor = new Color(0.1111111f, 0.1111111f, 0.1111111f);

	public Cubemap m_customReflectionCubemap;

	private GameObject m_audioListener;

	private void Start()
	{
		if (AudioListenerController.Get() != null)
		{
			m_audioListener = AudioListenerController.Get().gameObject;
		}
		SetRenderSettings();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void Update()
	{
		if (m_audioListener != null)
		{
			m_audioListener.transform.position = base.transform.position;
		}
		UpdateDebugControls();
	}

	private void SetRenderSettings()
	{
		RenderSettings.ambientIntensity = 1f;
		RenderSettings.ambientLight = m_ambientColor;
		RenderSettings.ambientMode = AmbientMode.Flat;
		RenderSettings.defaultReflectionMode = DefaultReflectionMode.Custom;
		RenderSettings.customReflection = m_customReflectionCubemap;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		SetRenderSettings();
	}

	private void UpdateDebugControls()
	{
		if (DebugParameters.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (!DebugParameters.Get().GetParameterAsBool("DebugCamera"))
			{
				return;
			}
			while (true)
			{
				float num = 5f;
				float d = 0.1f;
				float num2 = 5f;
				float num3 = 5f;
				if (Input.GetMouseButton(2))
				{
					Vector3 localEulerAngles = base.transform.localEulerAngles;
					float x = localEulerAngles.x - Input.GetAxis("Mouse Y") * num2;
					Vector3 localEulerAngles2 = base.transform.localEulerAngles;
					float y = localEulerAngles2.y + Input.GetAxis("Mouse X") * num3;
					base.transform.localEulerAngles = new Vector3(x, y, 0f);
				}
				Vector3 zero = Vector3.zero;
				if (!UIUtils.InputFieldHasFocus())
				{
					if (AccountPreferences.DoesApplicationHaveFocus())
					{
						if (Input.GetKey(KeyCode.W))
						{
							zero += base.transform.forward * d;
						}
						if (Input.GetKey(KeyCode.A))
						{
							zero -= base.transform.right * d;
						}
						if (Input.GetKey(KeyCode.S))
						{
							zero -= base.transform.forward * d;
						}
						if (Input.GetKey(KeyCode.D))
						{
							zero += base.transform.right * d;
						}
						if (Input.GetKey(KeyCode.R))
						{
							zero += base.transform.up * d;
						}
						if (Input.GetKey(KeyCode.F))
						{
							zero -= base.transform.up * d;
						}
						if (Input.GetKey(KeyCode.LeftShift))
						{
							zero *= num;
						}
					}
				}
				base.transform.position += zero;
				return;
			}
		}
	}
}
