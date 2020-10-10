using UnityEngine;

public class ThirdPersonControllers : MonoBehaviour
{
    public float minView = 38, maxView = 90;
    public float rotateSpeed = 150, scaleSpeed = 800;
    float scaleS, rotateX, rotateY;
    CharacterController character;
    public Camera camera;

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;


    private void Start()
    {
        character = GetComponent<CharacterController>() ? GetComponent<CharacterController>() : gameObject.AddComponent<CharacterController>();
    }

    void Update()
    {
        if (character.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        character.Move(moveDirection * Time.deltaTime);


        if (Input.GetMouseButton(1))
        {
            rotateX = Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed;
            rotateY = -Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed;
            transform.Rotate(new Vector3(rotateY, rotateX, 0));
        }

        scaleS = -Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * scaleSpeed;
        camera.fieldOfView =
            Mathf.Clamp(camera.fieldOfView + scaleS, minView, maxView);
    }
}
