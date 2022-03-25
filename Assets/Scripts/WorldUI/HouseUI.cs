using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseUI : WorldUI
{
    [SerializeField] private House _target;

    protected override void UpdateState()
    {
        HouseState factoryState = _target.ThisHouseState;

        switch(factoryState)
        {
            case HouseState.GivingStorageIsFull:
                ChangeState("FULL");
                break;
            default:
                ChangeState("");
                break;
        }
    }
}
