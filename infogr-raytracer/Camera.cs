using OpenTK;

namespace infogr_raytracer
{
    public struct Camera
    {
        /// <summary>
        /// The Camera's position (in world space)
        /// </summary>
        public Vector2 Position;
        
        private Vector2 _viewPort;
        private Vector2 _screenResolution;
        private float _screenSizeWorldSizeRatio;
        
        /// <summary>
        /// Convert a screen space coordinate to a world space coordinate
        /// Takes the current position into account
        /// </summary>
        /// <param name="screenSpaceX">The X component of the screen space coordinate</param>
        /// <param name="screenSpaceY">The Y component of the screen space coordinate</param>
        /// <returns>The screen space coordinate</returns>
        public Vector2 WorldSpace(int screenSpaceX, int screenSpaceY)
        {
            // float ratio = ((float) ViewPort.X / (float) ScreenResolution.X);
            return new Vector2(
                (float) screenSpaceX * _screenSizeWorldSizeRatio,
                (float) _viewPort.Y - screenSpaceY * _screenSizeWorldSizeRatio
            ) + Position;
        }

        /// <summary>
        /// Handle screen resizing by adjusting the screen resolution, viewport and world size ratio
        /// </summary>
        /// <param name="screenWidth">The new screen width</param>
        /// <param name="screenHeight">The new screen height</param>
        public void Resize(int screenWidth, int screenHeight)
        {
            float pixelsPerWorldSpaceUnit = 100;
            _screenResolution = new Vector2(screenWidth, screenHeight);
            _viewPort = new Vector2((float) screenWidth / pixelsPerWorldSpaceUnit, 
                (float) screenHeight / pixelsPerWorldSpaceUnit);
            
            _screenSizeWorldSizeRatio = ((float) _viewPort.X / (float) _screenResolution.X);
        }
    }
}