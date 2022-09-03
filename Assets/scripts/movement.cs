using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyNamespace;

public class movement : MonoBehaviour
{

    public int seed = 222;
    int count = 0;

    public float radius = 2000f;
    public Settings settingsValues;

    public GameObject particleObj;

    Particle[] yellowArray;
    Particle[] redArray;
    Particle[] greenArray;
    Particle[] blueArray;
    Particle[][] particleArray;

    public ComputeShader computeShader;
    public ComputeShader computeShaderHybrid;

    private float WIDTH;
    private float HEIGHT;

    private Dictionary<int, GameObject> particleDict = new Dictionary<int, GameObject>();

    public struct Particle
    {
        public float fx;
        public float fy;
        public float x;
        public float y;
        public float vx;
        public float vy;
        public Color color;
        public int objID;

        public Particle(float fx, float fy, float x, float y, float vx, float vy, Color color, int obj)
        {
            this.fx = fx;
            this.fy = fy;
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
            this.color = color;
            this.objID = obj;
        }
    }

    public struct GPUParticle
    {
        public float x;
        public float y;
        public float vx;
        public float vy;

        public GPUParticle(float x, float y, float vx, float vy)
        {
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
        }

        public GPUParticle(Particle p)
        {
            this.x = p.x;
            this.y = p.y;
            this.vx = p.vx;
            this.vy = p.vy;
        }
    }

    public struct HybridRule{
        public float range;
        public float g;
        public float fx;
        public float fy;
        public Particle p;
        
        public HybridRule(float range, float g, float fx, float fy, Particle p){
            this.range = range;
            this.g = g;
            this.fx = fx;
            this.fy = fy;
            this.p = p;
        }
    };

    struct Rule{
        uint color1;
        uint color2;
        float force;
        float dampening;
        float range;

        public Rule(uint color1, uint color2, float force, float dampening, float range)
        {
            this.color1 = color1;
            this.color2 = color2;
            this.force = force;
            this.dampening = dampening;
            this.range = range;
        }
    };

    // Start is called before the first frame update
    void Start()
    {
        WIDTH = settingsValues.xMax - settingsValues.xMin;
        HEIGHT = settingsValues.yMax - settingsValues.yMin;

        string[] split = "test".Split('_');

        settingsValues.OnCountChanged = new CountEvent();

        settingsValues.RegisterResetPosEvent(ResetPosition);
        settingsValues.RegisterRandomizePosEvent(RandomizeConnections);
        
        settingsValues.OnCountChanged.AddListener(c => updateCount(c));

        Random.InitState(seed);

        yellowArray = Create(settingsValues.yellow_count, Color.yellow);
        redArray = Create(settingsValues.red_count, Color.red);
        greenArray = Create(settingsValues.green_count, new Color(0.08627f, 0.3686f, 0.1137f));
        blueArray = Create(settingsValues.blue_count, Color.blue);

        particleArray = new Particle[][] { yellowArray, redArray, greenArray, blueArray };

//         settingsValues.yellow_count.
// ;


    }

    // Update is called once per frame
    void Update()
    {
        if (count >= (settingsValues.speed * 100)){
            count = 0;


            // CPURender(); 
            GPURender();
            // HybridRender();
            draw();



        }
        else count++;
    }

