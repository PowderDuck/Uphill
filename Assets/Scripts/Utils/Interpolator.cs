using UnityEngine;

namespace Uphill.Scripts.Utils
{
    public class Interpolator
    {
        public float Source { get; private set; }
        public float Destination { get; private set; }

        public Interpolator(float source, float destination)
        {
            Source = source;
            Destination = destination;
        }

        public void Update(float destination)
        {
            Source = Destination;
            Destination = destination;
        }

        public float GetValue(float percentage) =>
            Mathf.Lerp(Source, Destination, percentage);
    }
}