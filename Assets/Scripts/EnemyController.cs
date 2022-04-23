using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public int maxHealth;
    private int health;
    public Animator animator;
    public PlayerController player;
    public Rigidbody2D rb;
    public int damage;
    public GameObject enemy;
    public float knockDuration;
    public float knockPower;
    public CapsuleCollider2D enemyCollider;
    public LayerMask playerLayer;
    public AIPath aiPath;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        enemy = this.transform.gameObject;
    }

    void Update() {
        Collider2D[] hits = Physics2D.OverlapCapsuleAll(enemy.transform.position, enemyCollider.size, enemyCollider.direction, 0f, playerLayer);
        foreach(Collider2D hit in hits) {
            //Debug.Log("hit by enemy!");
            player.TakeDamage(this);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        animator.SetTrigger("Hurt");

        if (health <= 0) {
            Die();
        }
    }

    void Die() {
        //Debug.Log("Enemy died!");
        this.transform.gameObject.SetActive(false);
    }

    public IEnumerator Knockback(float knockbackDuration, float knockbackPower) {
        float timer = 0;
        while (knockbackDuration > timer) {
            timer += Time.deltaTime;
            Vector2 direction = (player.transform.position - this.transform.position).normalized;
            rb.AddForce(-direction * knockbackPower);
        }
        yield return 0;
    }
}
