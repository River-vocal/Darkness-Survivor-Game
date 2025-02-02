using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBallBossAttack : MonoBehaviour
{

    public Boss boss;
    
    [Header("Attack Parameters")]
    [SerializeField] private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private  float attackCooldown;
    [SerializeField] private int attackDamage;
    [SerializeField] private Vector3 attackOffset;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask attackMask;
    
    [Header("Fire Ball Attack")] 
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] fireballs;
    
    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private CapsuleCollider2D capsuleCollider;

    public void FireballAttack()
    {
        
        fireballs[FindFireball()].GetComponent<EnemyProjectile>().ActivateProjectile(attackDamage, boss.bossIsFlipped);
        
    }

    private int FindFireball()
    {
        // bossIsFlipped: boss face left
        // !bossIsFlipped: boss face right

        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
            {
                fireballs[i].transform.position = firepoint.position;
                return i;
                
            }
        }

        return 0;
    }
    // help to see the attack circle range
    // The white circle of the object
    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Gizmos.DrawWireSphere(pos, attackRange);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(capsuleCollider.bounds.center + transform.right * attackRange * transform.localScale.x * colliderDistance,
            new Vector3(capsuleCollider.bounds.size.x * attackRange, capsuleCollider.bounds.size.y, capsuleCollider.bounds.size.z));
    }
}