using OpenTK;

namespace infogr_raytracer
{
    public struct Ray
    {
        public Vector2 Origin;

        public Vector2 End()
        {
            return Origin + _direction * T;
        }

        private Vector2 _direction;
        public Vector2 Direction
        {
            get => _direction;
            set => _direction = value.Normalized();
        }
        
        public float T;
    }
}