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

        public Particle(float fx, float fy, float x, float y, float vx, float vy, Color color, GameObject obj)
        {
            this.fx = fx;
            this.fy = fy;
            this.x = x;
            this.y = y;
            this.vx = vx;
            this.vy = vy;
            this.color = color;
            this.obj = obj;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        // settingsValues.RegisterResetPosEvent(resetPos);
        settingsValues.RegisterResetPosEvent(updateCount);
        // settingsValues.RegisterChangeCountEvent(updateCount);

        Random.InitState(seed);

        // greenArray = Create(settingsValues.green_count, new Color(0.8627f, 0.3686f, 0.1137f));
        greenArray = Create(settingsValues.green_count, new Color(0.08627f, 0.3686f, 0.1137f));
        yellowArray = Create(settingsValues.yellow_count, Color.yellow);
        redArray = Create(settingsValues.red_count, Color.red);
        blueArray = Create(settingsValues.blue_count, Color.blue);

        particleArray = new Particle[][] { yellowArray, redArray, greenArray, blueArray };



        // rule(yellowArray, yellowArray, settingsValues.gravity);


    }

    // Update is called once per frame
    void Update()
    {
        // rule(yellowArray, yellowArray, settingsValues.gravity);
        // rule(yellowArray, redArray, settingsValues.gravity);
        // rule(redArray, yellowArray, settingsValues.gravity);

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


        // rule(yellowArray, redArray, 0.15f * settingsValues.gravity);
        // rule(greenArray, greenArray, -0.7f * settingsValues.gravity);
        // rule(greenArray, redArray, -0.2f * settingsValues.gravity);
        // rule(redArray, greenArray, -0.1f * settingsValues.gravity);
        // rule(blueArray, blueArray, 0.3f * settingsValues.gravity);
        // rule(blueArray, redArray, -0.1f * settingsValues.gravity);
        // rule(redArray, blueArray, -0.1f * settingsValues.gravity);
        // rule(blueArray, yellowArray, -0.5f * settingsValues.gravity);

        updateVelocityAndPosition();

        draw();
        // rule(redArray, yellowArray, settingsValues.gravity);

        // yellowArray
    }

    void resetPos()
    {
        Debug.Log("Resetting position");

        for (int i = 0; i < particleArray.Length; i++)
        {
            for (int j = 0; j < particleArray[i].Length; j++)
            {
                particleArray[i][j].vx = 0;
                particleArray[i][j].vy = 0;
                particleArray[i][j].x = Random.Range(settingsValues.xMin, settingsValues.xMax);
                particleArray[i][j].y = Random.Range(settingsValues.yMin, settingsValues.yMax);
            }
        }
        draw();

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
            result[i] = new Particle(0f,0f, go.transform.position.x, go.transform.position.y, 0f, 0f, color, go);
        }

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
                float dx = particlesA[i].x - particlesB[j].x;
                float dy = particlesA[i].y - particlesB[j].y;
                float distance = Mathf.Sqrt(dx * dx + dy * dy);
                // Debug.Log("d: " + distance);
                if (distance > 0 && distance < radius)
                {
                    float force = g * 1f / distance;
                    fx += (force * dx);
                    fy += (force * dy);
                }
            }
            particlesA[i].fx += fx;
            particlesA[i].fy += fy;

            if (particlesA[i].x > settingsValues.xMax || particlesA[i].x < settingsValues.xMin)
            {
                // Debug.Log("bounce x");
                particlesA[i].vx *= -1;
            }
            if (particlesA[i].y > settingsValues.yMax || particlesA[i].y < settingsValues.yMin)
            {
                // Debug.Log("bounce y");
                particlesA[i].vy *= -1;
            }

            // if a particle is on the edge loop it around
            // if (particlesA[i].x > settingsValues.xMax){
            //     particlesA[i].x = settingsValues.xMin;
            // }
            // if (particlesA[i].x < settingsValues.xMin){
            //     particlesA[i].x = settingsValues.xMax;
            // }
            // if (particlesA[i].y > settingsValues.yMax){
            //     particlesA[i].y = settingsValues.yMin;
            // }
            // if (particlesA[i].y < settingsValues.yMin){
            //     particlesA[i].y = settingsValues.yMax;
            // }

        }
    }

    public void updateVelocityAndPosition()
    {
        for (int i = 0; i < particleArray.Length; i++)
        {
            for (int j = 0; j < particleArray[i].Length; j++)
            {
                particleArray[i][j].vx = (particleArray[i][j].vx + particleArray[i][j].fx) * 0.5f;
                particleArray[i][j].vy = (particleArray[i][j].vy + particleArray[i][j].fy) * 0.5f;
                particleArray[i][j].x += particleArray[i][j].vx;
                particleArray[i][j].y += particleArray[i][j].vy;
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
