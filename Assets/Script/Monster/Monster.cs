using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Monster : MonoBehaviour
{
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3f;
    public GameObject bulletPrefab;
    MeshRenderer meshRenderer;
    BoxCollider2D boxCollider;
    Animator anim;
    private Transform target;
    private float dirc;
    private float dis;
    private float spawnRate = 5f;
    private float timeAfterSpawn;
    void Start()
    {
        timeAfterSpawn = 0f;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        target = FindObjectOfType<PlayerController>().transform;
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        dis = Vector3.Distance(transform.position, target.position);

        if (dis <= 10)
        {
            anim.SetBool("isAttack", true);
            Attack();
        }
        else
        {
            anim.SetBool("isAttack", false);
        }
    }


    private void Attack()
    {
        timeAfterSpawn += Time.deltaTime;

        if(timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0f;

            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.transform.LookAt(target);
            //spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
        
    }
}
