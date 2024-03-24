using System.Collections.Generic;
using Scripts.Context.Signals;
using Scripts.Enums;
using Scripts.Views.Common;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Scripts.Controller.UI
{
    public class UIPanelController : View
    {
        [Inject] public CoreUISignals CoreUISignals { get; set; }

        [SerializeField] private List<GameObject> layers = new List<GameObject>();

        private Dictionary<UIPanelTypes, PanelBase> loadedLayers;

        private PanelBase _currentActivePanel;
        
        private PanelBase _cachePanelBase;
        protected override void Awake()
        {
            base.Awake();
            loadedLayers = new Dictionary<UIPanelTypes, PanelBase>();

            for (int i = 0; i < 4; i++)
            {
                GetPanel((UIPanelTypes)i,i).gameObject.SetActive(false);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreUISignals.onOpenPanel += OnOpenPanel;
        }

        private void UnsubscribeEvents()
        {
            CoreUISignals.onOpenPanel -= OnOpenPanel;
        }

        protected override void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void OnOpenPanel(UIPanelTypes panel)
        {
            OpenPanel(panel, (int)panel);
        }

        public PanelBase GetPanel(UIPanelTypes panel,int layerValue)
        {
            if (loadedLayers.ContainsKey(panel))
            {
                return loadedLayers[panel];
            }
            _cachePanelBase = Instantiate(Resources.Load<PanelBase>($"Panels/{panel}Panel"),
                layers[layerValue].transform);
            
            _cachePanelBase.Init();
            loadedLayers.Add(panel,_cachePanelBase);
            return _cachePanelBase;
        }

        private void OpenPanel(UIPanelTypes panel,int layer)
        {
            if (_currentActivePanel != null)
            {
                _currentActivePanel.HidePanel();
            }
            _currentActivePanel = GetPanel(panel,layer);
            _currentActivePanel.Show();
        }
        
        private void OnCloseAllPanel()
        {
            foreach (var layer in layers)
            {
                for (int i = 0; i < layer.transform.childCount; i++)
                {
                    Destroy(layer.transform.GetChild(i).gameObject);
                }
            }
        }
    }
}