using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float runSpeed = 5.5f;
    public float gravity = -20.0f;
    public Transform spawnPoint;

    private Vector3 velocity;
    private CharacterController controller;
    private Animator anim;

    [Header("Footsteps")]
    public AudioClip walkClip;                
    public AudioClip runClip;                
    public float walkStepInterval = 0.5f;
    public float runStepInterval = 0.32f;
    [Range(0f, 1f)] public float footstepVolume = 1f;

    private AudioSource audioSource;
    private float stepTimer = 0f;

    void Start()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }

        controller = GetComponent<CharacterController>();

        anim = GetComponentInChildren<Animator>();
        if (anim != null) anim.applyRootMotion = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 0f;    
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (controller.isGrounded)
        {
            Vector3 inputDir = Vector3.ClampMagnitude(new Vector3(h, 0, v), 1f);
            bool moving = inputDir.magnitude > 0.1f;
            bool running = moving && Input.GetKey(KeyCode.LeftShift);

            if (anim != null)
            {
                anim.SetBool("isWalking", moving);
                anim.SetBool("isRunning", running);
            }

            float speed = running ? runSpeed : walkSpeed;

            velocity = transform.TransformDirection(inputDir);
            velocity *= speed;

            HandleFootsteps(moving, running);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleFootsteps(bool moving, bool running)
    {
        if (!moving)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer += Time.deltaTime;
        float interval = running ? runStepInterval : walkStepInterval;

        if (stepTimer >= interval)
        {
            stepTimer = 0f;

            if (running) PlayRunStep();
            else PlayWalkStep();
        }
    }

    private void PlayWalkStep()
    {
        if (walkClip == null || audioSource == null) return;
        audioSource.PlayOneShot(walkClip, footstepVolume);
    }

    private void PlayRunStep()
    {
        if (runClip == null || audioSource == null) return;
        audioSource.PlayOneShot(runClip, footstepVolume);
    }
}
