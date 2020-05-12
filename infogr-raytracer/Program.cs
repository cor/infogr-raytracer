namespace infogr_raytracer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            using (Game game = new Game(1200, 900, "INFOGR Raytracer"))
            {
                game.Run(60.0);
            }
        }
    }
}