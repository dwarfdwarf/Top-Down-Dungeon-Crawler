using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    Vector2 movement;
    public Animator animator;
    public Animator weaponAnimator;
    public SpriteRenderer sr;
    public int maxHealth;
    public int health;
    public GameObject healthBar;
    public List<Sprite> heartSprites;
    private float timeSinceHit;
    public float iFrames;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 6;
        health = maxHealth;
        timeSinceHit = iFrames;
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("Speed", movement.sqrMagnitude);
        timeSinceHit += Time.deltaTime;
    }

    void FixedUpdate() {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        if (movement.x == -1) {
            sr.flipX = true;
            weaponAnimator.SetFloat("Direction", -1);
        } else if (movement.x == 1) {
            sr.flipX = false;
            weaponAnimator.SetFloat("Direction", 1);
        }
    }

    public void TakeDamage(EnemyController enemy) {
        if(timeSinceHit > iFrames) {
            animator.SetTrigger("Damage");
            Knockback(enemy);
            health -= enemy.damage;
            UpdateHealthBar();
            timeSinceHit = 0;
            if (health <= 0) {
                Die();
            }
        }
    }

    private async void UpdateHealthBar() {
        int heartNum = (health / 2) + 1;
        heartNum = 3 - heartNum;
        if (health % 2 == 0 && health != maxHealth) {
            for (int i = 0; i <= heartNum; ++i) {
                healthBar.transform.GetChild(i).GetComponent<Image>().sprite = heartSprites[2];
            }
            
            for (int  i = heartNum + 1; i < 3; ++i) {
                healthBar.transform.GetChild(i).GetComponent<Image>().sprite = heartSprites[0];
            }
        } else if (health != maxHealth) {
            for (int i = 0; i < heartNum; ++i) {
                healthBar.transform.GetChild(i).GetComponent<Image>().sprite = heartSprites[2];
            }

            healthBar.transform.GetChild(heartNum).GetComponent<Image>().sprite = heartSprites[1];

            for (int  i = heartNum + 1; i < 3; ++i) {
                healthBar.transform.GetChild(i).GetComponent<Image>().sprite = heartSprites[0];
            }
        } else {
            for(int i = 0; i < 3; ++i) {
                healthBar.transform.GetChild(i).GetComponent<Image>().sprite = heartSprites[2];
            }
        }
    }

    public IEnumerator Knockback(EnemyController enemy) {
        float timer = 0;
        while (enemy.knockDuration > timer) {
            timer += Time.deltaTime;
            Vector2 direction = (enemy.transform.position - this.transform.gameObject.transform.position).normalized;
            rb.AddForce(-direction * enemy.knockPower);
        }
        yield return 0;
    }

    public void Die() {
        this.transform.gameObject.SetActive(false);
    }
}
