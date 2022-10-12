using UnityEngine;
using UnityEngine.Events;
using TMPro;
using MyNamespace;

[System.Serializable]
public class MyIntEvent : UnityEvent<int>
{

}

[System.Serializable]
public class MyFloatColorEvent : UnityEvent<PColors, PColors, float>
{

}

[System.Serializable]
public class MyFloatEvent : UnityEvent<float>
{

}

// enum for type of slider
public enum SliderType
{
    Count,
    Force,
    Range,
    Gavity,
    Damping,
    Speed
}


public class SliderCode : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Slider slider;

    [SerializeField]
	private TextMeshProUGUI text = null;

    public PColors color1;
    public PColors color2;

    public SliderType sliderType;

    public Settings settingsValues;


    [SerializeField] private MyIntEvent m_intEvent;
    [SerializeField] private MyFloatColorEvent m_flaotColorEvent;
    [SerializeField] private MyFloatEvent m_flaotEvent;

    void Start()
    {

        settingsValues.RegisterResetPosEvent(loadSaveHandler);
        settingsValues.RegisterRandomizeRangeEvent(loadSaveHandler);

        // text.text = "";
        //set text to slider value
        // text.text = slider.value.ToString();

        switch (sliderType) {
            case SliderType.Count:
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
                break;
            case SliderType.Force:
                slider.onValueChanged.AddListener((v) =>
                {
                    text.text = v.ToString();
                    m_flaotColorEvent.Invoke(color1, color2, v);
                });
                text.text = settingsValues.forces[(int)color1][(int)color2].ToString();
                slider.value = settingsValues.forces[(int)color1][(int)color2];
                break;
            case SliderType.Range:
                slider.onValueChanged.AddListener((v) =>
                {
                    text.text = v.ToString();
                    m_flaotColorEvent.Invoke(color1, color2, v);
                });
                text.text = settingsValues.ranges[(int)color1][(int)color2].ToString();
                slider.value = settingsValues.ranges[(int)color1][(int)color2];
                break;
            case SliderType.Gavity:
                slider.onValueChanged.AddListener((v) =>
                {
                    text.text = v.ToString();
                    m_flaotEvent.Invoke(v);
                });
                text.text = settingsValues.gravity.ToString();
                slider.value = settingsValues.gravity;
                break;
            case SliderType.Damping:
                slider.onValueChanged.AddListener((v) =>
                {
                    text.text = v.ToString();
                    m_flaotEvent.Invoke(v);
                });
                text.text = settingsValues.damping.ToString();
                slider.value = settingsValues.damping;
                break;
            case SliderType.Speed:              
                slider.onValueChanged.AddListener((v) =>
                {
                    // show speed as a percentage with two decimal places
                    text.text = $"{v * 1000:F2}%";
                    m_flaotEvent.Invoke(v);
                });
                text.text = $"{settingsValues.speed * 1000:F2}%";
                slider.value = settingsValues.speed;
                break;
        }
    }

    public void loadSaveHandler(){
        switch (sliderType)
        {
            case SliderType.Count:
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
                break;
            case SliderType.Force:
                text.text = settingsValues.forces[(int)color1][(int)color2].ToString();
                slider.value = settingsValues.forces[(int)color1][(int)color2];
                break;
            case SliderType.Range:
                text.text = settingsValues.ranges[(int)color1][(int)color2].ToString();
                slider.value = settingsValues.ranges[(int)color1][(int)color2];
                break;
            case SliderType.Gavity:
                text.text = settingsValues.gravity.ToString();
                slider.value = settingsValues.gravity;
                break;
            case SliderType.Damping:
                text.text = settingsValues.damping.ToString();
                slider.value = settingsValues.damping;
                break;
            case SliderType.Speed:
                text.text = $"{settingsValues.speed * 1000:F2}%";
                slider.value = settingsValues.speed;
                break;
        }
    }

}
