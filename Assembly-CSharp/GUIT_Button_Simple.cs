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
		GetComponentInChildren<GUIText>().material.color = labelColor;
		UpdateImage();
	}

	private void Update()
	{
		if (GetComponent<GUITexture>().GetScreenRect().Contains(Input.mousePosition))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (!over)
					{
						OnOver();
					}
					if (Input.GetMouseButtonDown(0))
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								OnClick();
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (!over)
		{
			return;
		}
		while (true)
		{
			OnOut();
			return;
		}
	}

	private void OnClick()
	{
		callbackObject.SendMessage(callback);
	}

	private void OnOver()
	{
		over = true;
		UpdateImage();
	}

	private void OnOut()
	{
		over = false;
		UpdateImage();
	}

	private void UpdateImage()
	{
		if (over)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					GetComponent<GUITexture>().texture = text_over;
					return;
				}
			}
		}
		GetComponent<GUITexture>().texture = text;
	}
}
