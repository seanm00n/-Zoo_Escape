using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    Transform target;
    public float dist = 3.0f;
    public float height = 3.0f;
    public float dampTraec = 20.0f;
    public float targetHeight = 2.0f;
    private void Start() {
        target = GameObject.Find("Player").transform;
    }

    private void LateUpdate () {
        //LateUpdate는 Update보다 적은 빈도로 호출됨
        transform.position = Vector3.Lerp(transform.position,
                                   target.position - (target.forward * dist) + (Vector3.up * height),
                                   Time.deltaTime * dampTraec);
        transform.LookAt(new Vector3(0, targetHeight, 0) + target.position);
    }
}
