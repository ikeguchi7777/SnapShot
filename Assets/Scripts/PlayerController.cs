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

    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;
    }

    void Update()
    {
      
            //Inputの検知->0未満となってバックすることを避ける
            if (Input.GetAxis("Vertical") > 0.0f)
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
    }
}