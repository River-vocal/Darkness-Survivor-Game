using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] public VoidEventChannel useInputEventChannel;
    [SerializeField] public Portal pair;
    [SerializeField] public ParticleSystem ps;

    private GameObject player;
    private PlayerInputHandler inputHandler;
    private Transform teleportTarget;
    private int status; // 0: idle, 1: wait player input, 2: pre-teleport, 3: in-teleport, 4: post-teleport
    private float timer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inputHandler = player.GetComponent<PlayerInputHandler>();
        ps.Stop();
    }

    private void OnEnable()
    {
        useInputEventChannel.AddListener(Teleport);
    }

    private void OnDisable()
    {
        useInputEventChannel.RemoveListener(Teleport);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (status != 0) return;
        ps.Play();
        pair.ps.Play();

        status = 1;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (status != 1) return;
        ps.Stop();
        pair.ps.Stop();

        status = 0;
    }

    private void Teleport()
    {
        if (status != 1) return;
        teleportTarget = pair.transform;
    }

    [SerializeField] public float preTeleportWaitTime = 0.6f;
    [SerializeField] public float postTeleportWaitTime = 0.4f;
    [SerializeField] public float teleportSpeed = 10f;

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        Debug.Log(status);

        switch (status)
        {
            case 0:
                break;
            case 1:
                if (teleportTarget) status = 2;
                break;
            case 2:
                SetSimulationSpeed(5f);
                inputHandler.enabled = false;
                timer = preTeleportWaitTime;
                status = 3;
                break;
            case 3:
                SetSimulationSpeed(1f);
                pair.SetSimulationSpeed(5f);
                player.SetActive(false);
                player.transform.position =
                    Vector3.MoveTowards(player.transform.position, teleportTarget.position,
                        teleportSpeed * Time.deltaTime);
                if (player.transform.position == teleportTarget.position) status = 4;
                break;
            case 4:
                teleportTarget = null;
                timer = postTeleportWaitTime;
                status = 5;
                break;
            case 5:
                pair.SetSimulationSpeed(1f);
                player.SetActive(true);
                inputHandler.enabled = true;
                timer = 1f;
                status = 0;
                break;
        }
    }

    public void SetSimulationSpeed(float speed)
    {
        var main = ps.main;
        main.simulationSpeed = speed;
    }
}