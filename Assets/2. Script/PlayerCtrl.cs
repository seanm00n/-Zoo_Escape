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
    float rayDistance = 1f;
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
                rigid.AddForce(new Vector3(0, 300, 0)); //점프
                isActing = true;
                return;
            }
        }
        if(isActing && Physics.Raycast(transform.position, -transform.up, rayDistance, floorLayerMask)) {
            isActing = false;
        }
        //어디선가 isActing이 true로 멈춘다
        
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            return;
        }

        if (h != 0 && isActing != true ) {
            transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.Self); //move
            
            //rotate
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