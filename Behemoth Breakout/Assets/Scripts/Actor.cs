using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    int currentHealth;
    public int maxHealth;
    public AIController aiController; // Reference to the AIController script attached to the enemy

    public NewBehaviourScript enenyManager;
    

    void Awake()
    {
        currentHealth = maxHealth;
        enenyManager = FindObjectOfType<NewBehaviourScript>();
        aiController = FindObjectOfType<AIController>();
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
        if(enenyManager != null)
        {
            // Debug.Log("ctime to die");
            enenyManager.EnemyKilled();
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
