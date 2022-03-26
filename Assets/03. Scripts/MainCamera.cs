using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    bool lockCursor = true;

    [SerializeField]
    public float mouseSensitivity = 10;
    [SerializeField]
    Transform target;
    [SerializeField]
    float disFromTarget = 2;
    [SerializeField]
    Vector3 positionOffset = new Vector3(1.0f, 0.2f, -2f);
    [SerializeField]
    float followDamping = 2;
    [SerializeField]
    Vector2 pitchMinMax = new Vector3(-20, 85);
    
    float rotationSmoothTime = 0.2f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotaion;
    Vector3 touchInitPos;

    CinemachineBrain cinemachineBrain;
    CinemachineVirtualCamera cv;

    float yawTemp;
    public float Yaw { get; set; }
    public float Pitch { get; set; }

    void Start()
    {
        //if (lockCursor)
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
        //}

        cinemachineBrain = GetComponent<CinemachineBrain>();
        cv = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
    }

    private void LateUpdate()
    {
        if (IngameManager.Instance.GameState == EnGameState.Start)
        {
            TouchInput();
        }
    }

    void ThridPersonCam()
    {
        Vector3 forward = transform.rotation * Vector3.forward;
        Vector3 right = transform.rotation * Vector3.right;
        Vector3 up = transform.rotation * Vector3.up;

        Vector3 targetPos = target.position;
        Vector3 desiredPos = targetPos +
            (forward * positionOffset.z) + (right * positionOffset.x) + (up * positionOffset.y);

        transform.position = desiredPos;
    }

    void TouchInput()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchInitPos = Input.GetTouch(0).position;
                yawTemp += Yaw;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector3 touchPos = Input.GetTouch(0).position;
                Yaw = yawTemp + (touchPos.x - touchInitPos.x) * 180 / Screen.width;
            }
            Pitch = cv.transform.eulerAngles.x;
            currentRotaion = Vector3.SmoothDamp(currentRotaion, new Vector3(Pitch, Yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            currentRotaion.x = Pitch;
            cv.transform.eulerAngles = currentRotaion;
        }
#else
        if (Input.GetMouseButton(1))
        {
            Yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            //Pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            //Pitch = Mathf.Clamp(Pitch, pitchMinMax.x, pitchMinMax.y);
            Pitch = cv.transform.eulerAngles.x;
            currentRotaion = Vector3.SmoothDamp(currentRotaion, new Vector3(Pitch, Yaw), ref rotationSmoothVelocity, rotationSmoothTime);
            currentRotaion.x = Pitch;
            cv.transform.eulerAngles = currentRotaion;
        }
#endif
    }

    public void ChangeTarget(Transform newTarget)
    {
        if (cv == null)
        {
            cv = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        }
        target = newTarget;
        cv.Follow = target;
    }
}
