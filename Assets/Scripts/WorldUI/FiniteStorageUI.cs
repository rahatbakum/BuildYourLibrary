using UnityEngine;
using System;

public class FiniteStorageUI : WorldUI
{
    [SerializeField] private FiniteGettingStorage _target;

    protected override void UpdateState()
    {
        int resourseTypeAmount = Enum.GetNames(typeof(ResourceType)).Length;
        string resultState = "";

        for(int i = 0; i < resourseTypeAmount; i++)
        {
            int requiringResourceAmount = _target.GetRequiringResourceAmount((ResourceType) i);
            if(requiringResourceAmount > 0)
            {
                resultState += ((ResourceType) i).ToString().ToUpper()[0] + ": " + requiringResourceAmount.ToString() + "\n";
            }   
        }

        ChangeState(resultState);
    }
}
