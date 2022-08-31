using UnityEngine;

public static class GameObjectExtensions 
 {
     
     private static Vector2 velocity = Vector2.zero;


 
 /**
    * @brief Applies a force to the object
    * @param force The force to apply
    */
     public static Vector2 GetVelocity(this GameObject gameObject)
     {
         return velocity;
     }

     public static void SetVelocity(this GameObject gameObject, Vector2 v)
     {
         velocity = v;
     }

     public static float GetVelocityX(this GameObject gameObject)
     {
         return velocity.x;
     }

        public static float GetVelocityY(this GameObject gameObject)
        {
            return velocity.y;
        }

        public static void SetVelocityX(this GameObject gameObject, float x)
        {
            velocity.x = x;
        }

        public static void SetVelocityY(this GameObject gameObject, float y)
        {
            velocity.y = y;
        }
 }