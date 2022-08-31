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

    #endregion


    public delegate void resetPos();
    private resetPos resetPosEvent;
    private resetPos validateEvent;
    public void RegisterResetPosEvent(resetPos resetPosEvent)
    {
        this.resetPosEvent += resetPosEvent;
    }
    public void RegisterChangeCountEvent(resetPos changeValidateEvent)
    {
        this.validateEvent += changeValidateEvent;
    }

    public void Reset()
    {
        resetPosEvent();
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
