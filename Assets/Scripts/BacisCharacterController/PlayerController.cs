using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

/// <summary>
/// Player movement
/// </summary>
public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    public Vector3 moveDirection; //移動方向
    private AudioSource audioSource;

    [Header("Player data")]
    public float Speed;
    [Tooltip("歩き速度")] public float walkSpeed;
    [Tooltip("走る速度")] public float runSpeed;
    [Tooltip("しゃがみ歩き速度")] public float crouchSpeed;
    [Tooltip("Player MaxHP")] public const float playerMaxHealth = 100;
    [Tooltip("Player HP")] public float playerHealth;

    [Tooltip("ジャンプの力")] public float jumpForce;
    [Tooltip("落下の力")] public float fallForce;
    [Tooltip("しゃがむときのプレイヤーの高さ")]public float crouchHeight;
    [Tooltip("立つときのプレイヤーの高さ")]public float standHeight;


    [Header("キーボード入力")]
    [Tooltip("ダッシュ")] public KeyCode runInputName = KeyCode.LeftShift;
    [Tooltip("ジャンプ")] public KeyCode jumpInputName = KeyCode.Space;
    [Tooltip("しゃがみ")] public KeyCode crouchInputName = KeyCode.LeftControl;

    [Header("状態判断")]
    public MovementState state;
    private CollisionFlags collisionFlags;
    public bool isWalk; //歩いているかどうか
    public bool isRun;  //走っているかどうか
    public bool isJump;  //ジャンプしているかどうか
    public bool isGround;  //ジャンプしているかどうか
    public bool isCanCrouch; //しゃがめるかどうか
    public bool isCrouching; //しゃがんでいるかどうか
    public bool playerIsDead;
    public bool isDamaged;

    public LayerMask crouchLayerMask;
    public TMP_Text playerHealthUI;

    [Header("音効")]
    [Tooltip("歩き音効")] public AudioClip walkSound;
    [Tooltip("走る音効")] public AudioClip runSound;





    void Start() 
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        walkSpeed = 4f;    
        runSpeed = 6f;    
        crouchSpeed = 2f;
        jumpForce = 0f;
        fallForce = 10f;
        crouchHeight = 1f;
        playerHealth = 100f;
        standHeight = characterController.height;  
        playerHealthUI.text = "HP:" + playerHealth;
    }

    void Update() 
    {
        CanCrouch();
        if(Input.GetKey(crouchInputName))
        {
            Crouch(true);
        }
        else
        {
            Crouch(false);
        }
        Jump();
        //PlayerFootSoundSet();
        Moving();    
    }

    /// <summary>
    /// 移動
    /// </summary>
    public void Moving()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        isRun = Input.GetKey(runInputName) && (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0);
        isWalk = !isRun && (Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0);
        if(isRun && isGround && !isCrouching)//走る
        {
            state = MovementState.running;
            Speed = runSpeed;
        }
        else if(isGround && !isCrouching) //歩き
        {
            state = MovementState.walking;
            Speed = walkSpeed;
            
        }
        else if(isGround && isCrouching)//しゃがみ歩き
        {
            state = MovementState.crouching;
            Speed = crouchSpeed;
        }

        //移動方向(normalized：斜めで移動したら速度変化しない)
        moveDirection = (transform.right * h + transform.forward * v).normalized;
        characterController.Move(moveDirection * Speed * Time.deltaTime); //移動
    }

    /// <summary>
    /// ジャンプ
    /// </summary>
    public void Jump()
    {
        if(!isCanCrouch) return;
        isJump = Input.GetKeyDown(jumpInputName);
        //ジャンプキー押したかつ地面にいるときジャンプができる
        if(isJump && isGround)
        {
            isGround = false;
            jumpForce = 5f; 
        }
        else if(!isJump && isGround)
        {
            isGround = false;
        }

        //地面にいないとき
        if(!isGround)
        {
            jumpForce -= fallForce * Time.deltaTime; //毎秒jumpForceを累減し落下させる
            Vector3 jump = new Vector3(0, jumpForce * Time.deltaTime, 0); //jumpForceをV3座標に変換
            collisionFlags = characterController.Move(jump); //characterControllerを使ってジャンプする

            if(collisionFlags == CollisionFlags.Below)
            {
                isGround = true;
                jumpForce = -2f;
            }
        }
    }

    /// <summary>
    /// しゃがめるかどうかを判断する
    /// </summary>
    public void CanCrouch()
    {
        //プレイヤー頭の位置
        Vector3 sphereLocation = transform.position + Vector3.up * standHeight;
        //頭の上に物体の存在によってしゃがめるかを判断する
        isCanCrouch = (Physics.OverlapSphere(sphereLocation, characterController.radius, crouchLayerMask).Length) == 0;
    }

    public void Crouch(bool newCrouching)
    {
        if(!isCanCrouch) return; //しゃがめないとき（トンネルの中）、立てない
        isCrouching = newCrouching;
        characterController.height = isCrouching ? crouchHeight : standHeight;
        characterController.center = characterController.height/ 2.0f * Vector3.up;
    }

    /// <summary>
    /// 移動音効
    /// </summary>
    public void PlayerFootSoundSet()
    {
        if(isGround && moveDirection.sqrMagnitude > 0)
        {
            audioSource.clip = isRun ? runSound : walkSound;
            if(!audioSource.isPlaying)
            {
                //足音を再生する
                audioSource.Play();
            }
        }
        else
        {
            if(audioSource.isPlaying)
            {
                //足音を停止する
                audioSource.Pause();
            }
        }
        //しゃがみ歩きの時再生しない
        if(isCrouching)
        {
            if(audioSource.isPlaying)
            {
                //足音を停止する
                audioSource.Pause();
            }
        }
    }

    public void PlayerDamaged(float damage)
    {
        playerHealth -= damage;
        isDamaged = true;
        playerHealthUI.text = "HP:" + (int)playerHealth;
        if(playerHealth <= 0)
        {
            playerIsDead = true;
            playerHealthUI.text = "Died";
            Time.timeScale = 0;
        }
    }

    public enum MovementState
    {
        walking,
        running,
        crouching,
        idle
    }   

}
