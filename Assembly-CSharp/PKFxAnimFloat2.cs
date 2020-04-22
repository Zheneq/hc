using UnityEngine;

[RequireComponent(typeof(PKFxFX))]
public class PKFxAnimFloat2 : MonoBehaviour
{
	public string propertyName;

	public Vector2 value;

	private PKFxFX fx;

	private void Start()
	{
		fx = GetComponent<PKFxFX>();
		if (!(fx == null))
		{
			return;
		}
		while (true)
		{
			base.enabled = false;
			return;
		}
	}

	private void LateUpdate()
	{
		fx.SetAttribute(new PKFxManager.Attribute(propertyName, value));
	}
}
