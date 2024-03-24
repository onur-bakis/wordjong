using Scripts.Managers;
using Scripts.Views.Common;
using TMPro;
using UnityEngine;

public class EndPanelController : PanelBase
{
    [SerializeField] private TextMeshProUGUI highScore;
    public override void Init()
    {
        base.Init();
    }

    public override void Show()
    {
        base.Show();
        if (LevelDataManager.levelFinishParams.highScore)
        {
            highScore.gameObject.SetActive(true);
        }
        else
        {
            highScore.gameObject.SetActive(false);
        }
    }

    public override void HidePanel()
    {
        base.HidePanel();
    }
}
