using UnityEngine;
using UnityEngine.Networking;

public class CardManager : NetworkBehaviour
{
	public GameObject m_dataPrefab;

	private CardManagerData m_dataPrefabInstanceComponent;

	private static CardManager s_instance;

	public GameObject[] m_cardIndex
	{
		get
		{
			object result;
			if (m_dataPrefabInstanceComponent == null)
			{
				result = null;
			}
			else
			{
				result = m_dataPrefabInstanceComponent.m_cardIndex;
			}
			return (GameObject[])result;
		}
	}

	public bool ShowingInGameCardUI
	{
		get;
		set;
	}

	internal static CardManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		m_dataPrefabInstanceComponent = CardManagerData.Get();
		s_instance = this;
		ShowingInGameCardUI = true;
		Log.Info("Set CardManager reference");
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public void SetDeckAndGiveCards(ActorData actor, CharacterCardInfo cardInfo, bool isDebugRequest = false)
	{
	}

	internal void RemoveUsedCards()
	{
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