    void resetPos()
    {
        // Debug.Log("Resetting position");

        for (int i = 0; i < particleArray.Length; i++)
        {
            for (int j = 0; j < particleArray[i].Length; j++)
            {

                particleArray[i][j].fx = 0;
                particleArray[i][j].fy = 0;
                particleArray[i][j].vx = 0;
                particleArray[i][j].vy = 0;
                particleArray[i][j].x = Random.Range(settingsValues.xMin, settingsValues.xMax);
                particleArray[i][j].y = Random.Range(settingsValues.yMin, settingsValues.yMax);
            }
        }
        draw();

    }
    void RandomizeConnections(){
        settingsValues.green_green = expDist();
        settingsValues.green_red = expDist();
        settingsValues.green_yellow = expDist();
        settingsValues.green_blue = expDist();
        settingsValues.red_red = expDist();
        settingsValues.red_green = expDist();
        settingsValues.red_yellow = expDist();
        settingsValues.red_blue = expDist();
        settingsValues.yellow_yellow = expDist();
        settingsValues.yellow_green = expDist();
        settingsValues.yellow_red = expDist();
        settingsValues.yellow_blue = expDist();
        settingsValues.blue_blue = expDist();
        settingsValues.blue_green = expDist();
        settingsValues.blue_red = expDist();
        settingsValues.blue_yellow = expDist();
    }
    void updateCount(PColors color){
        switch(color){
            case PColors.Yellow:
                updateCountHelper(ref yellowArray, settingsValues.yellow_count);
                break;
            case PColors.Red:
                updateCountHelper(ref redArray, settingsValues.red_count);
                break;
            case PColors.Green:
                updateCountHelper(ref greenArray, settingsValues.green_count);
                break;
            case PColors.Blue:
                updateCountHelper(ref blueArray, settingsValues.blue_count);
                break;
        }
        particleArray = new Particle[][] { yellowArray, redArray, greenArray, blueArray };
    }
    void updateCountHelper(ref Particle[] pArray, int newSize){
        int oldSize = pArray.Length;

        // if (oldSize == 1 && pArray[0].objID == 0){
        //     pArray = new Particle[0];
        // }

        if (newSize == 0){
            for (int i = newSize; i < oldSize; i++){
                Destroy(particleDict[pArray[i].objID]);
                particleDict.Remove(pArray[i].objID);
            }
            pArray = Create(newSize, pArray[0].color);
        }
        else if (oldSize > newSize){
            for (int i = newSize; i < oldSize; i++){
                Destroy(particleDict[pArray[i].objID]);
                particleDict.Remove(pArray[i].objID);
            }
            System.Array.Resize(ref pArray, newSize);
        }
        else if (oldSize < newSize){
            Particle[] newArray = Create(newSize - oldSize, pArray[0].color);
            System.Array.Resize(ref pArray, newSize);
            System.Array.Copy(newArray, 0, pArray, oldSize, newArray.Length);

            if (pArray[0].objID == 0){
                // remove the first element of pArray
                pArray = pArray.Skip(1).ToArray();
            }

        }
    }
    void RandomizeRanges(){
        settingsValues.green_green_range = Random.Range(0f, 1000f);
        settingsValues.green_red_range = Random.Range(0f, 1000f);
        settingsValues.green_yellow_range = Random.Range(0f, 1000f);
        settingsValues.green_blue_range = Random.Range(0f, 1000f);
        settingsValues.red_red_range = Random.Range(0f, 1000f);
        settingsValues.red_green_range = Random.Range(0f, 1000f);
        settingsValues.red_yellow_range = Random.Range(0f, 1000f);
        settingsValues.red_blue_range = Random.Range(0f, 1000f);
        settingsValues.yellow_yellow_range = Random.Range(0f, 1000f);
        settingsValues.yellow_green_range = Random.Range(0f, 1000f);
        settingsValues.yellow_red_range = Random.Range(0f, 1000f);
        settingsValues.yellow_blue_range = Random.Range(0f, 1000f);
        settingsValues.blue_blue_range = Random.Range(0f, 1000f);
        settingsValues.blue_green_range = Random.Range(0f, 1000f);
        settingsValues.blue_red_range = Random.Range(0f, 1000f);
        settingsValues.blue_yellow_range = Random.Range(0f, 1000f);
    }
    void ResetPosition()
    {
        Debug.Log("Resetting count");

        for (int i = 0; i < particleArray.Length; i++)
        {
            for (int j = 0; j < particleArray[i].Length; j++)
            {
                // check if it is a real particle or a placeholder
                if (particleArray[i][j].objID != 0){
                    GameObject go = particleDict[particleArray[i][j].objID];
                    GameObject.Destroy(go); 
                }
            }
        }

        particleDict.Clear();

        yellowArray = Create(settingsValues.yellow_count, Color.yellow);
        redArray = Create(settingsValues.red_count, Color.red);
        greenArray = Create(settingsValues.green_count, new Color(0.08627f, 0.3686f, 0.1137f));
        blueArray = Create(settingsValues.blue_count, Color.blue);

        particleArray = new Particle[][] { yellowArray, redArray, greenArray, blueArray };

        draw();
    }
    Particle[] Create(int number, Color color)
    {
        Particle[] result = new Particle[number];
        SpriteRenderer spriteRenderer;

        for (int i = 0; i < number; i++)
        {
            GameObject go = Instantiate(particleObj, new Vector3(Random.Range(settingsValues.xMin, settingsValues.xMax), Random.Range(settingsValues.yMin, settingsValues.yMax), 0f), Quaternion.identity, particleObj.transform.parent);
            spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
            result[i] = new Particle(0f,0f, go.transform.position.x, go.transform.position.y, 0f, 0f, color, go.GetInstanceID());
            particleDict.Add(go.GetInstanceID(), go);

            // go.transform.parent = transform;
        }

        if (number == 0){
            result = new Particle[1];
            result[0] = new Particle(0f,0f, 100000f, 100000f, 0f, 0f, color, 0);
        }

        return result;
    }
    Particle[] CreateDebug(int number, Color color)
    {
        Particle[] result = new Particle[number];
        SpriteRenderer spriteRenderer;

        
            GameObject go = Instantiate(particleObj, new Vector3(950, 500f, 0f), Quaternion.identity);
            spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
            result[0] = new Particle(0f,0f, go.transform.position.x, go.transform.position.y, 0, 0, color, go.GetInstanceID());

            go = Instantiate(particleObj, new Vector3(-800, 500f, 0f), Quaternion.identity);
            spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
            result[1] = new Particle(0f,0f, go.transform.position.x, go.transform.position.y, 0, 0, color, go.GetInstanceID());
        

        return result;
    }

