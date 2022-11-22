using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalCtrl : MonoBehaviour
{
    [SerializeField] Transform target;
    public void MoveToTarget (Transform tr) {
        tr.position = target.transform.position;
        tr.rotation = target.transform.rotation;
    }
}
