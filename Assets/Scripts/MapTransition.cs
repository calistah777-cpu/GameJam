using UnityEngine;
using Unity.Cinemachine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;
    CinemachineConfiner confiner;
    [SerializeField] Direction direction;
    [SerializeField] float additivePos = 2f;
    enum Direction {Up, Down, Left, Right}

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner>();
        UpdatePlayerPosition(collision.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.m_BoundingShape2D - mapBoundary;
        }
    }
    
    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 = newPos - player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                newPos.y += additivePos;
                break;
            case direction.Down:
                newPos.y -= additivePos;
                break;
            case Direction.Left:
                newPos.x += additivePos;
                break;
            case Direction.Right:
                newPos.x -= additivePos;
                break;
        }

        player.transform.position = newPos;
    }
}
