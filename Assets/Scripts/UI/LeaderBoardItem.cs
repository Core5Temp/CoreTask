using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardItem : MonoBehaviour
{
    [SerializeField] private Text _number;
    [SerializeField] private Text _name;
    [SerializeField] private Text _score;

    public void InitValue(string number, string name, string score)
    {
        _number.text = number;
        _name.text = name;
        _score.text = score;
    }
}
