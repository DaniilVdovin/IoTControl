using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace IoTControl.Core
{
    public static class TeamLoadManager
    {
        private static Team LoadFile(string path)
        {
            Team temp = new Team();
            temp.Name = path.Substring(path.LastIndexOf('/')).Replace("_","").Split('.')[0];
            temp.Path = path;
            using (var reader = File.OpenText(path))
            {
                string line = "";
				int currentLine = 1;
				while ((line = reader.ReadLine()) != null)
                {
                    if (currentLine >= 2)
                    {
						temp.IoTs.Add(new IoT(line.Split(';')));
					}
                    else
                    {
						temp.ServerIP = line.Split(';')[0];
						temp.Appkey = line.Split(';')[1];
					}
					currentLine++;
                }
                    
            }
            return temp;
        }
        public static List<Team> LoadTeams()
        {
            List<Team> teams = new List<Team>();
            foreach (string file in Directory.EnumerateFiles(Environment.CurrentDirectory+"/Teams/", "_*.txt", SearchOption.AllDirectories))
            {
                teams.Add(LoadFile(file));
                Console.WriteLine(file);
			}
			return teams;
        }
    }
}
