using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TreeEditor;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour {
    public int speed = 10;
    public LayerMask floorLayerMask;
    public LayerMask portalLayerMask;

    enum State { GROUND, AIR };
    State state = State.GROUND;
    Rigidbody rigid;
    float rayDistance = 1f;
    float x;
    void Start () {
        rigid = GetComponent<Rigidbody>();
    }
    void Update () {
        CheckState();
        Move();
        CheckState();
        Jump();
    }
    void CheckState () {
        if (Physics.Raycast(transform.position, -transform.up, rayDistance, floorLayerMask)
             || Physics.Raycast(transform.position, -transform.up, rayDistance, portalLayerMask)) {
            state = State.GROUND; // �ٴڿ� ����
        } else {
            state =  State.AIR; // ���߿� ��
        }
    }
    void Jump () {
        if (state == State.GROUND) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                rigid.AddForce(new Vector3(0, 300, 0));
                //force to vector front (x, y)
            }
        }
    }
    void Move () {
        if (state == State.GROUND) {
            x = Input.GetAxis("Horizontal");
            Vector3 vec = new Vector3(0, 0, 0);
            vec += new Vector3(x * speed*transform.forward.x, 0f, 0f);
            rigid.velocity = vec;
            //������ ���ְ� ���� ��?
            //�̵� ����� �� �� Ű �Է����� �����ϱ�
            //velocity/addforce/translate
        }
    }
    void Rotate () {
        //�̱���
    }
    private void OnTriggerStay (Collider other) {
        Debug.Log("Portal");
        if (Input.GetKeyDown(KeyCode.G)) {
            Debug.Log("Pressed G Button");
            other.GetComponent<PortalCtrl>().MoveToTarget(transform);
        }
        //�����ϱ���� �ð��� �ɸ��µ�?
    }
}