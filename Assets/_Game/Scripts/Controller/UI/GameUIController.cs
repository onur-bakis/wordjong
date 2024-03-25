using System;
using System.Collections.Generic;
using Scripts.Views.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Controller
{
    public class GameUIController:PanelBase
    {
        
        [SerializeField] private Image UndoUI;
        [SerializeField] private Image SubmitUI;
        [SerializeField] private Button UndoButton;
        [SerializeField] private Button SubmitButton;
        [SerializeField] private TextMeshProUGUI _levelTitleTMPRUI;
        [SerializeField] private TextMeshProUGUI _wordScoreTMPRUI;
        [SerializeField] private TextMeshProUGUI _topScoreTMPRUI;
        
        [SerializeField] private TextMeshProUGUI _addedWordTMPRUIPrefab;
        [SerializeField] private Transform _addedWordHolder;

        [SerializeField] private Sprite _spriteTikOpen;
        [SerializeField] private Sprite _spriteTikClose;

        private List<TextMeshProUGUI> _addedWordTMPRUIPool;
        private int wordCount;
        public override void Init()
        {
            base.Init();
            _addedWordTMPRUIPool = new List<TextMeshProUGUI>();
        }

        public void SetStartValues(string title)
        {
            _levelTitleTMPRUI.text = title;
            wordCount = 0;
            OnChangeUndoVisual(false);
            WordScoreChange(0,false);
            OnTopScoreChange(0);
        }

        public override void Show()
        {
            base.Show();
            wordCount = 0;

        }

        public override void HidePanel()
        {
            base.HidePanel();
            foreach (TextMeshProUGUI addedWordTMPRUI in _addedWordTMPRUIPool)
            {
                addedWordTMPRUI.text = String.Empty;
                addedWordTMPRUI.gameObject.SetActive(false);
            }
        }

        public void OnChangeUndoVisual(bool available)
        {
            UndoUI.color = available ? Color.white:Color.black;
            UndoButton.interactable = available;
        }

        public void WordScoreChange(int score, bool canSubmit)
        {
            _wordScoreTMPRUI.text = "Word Score: "+score.ToString();
            SubmitUI.color = canSubmit ? Color.white:Color.black;
            SubmitButton.image.sprite = canSubmit ? _spriteTikOpen : _spriteTikClose; 
            SubmitButton.interactable = canSubmit;
        }

        public void OnTopScoreChange(int topScore)
        {
            _topScoreTMPRUI.text = "Score: " + topScore;
            _wordScoreTMPRUI.text = "Word Score: "+0.ToString();
        }

        public void OnNewWordAdded(string addedWord)
        {
            if (wordCount >= _addedWordTMPRUIPool.Count)
            {
                TextMeshProUGUI addedWordTMPRUI= Instantiate(_addedWordTMPRUIPrefab, _addedWordHolder);
                _addedWordTMPRUIPool.Add(addedWordTMPRUI);
                addedWordTMPRUI.text = addedWord;
            }
            else
            {
                _addedWordTMPRUIPool[wordCount].gameObject.SetActive(true);
                _addedWordTMPRUIPool[wordCount].text = addedWord;
            }

            wordCount++;
        }
    }
}