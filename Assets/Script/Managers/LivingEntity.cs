using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float ownHp;
    protected float hp;

    void Start()
    {
        hp = ownHp;
    }

    public void HealthDown(float damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        GameObject.Destroy(gameObject);
    }
}
