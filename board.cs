using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleGame
{
    class Coordinate
    {
        public int x;
        public int y;
        public Coordinate(string coordinate)
        {
            string[] coordinates = coordinate.Split(',');
            x = int.Parse(coordinates[0]);
            y = int.Parse(coordinates[1]);
        }
        public Coordinate(int pX, int pY)
        {
            x = pX;
            y = pY;
        }
    }

    class CoordinateOrientation
    {
        public Coordinate position;
        char orientation; // N-North, S-South, W-West, E-East
        public CoordinateOrientation(string coordinateOrientation)
        {
            int orientationSeparate = coordinateOrientation.LastIndexOf(',');
            position = new Coordinate(coordinateOrientation.Substring(0, orientationSeparate));
            orientation = char.Parse(coordinateOrientation.Substring(orientationSeparate + 1));
        }

        public CoordinateOrientation()
        {
        }

        public void Rotate()
        {
            switch (orientation)
            {
                case 'N':
                    orientation = 'E';
                    break;
                case 'E':
                    orientation = 'S';
                    break;
                case 'S':
                    orientation = 'W';
                    break;
                case 'W':
                    orientation = 'N';
                    break;
            }
        }

        public void Move()
        {
            if (orientation == 'N') position.y--;
            if (orientation == 'S') position.y++;
            if (orientation == 'W') position.x--;
            if (orientation == 'E') position.x++;
        }

        public CoordinateOrientation Clone()
        {
            CoordinateOrientation clone = new CoordinateOrientation();
            clone.orientation = this.orientation;
            clone.position = new Coordinate(this.position.x, this.position.y);
            return clone;
        }
    }

    class Board
    {
        #region Properties
        private int rows;
        private int cols;
        private List<Coordinate> mines = null;
        private CoordinateOrientation turtleInitialPosition;
        private CoordinateOrientation turtlePosition;
        private Coordinate endPosition;
        #endregion

        #region Tokens
        private static string TOKEN_ISOUTOFBOARD = "Out of the Board!";
        private static string TOKEN_ISMINEHIT = "Mine hit!";
        private static string TOKEN_ISSTILLINDANGER = "Still In Danger!";
        private static string TOKEN_SUCCESS = "Success!";
        #endregion

        public Board(int pRows, int pCols, List<Coordinate> pMines, CoordinateOrientation pInitialPosition, Coordinate pEndPosition)
        {
            rows = pRows;
            cols = pCols;
            mines = pMines;
            turtleInitialPosition = pInitialPosition;
            endPosition = pEndPosition;
        }

     

        public void Play(string actions)
        {
            ReStart();
            bool end = false;
            int totalActions = actions.Length;
            int position = 0;
            while (!end & position < totalActions)
            {
                end = Action(actions[position++]);
            }
            if (!end)
                Console.WriteLine(TOKEN_ISSTILLINDANGER);
        }

        private void ReStart()
        {
            turtlePosition = turtleInitialPosition.Clone();
        }


        #region Actions
        private bool Action(char action)
        {
            bool end = false;
            if (action == 'r')
            {
                turtlePosition.Rotate();
                end = false;
            }
            else if (action == 'm')
            {
                turtlePosition.Move();
                end = (IsOutOfBoard() | IsInAMine() | IsAtTheEnd());
            }
            return end;
        }

        public bool IsOutOfBoard()
        {
            Coordinate coordinate = turtlePosition.position;
            bool isOutOfBoard = false;
            if (coordinate.x < 0 || coordinate.x >= cols) isOutOfBoard = true;
            if (coordinate.y < 0 || coordinate.y >= rows) isOutOfBoard = true;
            if (isOutOfBoard) Console.WriteLine(TOKEN_ISOUTOFBOARD);
            return isOutOfBoard;
        }

        public bool IsInAMine()
        {
            Coordinate coordinate = turtlePosition.position;
            bool isInAMine = false;
            foreach(Coordinate mine in mines)
            {
                if (coordinate.x == mine.x & coordinate.y == mine.y) isInAMine = true;
            }
            if (isInAMine) Console.WriteLine(TOKEN_ISMINEHIT);
            return isInAMine;
        }

        public bool IsAtTheEnd()
        {
            Coordinate coordinate = turtlePosition.position;
            bool isAtTheEnd = false;
            if (coordinate.x == endPosition.x & coordinate.y == endPosition.y) isAtTheEnd = true;
            if (isAtTheEnd) Console.WriteLine(TOKEN_SUCCESS);
            return isAtTheEnd;
        }

        #endregion
    }
}
