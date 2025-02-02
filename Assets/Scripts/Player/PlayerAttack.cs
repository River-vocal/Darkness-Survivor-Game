using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAttack : MonoBehaviour
{

    public int attackDamage;


    public Vector3 attackOffset;   // Adjust and set offset in Unity
    // It could avoid boss damaged when play stands on the head of the boss. 
    public float attackRange = 2f;
    public LayerMask attackMask;
    public PlayerController player;

    public GameObject bulletPrefab;
    public Transform firePoint;

    [SerializeField] protected AudioInfoBroadcaster audioInfoBroadcaster;

    [SerializeField] protected AudioInfoBroadcaster.AudioBroadcastValueType effectorType;

    [SerializeField] protected float effectorValue;

    // Animation Event
    public void Attack()
    {

        player.LockMovement();
        attackDamage = player.attackDamage;


        Vector3 pos = transform.position;

        // Make sure the attack circle is in front of the player

        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;
        effectorValue = audioInfoBroadcaster.GetEffectorValue(effectorType);
        // if heavy beats detected
        // Player can only fire bullet start from the second tutorial
        int tutorial2LevelIndex = 4;
        bool inHeavyNotes = effectorValue > 0.2f;
        if (SceneManager.GetActiveScene().buildIndex >= tutorial2LevelIndex && inHeavyNotes)
        // if (true)
        {

            // attack with bullet
            if (player.GetBulletCount() > 0)
            {
                Debug.Log("Fire bullets !!!!!!!!");
                TriggerScreenShake();
                Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                player.DecreaseBullet();
                return;
            }

            Debug.Log("BulletCount == 0 !!!!!!!!");
        }

        // normal attack
        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            if (inHeavyNotes)
            {
                TriggerScreenShake();
            }
            colInfo.GetComponent<Health>().CurHealth -= attackDamage;

            Debug.Log("Player Attack");
        }
    }
    // Animation Event
    public void EndAttack()
    {
        player.UnlockMovement();
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

    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Start()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void TriggerScreenShake()
    {
        cinemachineImpulseSource.GenerateImpulse();
    }

}