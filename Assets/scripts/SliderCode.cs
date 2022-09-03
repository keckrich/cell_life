using UnityEngine;
using UnityEngine.Events;
using TMPro;
using MyNamespace;

[System.Serializable]
public class MyIntEvent : UnityEvent<int>
{

}

[System.Serializable]
public class MyFloatEvent : UnityEvent<PColors, PColors, float>
{

}


public class SliderCode : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Slider slider;

    [SerializeField]
	private TextMeshProUGUI text = null;

    public PColors color1;
    public PColors color2;

    public bool isCount;
    public bool isForce;
    public bool isRange;

    public Settings settingsValues;


    [SerializeField] private MyIntEvent m_intEvent;
    [SerializeField] private MyFloatEvent m_flaotEvent;
    void Start()
    {

        // text.text = "";
        //set text to slider value
        // text.text = slider.value.ToString();

        if (isCount)
        {
            switch (color1)
            {
                case PColors.Yellow:
                    text.text = settingsValues.yellow_count.ToString();
                    slider.value = settingsValues.yellow_count;
                    break;
                case PColors.Red:
                    text.text = settingsValues.red_count.ToString();
                    slider.value = settingsValues.red_count;
                    break;
                case PColors.Green:
                    text.text = settingsValues.green_count.ToString();
                    slider.value = settingsValues.green_count;
                    break;
                case PColors.Blue:
                    text.text = settingsValues.blue_count.ToString();
                    slider.value = settingsValues.blue_count;
                    break;
            }
            slider.onValueChanged.AddListener((v) =>
            {
                text.text = v.ToString();
                m_intEvent.Invoke((int)v);
            });
        }
        else if (isForce)
        {
            slider.onValueChanged.AddListener((v) =>
            {
                text.text = v.ToString();
                m_flaotEvent.Invoke(color1, color2, v);
            });
            text.text = settingsValues.forces[(int)color1][(int)color2].ToString();
            slider.value = settingsValues.forces[(int)color1][(int)color2];
        }
        // else if (isRange)
        // {
        //     slider.onValueChanged.AddListener((v) =>
        //     {
        //         m_intEvent.Invoke((int)v);
        //     });
        // }
    }


}
