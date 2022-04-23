using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageDealer : MonoBehaviour
{
    public GameObject weaponHolder;
    private GameObject weapon;
    private SpriteRenderer weaponSprite;
    public LayerMask enemyLayers;
    public int weaponDamage;
    public float knockbackDuration = 0.5f;
    public float knockbackPower = 10f;
    public List<Collider2D> alreadyHit;

    void Start() {
        weapon = weaponHolder.transform.GetChild(0).gameObject;
        weaponSprite = weapon.GetComponent<SpriteRenderer>();
    }

    void CheckForDamage(int firstCheck) {
        if(firstCheck == 1) {
            alreadyHit.Clear();
        }
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(weapon.transform.position, weaponSprite.bounds.size, weapon.transform.eulerAngles.z, enemyLayers);
        foreach(Collider2D enemy in hitEnemies) {
            if (!(alreadyHit.Contains(enemy))) {
            enemy.transform.GetChild(0).GetComponent<EnemyController>().TakeDamage(weaponDamage);
            StartCoroutine(enemy.transform.GetChild(0).GetComponent<EnemyController>().Knockback(knockbackDuration, knockbackPower));
            alreadyHit.Add(enemy);
            }
        }
    }

    void OnDrawGizmosSelected() {
        // Gizmos.DrawCube();
    }
}
