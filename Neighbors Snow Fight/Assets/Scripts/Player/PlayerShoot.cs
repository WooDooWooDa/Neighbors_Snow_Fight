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

    private float minLaunchForce = 25f;
    private float maxLaunchForce = 40f;
    private float maxChargeTime = 1f;
    private bool fired;
    private float currentLaunchForce;
    private float chargeSpeed;

    private int baseMaxSnowBall = 3;
    private int maxSnowBall;
    private int nbSnowBallCreated = 0;      // --> munitions
    private GameObject currentSnowBall;

    private bool hasMold = false;

    public delegate void playerReloadDelegate(PlayerShoot p);
    public event playerReloadDelegate PlayerReload;

    public void ChangeMaxBalls(bool useBase, int amount = 0)
    {
        if (useBase)
            maxSnowBall = baseMaxSnowBall;
        else
            maxSnowBall = amount;
    }

    public void UseMold(bool use)
    {
        hasMold = use;
    }

    public void ReplaceBall(GameObject newBall)
    {
        currentSnowBall = newBall != null ? newBall : baseSnowBall;
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
        if (Input.GetKeyDown(KeyCode.R)) {
            StartCoroutine(Reload());
        }
        UpdateAndHandleShootingState();
    }

    private IEnumerator Reload()
    {
        var gauge = GetComponent<SnowGauge>();
        if (!gauge.CanReload() || nbSnowBallCreated > 0) {
            MessageAnnoncer.Message = "You can't reload right now, collect snow!";
            yield break;
        }

        PlayerReload?.Invoke(this);

        if (!hasMold) {
            GetComponent<PlayerMouvement>().SetSpeed(0.2f);
            yield return new WaitForSeconds(1.5f);
        }  

        gauge.UseSnow(1);
        nbSnowBallCreated = maxSnowBall;
        GetComponent<PlayerMouvement>().SetSpeed(1f);
    }

    private void UpdateAndHandleShootingState()
    {
        if (!HasBall()) return;

        var direction = GetComponentInChildren<MouseLook>().GetDirection();
        if (currentLaunchForce >= maxLaunchForce && !fired) {
            currentLaunchForce = maxLaunchForce;
            CmdLaunchBall(direction);
        } else if (Input.GetKeyDown(KeyCode.Mouse0)) {
            fired = false;
            currentLaunchForce = minLaunchForce;
        } else if (Input.GetKey(KeyCode.Mouse0) && !fired) {
            currentLaunchForce += chargeSpeed * Time.deltaTime;
            aimSlider.gameObject.SetActive(true);
            aimSlider.value = currentLaunchForce;
        } else if (Input.GetKeyUp(KeyCode.Mouse0) && !fired) {
            CmdLaunchBall(direction);
        }
    }

    [Command]
    private void CmdLaunchBall(Quaternion direction)
    {
        RpcLaunchBall(direction);
    }

    [ClientRpc]
    private void RpcLaunchBall(Quaternion direction)
    {
        fired = true;
        launchPos.rotation = direction;
        GameObject snowBall = Instantiate(currentSnowBall, launchPos.position, direction);

        snowBall.GetComponent<SnowBall>().SetLauncher(this);
        snowBall.GetComponent<Rigidbody>().velocity = launchPos.forward * currentLaunchForce;

        currentLaunchForce = minLaunchForce;
        nbSnowBallCreated--;
        if (aimSlider != null)
            aimSlider.gameObject.SetActive(false);
    }
}
