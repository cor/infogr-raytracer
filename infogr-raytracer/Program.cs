namespace infogr_raytracer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (Window window = new Window(1200, 900, "INFOGR Raytracer"))
            {
                window.Run(60.0);
            }
        }
    }
}