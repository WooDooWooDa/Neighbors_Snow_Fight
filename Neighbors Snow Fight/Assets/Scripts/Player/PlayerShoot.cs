using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : NetworkBehaviour
{
    [SerializeField] private GameObject baseSnowBall;
    [SerializeField] private Transform launchPos;

    [Header("UI")]
    [SerializeField] private Slider aimSlider;
    [SerializeField] private TextMeshProUGUI ballsCreated;
    [SerializeField] private GameObject hand;

    private Animator handAnimator;
    private float minLaunchForce = 25f;
    private float maxLaunchForce = 40f;
    private float maxChargeTime = 1f;
    [SyncVar]
    private bool fired;
    [SyncVar]
    private float currentLaunchForce;
    private float chargeSpeed;

    private int baseMaxSnowBall = 3;
    [SyncVar]
    private int maxSnowBall;
    [SyncVar]
    private int nbSnowBallCreated = 0;      // --> munitions
    private GameObject currentSnowBall;

    private bool hasMold = false;
    [SyncVar]
    public bool canShoot = false;

    public delegate void playerReloadDelegate(PlayerShoot p);
    public event playerReloadDelegate PlayerReload;

    public void ChangeMaxBalls(int amount = 0)
    {
        if (amount == 0)
            maxSnowBall = baseMaxSnowBall;
        else
            maxSnowBall = amount;
        GetComponent<SnowGauge>().ChangeBallsPerLayer(amount);
    }

    public void UseMold(bool use)
    {
        hasMold = use;
    }

    public void ReplaceBall(GameObject newBall)
    {
        PlayerHand playerHand = hand.GetComponent<PlayerHand>();
        if (newBall != null) {
            nbSnowBallCreated = maxSnowBall;
            currentSnowBall = newBall;
            playerHand.ChangeBallSize(1.2f);
        } else {
            currentSnowBall = baseSnowBall;
            playerHand.ChangeBallSize(1f);
        }
    }

    public bool HasBall()
    {
        return nbSnowBallCreated > 0;
    }

    private void OnEnable()
    {
        currentLaunchForce = minLaunchForce;
    }

    void Start()
    {
        handAnimator = hand.GetComponent<Animator>();
        currentSnowBall = baseSnowBall;
        chargeSpeed = (maxLaunchForce - minLaunchForce) / maxChargeTime;
        aimSlider.minValue = minLaunchForce;
        aimSlider.maxValue = maxLaunchForce;
        aimSlider.gameObject.SetActive(false);
        maxSnowBall = baseMaxSnowBall;
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        aimSlider.value = minLaunchForce;
        ballsCreated.text = nbSnowBallCreated.ToString();
        hand.GetComponent<PlayerHand>().ShowHand(HasBall());

        if (!canShoot) return;

        if (Input.GetKeyDown(KeyCode.R)) {
            Debug.Log("Reload");
            CmdReload();
        }
        UpdateAndHandleShootingState();
    }

    [Command]
    private void CmdReload()
    {
        StartCoroutine(Reload());
    }

    [Server]
    private IEnumerator Reload()
    {
        var gauge = GetComponent<SnowGauge>();
        if (!gauge.CanReload() || nbSnowBallCreated > 0) {
            //MessageAnnoncer.Message = "You can't reload right now, collect snow!";
            yield break;
        }

        if (!hasMold) {
            GetComponent<PlayerMouvement>().SetSpeed(0.2f);
            yield return new WaitForSeconds(1.5f);
        }

        nbSnowBallCreated = maxSnowBall;
        gauge.UseSnow(1);
        PlayerReload?.Invoke(this);
        GetComponent<PlayerMouvement>().SetSpeed(1f);
    }

    private void UpdateAndHandleShootingState()
    {
        if (!HasBall()) return;

        var direction = GetComponentInChildren<MouseLook>().GetDirection();
        if (currentLaunchForce >= maxLaunchForce && !fired) {
            currentLaunchForce = maxLaunchForce;
            fired = true;
            handAnimator.SetTrigger("Shoot");
            CmdLaunchBall(direction, currentLaunchForce);
        } else if (Input.GetKeyDown(KeyCode.Mouse0)) {
            fired = false;
            currentLaunchForce = minLaunchForce;
        } else if (Input.GetKey(KeyCode.Mouse0) && !fired) {
            currentLaunchForce += chargeSpeed * Time.deltaTime;
            aimSlider.gameObject.SetActive(true);
            aimSlider.value = currentLaunchForce;
        } else if (Input.GetKeyUp(KeyCode.Mouse0) && !fired) {
            fired = true;
            handAnimator.SetTrigger("Shoot");
            CmdLaunchBall(direction, currentLaunchForce);
        }
    }

    [Command]
    private void CmdLaunchBall(Quaternion direction, float force)
    {
        fired = true;
        launchPos.rotation = direction;
        GameObject snowBall = Instantiate(currentSnowBall, launchPos.position, direction);

        NetworkServer.Spawn(snowBall);
        snowBall.GetComponent<SnowBall>().SetLauncher(this);
        snowBall.GetComponent<Rigidbody>().velocity = launchPos.forward * force;
        nbSnowBallCreated--;
        RpcUpdateAfterLaunch(GetComponent<NetworkIdentity>().connectionToServer);
    }

    [TargetRpc]
    private void RpcUpdateAfterLaunch(NetworkConnection conn)
    {
        currentLaunchForce = minLaunchForce;
        if (aimSlider != null)
            aimSlider.gameObject.SetActive(false);
    }
}
