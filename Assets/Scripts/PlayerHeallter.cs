using UnityEngine;
using UnityEngine.UI;

public class PlayerHeallter : MonoBehaviour
{
    public float health;
    public Scrollbar healthscroll;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        health -= damage;

        healthscroll.size = health / 100;
        GetComponent<Animator>().SetTrigger(damage);
        if(health <= 0)
        {
            print("die");
        }
    }
}
