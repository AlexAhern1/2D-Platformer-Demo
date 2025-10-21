using System;
using UnityEngine;

namespace Game
{
    public static class MyExtensions
    {
        public static void SetHorizontalSpeed(this Rigidbody2D rb2d, float speed)
        {
            rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
        }

        public static void SetVerticalSpeed(this Rigidbody2D rb2d, float speed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, speed);
        }

        public static void SetVelocity(this Rigidbody2D rb2d, float horizontalSpeed, float verticalSpeed)
        {
            rb2d.velocity = new Vector2(horizontalSpeed, verticalSpeed);
        }

        public static bool IsFacing(this Transform transform, Vector2 target, float facingDirection)
        {
            return Mathf.Sign(target.x - transform.position.x) == Mathf.Sign(facingDirection);
        }

        public static Vector2 XPosition(this Transform transform, float x)
        {
            return new Vector2(x, transform.position.y);
        }

        public static Vector2 YPosition(this Transform transform, float y)
        {
            return new Vector2(transform.position.x, y);
        }

        public static void AccelerateVertically(this Rigidbody2D rb2d, float acceleration)
        {
            rb2d.velocity += acceleration * Time.fixedDeltaTime * Vector2.up;
        }

        public static void FaceRight(this Transform transform)
        {
            transform.rotation = Quaternion.identity;
        }

        public static void FaceLeft(this Transform transform)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        /// <summary>
        /// Calculates the angle in degrees between the x and y components of the vector v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float Arctan(this Vector2 v) => Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;

        /// <summary>
        /// Linearly maps t from the interval [<paramref name="a"/>, <paramref name="b"/>] onto the interval [<paramref name="c"/>, <paramref name="d"/>].
        /// </summary>
        /// <param name="t"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="leftOut"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        /// 
        public static float LinearMap(this float t, float a, float b, float c, float d)
        {
            return c + ((d - c) / (b - a)) * (t - a);
        }

        /// <summary>
        /// Linearly maps t from the interval [<paramref name="a"/>, <paramref name="b"/>] onto the interval [0, 1].
        /// </summary>
        /// <param name="t"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float LinearMapTo01(this float t, float a, float b)
        {
            return (t - a) / (b - a);
        }

        /// <summary>
        /// Checks if there is a clear line of sight to one single target. Draws a line in the inspector.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="hits"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static bool HasLineOfSight(Vector2 start, Vector2 end, RaycastHit2D[] hits, LayerMask layer)
        {
            bool result = Physics2D.LinecastNonAlloc(start, end, hits, layer) == 1;

            Color lineColor = !result ? Color.red : Color.green;

            Logger.DrawLine(start, end, Time.fixedDeltaTime, lineColor);

            return result;
        }

        public static Vector2 Normal(this Vector2 v)
        {
            return new(v.y, -v.x);
        }

        /// <summary>
        /// Returns 0 if <paramref name="x"/> is 0, otherwise the sign of <paramref name="x"/>
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float TrueSign(float x)
        {
            return x == 0 ? 0 : Mathf.Sign(x);
        }

        /// <summary>
        /// <para>Returns <paramref name="a"/> IF ONLY AND IF <paramref name="b"/>.</para>
        /// Ie. true if both a and b have the same truth value.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Iff(bool a, bool b)
        {
            return a == b;
        }
    }
}