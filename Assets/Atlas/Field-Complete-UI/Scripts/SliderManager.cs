using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Michsky.UI.FieldCompleteMainMenu
{
    public class SliderManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("TEXTS")]
        public Text valueText;
        public Text mainValueText;

        [Header("SETTINGS")]
        public bool showValue = true;
        public bool showMainValue = true;
        public bool useIntegerValue = false;

        private Slider mainSlider;
        private Animator sliderAnimator;

        void Start()
        {
            mainSlider = GetComponent<Slider>();
            sliderAnimator = GetComponent<Animator>();

            if (!showValue) Destroy(valueText);
            if (!showMainValue) Destroy(mainValueText);
        }

        void Update()
        {
            if (useIntegerValue == true)
            {
               // valueText.text = Mathf.Round(mainSlider.value * 1.0f).ToString();
                mainValueText.text = Mathf.Round(mainSlider.value * 1.0f).ToString();
            }
            else
            {
                //valueText.text = mainSlider.value + "%";
                mainValueText.text = mainSlider.value + "%";
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            sliderAnimator.Play("Value In");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            sliderAnimator.Play("Value Out");
        }
    }
}