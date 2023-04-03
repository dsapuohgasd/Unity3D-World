using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //[SerializeField]
    private float Speed = 4;

    public static int CoinCount;

    [SerializeField]
    private TMPro.TextMeshProUGUI coinCount;
    
    [SerializeField]
    private GameObject lighter;

    private CharacterController characterController;
    private Animator _animator;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f * 1.9f ;

    public float Stamina;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        playerVelocity = Vector3.zero;
        CoinCount = 0;
        Stamina= 1;
    }

    void Update()
    {   
        coinCount.text= CoinCount.ToString();
        float factor = Speed * Time.deltaTime;
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) )
        {
            if (Stamina > 0.01f)
            {
                factor *= 2;
                Stamina -= Time.deltaTime / 5;
                if (Stamina < 0)
                {
                    Stamina = 0;
                }
            }
        }
        else if (!(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift)))
        {
            Stamina += Time.deltaTime / 5;
            if (Stamina > 1)
            {
                Stamina= 1;
            }
        }
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");
        Vector3 moveDirection = (dx * this.transform.right + dz * this.transform.forward).normalized;                      

        moveDirection *= factor;              

        if (moveDirection.magnitude < .1)      
        {
           _animator.SetInteger("PlayerState", 0);
        }
        if (Input.GetKey(KeyCode.W)|| Input.GetKey(KeyCode.UpArrow))  
        {
           _animator.SetInteger("PlayerState", 1);  
        }
        if((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && Input.GetKey(KeyCode.LeftShift)){
           _animator.SetInteger("PlayerState", 2);
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
           _animator.SetInteger("PlayerState", 3);
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
           _animator.SetInteger("PlayerState", 4);
        }
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)) && Input.GetKey(KeyCode.LeftShift))
        {
            _animator.SetInteger("PlayerState", 5);
        }
        //characterController.SimpleMove(moveDirection);
        characterController.Move(moveDirection);

        //groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (Input.GetKey(KeyCode.Space) && groundedPlayer)
        {
            _animator.SetInteger("PlayerState", 6);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue) / 7;

        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);


        bool onLighter = false;
        if (Input.GetKey(KeyCode.H))
        {
            onLighter = !onLighter;
        }
        lighter.gameObject.SetActive(onLighter);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Terrain")
        {
            groundedPlayer = true;
        }
        Debug.Log("Enter" + other.name);

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Terrain")
        {
            groundedPlayer = false;
        }
        Debug.Log("Exit" + other.name);

    }
}

