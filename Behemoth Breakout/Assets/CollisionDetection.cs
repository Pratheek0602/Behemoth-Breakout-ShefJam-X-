using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public PlayerController wc;
    public GameObject HitParticle;

    private void OnTriggerEnter(Collider other){
        if(other.tag == "Enemy" && wc.isAttacking){
            Debug.Log(other.name);
            other.GetComponent<Animator>().SetTrigger("Hit");
            Debug.Log("HITTINGGGGGGGGGGGGGGGGGGGGGGGGGG");

            Instantiate(HitParticle, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z),
            other.transform.rotation);

        }
    }
}
