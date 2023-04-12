namespace UiIoT
{
    public static class pseudoDataWorker
    {
        public static string take_pseudodate()
        {
            Random rnd = new Random();

            return $"[M: {rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}#" +
                $"T:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}#" +
                $"L:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}:{rnd.Next(1, 10)}#]";
        }
    }
}
