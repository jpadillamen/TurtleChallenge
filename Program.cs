using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TurtleGame
{
    class Program
    {
        // Game Setting file format:
        // núm. rows / num. cols / mine coordinates separated by colon / star coordinates and orientation / end coordinates
        // rows/cols/x1,y1;x2,y2;x3,y3/0,1,N/4,2

        static void Main(string[] args)
        {
            if (args.Length >= 2)
            {
                string gameSettingFile = args[0];
                string movesFile = args[1];
                string game_setting = File.ReadAllText(gameSettingFile, Encoding.UTF8);
                string[] elements = game_setting.Split('/');

                if (elements.Count() < 5)
                {
                    Console.WriteLine("Wrong format file");
                }
                else
                {
                    int cols = int.Parse(elements[0]);
                    int rows = int.Parse(elements[1]);
                    string minePositions = elements[2];
                    List<Coordinate> mines = new List<Coordinate>();
                    foreach (string sPositon in minePositions.Split(';')) mines.Add(new Coordinate(sPositon));
                    CoordinateOrientation initialTurtlePosition = new CoordinateOrientation(elements[3]);
                    Coordinate endPosition = new Coordinate(elements[4]);

                    Board board = new Board(rows, cols, mines, initialTurtlePosition, endPosition);

                    StreamReader fileActionsSequences =  new StreamReader(movesFile);
                    string actionSequence = "";
                    while ( (actionSequence = fileActionsSequences.ReadLine()) != null )
                    {
                        board.Play(actionSequence);
                    }
                }
                Console.WriteLine("End Game");
            }
            else
            {
                Console.WriteLine("Program arguments: turtleChallengeCSharp.exe setting-file-name moves-file");
            }
            Console.ReadLine();
        }

    }
}
