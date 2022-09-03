using UnityEngine;
using UnityEngine.Events;
using MyNamespace;
// using UnityEngine.JSONSerializeModule;

[CreateAssetMenu(menuName = "My Assets/Setttings")]
[System.Serializable]
public class Settings : ScriptableObject
{
    [Header("Simulation Settings")]
    public bool darkTheme = true;
    public int yellow_count = 200;
    public int red_count = 1;
    public int green_count = 1;
    public int blue_count = 1;

    [Range(0, 1)] public float gravity = 1f;
    [Range(0, 1)] public float damping = 1f;
    [Range(0, 0.2f)] public float speed = 1f;

    [HideInInspector]
    public float yMin = -540.0f;
    [HideInInspector]
    public float yMax = 540.0f;
    [HideInInspector]
    public float xMin = -960.0f;
    [HideInInspector]
    public float xMax = 960.0f;

    public CountEvent OnCountChanged { get; set; }



    #region forces

    [Header("Forces")]
    [Range(-1, 1)] public float green_green = 0f;
    [Range(-1, 1)] public float green_red = 0f;
    [Range(-1, 1)] public float green_yellow = 0f;
    [Range(-1, 1)] public float green_blue = 0f;
    [Range(-1, 1)] public float red_red = 0f;
    [Range(-1, 1)] public float red_green = 0f;
    [Range(-1, 1)] public float red_yellow = 0f;
    [Range(-1, 1)] public float red_blue = 0f;
    [Range(-1, 1)] public float yellow_yellow = 0f;
    [Range(-1, 1)] public float yellow_red = 0f;
    [Range(-1, 1)] public float yellow_green = 0f;
    [Range(-1, 1)] public float yellow_blue = 0f;
    [Range(-1, 1)] public float blue_blue = 0f;
    [Range(-1, 1)] public float blue_red = 0f;
    [Range(-1, 1)] public float blue_yellow = 0f;
    [Range(-1, 1)] public float blue_green = 0f;


    [Range(0, 1000)] public float green_green_range = 100f;
    [Range(0, 1000)] public float green_red_range = 100f;
    [Range(0, 1000)] public float green_yellow_range = 100f;
    [Range(0, 1000)] public float green_blue_range = 100f;
    [Range(0, 1000)] public float red_red_range = 100f;
    [Range(0, 1000)] public float red_green_range = 100f;
    [Range(0, 1000)] public float red_yellow_range = 100f;
    [Range(0, 1000)] public float red_blue_range = 100f;
    [Range(0, 1000)] public float yellow_yellow_range = 100f;
    [Range(0, 1000)] public float yellow_red_range = 100f;
    [Range(0, 1000)] public float yellow_green_range = 100f;
    [Range(0, 1000)] public float yellow_blue_range = 100f;
    [Range(0, 1000)] public float blue_blue_range = 100f;
    [Range(0, 1000)] public float blue_red_range = 100f;
    [Range(0, 1000)] public float blue_yellow_range = 100f;
    [Range(0, 1000)] public float blue_green_range = 100f;

    [HideInInspector]
    public float [][] forces { get; set; }
    [HideInInspector]
    // public float [][] ranges { get; set; }
    // private float [][] ranges = new float[4][] {
    //     new float[4],
    //     new float[4],
    //     new float[4],
    //     new float[4]
    // };
    public float [][] ranges;

    #endregion


    public delegate void resetPos();
    private resetPos resetPosEvent;
    private resetPos randomizePosEvent;
    private resetPos validateEvent;
    private resetPos GPUEvent;
    private resetPos randomizeRangeEvent;
    public void RegisterResetPosEvent(resetPos resetPosEvent)
    {
        this.resetPosEvent += resetPosEvent;
    }
    public void RegisterChangeCountEvent(resetPos changeValidateEvent)
    {
        this.validateEvent += changeValidateEvent;
    }
    public void RegisterRandomizePosEvent(resetPos randomizePosEvent)
    {
        this.randomizePosEvent += randomizePosEvent;
    }
    public void RegisterGPUEvent(resetPos GPUEvent)
    {
        this.GPUEvent += GPUEvent;
    }
    public void RegisterRandomizeRangeEvent(resetPos randomizeRangeEvent)
    {
        this.randomizeRangeEvent += randomizeRangeEvent;
    }

