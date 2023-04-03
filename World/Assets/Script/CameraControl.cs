using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private GameObject Player;
    private Vector3 rod;
    private Vector3 rodBase;

    private float camAngleX;   
    private float camAngleX0;  
    private float camAngleY;
    private const float VertSens = 1.0f;
    private const float HorSens = 1.0f;
    public Vector2 CamMinMax_Y = new Vector2(-40, 40);

    private float zoomMax = 2.0f;
    private float zoomMin = 0.1f;
    private float zoomSens = 2.0f;
    private float zoom = 1.0f;

    void Start()
    {
        Player = GameObject.Find("Player");
        rod = rodBase = this.transform.position - Player.transform.position;
        Cursor.lockState = CursorLockMode.Locked;
        camAngleX0 = camAngleX = this.transform.eulerAngles.y;    
        camAngleY = this.transform.eulerAngles.x;
    }
    private void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * VertSens * Time.timeScale * (GameSettings.VerticalInverted? 1 : -1) * (GameSettings.Sensitivity * 1.5f + 0.5f);
        float mouseX = Input.GetAxis("Mouse X") * HorSens * Time.timeScale * (GameSettings.Sensitivity * 1.5f + 0.5f);
        camAngleY -= mouseY;
        camAngleX += mouseX;

        float scroll = Input.mouseScrollDelta.y * (GameSettings.InverseWheelZoom ? -1 : 1);
        if(scroll > 0)
        {
            if (rod.magnitude < 0.1f) {
                rod = Vector3.zero;
            }
            else
            {
                rod /= 1.5f;
            }
        }
        else if(scroll < 0)
        {
            if(rod.magnitude > rodBase.magnitude)
            {
                rod = rodBase;
            }
            else
            {
                rod *= 1.5f;
            }
        }
    }
    void LateUpdate()
    {
        transform.position = Player.transform.position
            + Quaternion.Euler(0, camAngleX - camAngleX0, 0) * rod * zoom;

        transform.eulerAngles = new Vector3(camAngleY, camAngleX, 0);
        if (Input.GetMouseButtonDown(0))
        {   

        }else if (Input.GetMouseButtonDown(2))
        {

        }
        else
        {
            Vector3 pf = Quaternion.Euler(0, -camAngleX0, 0) * this.transform.forward;
            pf.y = 0;
            Player.transform.forward = pf.normalized;
        }
    }
}
