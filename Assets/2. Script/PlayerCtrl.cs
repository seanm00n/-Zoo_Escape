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
        Vector3 lookDirection;
        if (Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.RightArrow) ||
            Input.GetKey(KeyCode.UpArrow) ||
            Input.GetKey(KeyCode.DownArrow))
        {
            float xx = Input.GetAxisRaw("Vertical");
            float zz = Input.GetAxisRaw("Horizontal");
            lookDirection = xx * Vector3.forward + zz * Vector3.right;

            this.transform.rotation = Quaternion.LookRotation(lookDirection);
            this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        //Vector3 dir = h * Vector3.right;
        //if (h != 0) {
        //    transform.Translate((h > 0 ? Vector3.forward : -Vector3.forward) * h * speed * Time.deltaTime, Space.Self); //¿Ï¼º!
        //    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10f);
        //    transform.rotation = Quaternion.LookRotation(dir, -dir);
        //    transform.Rotate(h > 0 ? Vector3.up : Vector3.down);
        //}
        
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