    public void Reset()
    {
        forces = new float[4][]{
            new float[4]{yellow_yellow, yellow_red, yellow_green, yellow_blue},
            new float[4]{red_yellow, red_red, red_green, red_blue},
            new float[4]{green_yellow, green_red, green_green, green_blue},
            new float[4]{blue_yellow, blue_red, blue_green, blue_blue}
        };
        ranges = new float[4][]{
            new float[4]{yellow_yellow_range, yellow_red_range, yellow_green_range, yellow_blue_range},
            new float[4]{red_yellow_range, red_red_range, red_green_range, red_blue_range},
            new float[4]{green_yellow_range, green_red_range, green_green_range, green_blue_range},
            new float[4]{blue_yellow_range, blue_red_range, blue_green_range, blue_blue_range}
        };
        resetPosEvent();
    }
    public void RandomizePos()
    {
        randomizePosEvent();
    }
    public void RandomizeRangeEvent()
    {
        forces = new float[4][]{
            new float[4]{yellow_yellow, yellow_red, yellow_green, yellow_blue},
            new float[4]{red_yellow, red_red, red_green, red_blue},
            new float[4]{green_yellow, green_red, green_green, green_blue},
            new float[4]{blue_yellow, blue_red, blue_green, blue_blue}
        };
        ranges = new float[4][]{
            new float[4]{yellow_yellow_range, yellow_red_range, yellow_green_range, yellow_blue_range},
            new float[4]{red_yellow_range, red_red_range, red_green_range, red_blue_range},
            new float[4]{green_yellow_range, green_red_range, green_green_range, green_blue_range},
            new float[4]{blue_yellow_range, blue_red_range, blue_green_range, blue_blue_range}
        };
        randomizeRangeEvent();
    }
    public void GPUClick()
    {
        GPUEvent();
    }

    void OnValidate()
    {
        // validateEvent();
    }
    #region getters and setters

    public int getYellow_count()
    {
        return this.yellow_count;
    }
    public void setYellow_count(int yellow_count)
    {
        this.yellow_count = yellow_count;
        OnCountChanged.Invoke(PColors.Yellow);
    }
    public int getRed_count()
    {
        return this.red_count;
    }
    public void setRed_count(int red_count)
    {
        this.red_count = red_count;
        OnCountChanged.Invoke(PColors.Red);
    }
    public int getGreen_count()
    {
        return this.green_count;
    }
    public void setGreen_count(int green_count)
    {
        this.green_count = green_count;
        OnCountChanged.Invoke(PColors.Green);
    }
    public int getBlue_count()
    {
        return this.blue_count;
    }
    public void setBlue_count(int blue_count)
    {
        this.blue_count = blue_count;
        OnCountChanged.Invoke(PColors.Blue);
    }

    public void setRanges(float[][] ranges)
    {
        this.ranges = ranges;
    }

    public void setColorsForce(PColors color1, PColors color2, float force)
    {
        switch (color1)
        {
            case PColors.Green:
                switch (color2)
                {
                    case PColors.Green:
                        green_green = force;
                        break;
                    case PColors.Red:
                        green_red = force;
                        break;
                    case PColors.Yellow:
                        green_yellow = force;
                        break;
                    case PColors.Blue:
                        green_blue = force;
                        break;
                }
                break;
            case PColors.Red:
                switch (color2)
                {
                    case PColors.Green:
                        red_green = force;
                        break;
                    case PColors.Red:
                        red_red = force;
                        break;
                    case PColors.Yellow:
                        red_yellow = force;
                        break;
                    case PColors.Blue:
                        red_blue = force;
                        break;
                }
                break;
            case PColors.Yellow:
                switch (color2)
                {
                    case PColors.Green:
                        yellow_green = force;
                        break;
                    case PColors.Red:
                        yellow_red = force;
                        break;
                    case PColors.Yellow:
                        yellow_yellow = force;
                        break;
                    case PColors.Blue:
                        yellow_blue = force;
                        break;
                }
                break;
            case PColors.Blue:
                switch (color2)
                {
                    case PColors.Green:
                        blue_green = force;
                        break;
                    case PColors.Red:
                        blue_red = force;
                        break;
                    case PColors.Yellow:
                        blue_yellow = force;
                        break;
                    case PColors.Blue:
                        blue_blue = force;
                        break;
                }
                break;
        }
    }

    public void setColorsRange(PColors color1, PColors color2, float range)
    {
        switch (color1)
        {
            case PColors.Green:
                switch (color2)
                {
                    case PColors.Green:
                        green_green_range = range;
                        break;
                    case PColors.Red:
                        green_red_range = range;
                        break;
                    case PColors.Yellow:
                        green_yellow_range = range;
                        break;
                    case PColors.Blue:
                        green_blue_range = range;
                        break;
                }
                break;
            case PColors.Red:
                switch (color2)
                {
                    case PColors.Green:
                        red_green_range = range;
                        break;
                    case PColors.Red:
                        red_red_range = range;
                        break;
                    case PColors.Yellow:
                        red_yellow_range = range;
                        break;
                    case PColors.Blue:
                        red_blue_range = range;
                        break;
                }
                break;
            case PColors.Yellow:
                switch (color2)
                {
                    case PColors.Green:
                        yellow_green_range = range;
                        break;
                    case PColors.Red:
                        yellow_red_range = range;
                        break;
                    case PColors.Yellow:
                        yellow_yellow_range = range;
                        break;
                    case PColors.Blue:
                        yellow_blue_range = range;
                        break;
                }
                break;
            case PColors.Blue:
                switch (color2)
                {
                    case PColors.Green:
                        blue_green_range = range;
                        break;
                    case PColors.Red:
                        blue_red_range = range;
                        break;
                    case PColors.Yellow:
                        blue_yellow_range = range;
                        break;
                    case PColors.Blue:
                        blue_blue_range = range;
                        break;
                }
                break;
        }
    }

    #endregion

}


public class CountEvent : UnityEvent<PColors>
{
    public CountEvent(){}
}