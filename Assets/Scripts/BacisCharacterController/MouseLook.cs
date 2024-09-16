using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// カメラ
/// </summary>
public class MouseLook : MonoBehaviour
{
    [Tooltip("マウス感度")] public float mouseSensitivity = 400f;
    private Transform playerBody; //プレイヤー位置
    private float yRotation = 0f; //上下回転

    private CharacterController characterController;
    [Tooltip("カメラの初期位置")]public float height = 1.8f;
    private float interpolationSpeed = 12f; //カメラ高さ変化の滑らかさ


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        playerBody = transform.GetComponentInParent<PlayerController>().transform;
        characterController = GetComponentInParent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation -= mouseY; //上下回転の値を累積
        yRotation = Mathf.Clamp(yRotation, -60f, 60f);//上下角度60度
        transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);//カメラ上下回転
        playerBody.Rotate(Vector3.up * mouseX);//プレイヤー左右移動

        //しゃがむ/立つときカメラの高さも変化させる
        float heightTarget = characterController.height * 0.9f;
        height = Mathf.Lerp(height, heightTarget, interpolationSpeed * Time.deltaTime);
        transform.localPosition = Vector3.up * height;
    }
}
