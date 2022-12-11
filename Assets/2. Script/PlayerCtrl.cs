using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
//using GameData;
using static UnityEngine.GraphicsBuffer;
using Newtonsoft.Json.Serialization;

[System.Serializable]
public class Anim
{
    public AnimationClip idle;
    public AnimationClip jump;
    public AnimationClip Lhook;
    public AnimationClip Rhook;
    public AnimationClip Allhook;
    public AnimationClip Lkick;
    public AnimationClip Rkick;
}



public class PlayerCtrl : MonoBehaviour {
    public enum STATE { IDLE, WALK, JUMP, ATTACK_All, ATTACK_L, ATTACK_R, KICK_L, KICk_R, DIE, HIT };
    public STATE state = STATE.IDLE;

    private Animator animator;

    public LayerMask floorLayerMask;
    public LayerMask portalLayerMask;

    [SerializeField] float jumpSpeed = 500;

    const int maxStemina = 100;
    const int maxHp = 100;

    float speed = 10f;
    float h; //Axis for Horizontal

    int stemina;
    int hp;

    bool isActing = false;
    Rigidbody rigid;

    public Anim anim;
    public Animation _animation;
    
    void Start () {
        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        stemina = maxStemina;
        hp = maxHp;

        _animation = GetComponent<Animation>();
        _animation.clip = anim.idle;
        _animation.Play();
        // StartCoroutine(PlayerAction());
    }

   /* IEnumerator PlayerAction()
     {
       // Debug.Log("들어옴");
         while (!isActing)
         {
             switch (state)
             {
                 case STATE.IDLE:
                    animator.SetBool("IsWalk", false);
                   // animator.SetBool("IsJump", false);
                    break;
                 case STATE.WALK:
                    animator.SetBool("IsWalk", true);
                   // animator.SetBool("IsAttack_All", false);
                   // animator.SetBool("IsAttack_L", false);
                   // animator.SetBool("IsAttack_R", false);
                   // animator.SetBool("IsKICK_L", false);
                   // animator.SetBool("IsKICk_R", false);
                    break;
                 case STATE.JUMP:
                    animator.SetBool("IsJump", true);
                   // animator.SetBool("IsWalk", false);
                    break;
                 case STATE.ATTACK_All:
                    animator.SetBool("IsAttack_All", true);
                    break;
                 case STATE.ATTACK_L:
                    animator.SetBool("IsAttack_L", true);
                    break;
                case STATE.ATTACK_R:
                    animator.SetBool("IsAttack_R", true);
                    break;
                case STATE.KICK_L:
                    animator.SetBool("IsKICK_L", true);
                    break;
                case STATE.KICk_R:
                    animator.SetBool("IsKICk_R", true);
                    break;
                case STATE.DIE:
                    animator.SetBool("IsDIE", true);
                    break;
                case STATE.HIT:
                    animator.SetBool("IsHIT", true);
                    break;
                    //  KICK_L, KICk_R, DIE, HIT
            }
            yield return new WaitForSeconds(0.2f);
         }
     }
    */
    void Update () {
        Movement();
        if(isActing) Debug.Log("Acting");
    }
    void Movement () {
        h = Input.GetAxis("Horizontal");
        //state = STATE.WALK;

        if ((Input.GetKeyDown(KeyCode.A)) || (Input.GetKeyDown(KeyCode.D)))
        {
            state = STATE.WALK;
        }
        else if ((Input.GetKeyUp(KeyCode.A)) || (Input.GetKeyUp(KeyCode.D)))
        {
            state = STATE.IDLE;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            state = STATE.JUMP;
            if (!isActing) {
                // _animation.CrossFade(anim.jump.name);
               // state = STATE.JUMP;
                rigid.AddRelativeForce(new Vector3(jumpSpeed, jumpSpeed, 0)); //jump. rotate 수정 필요
                isActing = true;
            }
        }

        if (h != 0 && isActing == false ) {//점프 시 이동 잠금
            transform.Translate(Vector3.right * h * speed * Time.deltaTime, Space.Self); //move
            //add rotate
        }
    }
    private void OnTriggerStay (Collider other) {
        if(other.gameObject.tag == "Portal") {
            if (Input.GetKeyDown(KeyCode.G)) {
                Debug.Log("Pressed G Button");
                MoveToTarget(other);
            }
        }
    }
    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag == "Floor") isActing = false;
    }
    void MoveToTarget(Collider other) {
        PortalCtrl otherComp = other.GetComponent<PortalCtrl>();
        transform.position = otherComp.target.position;
        transform.rotation = otherComp.target.rotation;
    }
}