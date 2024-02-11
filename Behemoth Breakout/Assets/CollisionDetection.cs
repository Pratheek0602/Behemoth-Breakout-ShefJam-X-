using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public PlayerController wc;
    public GameObject HitParticle;

    public Actor enemyActor; // Reference to the Actor script attached to the enemy
    public int damageAmount = 10; // Amount of damage to apply on collision

    private void OnTriggerEnter(Collider other){
        //  Debug.Log("HITTTTTTTTTTTT" + other.tag + wc.isAttacking);
        if(other.tag == "Enemy" && wc.isAttacking){
            // Debug.Log(other.name);
            // Debug.Log("WORKSSSSSSSSSSSSS");
            Actor enemy = other.GetComponent<Actor>();
            other.GetComponent<Animator>().SetTrigger("Hit");
            Instantiate(HitParticle, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z),
            other.transform.rotation);
            // Deal damage to the enemy
            enemy.TakeDamage(damageAmount);

        }
    }
}
