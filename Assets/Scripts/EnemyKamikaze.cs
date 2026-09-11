using UnityEngine;

public class EnemyKamikaze : EnemyScript
{
    private Transform playerTransform;

    protected override void Start()
    {
        base.Start();
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) playerTransform = player.transform;
    }

    protected override void Move()
    {
        if (playerTransform != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
        }
        else
        {
            base.Move();
        }
    }
}