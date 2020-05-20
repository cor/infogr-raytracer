namespace infogr_raytracer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (Window window = new Window(900, 900, "INFOGR Raytracer"))
            {
                window.Run(60.0);
            }
        }
    }
}