using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace JengaGame.UI.Info
{
    public class InfoWindow : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI textField;

        [SerializeField]
        private Transform windowRoot;

        public void ShowInfoWindow(string text)
        {
            textField.text = text;

            windowRoot.gameObject.SetActive(true);
        }

        public void CloseInfoWindow()
        {
            windowRoot.gameObject.SetActive(false);
        }
    }
}