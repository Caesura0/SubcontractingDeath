using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.UI
{
    public class HideUI : MonoBehaviour
    {
        [SerializeField] KeyCode HideKey = KeyCode.Escape;
        [SerializeField] GameObject PanelHolder = null;

        // Start is called before the first frame update
        void Start()
        {
            PanelHolder.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(HideKey))
            {
                Toggle();
            }
        }

        public void Toggle()
        {
            PanelHolder.SetActive(!PanelHolder.activeSelf);
        }
    }
}