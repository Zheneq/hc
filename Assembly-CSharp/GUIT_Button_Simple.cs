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
			if (!this.over)
			{
				this.OnOver();
			}
			if (Input.GetMouseButtonDown(0))
			{
				this.OnClick();
			}
		}
		else if (this.over)
		{
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
			base.GetComponent<GUITexture>().texture = this.text_over;
		}
		else
		{
			base.GetComponent<GUITexture>().texture = this.text;
		}
	}
}
