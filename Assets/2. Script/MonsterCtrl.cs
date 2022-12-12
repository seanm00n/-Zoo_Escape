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

    [SerializeField] CapsuleCollider monsterAttackColl;
    
    const int monsterAP = 1;

    int monsterHP;
    const int monsterDefaultHP = 3;
    const float attackDist = 3f;
    const float traceDist = 20f;
    bool isDie = false;
    bool isAttack = false;

    private void Start () {
        target = GameObject.Find("Player");
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        player = target.GetComponent<PlayerCtrl>();
        
        monsterHP = monsterDefaultHP;
        monsterAttackColl.enabled = false;
        StartCoroutine(CheckMonsterState()); //no need update() func
        StartCoroutine(MonsterAction());
    }
    IEnumerator CheckMonsterState () {
        while (!isDie)
        {
            yield return new WaitForSeconds(1f);
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
                    monsterAttackColl.enabled = false;
                    break;
                case MonsterState.Trace:
                    agent.destination = target.gameObject.transform.position;
                    agent.isStopped = false;
                    animator.SetBool("IsAttack", false);
                    animator.SetBool("IsTrace", true);
                    monsterAttackColl.enabled = false;
                    break;
                case MonsterState.Attack:
                    if (!isAttack) {
                        agent.isStopped = true;
                        animator.SetBool("IsAttack", true);
                        monsterAttackColl.enabled = true;
                        isAttack = true;
                        StartCoroutine(CoolTime());
                    }
                    break;
            }
            yield return null;
        }
    }
    IEnumerator CoolTime () {
        yield return new WaitForSeconds(1f);
        isAttack = false;
    }
    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "PlayerAttack") {
            int playerAP = player.GetPlayerAP();
            monsterHP -= playerAP;
            other.enabled = false;
            Debug.Log("Monster::Hit");
            player.AddStamina();
            if (monsterHP <= 0) {
                Death();
            } else {
                animator.SetTrigger("IsHit");
            }
        }
    }
    void Death () {
        StopAllCoroutines();
        isDie = true;
        monsterState = MonsterState.Die;
        agent.enabled = false;
        animator.SetTrigger("IsDie");
        GetComponent<SphereCollider>().enabled = false;
        rigid.constraints = RigidbodyConstraints.None;//no freeze
        rigid.isKinematic = false;
        Vector3 flyVector = new Vector3(Random.Range(-1, 2), 2f, Random.Range(-1, 2));// 랜덤하게
        rigid.AddForce(flyVector * 30f, ForceMode.Impulse);//날리기
        Destroy(gameObject,2f);
        Debug.Log("Monster::Die");
    }
    public int GetMonsterAP() { return monsterAP; }
}
