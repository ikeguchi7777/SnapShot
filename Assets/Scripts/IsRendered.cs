
using UnityEngine;
using System.Collections;

public class IsRendered : MonoBehaviour
{

    //メインカメラに付いているタグ名
    //private const string MAIN_CAMERA_TAG_NAME = "EyeCamera";

    //カメラに表示されているか
    static bool _isRendered = false;
    //private static bool check = false;

     int id;

   public  IsRendered(int id)
    {
        this.id = id;
    }

    private void Update()
    {



        if (_isRendered)
        {
            _isRendered = false;
        }


        /*if (check)
        {
            check = false;
        }*/
    }


    public void Renderedcheck()
    {
        
        if (_isRendered)
        {
            //_isRendered = true;
            Debug.Log(id + ":映ってる");
        }
        else
        {
            Debug.Log(id + ":映ってない");
        }
    }

    //カメラに映ってる間に呼ばれる
    private void OnWillRenderObject()
    {

        if (Camera.current.tag == "EyeCamera")
        {
            _isRendered = true;
        }

        /*Debug.Log(Camera.current.tag);
        if (Camera.current.tag == "EyeCamera")
        {
            //_isRendered = true;
            Debug.Log("映ってる");
        }
        else
        {
            Debug.Log("映ってない");
        }*/

        /* if (_isRendered)
        {
            Debug.Log("映ってる");
        }
        else
        {
            //Debug.Log("映ってない");
        }*/


        //_isRendered = false;

    }

}