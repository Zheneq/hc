using UnityEngine;

public class GrydDetonateBomb : Ability
{
    [Header("-- Sequences")]
    public GameObject m_castSequencePrefab;

    private void Start()
    {
        if (m_abilityName == "Base Ability")
        {
            m_abilityName = "Detonate";
        }
    }
}