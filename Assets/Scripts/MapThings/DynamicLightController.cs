using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicLightController : MapLightController
{
    [SerializeField] private float lightSwitchIntervalTime = 3f;
    [SerializeField][Range(-1,1)] private int initialType;

    private int curTypeIdx;
    // 0, 1, 2 red white green
    
    // Start is called before the first frame update
    protected override void Start()
    {
        typeOfLight = initialType;
        curTypeIdx = initialType + 1;
        base.Start();
        IEnumerator coroutine = UpdateColor();
        StartCoroutine(coroutine);
    }

    IEnumerator UpdateColor()
    {
        while (true)
        {
            yield return new WaitForSeconds(lightSwitchIntervalTime);
            if (playerIsIn)
            {
                playerEnergy.ConsumeSpeed += tmpSpeed * typeOfLight;
                if (tmpSpeed == 0)
                {
                    playerEnergy.ConsumeSpeed += originalConsumeSpeed;
                }
                playerLightController.LeaveLight();
            }
            curTypeIdx = (curTypeIdx + 1) % 3;
            typeOfLight = curTypeIdx - 1;
            if (playerIsIn)
            {
                OnTriggerEnterHelper(playerEnergy, playerLightController);
            }
            SetLightColor();
        }
    }
}
