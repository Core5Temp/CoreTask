
using UnityEngine;

public abstract class BasePage : MonoBehaviour
{
    [SerializeField] private PageState _pageState;

    private Canvas _canvas;

    public PageState PageState
    {
        get => _pageState;
        set
        {
            _pageState = value;
            _canvas.enabled = _pageState == PageState.Open;
        }
    }

    public virtual void Init()
    {
        _canvas = gameObject.GetComponent<Canvas>();
        PageState = _pageState;
    }

    public abstract void Open();
    public abstract void Close();
}

public enum PageState
{
    Open,
    Close
}

public interface IStartPage
{
}