using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerController : MonoBehaviourPun,IPunObservable
{
    //Component
    public Animator ani;
    public Rigidbody body;
    public Transform camTf; //カメラ

    //数値
    public int CurHp = 10;
    public int MaxHp = 10;
    public float MoveSpeed = 3.5f;

    public float H; //Horizon
    public float V; //Vertical
    public Vector3 dir; //移動方向

    public Vector3 offset; //カメラとプレイヤー間の偏移値 三人称用

    public float Mouse_X; //マウスX入力
    public float Mouse_Y; //マウスY入力
    public float scroll; //スクロール入力
    public float Angle_X; //X軸Rotation角度
    public float Angle_Y; //Y軸Rotation 角度

    public Quaternion camRotation; //カメラRotationのQuaternion

    public Gun gun; //銃のScript

    //音
    public AudioClip reloadClip;
    public AudioClip shootClip;

    public bool isDie = false;

    public Vector3 currentPos;
    public Quaternion currentRotation;

    void Start()
    {
        Angle_X = transform.eulerAngles.x;
        Angle_Y = transform.eulerAngles.y;
        ani = GetComponent<Animator>();
        body = GetComponent<Rigidbody>();
        gun = GetComponentInChildren<Gun>();
        camTf = Camera.main.transform;
        currentPos = transform.position;
        currentRotation = transform.rotation;
        if(photonView.IsMine)
        {
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Cursor.visible == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            //自分のキャラのみ操作できる
            if (photonView.IsMine)
            {
                if (isDie == true)
                {
                    return;
                }
                UpdatePosition();
                UpdateRotation();
                InputCtl();
            }
            else
            {
                UpdateLogic();
            }
        }
        
        
        
    }

    //ほかのプレイヤーのデータを更新
    public void UpdateLogic()
    {
        transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * MoveSpeed * 10);
        transform.rotation = Quaternion.Slerp(transform.rotation, currentRotation, Time.deltaTime * 500);
    }

    private void LateUpdate()
    {
        ani.SetFloat("Horizontal", H);
        ani.SetFloat("Vertical", V);
        ani.SetBool("isDie", isDie);
    }

    public void UpdatePosition()
    {
        H = Input.GetAxisRaw("Horizontal");
        V = Input.GetAxisRaw("Vertical");
        dir = camTf.forward * V + camTf.right * H;
        body.MovePosition(transform.position + dir * Time.deltaTime * MoveSpeed);
    }

    //Rotationの更新（同時にカメラのpositionのRotation値も設置）
    public void UpdateRotation()
    {
        Mouse_X = Input.GetAxisRaw("Mouse X");
        Mouse_Y = Input.GetAxisRaw("Mouse Y");
        scroll = Input.GetAxis("Mouse ScrollWheel");

        Angle_X = Angle_X - Mouse_Y;
        Angle_Y = Angle_Y + Mouse_X;

        Angle_X = ClampAngle(Angle_X, -60, 60);
        Angle_Y = ClampAngle(Angle_Y, -360, 360);

        camRotation = Quaternion.Euler(Angle_X, Angle_Y, 0);

        camTf.rotation = camRotation;

        offset.z += scroll;

        camTf.position = transform.position + camTf.rotation *  offset;

        transform.eulerAngles = new Vector3(0, camTf.eulerAngles.y, 0);
    }

    //キャラ操作
    public void InputCtl()
    {
        if(Input.GetMouseButtonDown(0))
        {
            //弾丸数判断
            if(gun.BulletCount > 0)
            {
                //Reloadアニメーション再生中ならFire出来ない
                if(ani.GetCurrentAnimatorStateInfo(1).IsName("Reload"))
                {
                    return;
                }
                gun.BulletCount--;
                Game.uiManager.GetUI<FightUI>("FightUI").UpdateBulletCount(gun.BulletCount);
                //Fire　Animationを再生
                ani.Play("Fire", 1, 0);

                StopAllCoroutines();
                StartCoroutine(AttackCo());
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            //Reload
            AudioSource.PlayClipAtPoint(reloadClip, transform.position); //Reloadの音を再生
            ani.Play("Reload");
            gun.BulletCount = 10;
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateBulletCount(gun.BulletCount);
        }
    }

    IEnumerator AttackCo()
    {
        //Delay0.1s後Fire
        yield return new WaitForSeconds(0.1f);

        //射撃音再生
        AudioSource.PlayClipAtPoint(shootClip, transform.position);

        //Raycast
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, Input.mousePosition.z));

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 10000, LayerMask.GetMask("Player")))
        {
            Debug.Log("Hit!");
            hit.transform.GetComponent<PlayerController>().GetHit();
        }

        photonView.RPC("AttackRpc", RpcTarget.All); //プレイヤー全員AttackRpc関数を実行する
    }

    [PunRPC]
    public void AttackRpc()
    {
        gun.Attack();
    }


    public void GetHit()
    {
        if(isDie == true)
        {
            return;
        }

        //全てのプレイヤーの負傷を同期する
        photonView.RPC("GetHitRPC", RpcTarget.All);
    }

    [PunRPC]
    public void GetHitRPC()
    {
        CurHp -= 1;
        if (CurHp <= 0)
        {
            CurHp = 0;
            isDie = true;
        }

        if(photonView.IsMine)
        {
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateBlood();

            if(CurHp == 0)
            {
                Invoke("gameOver", 3); //3s後失敗UIを表示
            }
        }

    }

    private void gameOver()
    {
        //マウスを表示する
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //失敗UIを表示する
        Game.uiManager.ShowUI<LossUI>("LossUI").onClickCallBack = OnReset;
    }

    //蘇生
    public void OnReset()
    {
        //マウス不可視
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        photonView.RPC("OnResetRPC", RpcTarget.All);
    }

    [PunRPC]
    public void OnResetRPC()
    {
        isDie = false;
        CurHp = MaxHp;
        if(photonView.IsMine)
        {
            Game.uiManager.GetUI<FightUI>("FightUI").UpdateHp(CurHp, MaxHp);
        }
    }

    //角度制限（-360~360）
    public float ClampAngle(float val, float min, float max)
    {
        if(val > 360)
        {
            val -= 360;
        }
        if(val < -360)
        {
            val += 360;
        }
        return Mathf.Clamp(val, min, max);
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (ani != null)
        {
            Vector3 angle = ani.GetBoneTransform(HumanBodyBones.Chest).localEulerAngles;
            angle.x = Angle_X;
            ani.SetBoneLocalRotation(HumanBodyBones.Chest, Quaternion.Euler(angle));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            //データを送る
            stream.SendNext(H);
            stream.SendNext(V);
            stream.SendNext(Angle_X);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            //データをもらう
            H = (float)stream.ReceiveNext();
            V = (float)stream.ReceiveNext();
            Angle_X = (float)stream.ReceiveNext();
            currentPos = (Vector3)stream.ReceiveNext();
            currentRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
