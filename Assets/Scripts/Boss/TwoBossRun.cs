using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TwoBossRun : StateMachineBehaviour
{
    Transform playerTransform;
    Transform bossTransform;
    Rigidbody2D rb2;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float attackRange = 2.5f;
    private TwoBoss boss;
    private GameObject healthBar;
    private GameObject healthBar1;
    private GameObject healthBar2;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        bossTransform = animator.GetComponent<Transform>();
        rb2 = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<TwoBoss>();

        healthBar1 = GameObject.Find("Boss Health Bar");
        healthBar2 = GameObject.Find("Boss Health Bar 2");
        if (boss.name == "Boss1") {
            healthBar = healthBar1;
        } else if (boss.name == "Boss2") {
            healthBar = healthBar2;
        }
    }

    
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.lookAtPlayer();
        if (Math.Abs(playerTransform.position.x - rb2.position.x) <= attackRange)
        {
            animator.SetTrigger("Attack");
            return;
        }
        Vector2 targetPosition = new Vector2(playerTransform.position.x, rb2.position.y);
        Vector2 newPosition = Vector2.MoveTowards(rb2.position, targetPosition, speed * Time.deltaTime);
        rb2.MovePosition(newPosition);


        Vector3 pos = bossTransform.position;
        if (boss.name == "Boss1") {
            pos.y += 2.0f;
        } else if (boss.name == "Boss2") {
            pos.y += 3.0f;
        }
        healthBar.transform.position = pos;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
