using System;
using UnityEngine;

namespace Menu.SwitchPanel
{
    public class SwitchPanels : MonoBehaviour
    {
        [SerializeField] private Panel[] panels;
        [SerializeField] private Transform[] selectedPanels;

        public event Action<int> OnSwitchPanel;
        
        private int _numPanel;

        private void Start()
        {
            Switch();
        }

        public void ClickSwitchPanel(int num)
        {
            if(_numPanel == num)
                return;
            
            _numPanel = num;
            OnSwitchPanel?.Invoke(_numPanel);
            Switch();
        }

        private void Switch()
        {
            for (int i = 0; i < panels.Length; i++)
            {
                if (i == _numPanel)
                {
                    selectedPanels[i].gameObject.SetActive(true);
                    panels[i].ActivatorPanel(true);
                }
                else
                {
                    selectedPanels[i].gameObject.SetActive(false);
                    panels[i].ActivatorPanel(false);
                }
            }
        }
    }
}
