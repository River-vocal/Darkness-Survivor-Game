using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField] public float MaxEnergy = 100f;
    [SerializeField] public float ConsumeSpeed = 1f;

    public event EventHandler OnEmpty;

    private float curEnergy;

    public float CurEnergy
    {
        get
        {
            return curEnergy;
        }
        set
        {
            value = Math.Min(value, MaxEnergy);
            value = Math.Max(value, 0f);
            setEnergy(value);
        }
    }

    private void setEnergy(float value)
    {
        curEnergy = value;
        if (curEnergy <= 0.0001f)
        {
            if (OnEmpty != null) OnEmpty(this, EventArgs.Empty);
        }
    }


    public void TakeDamage(float damage)
    {
        CurEnergy -= damage;
    }


    public float CurEnergyNormalized
    {
        get
        {
            return curEnergy / MaxEnergy;
        }
    }

    private void Awake()
    {
        curEnergy = MaxEnergy;
    }

    private void Update()
    {
        if (curEnergy > 0)
        {
            CurEnergy -= ConsumeSpeed * Time.deltaTime;
        }
    }
}
