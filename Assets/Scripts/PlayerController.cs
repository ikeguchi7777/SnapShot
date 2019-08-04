using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Vector3 moveDirection;

    [SerializeField] int id;
    [SerializeField] float gravity;
    [SerializeField] float speed;
    [SerializeField] float amountRotate;
    [SerializeField] float speedJump;

    [SerializeField] CharacterController controller;
    [SerializeField] GameObject[] Target;// { get; set;}
    [SerializeField] GameObject[] Enemy;//{ get; set;}
    //public GameObject[] GetEnemy() { return Enemy; }
    //IsRendered[] targets = new IsRendered[5];
    [SerializeField] SaveImage save;
    [SerializeField] SoundController soundController;
    [SerializeField] Camera camera;
    public int GetId() { return id; }

    // public GameObject[] GetTarget { get { return target; } }

    void Start()
    {
        //controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;

        /*for (int i = 0; i < target.Length; i++)
        {
         
            //targets[i] = new IsRendered(i);
            //targets[i] = GameObject.Find("target" + (i+1)).GetComponent<IsRenderded>();
        }*/
    }

    void Update()
    {


        if (Input.GetAxis("LVertical" + id) != 0.0f)
        {
            //Debug.Log(Input.GetAxis("LVertical" + id));
            moveDirection.z = Input.GetAxis("LVertical" + id) * speed;
        }
        else
        {
            moveDirection.z = 0;
        }

        //方向転換
        transform.Rotate(0, Input.GetAxis("LHorizontal" + id) * amountRotate, 0);
        //地上にいる場合のみ操作を行う
        if (controller.isGrounded)
        {
            //ジャンプ
            if (Input.GetButton("Jump" + id))
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
        if (Input.GetButtonDown("Snap" + id))
        {
            //save.SaveCameraImage();
            RayCheck();
            /*foreach (GameObject obj in Enemy)
            {
                var targets = obj.GetComponent<PlayerController>().Target;

                foreach (GameObject tar in targets)
                {
                    tar.GetComponent<IsRendered>().Renderedcheck();                    
                }
            }*/

            soundController.PlaySE(SoundController.Sound.camera);

        }

    }

    void RayCheck()
    {

        foreach (var ene in Enemy)
        {
            var targets = ene.GetComponent<PlayerController>().Target;

            foreach (var tar in targets)
            {
                // Debug.Log(tar.transform.position);
                Vector3 vec = (tar.transform.position - camera.transform.position).normalized;
                Ray ray = new Ray(camera.transform.position,vec);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 forward = camera.transform.TransformDirection(Vector3.forward);
                    float theta = Mathf.Acos(Vector3.Dot(forward,vec) / (forward.magnitude * vec.magnitude)) * Mathf.Rad2Deg;
                    //Debug.Log(theta);
                    var hFOV = Mathf.Rad2Deg * 2 * Mathf.Atan(Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * camera.aspect);
                    //Debug.Log(hFOV);
                    if (theta < hFOV/2)
                    {
                        Debug.DrawRay(camera.transform.position, vec * hit.distance, Color.red, 1);

                    }
                }
            }
        }        
    }
}