using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using Newtonsoft.Json.Serialization;

public class PlayerCtrl : MonoBehaviour {
    enum STATE { IDLE, RUN, JUMP, ATTACK_A, ATTACK_B, ATTACK_C, DIE, HIT };

    STATE state = STATE.IDLE;

    Rigidbody rigid;

    public LayerMask floorLayerMask;
    public LayerMask portalLayerMask;
    [HideInInspector] public int playerAP = 1; //const
    int playerHp;
    const int maxStemina = 100;
    const int playerDefaultHp = 5;
    float jumpSpeed = 500;
    float speed = 10f;
    float h; //Axis for Horizontal
    int stemina;
    
    bool isActing = false;
    
    void Start () {
        rigid = GetComponent<Rigidbody>();
        stemina = maxStemina;
        playerHp = playerDefaultHp;
    }
    void Update () {
        Movement();
        if(isActing) Debug.Log("Acting");
    }
    void Movement () {
        h = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!isActing) {
                rigid.AddRelativeForce(new Vector3(jumpSpeed, jumpSpeed, 0)); //jump. rotate 수정 필요
                isActing = true;
            }
        }

        if (h != 0 && isActing == false ) {//점프 시 이동 잠금
            transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.Self); //move
            //add rotate
        }
    }
    private void OnTriggerStay (Collider other) {
        if(other.gameObject.tag == "Portal") {
            if (Input.GetKeyDown(KeyCode.G)) {
                Debug.Log("Pressed G Button");
                MoveToTarget(other);
            }
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Floor") isActing = false; //연속 점프, 점프중 이동 방지
    }
    void MoveToTarget(Collider other) {
        PortalCtrl otherComp = other.GetComponent<PortalCtrl>();
        transform.position = otherComp.target.position;
        transform.rotation = otherComp.target.rotation;
    }
}