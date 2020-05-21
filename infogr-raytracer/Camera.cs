using OpenTK;

namespace infogr_raytracer
{
    public struct Camera
    {
        public Vector2 Position;
        public Vector2 ViewPort;
        public Vector2 ScreenResolution;

        private float _screenSizeWorldSizeRatio;
        
        public Vector2 WorldSpace(int screenSpaceX, int screenSpaceY)
        {
            // float ratio = ((float) ViewPort.X / (float) ScreenResolution.X);
            return new Vector2(
                (float) screenSpaceX * _screenSizeWorldSizeRatio,
                (float) ViewPort.Y - screenSpaceY * _screenSizeWorldSizeRatio
            );
        }

        public void Resize(int screenWidth, int screenHeight)
        {
            float pixelsPerWorldSpaceUnit = 100;
            ScreenResolution = new Vector2(screenWidth, screenHeight);
            ViewPort = new Vector2((float) screenWidth / pixelsPerWorldSpaceUnit, 
                (float) screenHeight / pixelsPerWorldSpaceUnit);
            
            _screenSizeWorldSizeRatio = ((float) ViewPort.X / (float) ScreenResolution.X);
        }
    }
}