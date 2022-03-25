using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryAnimator : MonoBehaviour
{

    [SerializeField] private Transform[] _slots;
    [SerializeField] private GameObject _bookShelfPrefab;
    private int _currentSlot = 0;


    public void StartMaking()
    {

    }

    public void FinishMaking()
    {
        Instantiate(_bookShelfPrefab, _slots[_currentSlot]);
        _currentSlot++;
    }
}
