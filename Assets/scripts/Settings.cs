using UnityEngine;

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

    [HideInInspector]
    public float yMin = -540.0f;
    [HideInInspector]
    public float yMax = 540.0f;
    [HideInInspector]
    public float xMin = -960.0f;
    [HideInInspector]
    public float xMax = 960.0f;

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
    

    #endregion


    public delegate void resetPos();
    private resetPos resetPosEvent;
    private resetPos randomizePosEvent;
    private resetPos validateEvent;
    private resetPos GPUEvent;
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

    public void Reset()
    {
        resetPosEvent();
    }
    public void Randomize()
    {
        randomizePosEvent();
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
    }
    public int getRed_count()
    {
        return this.red_count;
    }
    public void setRed_count(int red_count)
    {
        this.red_count = red_count;
    }
    public int getGreen_count()
    {
        return this.green_count;
    }
    public void setGreen_count(int green_count)
    {
        this.green_count = green_count;
    }
    public int getBlue_count()
    {
        return this.blue_count;
    }
    public void setBlue_count(int blue_count)
    {
        this.blue_count = blue_count;
    }
    #endregion
}
