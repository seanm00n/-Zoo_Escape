using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    public Text message;
    public Text result;
    public Image img;
    public Button button_get;
    public Button button_kill;
    public Button button_reset;
    public Button button_plus;
    public Button button_minus;

    private float hp;
    private float hp_max = 100;
    private float hp_delta = 20;
    private bool isDead;



    void Start()
    {

        Init_HP();
        SetFunction_UI();
    }

    //CodeFinder
    //From https://codefinder.janndk.com/
    private void Init_HP()
    {
        Function_Button_Reset();
    }

    private void ResetFunction_UI()
    {
        button_get.onClick.RemoveAllListeners();
        button_kill.onClick.RemoveAllListeners();
        button_reset.onClick.RemoveAllListeners();
        button_plus.onClick.RemoveAllListeners();
        button_minus.onClick.RemoveAllListeners();

        //Fill Amount Type
        img.type = Image.Type.Filled;
        img.fillMethod = Image.FillMethod.Horizontal;
        img.fillOrigin = (int)Image.OriginHorizontal.Left;
    }

    private void SetFunction_UI()
    {
        //Reset
        ResetFunction_UI();
        button_get.onClick.AddListener(Function_Button_Get);
        button_kill.onClick.AddListener(Function_Button_Kill);
        button_reset.onClick.AddListener(Function_Button_Reset);
        button_plus.onClick.AddListener(Function_Button_Plus);
        button_minus.onClick.AddListener(Function_Button_Minus);
    }

    private void Function_Button_Get()
    {
        string txt = string.Format("{0}/{1}", hp, hp_max);
        result.text = hp.ToString();
        Debug.LogError("Get Current HP!\n" + txt);
    }
    private void Function_Button_Kill()
    {
        Set_HP(0);
    }
    private void Function_Button_Reset()
    {
        Set_HP(hp_max);
    }
    private void Function_Button_Plus()
    {
        Change_HP(hp_delta);
    }
    private void Function_Button_Minus()
    {
        Change_HP(-hp_delta);
    }


    private void Change_HP(float _value)
    {
        hp += _value;
        Set_HP(hp);
    }

    private void Set_HP(float _value)
    {
        hp = _value;

        string txt = "";
        if (hp <= 0)
        {
            hp = 0;
            txt = "Dead";
        }
        else
        {
            if (hp > hp_max)
                hp = hp_max;
            txt = string.Format("{0}/{1}", hp, hp_max);
        }
        img.fillAmount = hp / hp_max;
        isDead = hp.Equals(0);

        message.text = txt;
        Debug.LogError("Current HP!\n" + txt);
    }

}
