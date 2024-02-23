using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TGPDemo : MonoBehaviour
{
	public bool rotate = true;

	public GameObject rotateGroup;

	public float rotationSpeed = 50f;

	public Texture[] rampTextures;

	public GUITexture rampUI;

	private int rampIndex;

	public GUIText qualityLabel;

	private Material[] matsSimple;

	private Material[] matsOutline;

	private Material[] matsAll;

	private GameObject sceneLight;

	public Shader[] shaders;

	private Vector3 lastMousePos;

	private float zoom = 2f;

	private float rotY;

	private float lightRotX;

	private float lightRotY;

	private float rimo_min = 0.4f;

	private float rimo_max = 0.6f;

	private float rim_pow = 0.5f;

	private bool bump = true;

	private bool spec = true;

	private bool outline = true;

	private bool outline_cst;

	private bool rim;

	private bool rimOutline;

	public GameObject[] actRim;

	public GameObject[] actRimOutline;

	public GUIT_Button subOutlines;

	private void Start()
	{
		GameObject gameObject = GameObject.Find("TGPDemo_Astrella").gameObject;
		Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
		List<Material> list = new List<Material>();
		List<Material> list2 = new List<Material>();
		List<Material> list3 = new List<Material>();
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				if (material.shader.name.Contains("Outline"))
				{
					list2.Add(material);
				}
				else if (material.shader.name.Contains("Toony"))
				{
					list.Add(material);
				}
				if (material.shader.name.Contains("Toony"))
				{
					list3.Add(material);
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					goto end_IL_00f9;
				}
				continue;
				end_IL_00f9:
				break;
			}
		}
		while (true)
		{
			matsSimple = list.ToArray();
			matsOutline = list2.ToArray();
			matsAll = list3.ToArray();
			sceneLight = GameObject.Find("_Light");
			Vector3 eulerAngles = sceneLight.transform.eulerAngles;
			lightRotX = eulerAngles.x;
			Vector3 eulerAngles2 = sceneLight.transform.eulerAngles;
			lightRotY = eulerAngles2.y;
			qualityLabel.text = new StringBuilder().Append("Quality: ").Append(QualitySettings.names[QualitySettings.GetQualityLevel()]).ToString();
			Shader.WarmupAllShaders();
			UpdateGUI();
			return;
		}
	}

	private void SwitchRotation()
	{
		rotate = !rotate;
	}

	private void Update()
	{
		if (rotate)
		{
			rotateGroup.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
			Vector3 eulerAngles = rotateGroup.transform.eulerAngles;
			rotY = eulerAngles.y;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis != 0f)
		{
			zoom -= axis;
			zoom = Mathf.Clamp(zoom, 1f, 3f);
			Transform transform = Camera.main.transform;
			Vector3 position = Camera.main.transform.position;
			float x = position.x;
			Vector3 position2 = Camera.main.transform.position;
			transform.position = new Vector3(x, position2.y, zoom);
		}
		Vector3 mousePosition = Input.mousePosition;
		if (mousePosition.x < (float)Screen.width * 0.8f)
		{
			Vector3 mousePosition2 = Input.mousePosition;
			if (mousePosition2.x > (float)Screen.width * 0.2f)
			{
				if (Input.GetMouseButton(0))
				{
					Vector3 a = lastMousePos - Input.mousePosition;
					Camera.main.transform.Translate(a * Time.deltaTime * 0.2f);
				}
			}
		}
		lastMousePos = Input.mousePosition;
	}

	private void OnGUI()
	{
		zoom = GUI.VerticalSlider(new Rect(Screen.width - 24, 16f, 10f, 224f), zoom, 1f, 3f);
		if (GUI.changed)
		{
			Transform transform = Camera.main.transform;
			Vector3 position = Camera.main.transform.position;
			float x = position.x;
			Vector3 position2 = Camera.main.transform.position;
			transform.position = new Vector3(x, position2.y, zoom);
			GUI.changed = false;
		}
		GUI.enabled = !rotate;
		rotY = GUI.HorizontalSlider(new Rect(16f, 170f, 128f, 10f), rotY, 0f, 360f);
		GUI.enabled = true;
		if (GUI.changed && !rotate)
		{
			rotateGroup.transform.eulerAngles = new Vector3(0f, rotY, 0f);
			GUI.changed = false;
		}
		lightRotY = GUI.HorizontalSlider(new Rect(16f, 224f, 128f, 10f), lightRotY, 0f, 360f);
		GUI.enabled = true;
		if (GUI.changed)
		{
			Transform transform2 = sceneLight.transform;
			Vector3 eulerAngles = sceneLight.transform.eulerAngles;
			transform2.eulerAngles = new Vector3(eulerAngles.x, lightRotY, 0f);
			GUI.changed = false;
		}
		lightRotX = GUI.HorizontalSlider(new Rect(16f, 244f, 128f, 10f), lightRotX, -90f, 90f);
		GUI.enabled = true;
		if (GUI.changed)
		{
			Transform transform3 = sceneLight.transform;
			float x2 = lightRotX;
			Vector3 eulerAngles2 = sceneLight.transform.eulerAngles;
			transform3.eulerAngles = new Vector3(x2, eulerAngles2.y, 0f);
			GUI.changed = false;
		}
		if (rim)
		{
			rim_pow = GUI.HorizontalSlider(new Rect(Screen.width - 150, 320f, 128f, 10f), rim_pow, -1f, 1f);
			GUI.enabled = true;
			if (GUI.changed)
			{
				for (int i = 0; i < matsAll.Length; i++)
				{
					matsAll[i].SetFloat("_RimPower", rim_pow);
				}
				GUI.changed = false;
			}
		}
		if (!rimOutline)
		{
			return;
		}
		while (true)
		{
			rimo_min = GUI.HorizontalSlider(new Rect(Screen.width - 150, 320f, 128f, 10f), rimo_min, 0f, 1f);
			GUI.enabled = true;
			if (GUI.changed)
			{
				for (int j = 0; j < matsOutline.Length; j++)
				{
					matsOutline[j].SetFloat("_RimMin", rimo_min);
				}
				GUI.changed = false;
			}
			rimo_max = GUI.HorizontalSlider(new Rect(Screen.width - 150, 360f, 128f, 10f), rimo_max, 0f, 1f);
			GUI.enabled = true;
			if (GUI.changed)
			{
				for (int k = 0; k < matsOutline.Length; k++)
				{
					matsOutline[k].SetFloat("_RimMax", rimo_max);
				}
				while (true)
				{
					GUI.changed = false;
					return;
				}
			}
			return;
		}
	}

	private void ReloadShader()
	{
		string str = "Normal";
		if (outline)
		{
			str = ((!outline_cst) ? "Outline" : "OutlineConst");
		}
		string text = "Basic";
		if (bump)
		{
			if (spec)
			{
				text = "Bumped Specular";
				goto IL_0090;
			}
		}
		if (spec)
		{
			text = "Specular";
		}
		else if (bump)
		{
			text = "Bumped";
		}
		goto IL_0090;
		IL_0090:
		if (rim)
		{
			text += " Rim";
		}
		else if (rimOutline)
		{
			str = "Rim Outline";
		}
		string text2 = new StringBuilder().Append("Toony Colors Pro/").Append(str).Append("/OneDirLight/").Append(text).ToString();
		Shader shader = FindShader(text2);
		if (shader == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Log.Error(new StringBuilder().Append("SHADER NOT FOUND: ").Append(text2).ToString());
					return;
				}
			}
		}
		for (int i = 0; i < matsOutline.Length; i++)
		{
			matsOutline[i].shader = shader;
		}

		text2 = new StringBuilder().Append("Toony Colors Pro/Normal/OneDirLight/").Append(text).ToString();
		shader = FindShader(text2);
		if (shader == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Log.Error(new StringBuilder().Append("SHADER NOT FOUND: ").Append(text2).ToString());
					return;
				}
			}
		}
		for (int j = 0; j < matsSimple.Length; j++)
		{
			string text3 = "Basic";
			if (spec)
			{
				text3 = "Specular";
			}
			if (rim)
			{
				text3 += " Rim";
			}
			Shader shader2 = FindShader(new StringBuilder().Append("Toony Colors Pro/Normal/OneDirLight/").Append(text3).ToString());
			if (shader2 != null)
			{
				matsSimple[j].shader = shader2;
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void UpdateGUI()
	{
		GameObject[] array = actRim;
		foreach (GameObject gameObject in array)
		{
			gameObject.SetActive(rim);
		}
		while (true)
		{
			GameObject[] array2 = actRimOutline;
			foreach (GameObject gameObject2 in array2)
			{
				gameObject2.SetActive(rimOutline);
			}
			while (true)
			{
				UpdateGUITButtons();
				return;
			}
		}
	}

	private void UpdateGUITButtons()
	{
		GUIT_Button[] array = (GUIT_Button[])Object.FindObjectsOfType(typeof(GUIT_Button));
		GUIT_Button[] array2 = array;
		foreach (GUIT_Button gUIT_Button in array2)
		{
			string callback = gUIT_Button.callback;
			if (callback == null)
			{
				continue;
			}
			if (!(callback == "SwitchOutline"))
			{
				if (!(callback == "SwitchRim"))
				{
					if (!(callback == "SwitchRimOutline"))
					{
					}
					else
					{
						gUIT_Button.UpdateState(rimOutline);
					}
				}
				else
				{
					gUIT_Button.UpdateState(rim);
				}
			}
			else
			{
				gUIT_Button.UpdateState(outline);
			}
		}
	}

	private Shader FindShader(string name)
	{
		Shader[] array = shaders;
		foreach (Shader shader in array)
		{
			if (!(shader.name == name))
			{
				continue;
			}
			while (true)
			{
				return shader;
			}
		}
		Log.Error(new StringBuilder().Append("SHADER NOT FOUND: ").Append(name).ToString());
		return null;
	}

	private void SwitchOutline()
	{
		outline = !outline;
		if (outline)
		{
			if (rimOutline)
			{
				rimOutline = false;
			}
		}
		ReloadShader();
		UpdateGUI();
	}

	private void SwitchOutlineCst()
	{
		outline_cst = !outline_cst;
		ReloadShader();
	}

	private void SwitchSpec()
	{
		spec = !spec;
		ReloadShader();
	}

	private void SwitchBump()
	{
		bump = !bump;
		ReloadShader();
	}

	private void SwitchRim()
	{
		rim = !rim;
		if (rim)
		{
			if (rimOutline)
			{
				rimOutline = false;
			}
		}
		ReloadShader();
		UpdateGUI();
	}

	private void SwitchRimOutline()
	{
		rimOutline = !rimOutline;
		if (rimOutline && rim)
		{
			rim = false;
		}
		if (rimOutline)
		{
			if (outline)
			{
				outline = false;
			}
		}
		ReloadShader();
		UpdateGUI();
	}

	private void NextRamp()
	{
		rampIndex++;
		if (rampIndex >= rampTextures.Length)
		{
			rampIndex = 0;
		}
		UpdateRamp();
	}

	private void PrevRamp()
	{
		rampIndex--;
		if (rampIndex < 0)
		{
			rampIndex = rampTextures.Length - 1;
		}
		UpdateRamp();
	}

	private void UpdateRamp()
	{
		rampUI.texture = rampTextures[rampIndex];
		Material[] array = matsAll;
		foreach (Material material in array)
		{
			material.SetTexture("_Ramp", rampTextures[rampIndex]);
		}
	}

	private void NextQuality()
	{
		QualitySettings.IncreaseLevel(true);
		qualityLabel.text = new StringBuilder().Append("Quality: ").Append(QualitySettings.names[QualitySettings.GetQualityLevel()]).ToString();
	}

	private void PrevQuality()
	{
		QualitySettings.DecreaseLevel(true);
		qualityLabel.text = new StringBuilder().Append("Quality: ").Append(QualitySettings.names[QualitySettings.GetQualityLevel()]).ToString();
	}
}
