using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{

    public int seed = 222;

    public float radius = 2000f;
    public Settings settingsValues;

    public GameObject particleObj;

    Particle[] yellowArray;
    Particle[] redArray;
    Particle[] greenArray;
    Particle[] blueArray;
    Particle[][] particleArray;

    private float WIDTH;
    private float HEIGHT;

    public struct Particle
    {
        public float fx;
        public float fy;
        public float x;
        public float y;
        public float vx;
        public float vy;
        public Color color;
        public GameObject obj;
        public bool isInBounds;

        public Particle(float fx, float fy, float x, float y, float vx, float vy, Color color, GameObject obj, bool isInBounds)
        {
            this.fx = fx;
            this.fy = fy;
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
            this.color = color;
            this.obj = obj;
            this.isInBounds = isInBounds;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        WIDTH = settingsValues.xMax - settingsValues.xMin;
        HEIGHT = settingsValues.yMax - settingsValues.yMin;

        settingsValues.RegisterResetPosEvent(updateCount);
        settingsValues.RegisterRandomizePosEvent(RandomizeConnections);

        Random.InitState(seed);

        yellowArray = Create(settingsValues.yellow_count, Color.yellow);
        redArray = Create(settingsValues.red_count, Color.red);
        greenArray = Create(settingsValues.green_count, new Color(0.08627f, 0.3686f, 0.1137f));
        blueArray = Create(settingsValues.blue_count, Color.blue);

        particleArray = new Particle[][] { yellowArray, redArray, greenArray, blueArray };
;


    }

    // Update is called once per frame
    void Update()
    {
        resetForce();

        rule(greenArray, greenArray, settingsValues.green_green * settingsValues.gravity);
        rule(greenArray, redArray, settingsValues.green_red * settingsValues.gravity);
        rule(greenArray, yellowArray, settingsValues.green_yellow * settingsValues.gravity);
        rule(greenArray, blueArray, settingsValues.green_blue * settingsValues.gravity);
        rule(redArray, redArray, settingsValues.red_red * settingsValues.gravity);
        rule(redArray, greenArray, settingsValues.red_green * settingsValues.gravity);
        rule(redArray, yellowArray, settingsValues.red_yellow * settingsValues.gravity);
        rule(redArray, blueArray, settingsValues.red_blue * settingsValues.gravity);
        rule(yellowArray, yellowArray, settingsValues.yellow_yellow * settingsValues.gravity);
        rule(yellowArray, greenArray, settingsValues.yellow_green * settingsValues.gravity);
        rule(yellowArray, redArray, settingsValues.yellow_red * settingsValues.gravity);
        rule(yellowArray, blueArray, settingsValues.yellow_blue * settingsValues.gravity);
        rule(blueArray, blueArray, settingsValues.blue_blue * settingsValues.gravity);
        rule(blueArray, greenArray, settingsValues.blue_green * settingsValues.gravity);
        rule(blueArray, redArray, settingsValues.blue_red * settingsValues.gravity);
        rule(blueArray, yellowArray, settingsValues.blue_yellow * settingsValues.gravity);

        updateVelocityAndPosition();

        draw();
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
        settingsValues.green_green = Random.Range(-1f, 1f);
        settingsValues.green_red = Random.Range(-1f, 1f);
        settingsValues.green_yellow = Random.Range(-1f, 1f);
        settingsValues.green_blue = Random.Range(-1f, 1f);
        settingsValues.red_red = Random.Range(-1f, 1f);
        settingsValues.red_green = Random.Range(-1f, 1f);
        settingsValues.red_yellow = Random.Range(-1f, 1f);
        settingsValues.red_blue = Random.Range(-1f, 1f);
        settingsValues.yellow_yellow = Random.Range(-1f, 1f);
        settingsValues.yellow_green = Random.Range(-1f, 1f);
        settingsValues.yellow_red = Random.Range(-1f, 1f);
        settingsValues.yellow_blue = Random.Range(-1f, 1f);
        settingsValues.blue_blue = Random.Range(-1f, 1f);
        settingsValues.blue_green = Random.Range(-1f, 1f);
        settingsValues.blue_red = Random.Range(-1f, 1f);
        settingsValues.blue_yellow = Random.Range(-1f, 1f);
    }
    void updateCount()
    {
        Debug.Log("Updating count");

        for (int i = 0; i < particleArray.Length; i++)
        {
            for (int j = 0; j < particleArray[i].Length; j++)
            {
                GameObject.Destroy(particleArray[i][j].obj);
            }
        }

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
            GameObject go = Instantiate(particleObj, new Vector3(Random.Range(settingsValues.xMin, settingsValues.xMax), Random.Range(settingsValues.yMin, settingsValues.yMax), 0f), Quaternion.identity);
            spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
            result[i] = new Particle(0f,0f, go.transform.position.x, go.transform.position.y, 0f, 0f, color, go, true);
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
            result[0] = new Particle(0f,0f, go.transform.position.x, go.transform.position.y, 0, 0, color, go, true);

            go = Instantiate(particleObj, new Vector3(-800, 500f, 0f), Quaternion.identity);
            spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.color = color;
            result[1] = new Particle(0f,0f, go.transform.position.x, go.transform.position.y, 0, 0, color, go, true);
        

        return result;
    }

    void draw()
    {
        for (int i = 0; i < particleArray.Length; i++)
        {
            for (int j = 0; j < particleArray[i].Length; j++)
            {
                particleArray[i][j].obj.transform.position = new Vector3(particleArray[i][j].x, particleArray[i][j].y, 0f);
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
                    float force = g * 1f / distance;
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


}
