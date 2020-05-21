using OpenTK;

namespace infogr_raytracer
{
    public struct Ray
    {
        private Vector2 _direction;
     
        /// <summary>
        /// The origin of our ray.
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        /// The direction component of our ray. Is normalized when set.
        /// </summary>
        public Vector2 Direction
        {
            get => _direction;
            set => _direction = value.Normalized();
        }
        
        /// <summary>
        /// The magnitude of this ray
        /// </summary>
        public float Magnitude;
    }
}