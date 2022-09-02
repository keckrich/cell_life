using UnityEngine;
using UnityEngine.Events;
using TMPro;


[System.Serializable]
public class MyIntEvent : UnityEvent<int>
{

}

public class SliderCode : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Slider slider;

    [SerializeField]
	private TextMeshProUGUI text = null;


    [SerializeField] private MyIntEvent m_intEvent;
    void Start()
    {

        // text.text = "";
        //set text to slider value
        text.text = slider.value.ToString();

        slider.onValueChanged.AddListener((v) =>
        {
            text.text = v.ToString() ;
            m_intEvent.Invoke((int)v);
        });
    }


}
