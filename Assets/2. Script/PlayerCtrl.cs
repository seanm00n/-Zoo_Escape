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
    public LayerMask floorLayerMask;
    public LayerMask portalLayerMask;
    [HideInInspector] public int playerAP = 1; //const
    const int playerDefaultHp = 5;
    int playerHp;
    Vector3 destinationPosision;
    Quaternion destinationRotation;
    //const int maxStemina = 100;
    
    float speed = 10f;
    float h; //Axis for Horizontal
    //int stemina;
    Animator animator;
    Rigidbody rigid;
    bool isDie = false;
    bool isPortalEnter = false;
    
    void Start () {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerHp = playerDefaultHp;
        //stemina = maxStemina;
        destinationPosision = Vector3.zero;
        destinationRotation = Quaternion.identity;
    }
    void Update () {
        if (isDie) { return; }
        Movement();
        Jumping();
        Attack();
        UsePortal();
        //rotation
    }
    void Movement () {
        h = Input.GetAxis("Horizontal");
        if (h < -0.1f) {//Left
            animator.SetBool("Move", true);
            transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.Self);
        } else if(h > 0.1f){//Right
            animator.SetBool("Move", true);
            transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.Self);
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
            }
        }
    }
    void Attack () {
        if (Input.GetKeyDown(KeyCode.W)) {
            animator.SetTrigger("Attack");
        }
    }
    void Die () {
        isDie = true;
        animator.SetBool("Die", true);
    }
    void Hit (int damage) {
        Debug.Log("Player Hit");
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
            }
        }
    }
    private void OnTriggerEnter (Collider other) {
        if(other.gameObject.tag == "Portal") { //포탈이면 좌표,회전값 복사
            isPortalEnter = true;
            destinationPosision = other.transform.position;
            destinationRotation = other.transform.rotation;
        }
        if(other.gameObject.tag == "MonsterAttack") {
            int damage = other.GetComponent<MonsterCtrl>().monsterAP;
            Hit(damage);
        }
    }
    private void OnTriggerExit (Collider other) {
        if (other.gameObject.tag == "Portal") isPortalEnter = false;
    }
}