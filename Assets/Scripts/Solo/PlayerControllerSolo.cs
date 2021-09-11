using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerControllerSolo : MonoBehaviour
{
    [SerializeField] GameObject cameraHolder;
    private GameObject gameController;

    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    float verticalLookRotation;
    bool grounded;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;

    Rigidbody rb;

    [SerializeField] GameObject image;
    [SerializeField] private TMP_Text score;
    [SerializeField] private TMP_Text temps;
    [SerializeField] private TMP_Text scoreInGame;
    [SerializeField] private TMP_Text round;
    [SerializeField] private TMP_Text nextWaveTimer;
    private int compte;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gameController = GameObject.Find("Game Controller");
    }

    private void Update()
    {
        if (!GetComponent<HealthBar>().dead)
        {
            Move();
            if (PauseMenuSolo.GameIsPaused)
                return;
            Look();
            Jump();
            scoreInGame.text = "Score : " + gameController.GetComponent<Survival>().zombieKilled;
            round.text = "Vague : " + gameController.GetComponent<Survival>().curWave;
            if (gameController.GetComponent<Survival>().waiting)
            {
                nextWaveTimer.gameObject.SetActive(true);
                nextWaveTimer.text = "Prochaine vague dans " + gameController.GetComponent<Survival>().currCountdownValue;
            }
            else
            {
                nextWaveTimer.gameObject.SetActive(false);
            }
        }
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
        if (PauseMenuSolo.GameIsPaused)
        {
            Vector3 moveDirP = new Vector3(0, 0, 0);
            moveAmount = Vector3.SmoothDamp(moveAmount, moveDirP * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
        }
        else
        {
            Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            rb.AddForce(transform.up * jumpForce);
        }
    }

    public void SetGroudedState(bool _grounded)
    {
        grounded = _grounded;
    }

    private void FixedUpdate()
    {
        if (PauseMenuSolo.GameIsPaused)
            return;
        
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.fixedDeltaTime);
    }
    
    public void SetSensitivity(float sensibility)
    {
        mouseSensitivity = sensibility;
    }

    public void Die()
    {
        image.SetActive(true);
        score.text = "Zombies tue : " + gameController.GetComponent<Survival>().zombieKilled;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        StartCoroutine(Fin());
    }

    IEnumerator Fin()
    {
        compte = 8;
        while (compte > 0)
        {
            temps.text = "Retour menu principal dans " + compte + "s";
            yield return new WaitForSeconds(1f);
            compte--;
        }

        StartCoroutine(LoadAsynchronously(0));
    }
    
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        PhotonNetwork.IsMessageQueueRunning = false;
        
        PhotonNetwork.LoadLevel(sceneIndex);
        var operation = PhotonNetwork.LevelLoadingProgress;

        MenuManager.Instance.OpenMenu("loading");
        
        while (operation < 1f)
        {
            operation = PhotonNetwork.LevelLoadingProgress;
            yield return null;
        }
    }
}
