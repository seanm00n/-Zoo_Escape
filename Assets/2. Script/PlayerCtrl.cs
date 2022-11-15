using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCtrl : MonoBehaviour
{
    public float moveSpeed;

    Rigidbody rigid;
    private float x;
    Vector3 pVector;
    void Start()
    {
        pVector = new Vector3(0, 0, 0);
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Movement();
    }
    void Movement()
    {
        x = Input.GetAxis("Horizontal");
        pVector += new Vector3(x * moveSpeed, transform.position.y, transform.position.z);

        rigid.velocity = pVector;
    }
}
