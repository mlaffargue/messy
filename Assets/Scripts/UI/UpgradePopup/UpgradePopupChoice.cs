using System.Collections;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Messy
{
    public class UpgradePopupChoice : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public Enum upgrade;

        private Image canvasImage;
        private TMPro.TextMeshProUGUI text;
        private Color defaultCanvasColor;
        private Color defaultTextColor;

        // Start is called before the first frame update
        void Start()
        {
            canvasImage = gameObject.GetComponent<Image>();
            text = gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            defaultCanvasColor = canvasImage.color;
            defaultTextColor = text.color;
        }

        // Update is called once per frame
        void Update()
        {
            text.text = UpgradesEnumHelper.GetText(upgrade);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            canvasImage.color = defaultCanvasColor;
            text.color = defaultTextColor;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            canvasImage.color = Color.grey;
            text.color = Color.red;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            GameManager gameManager = ObjectRetriever.GetGameManager();
            Player player = ObjectRetriever.GetPlayer();
            player.ApplyUpgrade(upgrade);
            gameManager.UpgradeChoiceDone(upgrade);
        }
    }
}