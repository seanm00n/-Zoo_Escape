using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour {

    [SerializeField] LayerMask floorLayerMask;
    [SerializeField] LayerMask portalLayerMask;

    int playerAP;
    const int playerDefaultAP = 1;
    const int playerBurningAP = 3;

    int playerHp;
    const int playerDefaultHp = 5;

    int playerStamina;
    const int playerDefaultStamina = 0;
    const int playerAfterBurningStamina = 30;
    const int playerMaxStamina = 100;
    
    Vector3 destinationPosision;
    Quaternion destinationRotation;

    Quaternion leftRotationH;
    Quaternion RightRotationH;
    Quaternion leftRotationV;
    Quaternion RightRotationV;

    float speed = 10f;
    float h; //Axis for Horizontal
    
    Animator animator;
    Rigidbody rigid;
    GameObject child; //index 0 GO

    [SerializeField] SphereCollider lhColl;
    [SerializeField] SphereCollider rhColl;
    [SerializeField] SphereCollider ltColl;
    [SerializeField] SphereCollider rtColl;

    bool isDie = false;
    bool isPortalEnter = false;
    bool isHorizontal = true;
    bool isCoolTime = false;
    bool isHit = false;
    
    void Start () {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        child = transform.GetChild(0).gameObject;

        playerAP = playerDefaultAP;
        playerHp = playerDefaultHp;
        playerStamina = playerDefaultStamina;

        RightRotationH =  Quaternion.Euler(0, 90, 0);
        leftRotationH = Quaternion.Euler(0, -90, 0);
        RightRotationV = Quaternion.Euler(0, 180, 0);
        leftRotationV = Quaternion.Euler(0, 0, 0);

        destinationPosision = Vector3.zero;
        destinationRotation = Quaternion.identity;

        lhColl.enabled = false;
        rhColl.enabled = false;
        ltColl.enabled = false;
        rtColl.enabled = false;

    }
    void Update () {
        if (isDie) { return; }
        Movement();
        Jumping();
        Attack();
        UsePortal();
        CheckStamina();
    }
    void Movement () { //need rotation
        h = Input.GetAxis("Horizontal");
        if (h < -0.1f) {//Left
            animator.SetBool("Move", true);
            transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.Self);
            if (!isCoolTime) {
                if (isHorizontal) {
                    child.transform.rotation = leftRotationH;
                } else {
                    child.transform.rotation = leftRotationV;
                }
            }
        } else if(h > 0.1f){//Right
            animator.SetBool("Move", true);
            transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.Self);
            if (!isCoolTime) {
                if (isHorizontal) {
                    child.transform.rotation = RightRotationH;
                } else {
                    child.transform.rotation = RightRotationV;
                }
            }
        } else {//None
            animator.SetBool("Move", false);
        }
    }
    void Jumping () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            bool isGround = Physics.Raycast(transform.position, Vector3.down, 1.0f, floorLayerMask);
            if (isGround) {
                animator.SetTrigger("Jump");
                rigid.AddRelativeForce(new Vector3(0, 500f, 0)); //jump. rotate 수정 필요
                Debug.Log("Player::Jump");
            }
        }
    }
    void Attack () {
        if (!isCoolTime) {
            if (Input.GetKeyDown(KeyCode.W)) {
                animator.SetTrigger("Attack_W");
                lhColl.enabled = true;
                Debug.Log("Player::Attack_W");
                isCoolTime = true;
                StartCoroutine(AttackCoolTime(1f, lhColl));
            }
            if (Input.GetKeyDown(KeyCode.E)) {
                animator.SetTrigger("Attack_E");
                rhColl.enabled = true;
                Debug.Log("Player::Attack_E");
                isCoolTime = true;
                StartCoroutine(AttackCoolTime(1f, rhColl));
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                animator.SetTrigger("Attack_S");
                ltColl.enabled = true;
                Debug.Log("Player::Attack_S");
                isCoolTime = true;
                StartCoroutine(AttackCoolTime(1.5f, ltColl));
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                animator.SetTrigger("Attack_D");
                rtColl.enabled = true;
                Debug.Log("Player::Attack_D");
                isCoolTime = true;
                StartCoroutine(AttackCoolTime(1.5f, rtColl));
            }
        }
    }
    IEnumerator AttackCoolTime (float time, SphereCollider coll) {
        yield return new WaitForSeconds(time);
        isCoolTime = false;
        coll.enabled = false;
    }
    void Die () {
        isDie = true;
        animator.SetBool("Die", true);
        Debug.Log("Player::Die");
        StopAllCoroutines();
    }
    void UsePortal () {
        if (Input.GetKeyDown(KeyCode.G)) {
            if (isPortalEnter) {
                transform.position = destinationPosision;
                transform.rotation = destinationRotation;
                isHorizontal = !isHorizontal;
                Debug.Log("Player::UsePortal move into " + destinationPosision);
            }
        }
    }
    public void AddStamina () {
        playerStamina += 14;
        Debug.Log("Player::GetStamina");
    }
    void CheckStamina () {
        if(playerStamina >= 100) {
            playerStamina = playerMaxStamina;
            StartCoroutine(Burning());
        }
    }
    IEnumerator Burning () {
        Debug.Log("Player::StartCoroutine::Burning");
        playerAP = playerBurningAP;
        yield return new WaitForSeconds(8.0f);
        playerAP = playerDefaultAP;
        playerStamina = playerAfterBurningStamina;
        Debug.Log("Player::Burning End");
    }
    public int GetPlayerAP () {
        return playerAP;
    }
    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Portal") { //포탈이면 좌표,회전값 복사
            isPortalEnter = true;
            Debug.Log("Player::Enter Portal");
            destinationPosision = other.transform.position;
            destinationRotation = other.transform.rotation;
        }
        if (other.gameObject.tag == "MonsterAttack") {
            if (!isHit) {
                int damage = other.GetComponentInParent<MonsterCtrl>().GetMonsterAP(); //리소스 너무 많이먹음
                other.enabled = false;
                isHit = true;
                StartCoroutine(Hit(damage));
            }
        }
    }
    IEnumerator Hit (int damage) {
        playerHp -= damage;
        Debug.Log("Player::Hit");
        if (playerHp < 0) Die();
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }
    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Portal") {
            isPortalEnter = false;
            Debug.Log("Player::Exit Portal");
        }
    }
}