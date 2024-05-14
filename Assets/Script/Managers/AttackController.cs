using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [SerializeField]
    private int damage;
    [SerializeField]
    private bool isEnemy;
    private void OnTriggerStay2D(Collider2D col)
    {

        if (col.CompareTag ("Player"))
        {
            col.gameObject.GetComponent<NewController>().OnDamaged(transform.position);
            col.gameObject.GetComponent<LivingEntity>().HealthDown(damage);
            transform.gameObject.SetActive(false);

        }
    }

}
