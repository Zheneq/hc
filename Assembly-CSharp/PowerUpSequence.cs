using UnityEngine;

public class PowerUpSequence : Sequence
{
    public Transform m_powerUpPrefab;
    private Transform m_powerUpVFX;
    private bool m_created;

    private void Update()
    {
        if (m_powerUpPrefab && m_initialized && !m_created)
        {
            m_created = true;
            m_powerUpVFX = Instantiate(m_powerUpPrefab, TargetSquare.ToVector3(), Quaternion.identity);
        }
    }

    private void OnDisable()
    {
        if (m_powerUpVFX)
        {
            Destroy(m_powerUpVFX.gameObject);
        }
    }
}