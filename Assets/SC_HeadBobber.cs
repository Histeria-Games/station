using UnityEngine;

public class SC_HeadBobber : MonoBehaviour
{
    public bool activeBob = false;
    public float bobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    public float walkingBobbingSpeed = 14f;
    public float walkingbobbingAmount = 0.05f;
    private bool isOnZoom;
    float fov;
    
    public SC_CharacterController controller;

    public bool onlyWhenMoving = false;

    float defaultPosY = 0;
    float timer = 0;

    public Texture blurcamera1;
    public Texture blurcamera2;
    public Texture blurcamera3;
    public Texture blurcamera4;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {   

        fov = GetComponent<Camera>().fieldOfView;
        if(fov < 60){
            isOnZoom = true;
        } else {
            isOnZoom = false;
        }
        // If headbob is active
        if(activeBob && !isOnZoom){

            // If moving, apply head bob
            if(Mathf.Abs(controller.moveDirection.x) > 0.1f || Mathf.Abs(controller.moveDirection.z) > 0.1f) {
                bobWalking();
            } else {
                // when not moving
                // If configured Only when moving then idle else continue applying head bob
                if(!onlyWhenMoving){
                    bob();
                } else {
                    idle();
                }
            }

        }
        
    }

    void bob () {
        timer += Time.deltaTime * bobbingSpeed;
        transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
    }
    void bobWalking () {
        timer += Time.deltaTime * walkingBobbingSpeed;
        transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * walkingbobbingAmount, transform.localPosition.z);
    }
    void idle () {
        timer = 0;
        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * walkingBobbingSpeed), transform.localPosition.z);
    }

    void OnGUI(){
        fov = GetComponent<Camera>().fieldOfView;
        if(fov < 60 && fov > 45){
            GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), blurcamera4);
        } 
        if(fov <= 45 && fov > 40){
            GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), blurcamera3);
        }
        if(fov <= 40 && fov > 30){
            GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), blurcamera2);
        }
        if(fov == 30){
            GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), blurcamera1);
        }
    }
}
