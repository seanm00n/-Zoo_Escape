using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Transform canvas;

    RectTransform hpBarTf;

    public GameObject prfHpBar;

    public float hpBarHeight = 1.7f;
   
    void Start()
    {
        hpBarTf = Instantiate(prfHpBar, canvas).GetComponent<RectTransform>();
    }

    private void Update()
    {
        Vector3 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y + hpBarHeight, transform.position.z));
        hpBarTf.position = _hpBarPos;
    }


}
