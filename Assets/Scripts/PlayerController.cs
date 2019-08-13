using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Vector3 moveDirection;
    //ScoreCalculator scoreCalc;

    [SerializeField] int id;    public int GetId() { return id; }
    [SerializeField] float gravity;
    [SerializeField] float speed;
    [SerializeField] float amountRotate;
    [SerializeField] float speedJump;

    [SerializeField] CharacterController controller;
    [SerializeField] GameObject[] Target;    public GameObject[] GetTarget() { return Target; }
    [SerializeField] GameObject[] Enemy;    public GameObject[] GetEnemy() { return Enemy; }
    [SerializeField] ImageManager save;
    [SerializeField] SoundController soundController;
    [SerializeField] Camera camera;
    // public Camera GetCamera() { return camera; }

    int pictureNum = 0;
    List<PictureScore> pictures = new List<PictureScore>();


    // public GameObject[] GetTarget { get { return target; } }

    void Start()
    {
        //controller = GetComponent<CharacterController>();
        moveDirection = Vector3.zero;
        
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

            pictureNum++;
            save.SaveCameraImage();
            Snapshot();

            soundController.PlaySE(SoundController.Sound.camera);

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            save.DeleteImage();
        }



        if (Input.GetKeyDown(KeyCode.L))
        {

            foreach (var pic in pictures)
            {
                Debug.Log("player" + pic.ID + " " + pic.num + "枚目:" + pic.GetPictureScore() + "点");
            }
        }

    }

    void Snapshot()
    {
        ScoreCalculator scoreCalc = new ScoreCalculator(camera);
        foreach (var ene in Enemy)
        {
            var targets = ene.GetComponent<PlayerController>().Target;

            foreach (var tar in targets)
            {
                // Debug.Log(tar.transform.position);
                Vector3 vec = (tar.transform.position - camera.transform.position).normalized;
                Ray ray = new Ray(camera.transform.position, vec);
                int layer_mask = LayerMask.GetMask(new string[] { "RayCheckable" });
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer_mask))
                {
                    Vector3 forward = camera.transform.TransformDirection(Vector3.forward);
                    float theta = Mathf.Acos(Vector3.Dot(forward, vec) / (forward.magnitude * vec.magnitude)) * Mathf.Rad2Deg;
                    //Debug.Log(theta);
                    var hFOV = Mathf.Rad2Deg * 2 * Mathf.Atan(Mathf.Tan(camera.fieldOfView * Mathf.Deg2Rad / 2) * camera.aspect);
                    //Debug.Log(hFOV);
                    if (theta < hFOV / 2 && hit.collider.tag == "target")
                    {
                        scoreCalc.Calc(tar);
                        //scoreCalc.PrintScore();

                        Debug.DrawRay(camera.transform.position, vec * hit.distance, Color.red, 1);
                        // Debug.Log((Vector2)camera.WorldToScreenPoint(tar.transform.position));
                        // Debug.Log((Vector2)camera.WorldToViewportPoint(tar.transform.position));
                    }
                }
            }
        }
        pictures.Add(new PictureScore(id, pictureNum, scoreCalc.distanceScore, scoreCalc.angleScore, scoreCalc.positionScore));
        //scoreCalculator.PrintScore();
    }
}