using System;
using System.Collections.Generic;
using System.Threading;
using MatrixLibrary;

namespace MatrixWithMutations
{
    class Program
    {
        public const int WINDOW_WIDTH = 200;
        public const int WINDOW_HEIGHT = 60;

        static void Main(string[] args)
        {
            Console.Title = "The Matrix";
            Console.SetWindowSize(WINDOW_WIDTH, WINDOW_HEIGHT + 1);
            Console.SetBufferSize(WINDOW_WIDTH, WINDOW_HEIGHT + 1);
            Console.CursorVisible = false;
            Console.Clear();
            
            var green = new ColorSet((int)ConsoleColor.White, (int)ConsoleColor.Green, (int)ConsoleColor.DarkGreen);
            var blue = new ColorSet((int)ConsoleColor.White, (int)ConsoleColor.Blue, (int)ConsoleColor.DarkBlue);
            var red = new ColorSet((int)ConsoleColor.White, (int)ConsoleColor.Red, (int)ConsoleColor.DarkRed);
            var magenta = new ColorSet((int)ConsoleColor.White, (int)ConsoleColor.Magenta, (int)ConsoleColor.DarkMagenta);
            var yellow = new ColorSet((int)ConsoleColor.White, (int)ConsoleColor.Yellow, (int)ConsoleColor.DarkYellow);
            var cyan = new ColorSet((int)ConsoleColor.White, (int)ConsoleColor.Cyan, (int)ConsoleColor.DarkCyan);
            var gray = new ColorSet((int)ConsoleColor.White, (int)ConsoleColor.Gray, (int)ConsoleColor.DarkGray);

            MatrixCode matrix = new MatrixCode(WINDOW_WIDTH, WINDOW_HEIGHT, UpdateChar, new ColorSet[] { green, blue, red, magenta, yellow, cyan, gray });
            matrix.Play(10);
        }

        /// <summary>
        /// Updates the character in the console.
        /// </summary>
        /// <param name="x">X position to update.</param>
        /// <param name="y">Y position to update.</param>
        /// <param name="character">Character to udpate to.</param>
        /// <param name="color">Colour to update to.</param>
        public static void UpdateChar(int x, int y, char character, int color)
        {
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color < 0 || color > 15 ? Console.BackgroundColor : (ConsoleColor)color;
            Console.Write(character);
        }
    }
}
