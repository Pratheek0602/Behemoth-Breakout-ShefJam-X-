using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;
    public AIController aiController; // Reference to the AIController script attached to the enemy
    

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log("currebt health"+currentHealth);

        if (currentHealth <= 0)
        { 
            Debug.Log("ctime to die");
            Death(); 
        }
    }

    private void Death()
    {
        if(aiController != null)
        {
            // Debug.Log("ctime to die");
            aiController.Die();
        }
        else{
            Debug.Log("oopds none");
        }
        // Death function
        // TEMPORARY: Destroy Object
        // Destroy(gameObject);
    }
}
