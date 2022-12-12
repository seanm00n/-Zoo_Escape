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
    const int playerBurningAP = 100;

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
    GameObject child;

    bool isDie = false;
    bool isPortalEnter = false;
    bool isHorizontal = true;
    
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
            if (isHorizontal) {
                child.transform.rotation = leftRotationH;
            } else {
                child.transform.rotation = leftRotationV;
            }
        } else if(h > 0.1f){//Right
            animator.SetBool("Move", true);
            transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.Self);
            if (isHorizontal) {
                child.transform.rotation = RightRotationH;
            } else {
                child.transform.rotation = RightRotationV;
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
        if (Input.GetKeyDown(KeyCode.W)) {
            animator.SetTrigger("Attack");
            Debug.Log("Player::Attack");
            //쿨타임 넣으려면 bool check 후 coroutine 실행
        }
    }
    void Die () {
        isDie = true;
        animator.SetBool("Die", true);
        Debug.Log("Player::Die");
        StopAllCoroutines();
    }
    void Hit (int damage) {
        Debug.Log("Player::Hit");
        playerHp -= damage;
        if (playerHp < 0) {
            Die();
        }
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
            int damage = other.GetComponent<MonsterCtrl>().GetMonsterAP();
            Hit(damage);
        }
    }
    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Portal") {
            isPortalEnter = false;
            Debug.Log("Player::Exit Portal");
        }
    }
}