    /**
    * Draw the particles on the screen
    */
    void draw()
    {
        for (int i = 0; i < particleArray.Length; i++)
        {
            for (int j = 0; j < particleArray[i].Length; j++)
            {
                // check if it is a real particle or a placeholder
                if (particleArray[i][j].objID != 0){
                    GameObject go = particleDict[particleArray[i][j].objID];
                    go.transform.position = new Vector3(particleArray[i][j].x, particleArray[i][j].y, 0f);
                }
            }
        }
    }

    public void rule(Particle[] particlesA, Particle[] particlesB, float g)
    {
        for (int i = 0; i < particlesA.Length; i++)
        {
            float fx = 0f;
            float fy = 0f;
            
            for (int j = 0; j < particlesB.Length; j++)
            {
                float dx = System.MathF.Abs(particlesB[j].x - particlesA[i].x);
                float dy = System.MathF.Abs(particlesB[j].y - particlesA[i].y);

                if (dx > (settingsValues.xMax - settingsValues.xMin)/2)
                {
                    dx = (settingsValues.xMax - settingsValues.xMin) - dx;
                }
                if (dy > (settingsValues.yMax - settingsValues.yMin)/2)
                {
                    dy = (settingsValues.yMax - settingsValues.yMin) - dy;
                }
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                // Debug.Log("d: " + distance);

                dx = dx * getDirection(particlesA[i].x, particlesB[j].x, WIDTH);
                dy = dy * getDirection(particlesA[i].y, particlesB[j].y, HEIGHT);

                if (distance > 0 && distance < radius )
                {
                    float force = g * (1f / distance);
                    fx += (force * dx);
                    fy += (force * dy);
                }
            }
            particlesA[i].fx += fx;
            particlesA[i].fy += fy;
            // particlesA[i].vx = (particlesA[i].vx + fx) * settingsValues.damping;
            // particlesA[i].vy = (particlesA[i].vy + fy) * settingsValues.damping;
            // particlesA[i].x += particlesA[i].vx;
            // particlesA[i].y += particlesA[i].vy;
            

            // if (particlesA[i].x > settingsValues.xMax || particlesA[i].x < settingsValues.xMin)
            // {
            //     // Debug.Log("bounce x");
            //     particlesA[i].vx *= -1;
            // }
            // if (particlesA[i].y > settingsValues.yMax || particlesA[i].y < settingsValues.yMin)
            // {
            //     // Debug.Log("bounce y");
            //     particlesA[i].vy *= -1;
            // }

            // TODO figure out if the velocity is getting canceled out when it loops around the screen

            // if a particle is on the edge loop it around
            // if (particlesA[i].x > settingsValues.xMax){
            //     particlesA[i].x = particlesA[i].x - WIDTH ;
            // }
            // if (particlesA[i].x < settingsValues.xMin){
            //     particlesA[i].x = particlesA[i].x + WIDTH ;
            // }
            // if (particlesA[i].y > settingsValues.yMax){
            //     particlesA[i].y = particlesA[i].y - HEIGHT ;
            // }
            // if (particlesA[i].y < settingsValues.yMin){
            //     particlesA[i].y = particlesA[i].y + HEIGHT ;
            // }

        }
    }
    public void ruleHybrid(Particle[] particlesA, Particle[] particlesB, float g, float range)
    {
    
        for (int i = 0; i < particlesA.Length; i++)
        {
            // continue if the particle is a placeholder
            if (particlesA[i].objID == 0 || particlesB.Length == 0 || particlesB[0].objID == 0){ 
                continue;
            }

            HybridRule hybridRule = new HybridRule(range, g, 0, 0, particlesA[i]);
            HybridRule[] hybridRuleArray = new HybridRule[1];
            hybridRuleArray[0] = hybridRule;

            ComputeBuffer particleBuffer = new ComputeBuffer(particlesB.Length, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Particle)));
            ComputeBuffer hybridRuleBuffer = new ComputeBuffer(1, System.Runtime.InteropServices.Marshal.SizeOf(hybridRule));

