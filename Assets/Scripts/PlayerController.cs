using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector3 moveDirection;

    public float gravity;
    public float speed;
    public float amountRotate;
    public float speedJump;


    //GameObject[] targets = new GameObject[5];
    IsRendered[] targets = new IsRendered[5];
    SaveImage save = new SaveImage();
    [SerializeField] SoundController soundController;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;

        for (int i = 0; i < targets.Length; i++)
        {
         
            //targets[i] = new IsRendered(i);
            //targets[i] = GameObject.Find("target" + (i+1)).GetComponent<IsRenderded>();
        }
    }

    void Update()
    {

        //Inputの検知->0未満となってバックすることを避ける
        if (Input.GetAxis("Vertical") != 0.0f)
        {
            moveDirection.z = Input.GetAxis("Vertical") * speed;
        }
        else
        {
            moveDirection.z = 0;
        }

        //方向転換
        transform.Rotate(0, Input.GetAxis("Horizontal") * amountRotate, 0);
        //地上にいる場合のみ操作を行う
        if (controller.isGrounded)
        {
            //ジャンプ
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = speedJump;
            }
        }
        //毎フレーム重力を加算
        moveDirection.y -= gravity * Time.deltaTime;

        //移動の実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //移動後地面についてるなら重力をなくす
        if (controller.isGrounded)
        {
            moveDirection.y = 0;
        }


        //キーボードの「s」を押したら画像を保存
        if (Input.GetKeyDown(KeyCode.Z))
        {
            save.SaveCameraImage();
            for (int i = 0; i < targets.Length; i++)
            {
                targets[i].Renderedcheck(); 
            }

           
            soundController.PlaySE(SoundController.Sound.camera);

        }
    }
}