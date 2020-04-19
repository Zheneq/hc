using System;
using UnityEngine;

[RequireComponent(typeof(GUITexture))]
public class GUIT_Button_Simple : MonoBehaviour
{
	public Color labelColor;

	public Texture text;

	public Texture text_over;

	public GameObject callbackObject;

	public string callback;

	private bool over;

	private void Awake()
	{
		base.GetComponentInChildren<GUIText>().material.color = this.labelColor;
		this.UpdateImage();
	}

	private void Update()
	{
		if (base.GetComponent<GUITexture>().GetScreenRect().Contains(Input.mousePosition))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GUIT_Button_Simple.Update()).MethodHandle;
			}
			if (!this.over)
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
				this.OnOver();
			}
			if (Input.GetMouseButtonDown(0))
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
				this.OnClick();
			}
		}
		else if (this.over)
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
			this.OnOut();
		}
	}

	private void OnClick()
	{
		this.callbackObject.SendMessage(this.callback);
	}

	private void OnOver()
	{
		this.over = true;
		this.UpdateImage();
	}

	private void OnOut()
	{
		this.over = false;
		this.UpdateImage();
	}

	private void UpdateImage()
	{
		if (this.over)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GUIT_Button_Simple.UpdateImage()).MethodHandle;
			}
			base.GetComponent<GUITexture>().texture = this.text_over;
		}
		else
		{
			base.GetComponent<GUITexture>().texture = this.text;
		}
	}
}
