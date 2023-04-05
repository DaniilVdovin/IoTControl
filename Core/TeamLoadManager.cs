namespace UiIoT.Core
{
    public static class TeamLoadManager
    {
        private static Team LoadFile(string path)
        {
            Team temp = new Team();
            temp.Name = path.Replace("_", "").Split('.')[0];
            temp.Path = Environment.CurrentDirectory + "/Teams/" + path;
            using (var reader = File.OpenText(path))
            {
                string line = "";
                while ((line = reader.ReadLine()) != null)
                    temp.IoTs.Add(new IoT(line.Split(';')));
            }
            return temp;
        }
        public static List<Team> LoadTeams()
        {
            List<Team> teams = new List<Team>();
            foreach (string file in Directory.EnumerateFiles(Environment.CurrentDirectory + "/Teams/", "_*.txt", SearchOption.AllDirectories))
            {
                teams.Add(LoadFile(file));
                Console.WriteLine(file);
            }
            return teams;
        }
    }
}
