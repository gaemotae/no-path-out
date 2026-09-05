using UnityEngine;
using UnityEngine.AI;

public class WatcherFSM : FSM
{
    public enum WatcherState
    {
        Patrol,
        Chase,
        Search,
        Catch,
        Disabled,
    }

    [Header("State")]
    public WatcherState currentState = WatcherState.Patrol;

    [Header("Patrol")]
    public float randomOffsetRadius = 0.0f;
    public float arriveThreshold = 0.2f;
    public float searchArriveDistance = 0.6f;

    [Header("Catch")]
    public float catchDistance = 2.0f;

    [Header("FOV")]
    public float viewDistance = 8f;
    public float viewAngle = 90f;
    public Transform eye;
    public LayerMask obstacleMask;

    [Header("NPC Footsteps")]
    public float walkStepInterval = 0.55f;
    public float runStepInterval = 0.38f;
    private float stepTimer = 0f;

    private NavMeshAgent agent;
    private Animator anim;
    private Transform player;
    private UIStateControl uiState;

    private GameObject[] patrolPointList;
    private Vector3 patrolDestination;
    private Vector3 lastSeenPosition;

    private bool isDisabled = false;

    public Transform spawnPoint;
    public AudioController npcSound;

    protected override void Initialize()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }

        agent = GetComponent<NavMeshAgent>();
        uiState = FindObjectOfType<UIStateControl>();
        patrolPointList = GameObject.FindGameObjectsWithTag("WayPoint");

        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        if (objPlayer != null)
            player = objPlayer.transform;

        anim = GetComponentInChildren<Animator>();
        if (anim != null) anim.applyRootMotion = false;

        FindNextPatrolPoint();
        agent.SetDestination(patrolDestination);

        if (npcSound == null)
            npcSound = GetComponent<AudioController>();
    }

    protected override void FSMUpdate()
    {
        if (uiState == null || !uiState.IsPlaying) return;

        UpdateAnimationByState();
        UpdateFootstepSound();
        switch (currentState)
        {
            case WatcherState.Patrol:
                UpdatePatrolState();
                break;
            case WatcherState.Chase:
                UpdateChaseState();
                break;
            case WatcherState.Search:
                UpdateSearchState();
                break;
            case WatcherState.Catch:
                UpdateCatchState();
                break;
            case WatcherState.Disabled:
                UpdateDisabledState();
                break;
        }

    }

    private void UpdatePatrolState()
    {
        if (agent.pathPending) return;

        if (CanSeePlayer())
        {
            currentState = WatcherState.Chase;
            if (npcSound != null) npcSound.StartChaseSound();

            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance + arriveThreshold)
        {
            FindNextPatrolPoint();
            agent.SetDestination(patrolDestination);
        }
    }

    protected void UpdateChaseState()
    {
        if (player == null)
        {
            currentState = WatcherState.Patrol;
            return;
        }

        agent.SetDestination(player.position);

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist <= catchDistance && TryCatch())
        {
            return;
        }

        if (!CanSeePlayer())
        {
            lastSeenPosition = player.position;
            currentState = WatcherState.Search;
            agent.SetDestination(lastSeenPosition);

            if (npcSound != null) npcSound.StopChaseSound();

            return;
        }
    }

    private void UpdateSearchState()
    {
        if (CanSeePlayer())
        {
            currentState = WatcherState.Chase;
            if (npcSound != null) npcSound.StartChaseSound();

            return;
        }

        if (agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance + searchArriveDistance)
        {
            FindNextPatrolPoint();
            agent.SetDestination(patrolDestination);
            currentState = WatcherState.Patrol;

            if (npcSound != null) npcSound.StopChaseSound();
        }
    }

    private void UpdateCatchState()
    {
        TryCatch();
    }

    private bool GameOver()
    {
        if (uiState == null || !uiState.ShowGameOver()) return false;

        currentState = WatcherState.Catch;
        UpdateAnimationByState();
        if (npcSound != null) npcSound.StopChaseSound();
        agent.isStopped = true;
        Debug.Log("GAME OVER : Player Caught");
        return true;

    }

    private void UpdateDisabledState()
    {
        if (isDisabled) return;
        isDisabled = true;
        Destroy(gameObject, 1.0f);
    }

    private void FindNextPatrolPoint()
    {
        int rndIndex = Random.Range(0, patrolPointList.Length);
        patrolDestination = patrolPointList[rndIndex].transform.position;
    }

    private bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 eyePos = (eye != null) ? eye.position : transform.position + Vector3.up * 1.5f;
        Vector3 toPlayer = player.position - eyePos;

        if (toPlayer.magnitude > viewDistance) return false;
        if (Vector3.Angle(transform.forward, toPlayer.normalized) > viewAngle * 0.5f) return false;

        if (Physics.Raycast(eyePos, toPlayer.normalized, toPlayer.magnitude, obstacleMask))
            return false;

        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ForceGameOver();
        }
    }

    public void ForceGameOver()
    {
        TryCatch();
    }

    private bool TryCatch()
    {
        if (uiState == null || !uiState.IsPlaying || player == null) return false;

        // Catch checks wall occlusion without applying the FOV angle limit.
        if (Physics.Linecast(transform.position, player.position, obstacleMask,
            QueryTriggerInteraction.Ignore))
            return false;

        // Confirm immediately so later catch/exit requests cannot replace the result.
        return GameOver();
    }

    private void UpdateAnimationByState()
    {
        if (anim == null) return;

        bool walking = (currentState == WatcherState.Patrol || currentState == WatcherState.Search);
        bool running = (currentState == WatcherState.Chase);

        if (currentState == WatcherState.Catch || currentState == WatcherState.Disabled)
        {
            walking = false;
            running = false;
        }

        anim.SetBool("isWalking", walking);
        anim.SetBool("isRunning", running);
    }

    private void UpdateFootstepSound()
    {
        if (npcSound == null || agent == null) return;

        if (currentState == WatcherState.Catch || currentState == WatcherState.Disabled)
        {
            stepTimer = 0f;
            return;
        }

        bool moving = agent.velocity.magnitude > 0.1f && !agent.isStopped;
        if (!moving)
        {
            stepTimer = 0f;
            return;
        }

        bool running = (currentState == WatcherState.Chase);

        stepTimer += Time.deltaTime;
        float interval = running ? runStepInterval : walkStepInterval;

        if (stepTimer >= interval)
        {
            stepTimer = 0f;

            if (running) npcSound.PlayRun();
            else npcSound.PlayWalk();
        }
    }

}
