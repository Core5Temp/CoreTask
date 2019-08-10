
using UnityEngine;
using UnityEngine.UI;

public class AfterGamePage : BasePage
{
    [SerializeField] private GameObject _newRecordContainer;
    [SerializeField] private GameObject _donoNewRecond;
    [SerializeField] private Text _nameText;
    
    private bool _isNewRecord;
    
    public override void Open()
    {
        _isNewRecord = LeaderBoardController.IsNewRecord();
        
        _newRecordContainer.SetActive(_isNewRecord);
        _donoNewRecond.SetActive(!_isNewRecord);
    }

    public override void Close()
    {
    }

    public void OkOnClick()
    {
        if(_isNewRecord)
            LeaderBoardController.SaveNewRecord(_nameText.text);
        
        PageManager.Open<TryAgainPage>();
    }
}
