using System.Collections;
using Unity.VisualScripting;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour
{
    enum MonsterState { Idle, Trace, Attack, Die };
    MonsterState monsterState = MonsterState.Idle;

    GameObject target;
    NavMeshAgent agent;
    Animator animator;
    PlayerCtrl player;
    Rigidbody rigid;

    int monsterAP = 1;
    int monsterHP;
    const int monsterDefaultHP = 5;
    const float attackDist = 3f;
    const float traceDist = 20f;
    bool isDie = false;

    private void Start () {
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = target.GetComponent<PlayerCtrl>();
        monsterHP = monsterDefaultHP;

        StartCoroutine(CheckMonsterState()); //no need update() func
        StartCoroutine(MonsterAction());
    }
    private void Update () {
        Debug.Log(monsterState);
    }
    IEnumerator CheckMonsterState () {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);
            float dist = Vector3.Distance(target.gameObject.transform.position, transform.position);
            if (dist <= attackDist) {
                monsterState = MonsterState.Attack;
            } else if (dist <= traceDist) {
                monsterState = MonsterState.Trace;
            } else {
                monsterState = MonsterState.Idle;
            }
        }
    }
    IEnumerator MonsterAction () {
        while (!isDie) {
            switch (monsterState) {
                case MonsterState.Idle:
                    agent.isStopped = true;
                    animator.SetBool("IsTrace", false);
                    break;
                case MonsterState.Trace:
                    agent.destination = target.gameObject.transform.position;
                    agent.isStopped = false;
                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsTrace", true);
                    break;
                case MonsterState.Attack:
                    agent.isStopped = true;
                    animator.SetBool("IsAttack", true);
                    break;
            }
            yield return null;
        }
    }
    private void OnCollisionEnter (Collision other) {
        if(other.gameObject.tag == "PlayerAttack") {
            int playerAP = player.GetPlayerAP();
            monsterHP -= playerAP;
            player.AddStamina();
            if (monsterHP <= 0) {
                Death();
            } else {
                animator.SetTrigger("IsHit");
            }
        }
        if (other.gameObject.tag == "DeadLine") {
            Destroy(gameObject);
        }
    }
    void Death () {
        StopAllCoroutines();
        isDie = true;
        monsterState = MonsterState.Die;
        agent.isStopped = true;
        animator.SetTrigger("IsDie");
        rigid.constraints = RigidbodyConstraints.None;//no freeze
        Vector3 flyVector = new Vector3(Random.Range(-1, 2), 1, Random.Range(-1, 2));// 랜덤하게
        rigid.AddForce(flyVector * 1000f, ForceMode.Impulse);//날리기
    }
    public int GetMonsterAP() { return monsterAP; }
}
