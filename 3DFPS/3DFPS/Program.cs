using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DFPS
{
    class Program
    {
        static char[,] map =
        {
            {'#','#','#','#','#','#','#','#','#','#','#','#','#','#' }, // 0
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 1
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 2
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 3
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 4
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 5
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 6
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 6
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 7
            {'#','*','*','*','*','*','#','#','*','*','*','*','*','#' }, // 8
            {'#','*','*','*','*','*','#','#','*','*','*','*','*','#' }, // 9
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 10
            {'#','*','*','*','*','#','*','*','*','*','*','*','*','#' }, // 11
            {'#','*','*','*','*','#','*','*','*','*','*','*','*','#' }, // 12
            {'#','*','*','*','*','#','#','*','*','*','*','*','*','#' }, // 13
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 14
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 15
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 16
            {'#','*','*','*','*','*','*','*','*','*','*','*','*','#' }, // 17
            {'#','#','#','#','#','#','#','#','#','#','#','#','#','#' }, // 18
           // 0   1   2   3   4   5   6   7   8   9   10  11 12   13
        };

        static float[] playerPosition = {2, 2};

        static int nScreenWidth = 120;         
        static int nScreenHeight = 30;        
        static int nMapWidth = 16;             
        static int nMapHeight = 16;

        static float PlayerStartRot = 0.0f;
        static float fFOV = 3.14159f / 4.0f;
        static float fDepth = 17.0f;
        static char close = System.Convert.ToChar(219);
        static char middle = System.Convert.ToChar(178);
        static char far = System.Convert.ToChar(177);
        static char veryFar = System.Convert.ToChar(176);
        static char max = ' ';
        static float distCloses = 2;
        static float distMiddle = 5;
        static float distFar = 12;
        static float distVF = 15;
        static float distMax = 17;
        static void Main(string[] args)
        {
            Console.SetWindowSize(121, 31);
            Console.CursorVisible = false;
            while (true)
            {

                Move();
                Render();
            }
        }

        static void Move()
        {

            ConsoleKeyInfo key;
    
             key = Console.ReadKey();
            if (key.Key == ConsoleKey.A)
            {
                PlayerStartRot -= 0.1f;
            }
            else if (key.Key == ConsoleKey.D)
            {
                PlayerStartRot += 0.1f;
            }
            if (key.Key == ConsoleKey.W)
            {
                playerPosition[0] += (float)(Math.Sin(PlayerStartRot) * 0.5);
                playerPosition[1] += (float)(Math.Cos(PlayerStartRot) * 0.5);
            }
            if (key.Key == ConsoleKey.S)
            {
                playerPosition[0] -= (float)(Math.Sin(PlayerStartRot) * 0.5);
                playerPosition[1] -= (float)(Math.Cos(PlayerStartRot) * 0.5);
            }
        }
        static void Render()
        {
            string[,] screen = new string[nScreenWidth, nScreenHeight];
            var whiteSpace = new StringBuilder();
            whiteSpace.Append(' ', 16);
            Console.SetCursorPosition(0, 0);
            var sb = new StringBuilder();
            sb.AppendLine("PlayerX: " + playerPosition[0]+" PlayerY: "+playerPosition[1]);

            for (int x = 0; x < nScreenWidth; x++)
            {
                float fRayAngle = (PlayerStartRot - fFOV / 2.0f) + ((float)x / (float)nScreenWidth) * fFOV;

                float distanceToWall = 0.0f;
                bool hitWall = false;

                float FEyeX = float.Parse(Math.Sin(fRayAngle)+"");
                float FEyeY = float.Parse(""+Math.Cos(fRayAngle));
                while(!hitWall && distanceToWall < distMax)
                {
                    distanceToWall+= 0.1f;
                    int xTest = (int)(playerPosition[0] + FEyeX * distanceToWall);
                    int yTest = (int)(playerPosition[1] + FEyeY * distanceToWall);

                    if (xTest < 0 || xTest >= nMapWidth || yTest < 0 || yTest >= nMapHeight)
                    {
                        hitWall = true; 
                        distanceToWall = fDepth;
                    }
                    else
                    {
                        if(map[xTest,yTest] == '#')
                        {
                            hitWall = true;
                        }
                    }
                }

                int nCeiling = (int)((nScreenHeight / 2.0f) - nScreenHeight / distanceToWall);
                int nFloor = nScreenHeight - nCeiling;

                for (int y = 0; y < nScreenHeight; y++)
                {
                    if(y < nCeiling)
                    {
                        screen[x,y] = " ";
                    }
                    else if(y > nCeiling && y < nFloor)
                    {
                        if (distanceToWall <= distCloses) screen[x, y] = "█";
                        else if (distanceToWall <= distMiddle) screen[x, y] = "▓";
                        else if (distanceToWall <= distFar) screen[x, y] = "▒";
                        else if (distanceToWall <= distMax) screen[x, y] = "░";
                    }
                    else
                    {
                        char nShade;
                        float b = 1.0f - (((float)y - nScreenHeight / 2.0f) / ((float)nScreenHeight / 2.0f));
                        if (b < 0.25) nShade = '#';
                        else if (b < 0.5) nShade = 'x';
                        else if (b < 0.75) nShade = '.';
                        else if (b < 0.9) nShade = '-';
                        else nShade = ' ';

                        screen[x, y] = nShade + "";
                    }
                }

            }


            for (int i = 0; i < screen.GetLength(1); i++)
            {
                string y = "";
                for (int x = 0; x < screen.GetLength(0); x++)
                {
                    y += screen[x, i];
                }
                sb.AppendLine(y);
            }


            Console.Clear();
            Console.Write(sb);
        }
    }
}
