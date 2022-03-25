using UnityEngine;
using System;
using UnityEngine.UI;

public abstract class WorldUI : MonoBehaviour
{
    private string _state = "null";
    [SerializeField] private bool isVisible = false;
    [SerializeField] private Text _text;
    [SerializeField] private CanvasGroup _canvasGroup;

    private void Start()
    {
        UpdateState();
        ForceApplyVisibility();
    }

    private void ForceApplyVisibility()
    {
        if(isVisible)
            _canvasGroup.alpha = 1f;
        else
            _canvasGroup.alpha = 0f;
    }

    protected virtual void ApplyVisibility()
    {
        //Start Animation
        if(isVisible)
            _canvasGroup.alpha = 1f;
        else
            _canvasGroup.alpha = 0f;
    }

    protected void ChangeState(string newState)
    {
        _state = newState;
        if(_text.text == null)
            return;
        _text.text = _state;
    }

    private void ChangeVisibility(bool visibility)
    {
        isVisible = visibility;
        ApplyVisibility();
    }

    protected abstract void UpdateState();

    public virtual void ShowState()
    {
        UpdateState();
        ChangeVisibility(true);
    }

    public virtual void Hide()
    {
        ChangeVisibility(false);
    }
}
