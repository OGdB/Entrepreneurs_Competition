using UnityEngine;

public class Hover : MonoBehaviour
{
    [SerializeField]
    private float hoverSpeedMultiplier = 1f;
    private float direction = 1f;

    private float currentYOffset = 0f;
    [SerializeField]
    private float maxOffset = 1f;

    public Vector3 basePosition;

    private void Start()
    {
        basePosition = transform.position;
    }

    private void Update()
    {
        if (currentYOffset >= maxOffset || currentYOffset <= -maxOffset)
        {
            direction *= -1f;
        }

        currentYOffset += Time.deltaTime * hoverSpeedMultiplier * direction;

        Vector3 currentPosition = transform.position;
        currentPosition.y = basePosition.y + currentYOffset;
        transform.position = currentPosition;
    }
}
