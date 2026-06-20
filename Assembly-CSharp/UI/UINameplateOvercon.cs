using UnityEngine;
using UnityEngine.UI;

public class UINameplateOvercon : MonoBehaviour
{
    public Image m_foregroundImg;
    public GameObject m_customPrefabParent;

    private CanvasGroup m_canvasGroup;
    private bool m_initialized;
    private float m_timeToDestroy = -1f;

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
        if (m_canvasGroup != null)
        {
            m_canvasGroup.blocksRaycasts = false;
            m_canvasGroup.interactable = false;
        }
    }

    public void Initialize(ActorData actor, UIOverconData.NameToOverconEntry entry)
    {
        m_initialized = true;
        if (entry != null)
        {
            if (m_foregroundImg != null && !string.IsNullOrEmpty(entry.m_staticSpritePath))
            {
                Sprite sprite = Resources.Load(entry.m_staticSpritePath, typeof(Sprite)) as Sprite;
                if (sprite != null)
                {
                    m_foregroundImg.sprite = sprite;
                    Color color = m_foregroundImg.color;
                    color.a = entry.m_initialAlpha;
                    m_foregroundImg.color = color;
                }
                else if (Application.isEditor)
                {
                    Debug.LogWarning("Did not find overcon sprite at: " + entry.m_staticSpritePath);
                }
            }

            if (!string.IsNullOrEmpty(entry.m_customPrefabPath) && m_customPrefabParent != null)
            {
                GameObject prefab = Resources.Load(entry.m_customPrefabPath, typeof(GameObject)) as GameObject;
                if (prefab != null)
                {
                    GameObject overconObject = Instantiate(prefab);
                    if (overconObject != null)
                    {
                        overconObject.transform.SetParent(m_customPrefabParent.transform);
                        overconObject.transform.localPosition = new Vector3(0f, entry.m_customPrefabHeightOffset, 0f);
                    }
                }
                else if (Application.isEditor)
                {
                    Debug.LogWarning("Did not find overcon prefab at: " + entry.m_customPrefabPath);
                }
            }

            m_timeToDestroy = Time.time + (entry.m_ageInSeconds > 0f
                ? entry.m_ageInSeconds
                : UIOverconData.c_defaultOverconAgeInSeconds);
            return;
        }

        m_timeToDestroy = Time.time;
    }

    public void SetCanvasGroupVisibility(bool visible)
    {
        if (m_canvasGroup == null)
        {
            return;
        }

        if (visible)
        {
            m_canvasGroup.alpha = 1f;
            m_canvasGroup.blocksRaycasts = true;
            m_canvasGroup.interactable = true;
        }
        else
        {
            m_canvasGroup.alpha = 0f;
            m_canvasGroup.blocksRaycasts = false;
            m_canvasGroup.interactable = false;
        }
    }

    private void OnDestroy()
    {
        if (m_customPrefabParent != null)
        {
            Destroy(m_customPrefabParent);
        }
    }

    private void Update()
    {
        if (!m_initialized)
        {
            return;
        }

        if (Time.time >= m_timeToDestroy)
        {
            Destroy(gameObject);
        }
    }
}