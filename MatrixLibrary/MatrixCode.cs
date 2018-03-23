using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MatrixLibrary
{
    /// <summary>
    /// Set of three colours to use for the matrix.
    /// </summary>
    public struct ColorSet
    {
        public int c1;
        public int c2;
        public int c3;

        public ColorSet(int c1, int c2, int c3)
        {
            this.c1 = c1;
            this.c2 = c2;
            this.c3 = c3;
        }
    }

    /// <summary>
    /// Event to update a character.
    /// </summary>
    /// <param name="x">X position to update.</param>
    /// <param name="y">Y position to update.</param>
    /// <param name="character">Character to update to.</param>
    /// <param name="color">Colour to update to.</param>
    public delegate void CharUpdateDelegate(int x, int y, char character, int color);

    public class MatrixCode
    {
        /// <summary>
        /// Width of the window.
        /// </summary>
        public readonly int WINDOW_WIDTH;

        /// <summary>
        /// Height of the window.
        /// </summary>
        public readonly int WINDOW_HEIGHT;

        /// <summary>
        /// Array of colours to use.
        /// </summary>
        public ColorSet[] Colors { get; private set; }

        /// <summary>
        /// Colour currently in use.
        /// </summary>
        public ColorSet CurrentColor { get; private set; }

        /// <summary>
        /// Grid of characters displayed.
        /// </summary>
        private char[,] _charGrid { get; set; }

        /// <summary>
        /// Array of MatrixLine objects.
        /// </summary>
        private MatrixLine[] _lines { get; set; }

        /// <summary>
        /// Constructor of the MatrixCode object.
        /// </summary>
        /// <param name="width">Width of the window.</param>
        /// <param name="height">Height of the window.</param>
        /// <param name="updateChar">Method to use for updating a character.</param>
        /// <param name="colorsets">Array of ColourSets to use. First colour in the set is used at the start.</param>
        public MatrixCode(int width, int height, CharUpdateDelegate updateChar, ColorSet[] colorsets)
        {
            if (colorsets.Length < 1)
                throw new ArgumentException("The array of ColorSets can not be empty!");

            WINDOW_WIDTH = width;
            WINDOW_HEIGHT = height;
            Colors = colorsets;
            CurrentColor = colorsets[0];
            Random r = new Random();

            _charGrid = new char[WINDOW_WIDTH, WINDOW_HEIGHT];
            _lines = new MatrixLine[WINDOW_WIDTH];
            for (int x = 0; x < WINDOW_WIDTH; x++)
            {
                MatrixLine line = new MatrixLine(x, this, r);
                line.CharUpdateEvent += updateChar;
                _lines[x] = line;
                for (int y = 0; y < WINDOW_HEIGHT; y++)
                {
                    _charGrid[x, y] = GetLetter(r);
                    updateChar(x, y, _charGrid[x, y], -1);
                }
            }
        }

        /// <summary>
        /// Constructor of the MatrixCode object.
        /// </summary>
        /// <param name="width">Width of the window.</param>
        /// <param name="height">Height of the window.</param>
        /// <param name="updateChar">Method to use for updating a character.</param>
        /// <param name="colorsets">ColourSet to use.</param>
        public MatrixCode(int width, int height, CharUpdateDelegate updateChar, ColorSet colorset)
            : this(width, height, updateChar, new ColorSet[] { colorset })
        {

        }

        /// <summary>
        /// Start playing the matrix visualization.
        /// </summary>
        /// <param name="delay">Delay between each frame.</param>
        public void Play(int delay = 0)
        {
            Random r = new Random();
            while (true)
            {
                for (int x = 0; x < WINDOW_WIDTH; x++)
                {
                    int end = Math.Min(Math.Max(_lines[x].End, 0), this.WINDOW_HEIGHT);
                    int start = Math.Min(Math.Max(_lines[x].Start, 0), this.WINDOW_HEIGHT);
                    for (int y = end; y < start; y++)
                    {
                        if(r.Next(50) == 49)
                        {
                            char newChar = GetLetter(r);
                            _charGrid[x, y] = newChar;
                            _lines[x].ChangeChar(y, newChar, CurrentColor);
                        }
                    }
                    _lines[x].Tick(_charGrid, this);
                }
                if(Colors.Length > 1)
                {
                    int num = r.Next(200 * Colors.Length);
                    if(num < Colors.Length)
                    {
                        this.CurrentColor = Colors[num];
                    }
                }
                Thread.Sleep(delay);
            }
        }

        /// <summary>
        /// Get a random character.
        /// </summary>
        /// <param name="r">Random number generator.</param>
        /// <returns>Random character.</returns>
        public static char GetLetter(Random r)
        {
            string chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
            int num = r.Next(0, chars.Length - 1);
            return chars[num];
        }
    }
}
