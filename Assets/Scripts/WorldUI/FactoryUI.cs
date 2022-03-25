using UnityEngine;

public class FactoryUI : WorldUI
{
    [SerializeField] private Factory _target;

    protected override void UpdateState()
    {
        FactoryState factoryState = _target.ThisFactoryState;

        switch(factoryState)
        {
            case FactoryState.GettingStorageIsEmpty:
                ChangeState(_target.InputResource.ToString().ToUpper()[0] + ": " + _target.AmountOfInputResource.ToString());
                break;
            case FactoryState.GivingStorageIsFull:
                ChangeState("FULL");
                break;
            default:
                ChangeState("");
                break;
        }

    }
}
