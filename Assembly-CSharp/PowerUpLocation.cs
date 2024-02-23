using UnityEngine;

public class PowerUpLocation : MonoBehaviour
{
    private BoardSquare m_boardSquare;

    public BoardSquare boardSquare
    {
        get { return m_boardSquare; }
    }

    public void Initialize()
    {
        Vector3 position = transform.position;
        m_boardSquare = Board.Get().GetSquareFromPos(position.x, position.z);
    }

    private void OnDrawGizmos()
    {
        if (CameraManager.ShouldDrawGizmosForCurrentCamera())
        {
            Gizmos.DrawIcon(transform.position, "icon_PowerUp.png");
        }
    }
}