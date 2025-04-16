using UnityEngine;
using UnityEngine.AI;

public abstract class BaseCreature : MonoBehaviour
{
    public float detectionRange = 5f;
    public float moveSpeed = 2f;
    public float maxLifeDuration = -1f; // -1 means live forever

    protected GameObject player;
    protected Rigidbody2D rb;
    protected Vector2 movement;
    protected float spawnTime;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
    }

    protected virtual void Update()
    {
        if (maxLifeDuration > 0 && Time.time - spawnTime > maxLifeDuration)
        {
            gameObject.SetActive(false);
            return;
        }

        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance < detectionRange)
        {
            movement = (player.transform.position - transform.position).normalized;
        }
        else
        {
            // random wandering movement
            if (Random.Range(0f, 1f) < 0.01f)
                movement = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        rb.linearVelocity = movement * moveSpeed;
    }

    public abstract void OnPlayerCollision(GameObject player);
}