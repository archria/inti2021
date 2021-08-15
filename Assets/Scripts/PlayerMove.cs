using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxSpeed;
    public float counterJumpPower;
    public float jumpPower;
    public Transform frontCheck;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    private void Start() {
        Debug.Log("start");        
    }

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        //Jump
        if(Input.GetButtonDown("Jump") && !anim.GetBool("isJumping")){
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping",true);
        }
        if(Input.GetButtonDown("Jump") && anim.GetBool("isWallStick")){
            rigid.AddForce(Vector2.left * counterJumpPower, ForceMode2D.Impulse);
            rigid.AddForce(Vector2.up* jumpPower, ForceMode2D.Impulse);
        }

        //Stop speed
        if(Input.GetButtonUp("Horizontal")) // up = 떼는거
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.5f , rigid.velocity.y);
        //directon sprite
        if(Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1; // 왼쪽일때 flip x
        
        //Animation
        if(Mathf.Abs( rigid.velocity.x ) < 0.3)
            anim.SetBool("isWalking", false);
        else
            anim.SetBool("isWalking", true);
        
        if(true){
            Debug.DrawRay(rigid.position, Vector3.right * Mathf.Abs(Input.GetAxisRaw("Horizontal")) , new Color(0,1,0));
            RaycastHit2D rayHitright = Physics2D.Raycast(rigid.position, Vector3.right, 1, LayerMask.GetMask("Platform"));
            if(rayHitright.collider != null && rayHitright.distance < 0.5f){
                Debug.Log("wall sticking");
                anim.SetBool("isWallStick", true);
            }
            else
                anim.SetBool("isWallStick",false);
        }

        


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");

        // move by key control
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse); // accelerator
        //rigid.velocity = new Vector2(maxSpeed*h, rigid.velocity.y); // fixed velocity

        //set Max speed
        if(rigid.velocity.x > maxSpeed) // right max speed
            rigid.velocity = new Vector2(maxSpeed, rigid.velocity.y);
        else if (rigid.velocity.x < maxSpeed * (-1)) // left max speed
            rigid.velocity = new Vector2(maxSpeed * (-1) ,rigid.velocity.y);

        //Landing Platform
        if(rigid.velocity.y < 0){
            Debug.DrawRay(rigid.position, Vector3.down, new Color(0,1,0));
            RaycastHit2D rayHitDown = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if(rayHitDown.collider != null){
                if(rayHitDown.distance < 0.5f){
                    anim.SetBool("isJumping", false);
            }
            //Debug.Log(rayHit.collider.name);
        }
        
        
        //wall jump

        }
        
        // master branch testing
        
    }
}
