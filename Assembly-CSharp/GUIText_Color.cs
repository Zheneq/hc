using UnityEngine;

[RequireComponent(typeof(GUIText))]
public class GUIText_Color : MonoBehaviour
{
	public Color labelColor;

	private void Awake()
	{
		GetComponent<GUIText>().material.color = labelColor;
	}
}
