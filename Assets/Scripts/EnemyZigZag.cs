using UnityEngine;

public class EnemyZigZag : EnemyScript
{
    public float frequency = 5f;
    public float amplitude = 2f;
    private Vector3 startPosition;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
    }

    protected override void Move()
    {
        float xOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position += Vector3.back * speed * Time.deltaTime;
        transform.position = new Vector3(startPosition.x + xOffset, transform.position.y, transform.position.z);
    }
}