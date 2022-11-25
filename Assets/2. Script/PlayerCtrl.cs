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

    const int maxStemina = 100;
    const int maxHp = 100;

    float speed = 10f;
    float rayDistance = 1.05f;
    float h;

    int stemina = 0;
    int hp;

    bool isActing = false;
    Rigidbody rigid;

    void Start () {
        rigid = GetComponent<Rigidbody>();
    }
    void Update () {
        Movement();
        if(isActing) Debug.Log("Acting");
    }
    void Movement () {
        h = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space)) { //space 입력 했을 때
            if (!isActing) { //바닥에 있으면
                rigid.AddForce(new Vector3(300, 500, 0)); //점프
                isActing = true;
            }
        }
        Debug.DrawRay(transform.position,-transform.up,Color.red,rayDistance);
        if(isActing && Physics.Raycast(transform.position, -transform.up, rayDistance, floorLayerMask)) {
            isActing = false; //너무 빨리 작동해서 이동이 동작 가능해져 버림, oncoll함수로 변경
        }
        
        if (Input.GetKeyDown(KeyCode.W))   {
            return;
        }

        if (h != 0 && isActing != true ) { //점프중 이동되는 버그
            transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.Self); //move
            
            //add rotate
        }
    }
    private void OnTriggerStay (Collider other) {
        if(other.gameObject.tag == "Portal")
        {
            if (Input.GetKeyDown(KeyCode.G)) {
                Debug.Log("Pressed G Button");
                MoveToTarget(other);
            }
        }
    }
    void MoveToTarget(Collider other)
    {
        PortalCtrl otherComp = other.GetComponent<PortalCtrl>();
        transform.position = otherComp.target.position;
        transform.rotation = otherComp.target.rotation;
    }
}