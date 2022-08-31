using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class demo : MonoBehaviour
{
    public ComputeShader cs;
    public RenderTexture rt;
    // Start is called before the first frame update
    void Start()
    {
        rt = new RenderTexture(64, 64, 24);
        rt.enableRandomWrite = true;
        rt.Create();

        int kernel = cs.FindKernel("CSMain");
        cs.SetTexture(kernel, "Result", rt);
        cs.Dispatch(kernel, rt.width/8, rt.height/8, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(rt, destination);
    }
}
