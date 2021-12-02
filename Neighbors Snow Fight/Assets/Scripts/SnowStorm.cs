using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowStorm : NetworkBehaviour
{
    [SerializeField] private ParticleSystem snowParticles;
    private SnowLayer[] layers;

    private bool isStormActive = false;

    private float addSnowMaxTimer = 0.5f;
    private float addSnowtimer = 0f;
    private int snowAdded = 0;
    private int maxSnowToAdd = 100;

    public bool IsActive()
    {
        return isStormActive;
    }

    [Server]
    public void StartSnowStorm()
    {
        isStormActive = true;
        layers = GameObject.FindObjectsOfType<SnowLayer>();
        GetComponent<ItemSpawner>().SetSpawnRate(0.5f);
        RpcToggleStorm();
    }

    private void Start()
    {
        if (isServer)
            snowParticles.Stop();
    }

    void Update()
    {
        if (!isServer) return;
        if (!isStormActive) return;

        addSnowtimer += Time.deltaTime;
        if (addSnowtimer >= addSnowMaxTimer && snowAdded <= maxSnowToAdd) {
            layers[Random.Range(0, layers.Length)].AddSnow(1);
            snowAdded++;
            addSnowtimer = 0f + Random.Range(-0.1f, 0.1f);
        }
    }

    [ClientRpc]
    private void RpcToggleStorm()
    {
        StartCoroutine(GradualFog());
        var emission = snowParticles.emission;
        emission.rateOverTime = 750;
    }

    private IEnumerator GradualFog()
    {
        RenderSettings.fog = true;
        for (int i = 0; i < 12; i++) {
            RenderSettings.fogDensity += 0.005f;
            yield return new WaitForSeconds(0.5f);
        }
    }

}
