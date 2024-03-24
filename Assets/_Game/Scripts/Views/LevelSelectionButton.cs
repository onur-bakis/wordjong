using Scripts.Controller.UI;
using DG.Tweening;
using strange.extensions.mediation.impl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Views
{
    public class LevelSelectionButton : View
    {
        [SerializeField] private Button _onPlayButton;
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private TextMeshProUGUI _highScore;
        [SerializeField] private GameObject _lockImage;

        private LevelSelectionPanel _levelSelectionPanel;
        private int _id;

        public void Init(LevelSelectionPanel levelSelectionPanel, int id, string name, int highScore,int lastUnlock)
        {
            _levelSelectionPanel = levelSelectionPanel;
            _levelName.text = "Level "+(id+1)+" - "+name;
            _id = id;
            bool locked = false;

            
            if (highScore > 0)
            {
                _highScore.text = "High Score: "+highScore;
                locked = false;
            }
            else if (id == 0)
            {
                _highScore.text = "No Score";
                locked = false;
            }
            else if (id == lastUnlock)
            {
                _highScore.text = "No Score";
                locked = false;
                PlayAnim();
            }
            else if (highScore == -1)
            {
                _highScore.text = "Locked Level";
                locked = true;
            }
            
            _onPlayButton.gameObject.SetActive(!locked);
            _lockImage.SetActive(locked);
        }

        public void PlayAnim()
        {
            _onPlayButton.transform.localScale = new Vector3(1f,0f,1f);
            _onPlayButton.transform.DOScale(Vector3.one, 1f);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _onPlayButton.onClick.AddListener(Play);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _onPlayButton.onClick.RemoveListener(Play);
        }

        private void Play()
        {
            _levelSelectionPanel.GoLevel(_id);
        }
    }
}