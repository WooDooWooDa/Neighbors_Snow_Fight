using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGolem : NetworkBehaviour
{
    [SerializeField] private GameObject snowBallPrefab;
    [SerializeField] private Transform launchPos;
    [SerializeField] private Transform head;

    private GameObject[] targets;
    private Transform currentTarget;
    private PlayerItem player;

    private float launchForce = 45f;

    public void SetPlayer(PlayerItem player)
    {
        this.player = player;
    }
    
    public void LaunchBall()
    {
        var direction = head.rotation;
        var snowBall = Instantiate(snowBallPrefab, launchPos.position, direction);
        launchPos.rotation = direction;
        snowBall.GetComponent<SnowBall>().SetLauncher(player.GetComponent<PlayerShoot>());
        snowBall.GetComponent<Rigidbody>().velocity = launchPos.forward * launchForce;
        NetworkServer.Spawn(snowBall);
    }

    private void Start()
    {
        if (isServer)
            GetTargets();
    }

    void Update()
    {
        if (!isServer) return;

        currentTarget = GetClosestTarget();
        Vector3 direction = currentTarget.position - transform.position;
        direction = Vector3.RotateTowards(transform.forward, direction, 1f, 0f);
        direction.y = 0;
        //Debug.DrawRay(transform.position, direction, Color.red);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    [Server]
    private Transform GetClosestTarget()
    {
        Transform closest = null;
        float minDist = Mathf.Infinity;
        foreach (GameObject target in targets) {
            if (target.GetComponent<PlayerItem>() == player) continue;
            float dist = Vector3.Distance(target.transform.position, transform.position);
            if (dist < minDist) {
                closest = target.transform;
                minDist = dist;
            }
        }
        return closest;
    }

    [Server]
    private void GetTargets()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
    }
}
