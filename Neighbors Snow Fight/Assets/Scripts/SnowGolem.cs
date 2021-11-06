using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGolem : MonoBehaviour
{
    [SerializeField] private Rigidbody snowBallPrefab;
    [SerializeField] private Transform launchPos;
    [SerializeField] private Transform head;

    private GameObject[] targets;
    private Transform currentTarget;
    private PlayerItem player;

    private float launchForce = 40f;

    public void SetPlayer(PlayerItem player)
    {
        this.player = player;
    }
    
    public void LaunchBall()
    {
        var direction = head.rotation;
        var snowBall = Instantiate(snowBallPrefab, launchPos.position, direction);
        launchPos.rotation = direction;
        snowBall.velocity = launchPos.forward * launchForce;
    }

    private void Start()
    {
        GetTargets();
    }

    void Update()
    {
        currentTarget = GetClosestTarget();
        Vector3 direction = currentTarget.position - transform.position;
        direction = Vector3.RotateTowards(transform.forward, direction, 1f, 0f);
        Debug.DrawRay(transform.position, direction, Color.red);
        transform.rotation = Quaternion.LookRotation(direction);
    }

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

    private void GetTargets()
    {
        targets = GameObject.FindGameObjectsWithTag("Player");
    }
}
