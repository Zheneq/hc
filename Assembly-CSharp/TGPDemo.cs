using System;
using System.Collections.Generic;
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
		foreach (Renderer renderer in componentsInChildren)
		{
			foreach (Material material in renderer.materials)
			{
				if (material.shader.name.Contains("Outline"))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.Start()).MethodHandle;
					}
					list2.Add(material);
				}
				else if (material.shader.name.Contains("Toony"))
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
					list.Add(material);
				}
				if (material.shader.name.Contains("Toony"))
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					list3.Add(material);
				}
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		this.matsSimple = list.ToArray();
		this.matsOutline = list2.ToArray();
		this.matsAll = list3.ToArray();
		this.sceneLight = GameObject.Find("_Light");
		this.lightRotX = this.sceneLight.transform.eulerAngles.x;
		this.lightRotY = this.sceneLight.transform.eulerAngles.y;
		this.qualityLabel.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
		Shader.WarmupAllShaders();
		this.UpdateGUI();
	}

	private void SwitchRotation()
	{
		this.rotate = !this.rotate;
	}

	private void Update()
	{
		if (this.rotate)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.Update()).MethodHandle;
			}
			this.rotateGroup.transform.Rotate(Vector3.up * this.rotationSpeed * Time.deltaTime);
			this.rotY = this.rotateGroup.transform.eulerAngles.y;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis != 0f)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			this.zoom -= axis;
			this.zoom = Mathf.Clamp(this.zoom, 1f, 3f);
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, this.zoom);
		}
		if (Input.mousePosition.x < (float)Screen.width * 0.8f)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (Input.mousePosition.x > (float)Screen.width * 0.2f)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (Input.GetMouseButton(0))
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					Vector3 a = this.lastMousePos - Input.mousePosition;
					Camera.main.transform.Translate(a * Time.deltaTime * 0.2f);
				}
			}
		}
		this.lastMousePos = Input.mousePosition;
	}

	private void OnGUI()
	{
		this.zoom = GUI.VerticalSlider(new Rect((float)(Screen.width - 0x18), 16f, 10f, 224f), this.zoom, 1f, 3f);
		if (GUI.changed)
		{
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, this.zoom);
			GUI.changed = false;
		}
		GUI.enabled = !this.rotate;
		this.rotY = GUI.HorizontalSlider(new Rect(16f, 170f, 128f, 10f), this.rotY, 0f, 360f);
		GUI.enabled = true;
		if (GUI.changed && !this.rotate)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.OnGUI()).MethodHandle;
			}
			this.rotateGroup.transform.eulerAngles = new Vector3(0f, this.rotY, 0f);
			GUI.changed = false;
		}
		this.lightRotY = GUI.HorizontalSlider(new Rect(16f, 224f, 128f, 10f), this.lightRotY, 0f, 360f);
		GUI.enabled = true;
		if (GUI.changed)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.sceneLight.transform.eulerAngles = new Vector3(this.sceneLight.transform.eulerAngles.x, this.lightRotY, 0f);
			GUI.changed = false;
		}
		this.lightRotX = GUI.HorizontalSlider(new Rect(16f, 244f, 128f, 10f), this.lightRotX, -90f, 90f);
		GUI.enabled = true;
		if (GUI.changed)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.sceneLight.transform.eulerAngles = new Vector3(this.lightRotX, this.sceneLight.transform.eulerAngles.y, 0f);
			GUI.changed = false;
		}
		if (this.rim)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.rim_pow = GUI.HorizontalSlider(new Rect((float)(Screen.width - 0x96), 320f, 128f, 10f), this.rim_pow, -1f, 1f);
			GUI.enabled = true;
			if (GUI.changed)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int i = 0; i < this.matsAll.Length; i++)
				{
					this.matsAll[i].SetFloat("_RimPower", this.rim_pow);
				}
				GUI.changed = false;
			}
		}
		if (this.rimOutline)
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
			this.rimo_min = GUI.HorizontalSlider(new Rect((float)(Screen.width - 0x96), 320f, 128f, 10f), this.rimo_min, 0f, 1f);
			GUI.enabled = true;
			if (GUI.changed)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int j = 0; j < this.matsOutline.Length; j++)
				{
					this.matsOutline[j].SetFloat("_RimMin", this.rimo_min);
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				GUI.changed = false;
			}
			this.rimo_max = GUI.HorizontalSlider(new Rect((float)(Screen.width - 0x96), 360f, 128f, 10f), this.rimo_max, 0f, 1f);
			GUI.enabled = true;
			if (GUI.changed)
			{
				for (int k = 0; k < this.matsOutline.Length; k++)
				{
					this.matsOutline[k].SetFloat("_RimMax", this.rimo_max);
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				GUI.changed = false;
			}
		}
	}

	private void ReloadShader()
	{
		string str = "Normal";
		if (this.outline)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.ReloadShader()).MethodHandle;
			}
			str = ((!this.outline_cst) ? "Outline" : "OutlineConst");
		}
		string text = "Basic";
		if (this.bump)
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
			if (this.spec)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				text = "Bumped Specular";
				goto IL_90;
			}
		}
		if (this.spec)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			text = "Specular";
		}
		else if (this.bump)
		{
			text = "Bumped";
		}
		IL_90:
		if (this.rim)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			text += " Rim";
		}
		else if (this.rimOutline)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			str = "Rim Outline";
		}
		string text2 = "Toony Colors Pro/" + str + "/OneDirLight/" + text;
		Shader shader = this.FindShader(text2);
		if (shader == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Error("SHADER NOT FOUND: " + text2, new object[0]);
			return;
		}
		for (int i = 0; i < this.matsOutline.Length; i++)
		{
			this.matsOutline[i].shader = shader;
		}
		text2 = "Toony Colors Pro/Normal/OneDirLight/" + text;
		shader = this.FindShader(text2);
		if (shader == null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Error("SHADER NOT FOUND: " + text2, new object[0]);
			return;
		}
		for (int j = 0; j < this.matsSimple.Length; j++)
		{
			string text3 = "Basic";
			if (this.spec)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				text3 = "Specular";
			}
			if (this.rim)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				text3 += " Rim";
			}
			Shader shader2 = this.FindShader("Toony Colors Pro/Normal/OneDirLight/" + text3);
			if (shader2 != null)
			{
				this.matsSimple[j].shader = shader2;
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void UpdateGUI()
	{
		foreach (GameObject gameObject in this.actRim)
		{
			gameObject.SetActive(this.rim);
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.UpdateGUI()).MethodHandle;
		}
		foreach (GameObject gameObject2 in this.actRimOutline)
		{
			gameObject2.SetActive(this.rimOutline);
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		this.UpdateGUITButtons();
	}

	private void UpdateGUITButtons()
	{
		GUIT_Button[] array = (GUIT_Button[])UnityEngine.Object.FindObjectsOfType(typeof(GUIT_Button));
		GUIT_Button[] array2 = array;
		int i = 0;
		while (i < array2.Length)
		{
			GUIT_Button guit_Button = array2[i];
			string callback = guit_Button.callback;
			if (callback != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.UpdateGUITButtons()).MethodHandle;
				}
				if (!(callback == "SwitchOutline"))
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!(callback == "SwitchRim"))
					{
						if (!(callback == "SwitchRimOutline"))
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						else
						{
							guit_Button.UpdateState(this.rimOutline);
						}
					}
					else
					{
						guit_Button.UpdateState(this.rim);
					}
				}
				else
				{
					guit_Button.UpdateState(this.outline);
				}
			}
			IL_B6:
			i++;
			continue;
			goto IL_B6;
		}
	}

	private Shader FindShader(string name)
	{
		foreach (Shader shader in this.shaders)
		{
			if (shader.name == name)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.FindShader(string)).MethodHandle;
				}
				return shader;
			}
		}
		Log.Error("SHADER NOT FOUND: " + name, new object[0]);
		return null;
	}

	private void SwitchOutline()
	{
		this.outline = !this.outline;
		if (this.outline)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.SwitchOutline()).MethodHandle;
			}
			if (this.rimOutline)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				this.rimOutline = false;
			}
		}
		this.ReloadShader();
		this.UpdateGUI();
	}

	private void SwitchOutlineCst()
	{
		this.outline_cst = !this.outline_cst;
		this.ReloadShader();
	}

	private void SwitchSpec()
	{
		this.spec = !this.spec;
		this.ReloadShader();
	}

	private void SwitchBump()
	{
		this.bump = !this.bump;
		this.ReloadShader();
	}

	private void SwitchRim()
	{
		this.rim = !this.rim;
		if (this.rim)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.SwitchRim()).MethodHandle;
			}
			if (this.rimOutline)
			{
				this.rimOutline = false;
			}
		}
		this.ReloadShader();
		this.UpdateGUI();
	}

	private void SwitchRimOutline()
	{
		this.rimOutline = !this.rimOutline;
		if (this.rimOutline && this.rim)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.SwitchRimOutline()).MethodHandle;
			}
			this.rim = false;
		}
		if (this.rimOutline)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.outline)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				this.outline = false;
			}
		}
		this.ReloadShader();
		this.UpdateGUI();
	}

	private void NextRamp()
	{
		this.rampIndex++;
		if (this.rampIndex >= this.rampTextures.Length)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.NextRamp()).MethodHandle;
			}
			this.rampIndex = 0;
		}
		this.UpdateRamp();
	}

	private void PrevRamp()
	{
		this.rampIndex--;
		if (this.rampIndex < 0)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TGPDemo.PrevRamp()).MethodHandle;
			}
			this.rampIndex = this.rampTextures.Length - 1;
		}
		this.UpdateRamp();
	}

	private void UpdateRamp()
	{
		this.rampUI.texture = this.rampTextures[this.rampIndex];
		foreach (Material material in this.matsAll)
		{
			material.SetTexture("_Ramp", this.rampTextures[this.rampIndex]);
		}
	}

	private void NextQuality()
	{
		QualitySettings.IncreaseLevel(true);
		this.qualityLabel.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
	}

	private void PrevQuality()
	{
		QualitySettings.DecreaseLevel(true);
		this.qualityLabel.text = "Quality: " + QualitySettings.names[QualitySettings.GetQualityLevel()];
	}
}
