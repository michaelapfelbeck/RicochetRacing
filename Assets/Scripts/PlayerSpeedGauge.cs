using KartGame.KartSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeedGauge : MonoBehaviour
{
    public float defaultMax = 70;
    public float extensionWidth = 40;

    [SerializeField]
    private string boostId = "BounceBoost";
    [SerializeField]
    private ArcadeKart kart;
    [SerializeField]
    private RectTransform speedGauge;
    [SerializeField]
    private GameObject extension1;
    [SerializeField]
    private GameObject extension2;
    [SerializeField]
    private GameObject extension3;

    void Start()
    {
        if(speedGauge == null)
        {
            enabled = false;
        }
        extension1?.SetActive(false);
        extension2?.SetActive(false);
        extension3?.SetActive(false);
    }

    void Update()
    {
        if(kart == null)
        {
            speedGauge.sizeDelta = new Vector2(defaultMax, speedGauge.sizeDelta.y);
            return;
        }
        //LocalSpeed(), CurrentStats
        SetSpeedGaugeWidth();
        SetExtensionVisibility();
    }

    private void SetSpeedGaugeWidth()
    {
        float kartSpeed = kart.LocalSpeed();
        int powerupCount = kart.GetPowerupCount(boostId);
        float maxWidth = defaultMax + extensionWidth * powerupCount;
        speedGauge.sizeDelta = new Vector2(maxWidth * kartSpeed, speedGauge.sizeDelta.y);
    }

    private void SetExtensionVisibility()
    {
        int count = kart.GetPowerupCount(boostId);

        if (count == 0)
        {
            extension1.SetActive(false);
            extension2.SetActive(false);
            extension3.SetActive(false);
        }
        else if (count == 1)
        {
            extension1.SetActive(true);
            extension2.SetActive(false);
            extension3.SetActive(false);
        }
        else if (count == 2)
        {
            extension1.SetActive(true);
            extension2.SetActive(true);
            extension3.SetActive(false);
        }
        else if (count == 3)
        {
            extension1.SetActive(true);
            extension2.SetActive(true);
            extension3.SetActive(true);
        }
        else
        {
            extension1.SetActive(false);
            extension2.SetActive(false);
            extension3.SetActive(false);
        }

    }
}
