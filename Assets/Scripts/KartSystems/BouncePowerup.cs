using KartGame.KartSystems;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(ArcadeKart))]
public class BouncePowerup : MonoBehaviour
{
    public int maxConcurrent = 3;
    public ArcadeKart.StatPowerup boostStats = new ArcadeKart.StatPowerup
    {
        MaxTime = 5
    };

    private int currentApplied;

    private ArcadeKart kart;

    private void Start()
    {
        currentApplied = 0;
        kart = GetComponent<ArcadeKart>();
        Assert.IsNotNull(kart);
    }

    private void OnDestroy()
    {
        currentApplied = 0;
        StopAllCoroutines();

    }

    public void ApplyBouncePowerup()
    {
        if (currentApplied >= maxConcurrent)
        {
            return;
        }
        currentApplied++;
        ArcadeKart.StatPowerup stats = new ArcadeKart.StatPowerup
        {
            MaxTime = boostStats.MaxTime,
            PowerUpID = boostStats.PowerUpID,
            modifiers = new ArcadeKart.Stats
            {
                TopSpeed = boostStats.modifiers.TopSpeed,
                Acceleration = boostStats.modifiers.Acceleration,
            }
        };
        kart.AddPowerup(stats);
        StartCoroutine(DecrementCount());
    }

    private IEnumerator DecrementCount()
    {
        yield return new WaitForSeconds(boostStats.MaxTime);
        currentApplied--;
    }
}
