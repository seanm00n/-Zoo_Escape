using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using GameData;
using static UnityEngine.GraphicsBuffer;

public class PlayerCtrl : MonoBehaviour {
    
    public float speed = 10f;
    public float rotSpeed = 100f;
    public LayerMask floorLayerMask;
    public LayerMask portalLayerMask;

    private float h;

    Rigidbody rigid;
    void Start () {
        rigid = GetComponent<Rigidbody>();
    }
    void FixedUpdate () {
        Movement();
    }
    void Movement () {
        h = Input.GetAxis("Horizontal");
        if (h != 0) {
            transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.Self); //move
            //rotate
        }

        if (Input.GetKeyDown(KeyCode.Space)) { //force vector3(x, y) to jump
        }

        if (Input.GetKeyDown(KeyCode.W)) //attack
        {

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