using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float horizontalRange = 5f;

    private Vector3 _startPosition;
    private int _direction = 1;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        Vector3 pos = transform.position;
        pos.x += _direction * moveSpeed * Time.deltaTime;

        float offsetFromStart = pos.x - _startPosition.x;
        if (Mathf.Abs(offsetFromStart) > horizontalRange)
        {
            _direction *= -1;
            float clamped = Mathf.Sign(offsetFromStart) * horizontalRange;
            pos.x = _startPosition.x + clamped;
        }

        transform.position = pos;
    }
}
