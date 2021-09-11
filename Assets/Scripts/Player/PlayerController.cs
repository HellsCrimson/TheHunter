using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    PlayerManager playerManager;
    private Animator anim;
    
    [SerializeField] GameObject cameraHolder;

    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    PhotonView PV;

    [SerializeField] private AudioSource source;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();

        playerManager = PhotonView.Find((int) PV.InstantiationData[0]).GetComponent<PlayerManager>();
    }

    private void Start()
    {
        anim = GetComponent<Animator>();
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(GetComponentInChildren<Canvas>().gameObject);
            Destroy(rb);
        }
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;

        Move();
        if (PauseMenuMulti.GameIsPaused)
            return;
        Look();
        Jump();
    }

    void Look()
    {
        transform.Rotate(Vector3.up * (Input.GetAxisRaw("Mouse X") * mouseSensitivity));
        
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        
        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void Move()
    {
        if (PauseMenuMulti.GameIsPaused)
        {
            Vector3 moveDirP = new Vector3(0, 0, 0);
            moveAmount = Vector3.SmoothDamp(moveAmount, moveDirP * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
            if (source.isPlaying)
                source.Stop();
        }
        else
        {
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
            if (anim != null)
            {
                if (moveAmount != new Vector3(0, 0, 0))
                {
                    if (!source.isPlaying)
                        source.Play();
                    anim.SetBool("run", true);
                }
                else
                {
                    if (source.isPlaying)
                        source.Stop();
                    anim.SetBool("run", false);
                }
            }
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            if (anim != null)
                anim.SetBool("jump", true);
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void SetGroudedState(bool _grounded)
    {
        if (anim != null)
            anim.SetBool("jump", false);
        grounded = _grounded;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine)
            return;

        if (PauseMenuMulti.GameIsPaused)
            return;
        
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    
    public void SetSensitivity(float sensibility)
    {
        mouseSensitivity = sensibility;
    }

    public void TakeDamage(int damage)
    {
        PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(int damage)
    {
        if (!PV.IsMine)
            return;

        GetComponent<HealthBar>().GetDamage(damage);
    }

    public void Die()
    {
        playerManager.Die();
    }
}
