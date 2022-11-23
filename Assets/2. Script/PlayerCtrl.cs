using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {
    public int speed = 10;
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
        Vector3 vec = new Vector3(h, 0, 0);
        if (h != 0) {
            transform.Translate(Vector3.right*h*speed*Time.deltaTime, Space.Self); //¿Ï¼º!
            //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(vec), 100f);
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