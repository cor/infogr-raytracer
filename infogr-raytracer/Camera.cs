using OpenTK;

namespace infogr_raytracer
{
    public struct Camera
    {
        public Vector2 Position;
        public Vector2 ViewPort;
        public Vector2 ScreenResolution;

        public float WorldSpaceX(int screenSpaceX)
        {
            float ratio = ((float) ViewPort.X / (float) ScreenResolution.X);
            return (float) screenSpaceX * ratio;
        }

        public float WorldSpaceY(int screenSpaceY)
        {
            float ratio = ((float) ViewPort.Y / (float) ScreenResolution.Y);
            return ViewPort.Y - screenSpaceY * ratio;
        }
    }
}