using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LivingEntity : MonoBehaviour
{
    public float ownHp;
    public float hp;
    private Rigidbody2D rig;
    private MeshRenderer meshRenderer;

    void Awake()
    {
        hp = ownHp;
        rig = GetComponent<Rigidbody2D>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void OnDamaged()
    {
        rig.velocity = Vector2.zero;
        meshRenderer.materials[0].color = new Color(1, 1, 1, 0.4f);
        Invoke("Regain", 0.1f);
    }

    private void Regain()
    {
        meshRenderer.materials[0].color = new Color(1, 1, 1, 1f);
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
