using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] private Rigidbody baseSnowBall;
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

    private int maxSnowBall = 3;
    private int nbSnowBallCreated = 0;
    private Rigidbody currentSnowBall;

    public void ReplaceBall(Rigidbody newBall)
    {
        currentSnowBall = newBall;
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
        nbSnowBallCreated = 3;
    }

    void Update()
    {
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

        GetComponent<PlayerMouvement>().SetSpeed(0.2f);
        yield return new WaitForSeconds(1.5f);

        gauge.UseSnow(1);
        nbSnowBallCreated = maxSnowBall;
        GetComponent<PlayerMouvement>().SetSpeed(1f);

        currentSnowBall = baseSnowBall;
    }

    private void UpdateAndHandleShootingState()
    {
        if (!CanLaunch()) return;

        if (currentLaunchForce >= maxLaunchForce && !fired) {
            currentLaunchForce = maxLaunchForce;
            LaunchBall();
        } else if (Input.GetKeyDown(KeyCode.Q)) {
            fired = false;
            currentLaunchForce = minLaunchForce;
        } else if (Input.GetKey(KeyCode.Q) && !fired) {
            currentLaunchForce += chargeSpeed * Time.deltaTime;
            aimSlider.gameObject.SetActive(true);
            aimSlider.value = currentLaunchForce;
        } else if (Input.GetKeyUp(KeyCode.Q) && !fired) {
            LaunchBall();
        }
    }

    private void LaunchBall()
    {
        fired = true;
        var direction = GetComponentInChildren<MouseLook>().GetDirection();
        var snowBall = Instantiate(currentSnowBall, launchPos.position, direction);
        launchPos.rotation = direction;
        snowBall.velocity = launchPos.forward * currentLaunchForce;

        currentLaunchForce = minLaunchForce;
        nbSnowBallCreated--;
        Debug.Log("Ball Left : " + nbSnowBallCreated);
        aimSlider.gameObject.SetActive(false);
    }

    private bool CanLaunch()
    {
        return nbSnowBallCreated > 0;
    }
}
