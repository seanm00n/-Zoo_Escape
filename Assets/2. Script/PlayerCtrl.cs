using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {
    public float speed = 10f;
    public float rotSpeed = 100f;
    public LayerMask floorLayerMask;
    public LayerMask portalLayerMask;

    Rigidbody rigid;
    void Start () {
        rigid = GetComponent<Rigidbody>();
    }
    void FixedUpdate () {
        Movement();
    }
    void Movement () {
        float h = Input.GetAxis("Horizontal");
        if (h != 0) {
            transform.Translate(Vector3.right*h*speed*Time.deltaTime, Space.Self); //완성!
            //transform.rotation = Quaternion.Slerp(transform.rotation, Vector3.up * h, 100f);
            //Vector3.up 활용 해보자!
            transform.Rotate(Vector3.up*h*rotSpeed);

        }
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            //force to vector front (x, y) to jump
        }
    }
    private void OnTriggerStay (Collider other) {
        Debug.Log("Portal");
        if (Input.GetKeyDown(KeyCode.G)) {
            Debug.Log("Pressed G Button");
            other.GetComponent<PortalCtrl>().MoveToTarget(transform);
        }
        //need frame check
    }
}