            particleBuffer.SetData(particlesB);
            hybridRuleBuffer.SetData(hybridRuleArray);

            computeShaderHybrid.SetBuffer(0, "ParticleArray", particleBuffer);
            computeShaderHybrid.SetBuffer(0, "rules", hybridRuleBuffer);
            computeShaderHybrid.SetFloats("dimensions", new float[] {WIDTH, HEIGHT});

            computeShaderHybrid.Dispatch(0, particlesB.Length / 8, 1, 1);

            // particleBuffer.GetData(particlesB);
            hybridRuleBuffer.GetData(hybridRuleArray);

            particleBuffer.Release();
            hybridRuleBuffer.Release();



            particlesA[i].fx += hybridRuleArray[0].fx;
            particlesA[i].fy += hybridRuleArray[0].fy;
        }
    }

    // return 1 if right/up and -1 if left/down
    int getDirection(float a, float b, float dim){
        if (a < b){
            float d1 = b - a;
            float d2 = (dim - b) + a;
            if (d1 < d2){
                return 1;
            } else {
                return -1;
            }
        }
        else{
            float d1 = a - b;
            float d2 = (dim - a) + b;
            if (d1 < d2){
                return -1;
            } else {
                return 1;
            }
        }
    }

    void CPURender(){
        resetForce();

        rule(yellowArray, yellowArray, settingsValues.yellow_yellow * settingsValues.gravity);
        rule(yellowArray, redArray, settingsValues.yellow_red * settingsValues.gravity);
        rule(yellowArray, greenArray, settingsValues.yellow_green * settingsValues.gravity);
        rule(yellowArray, blueArray, settingsValues.yellow_blue * settingsValues.gravity);
        rule(redArray, redArray, settingsValues.red_red * settingsValues.gravity);
        rule(redArray, greenArray, settingsValues.red_green * settingsValues.gravity);
        rule(redArray, yellowArray, settingsValues.red_yellow * settingsValues.gravity);
        rule(redArray, blueArray, settingsValues.red_blue * settingsValues.gravity);
        rule(greenArray, greenArray, settingsValues.green_green * settingsValues.gravity);
        rule(greenArray, redArray, settingsValues.green_red * settingsValues.gravity);
        rule(greenArray, yellowArray, settingsValues.green_yellow * settingsValues.gravity);
        rule(greenArray, blueArray, settingsValues.green_blue * settingsValues.gravity);
        rule(blueArray, blueArray, settingsValues.blue_blue * settingsValues.gravity);
        rule(blueArray, greenArray, settingsValues.blue_green * settingsValues.gravity);
        rule(blueArray, redArray, settingsValues.blue_red * settingsValues.gravity);
        rule(blueArray, yellowArray, settingsValues.blue_yellow * settingsValues.gravity);

        updateVelocityAndPosition();
    }

    void HybridRender(){
        resetForce();

        ruleHybrid(yellowArray, yellowArray, settingsValues.yellow_yellow * settingsValues.gravity, settingsValues.yellow_yellow_range);
        // ruleHybrid(yellowArray, redArray, settingsValues.yellow_red * settingsValues.gravity, settingsValues.yellow_red_range);
        // ruleHybrid(yellowArray, greenArray, settingsValues.yellow_green * settingsValues.gravity, settingsValues.yellow_green_range);
        // ruleHybrid(yellowArray, blueArray, settingsValues.yellow_blue * settingsValues.gravity, settingsValues.yellow_blue_range);
        // ruleHybrid(redArray, redArray, settingsValues.red_red * settingsValues.gravity, settingsValues.red_red_range);
        // ruleHybrid(redArray, greenArray, settingsValues.red_green * settingsValues.gravity, settingsValues.red_green_range);
        // ruleHybrid(redArray, yellowArray, settingsValues.red_yellow * settingsValues.gravity, settingsValues.red_yellow_range);
        // ruleHybrid(redArray, blueArray, settingsValues.red_blue * settingsValues.gravity, settingsValues.red_blue_range);
        // ruleHybrid(greenArray, greenArray, settingsValues.green_green * settingsValues.gravity, settingsValues.green_green_range);
        // ruleHybrid(greenArray, redArray, settingsValues.green_red * settingsValues.gravity, settingsValues.green_red_range);
        // ruleHybrid(greenArray, yellowArray, settingsValues.green_yellow * settingsValues.gravity, settingsValues.green_yellow_range);
        // ruleHybrid(greenArray, blueArray, settingsValues.green_blue * settingsValues.gravity, settingsValues.green_blue_range);
        // ruleHybrid(blueArray, blueArray, settingsValues.blue_blue * settingsValues.gravity, settingsValues.blue_blue_range);
        // ruleHybrid(blueArray, greenArray, settingsValues.blue_green * settingsValues.gravity, settingsValues.blue_green_range);
        // ruleHybrid(blueArray, redArray, settingsValues.blue_red * settingsValues.gravity, settingsValues.blue_red_range);
        // ruleHybrid(blueArray, yellowArray, settingsValues.blue_yellow * settingsValues.gravity, settingsValues.blue_yellow_range);

        updateVelocityAndPosition();
    }

    void GPURender(){
            Rule[] ruleArray = {
                new Rule(0, 0, settingsValues.yellow_yellow * settingsValues.gravity, settingsValues.damping, settingsValues.yellow_yellow_range),
                new Rule(0, 1, settingsValues.yellow_red * settingsValues.gravity, settingsValues.damping, settingsValues.yellow_red_range),
                new Rule(0, 2, settingsValues.yellow_green * settingsValues.gravity, settingsValues.damping, settingsValues.yellow_green_range),
                new Rule(0, 3, settingsValues.yellow_blue * settingsValues.gravity, settingsValues.damping, settingsValues.yellow_blue_range),
                new Rule(1, 1, settingsValues.red_red * settingsValues.gravity, settingsValues.damping, settingsValues.red_red_range),
                new Rule(1, 2, settingsValues.red_green * settingsValues.gravity, settingsValues.damping, settingsValues.red_green_range),
                new Rule(1, 0, settingsValues.red_yellow * settingsValues.gravity, settingsValues.damping, settingsValues.red_yellow_range),
                new Rule(1, 3, settingsValues.red_blue * settingsValues.gravity, settingsValues.damping, settingsValues.red_blue_range),
                new Rule(2, 2, settingsValues.green_green * settingsValues.gravity, settingsValues.damping, settingsValues.green_green_range),
                new Rule(2, 1, settingsValues.green_red * settingsValues.gravity, settingsValues.damping, settingsValues.green_red_range),
                new Rule(2, 0, settingsValues.green_yellow * settingsValues.gravity, settingsValues.damping, settingsValues.green_yellow_range),
                new Rule(2, 3, settingsValues.green_blue * settingsValues.gravity, settingsValues.damping, settingsValues.green_blue_range),
                new Rule(3, 3, settingsValues.blue_blue * settingsValues.gravity, settingsValues.damping, settingsValues.blue_blue_range),
                new Rule(3, 2, settingsValues.blue_green * settingsValues.gravity, settingsValues.damping, settingsValues.blue_green_range),
                new Rule(3, 1, settingsValues.blue_red * settingsValues.gravity, settingsValues.damping, settingsValues.blue_red_range),
                new Rule(3, 0, settingsValues.blue_yellow * settingsValues.gravity, settingsValues.damping, settingsValues.blue_yellow_range)
            };

            ComputeBuffer ruleBuffer = new ComputeBuffer(ruleArray.Length, System.Runtime.InteropServices.Marshal.SizeOf(typeof(Rule)));
            ruleBuffer.SetData(ruleArray);
            computeShader.SetBuffer(0, "rules", ruleBuffer);

            int particleSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Particle));

            // convert particle array to gpu particle array
            // GPUParticle[] gpuGreenArray = new GPUParticle[greenArray.Length];
            // GPUParticle[] gpuRedArray = new GPUParticle[redArray.Length];
            // GPUParticle[] gpuYellowArray = new GPUParticle[yellowArray.Length];
            // GPUParticle[] gpuBlueArray = new GPUParticle[blueArray.Length];
            // for (int i = 0; i < greenArray.Length; i++)
            // {
            //     gpuGreenArray[i] = new GPUParticle(greenArray[i]);
            //     gpuRedArray[i] = new GPUParticle(redArray[i]);
            //     gpuYellowArray[i] = new GPUParticle(yellowArray[i]);
            //     gpuBlueArray[i] = new GPUParticle(blueArray[i]);
            // }

            // create buffers
            ComputeBuffer greenBuffer = new ComputeBuffer(greenArray.Length, particleSize);
            ComputeBuffer redBuffer = new ComputeBuffer(redArray.Length, particleSize);
            ComputeBuffer yellowBuffer = new ComputeBuffer(yellowArray.Length, particleSize);
            ComputeBuffer blueBuffer = new ComputeBuffer(blueArray.Length, particleSize);

            int maxBuffer = Mathf.Max(greenArray.Length, redArray.Length, yellowArray.Length, blueArray.Length);
            maxBuffer = maxBuffer < 8 ? 8 : maxBuffer;

            // set buffers
            greenBuffer.SetData(greenArray);
            redBuffer.SetData(redArray);
            yellowBuffer.SetData(yellowArray);
            blueBuffer.SetData(blueArray);

            // set shader buffers
            computeShader.SetBuffer(0, "green", greenBuffer);
            computeShader.SetBuffer(0, "red", redBuffer);
            computeShader.SetBuffer(0, "yellow", yellowBuffer);
            computeShader.SetBuffer(0, "blue", blueBuffer);

            computeShader.SetFloats("dimensions", new float[] {WIDTH, HEIGHT});
            computeShader.SetFloat("radius", radius);

            computeShader.Dispatch(0, maxBuffer / 8, maxBuffer / 8, 1);

            // get data back
            greenBuffer.GetData(greenArray);
            redBuffer.GetData(redArray);
            yellowBuffer.GetData(yellowArray);
            blueBuffer.GetData(blueArray);
            
            // release buffers
            greenBuffer.Release();
            redBuffer.Release();
            yellowBuffer.Release();
            blueBuffer.Release();
            ruleBuffer.Release();

            // for (int i = 0; i < greenArray.Length; i++)
            // {
            //     greenArray[i].vx = gpuGreenArray[i].vx;
            //     greenArray[i].vy = gpuGreenArray[i].vy;
            //     redArray[i].vx = gpuRedArray[i].vx;
            //     redArray[i].vy = gpuRedArray[i].vy;
            //     yellowArray[i].vx = gpuYellowArray[i].vx;
            //     yellowArray[i].vy = gpuYellowArray[i].vy;
            //     blueArray[i].vx = gpuBlueArray[i].vx;
            //     blueArray[i].vy = gpuBlueArray[i].vy;
            // }

            updateVelocityAndPositionGPU();

    }

    public void updateVelocityAndPositionGPU(){
        for (int i = 0; i < particleArray.Length; i++){
            for (int j = 0; j < particleArray[i].Length; j++){
                particleArray[i][j].vx = particleArray[i][j].vx * settingsValues.damping;
                particleArray[i][j].vy = particleArray[i][j].vy * settingsValues.damping;
                particleArray[i][j].x += particleArray[i][j].vx;
                particleArray[i][j].y += particleArray[i][j].vy;

                // particleArray[i][j].vx = (particleArray[i][j].vx + (particleArray[i][j].fx * )) * settingsValues.damping;
                // particleArray[i][j].vy = (particleArray[i][j].vy + (particleArray[i][j].fy * settingsValues.damping)) * settingsValues.damping;

                // if a particle is on the edge loop it around
                if (particleArray[i][j].x > settingsValues.xMax){
                    particleArray[i][j].x = particleArray[i][j].x - WIDTH ;
                }
                if (particleArray[i][j].x < settingsValues.xMin){
                    particleArray[i][j].x = particleArray[i][j].x + WIDTH ;
                }
                if (particleArray[i][j].y > settingsValues.yMax){
                    particleArray[i][j].y = particleArray[i][j].y - HEIGHT ;
                }
                if (particleArray[i][j].y < settingsValues.yMin){
                    particleArray[i][j].y = particleArray[i][j].y + HEIGHT ;
                }
            }
        }
    }

    public void updateVelocityAndPosition()
    {
        for (int i = 0; i < particleArray.Length; i++)
        {
            for (int j = 0; j < particleArray[i].Length; j++)
            {
                particleArray[i][j].vx = (particleArray[i][j].vx + particleArray[i][j].fx) * settingsValues.damping;
                particleArray[i][j].vy = (particleArray[i][j].vy + particleArray[i][j].fy) * settingsValues.damping;
                particleArray[i][j].x += particleArray[i][j].vx;
                particleArray[i][j].y += particleArray[i][j].vy;

                // if a particle is on the edge loop it around
                if (particleArray[i][j].x > settingsValues.xMax){
                    particleArray[i][j].x = particleArray[i][j].x - WIDTH ;
                }
                if (particleArray[i][j].x < settingsValues.xMin){
                    particleArray[i][j].x = particleArray[i][j].x + WIDTH ;
                }
                if (particleArray[i][j].y > settingsValues.yMax){
                    particleArray[i][j].y = particleArray[i][j].y - HEIGHT ;
                }
                if (particleArray[i][j].y < settingsValues.yMin){
                    particleArray[i][j].y = particleArray[i][j].y + HEIGHT ;
                }
            }
        }
    }

    public void resetForce(){
        for (int i = 0; i < particleArray.Length; i++)
        {
            for (int j = 0; j < particleArray[i].Length; j++)
            {
                particleArray[i][j].fx = 0f;
                particleArray[i][j].fy = 0f;
            }
        }
    }

    float expDist(){
        float r = Random.Range(-4.5f, 4.5f);
        float s = Mathf.Sign(r);
        r = Mathf.Abs(r);
        r = -Mathf.Log10(2*r +1)+1; 
        r = r*s;
        // Debug.Log(r);
        return r;
    }
}
