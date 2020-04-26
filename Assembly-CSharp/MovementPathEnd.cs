using UnityEngine;

public class MovementPathEnd : MonoBehaviour
{
	public GameObject m_TopIndicatorPiece;

	public GameObject m_IndicatorParent;

	public GameObject m_diamondContainer;

	public GameObject m_movementLineParent;

	public GameObject m_chasingParent;

	public Animator m_animationController;

	public void SetupColor(Color newColor)
	{
		MeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<MeshRenderer>(true);
		MeshRenderer[] array = componentsInChildren;
		foreach (MeshRenderer meshRenderer in array)
		{
			if (meshRenderer.materials.Length > 0)
			{
				if (meshRenderer.materials[0] != null)
				{
					meshRenderer.materials[0].SetColor("_TintColor", newColor);
				}
			}
		}
	}

	public void Setup(ActorData actor, bool isChasing)
	{
		if (!isChasing)
		{
			m_IndicatorParent.SetActive(true);
			m_animationController.Play("Initial");
		}
		else
		{
			m_IndicatorParent.SetActive(false);
		}
		m_movementLineParent.SetActive(!isChasing);
		m_chasingParent.SetActive(isChasing);
	}

	public void Update()
	{
	}
}
