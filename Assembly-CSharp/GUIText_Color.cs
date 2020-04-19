using System;
using UnityEngine;

[RequireComponent(typeof(GUIText))]
public class GUIText_Color : MonoBehaviour
{
	public Color labelColor;

	private void Awake()
	{
		base.GetComponent<GUIText>().material.color = this.labelColor;
	}
}
