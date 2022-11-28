using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using GameData;
using static UnityEngine.GraphicsBuffer;
using Newtonsoft.Json.Serialization;

public class PlayerCtrl : MonoBehaviour {

    public STATE state = STATE.IDLE;
    public LayerMask floorLayerMask;
    public LayerMask portalLayerMask;

    [SerializeField] float jumpSpeed = 500;

    const int maxStemina = 100;
    const int maxHp = 100;

    float speed = 10f;
    float h; //Axis for Horizontal

    int stemina;
    int hp;

    bool isActing = false;
    Rigidbody rigid;

    void Start () {
        rigid = GetComponent<Rigidbody>();
        stemina = maxStemina;
        hp = maxHp;
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
        if(collision.gameObject.tag == "Floor") isActing = false;
    }
    void MoveToTarget(Collider other) {
        PortalCtrl otherComp = other.GetComponent<PortalCtrl>();
        transform.position = otherComp.target.position;
        transform.rotation = otherComp.target.rotation;
    }
}