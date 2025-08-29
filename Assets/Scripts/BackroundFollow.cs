using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform player;

    private Vector3 offset;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player Transform atanmamış!");
            return;
        }

        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}
