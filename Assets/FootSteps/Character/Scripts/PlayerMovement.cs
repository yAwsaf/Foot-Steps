using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject cam;
    public GameObject SandGround;
    public Transform leftFoot;
    public Transform rightFoot;
    public float walkSpeed = 5;
    public float rotationSpeed = 5;

    private Animator anim;
    private Vector2 HV_Input;
    private Vector3 forwardDir;
    private Vector3 rightDir;
    private bool isWalking = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        InputControl();
        CameraControl();

        Vector3 inputDir = forwardDir * HV_Input.y + rightDir * HV_Input.x;
        inputDir = inputDir.normalized;
        transform.position = transform.position + (inputDir * walkSpeed * Time.deltaTime);

        if (inputDir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, inputDir, Time.deltaTime * rotationSpeed);
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        anim.SetBool("isWalking", isWalking);


    }

    private void InputControl()
    {
        HV_Input.x = Input.GetAxis("Horizontal");
        HV_Input.y = Input.GetAxis("Vertical");
    }

   
    private void CameraControl()
    {
        forwardDir = new Vector3(cam.transform.forward.x, 0.0f, cam.transform.forward.z);
        rightDir = new Vector3(cam.transform.right.x, 0.0f, cam.transform.right.z);
    }

    public void printLeftFoot()
    {
        float radiantRotation = Mathf.Deg2Rad * (transform.eulerAngles.y + 180f);

        Ray ray = new Ray(leftFoot.position, -leftFoot.forward);
        SandGround.GetComponent<SandController>().PrintFoot(ray, radiantRotation);
    }
    public void printRightFoot()
    {
        float radiantRotation = Mathf.Deg2Rad * (transform.eulerAngles.y + 180f);

        Ray ray = new Ray(rightFoot.position, -rightFoot.forward);
        SandGround.GetComponent<SandController>().PrintFoot(ray, radiantRotation);
    }

    public void printBothFoot()
    {
        float radiantRotation = Mathf.Deg2Rad * (transform.eulerAngles.y + 180f);

        Ray ray1 = new Ray(leftFoot.position, -leftFoot.forward);
        SandGround.GetComponent<SandController>().PrintFoot(ray1, radiantRotation);

        Ray ray2 = new Ray(rightFoot.position, -rightFoot.forward);
        SandGround.GetComponent<SandController>().PrintFoot(ray2, radiantRotation);
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(leftFoot.position, -leftFoot.forward);
        Gizmos.DrawRay(ray);
        Ray ray2 = new Ray(rightFoot.position, -rightFoot.forward);
        Gizmos.DrawRay(ray2);

        Gizmos.color = Color.blue;
        Ray ray3 = new Ray(this.transform.position + Vector3.up, new Vector3(cam.transform.forward.x, 0.0f, cam.transform.forward.z));
        Gizmos.DrawRay(ray3);

        Gizmos.color = Color.red;
        Ray ray4 = new Ray(this.transform.position + Vector3.up, new Vector3(cam.transform.right.x, 0.0f, cam.transform.right.z));
        Gizmos.DrawRay(ray4);
    }
}
