using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class SC_CharacterController : MonoBehaviour
{
    public float speed = 7.5f;
    public float runningSpeed = 10f;
    private bool running = false;
    public bool jumpActive;
    public float jumpSpeed = 8.0f;
    public float runningJumpSpeed = 11f;
    private float useJumpSpeed;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public Light flashlight;
    public float lookSpeed = 2.0f;
    public float onZoomLookSpeed = 0.2f;
    private float useLookSpeed;
    public float lookXLimit = 45.0f;
    private bool onZoom = false;
    private float fov;
    public float zoomRate = 3;
    public float zoomNormal = 60;
    public float zoomMax = 45;
    private float rateAudioVolume;

    CharacterController characterController;
    [HideInInspector]
    public Vector3 moveDirection = Vector3.zero;
    Vector2 rotation = Vector2.zero;

    [HideInInspector]
    public bool canMove = true;
    private float useSpeed;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        rotation.y = transform.eulerAngles.y;               
    }

    void Update()
    {
        
        if (characterController.isGrounded)
        {

            // get camera field of view
            fov = playerCamera.GetComponent<Camera>().fieldOfView;
            
            running = false;
            if(rateAudioVolume <= 0){
                rateAudioVolume = 0;
            }            

            if (Input.GetButton("Run") && !onZoom && canMove)
            {
                running = true;                                
            }

            // We are grounded, so recalculate move direction based on axes
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);
            if(running){
                useSpeed = runningSpeed;
            } else {
                useSpeed = speed;
            }
            float curSpeedX = canMove ? useSpeed * Input.GetAxis("Vertical") : 0;
            float curSpeedY = canMove ? useSpeed * Input.GetAxis("Horizontal") : 0;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            // If jumping
            if(jumpActive && !onZoom && canMove){
                if (Input.GetButton("Jump"))
                {
                    if(running){
                        useJumpSpeed = runningJumpSpeed;
                    } else {
                        useJumpSpeed = jumpSpeed;
                    }
                    moveDirection.y = useJumpSpeed;
                }
            }       

            if (Input.GetButtonDown("Flashlight") && !onZoom && canMove)
            {
                flashlight.GetComponent<AudioSource>().Play();
                flashlight.GetComponent<Light>().enabled = !flashlight.GetComponent<Light>().enabled;
            }  

            // If pushing zoom button
            // set onZoom value true/false
            if (Input.GetButtonDown("Zoom") && !running && canMove)
            {   
                onZoom = !onZoom;
                rateAudioVolume = 0.02f;
                if(onZoom){
                    GetComponent<AudioSource>().volume = 0.35f;
                    GetComponent<AudioSource>().Play();                                        
                }                                
            }

            // if on zoom
            if(onZoom){
                
                if(fov > zoomMax){
                    fov=fov-zoomRate;                    
                }        
                if(fov <= zoomMax){
                    fov = zoomMax;
                }                   
                
            } else {
                
                if(fov < zoomNormal){
                    fov=fov+zoomRate;   
                }        
                if(fov >= zoomNormal){
                    fov = zoomNormal;                    
                }
                if(rateAudioVolume > 0){
                    GetComponent<AudioSource>().volume = (GetComponent<AudioSource>().volume - rateAudioVolume);
                }

            }

            playerCamera.GetComponent<Camera>().fieldOfView = fov;               
            
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        if(!onZoom){
            characterController.Move(moveDirection * Time.deltaTime);
        }        

        // Player and Camera rotation
        if (canMove)
        {
            if(onZoom){
                useLookSpeed = onZoomLookSpeed;
            } else {
                useLookSpeed = lookSpeed;
            }
            rotation.y += Input.GetAxis("Mouse X") * useLookSpeed;
            rotation.x += -Input.GetAxis("Mouse Y") * useLookSpeed;
            rotation.x = Mathf.Clamp(rotation.x, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotation.x, 0, 0);
            transform.eulerAngles = new Vector2(0, rotation.y);
        }
    }

}