using Scripts.Managers;
using Scripts.Views.Common;
using TMPro;
using UnityEngine;

public class EndPanelController : PanelBase
{
    [SerializeField] private TextMeshProUGUI highScore;
    [SerializeField] private ParticleSystem[] winAlways;
    [SerializeField] private ParticleSystem[] winHighScore;
    [SerializeField] private ParticleSystem[] winClearOnly;

    private bool highScoreWin;
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
            highScoreWin = true;
        }
        else
        {
            highScore.gameObject.SetActive(false);
            highScoreWin = false;
        }
    }

    public override void OnShowComplete()
    {
        base.OnShowComplete();
        if (highScoreWin)
        {
            foreach (var particles in winAlways)
            {
                particles.gameObject.SetActive(true);
                particles.Play();
            }
            foreach (var particles in winHighScore)
            {
                particles.gameObject.SetActive(true);
                particles.Play();
            }
        }
        else
        {
            foreach (var particles in winAlways)
            {
                particles.gameObject.SetActive(true);
                particles.Play();
            }
            foreach (var particles in winClearOnly)
            {
                particles.gameObject.SetActive(true);
                particles.Play();
            }
        }
    }

    public override void HidePanel()
    {
        base.HidePanel();
        foreach (var particles in winAlways)
        {
            particles.gameObject.SetActive(false);
            particles.Stop();
        }
        foreach (var particles in winHighScore)
        {
            particles.gameObject.SetActive(false);
            particles.Stop();
        }
        foreach (var particles in winClearOnly)
        {
            particles.gameObject.SetActive(false);
            particles.Stop();
        }
    }
}
