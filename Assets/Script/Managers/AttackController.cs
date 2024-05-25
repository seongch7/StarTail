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
        if (col != null)
        {
            switch (col.gameObject.layer)
            {
                case 6:
                    col.gameObject.GetComponent<LivingEntity>().OnDamaged();
                    col.gameObject.GetComponent<LivingEntity>().HealthDown(damage);
                    transform.gameObject.SetActive(false);
                    break;
                case 7:
                    col.gameObject.GetComponent<NewController>().OnDamaged(transform.position);
                    col.gameObject.GetComponent<LivingEntity>().HealthDown(damage);
                    transform.gameObject.SetActive(false);
                    break;
            }
        }
    }

}
