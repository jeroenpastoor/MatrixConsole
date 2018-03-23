using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixLibrary
{
    public class MatrixLine
    {
        public event CharUpdateDelegate CharUpdateEvent;

        /// <summary>
        /// X Positon of this line.
        /// </summary>
        public int XPos { get; set; }

        /// <summary>
        /// Current position of the start of the line.
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        /// Current position where light transitions to dark.
        /// </summary>
        public int Middle { get; set; }

        /// <summary>
        /// Current end of the line.
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// Performs one tick on the line.
        /// </summary>
        /// <param name="charGrid">Grid of characters.</param>
        /// <param name="matrixCode">Base Matrix Code object.</param>
        public void Tick(char[,] charGrid, MatrixCode matrixCode)
        {
            Start++;
            Middle++;
            End++;
            if (Start < 0)
                return;
            if (End >= matrixCode.WINDOW_HEIGHT)
            {
                Reset(new Random(), matrixCode);
                return;
            }

            if (Start <= matrixCode.WINDOW_HEIGHT)
            {
                if(Start < matrixCode.WINDOW_HEIGHT)
                    CharUpdateEvent?.Invoke(XPos, Start, charGrid[XPos, Start], matrixCode.CurrentColor.c1);
                if (Start > 0)
                {
                    CharUpdateEvent?.Invoke(XPos, Start - 1, charGrid[XPos, Start - 1], matrixCode.CurrentColor.c2);
                }
            }

            if (Middle < 0)
                return;
            if (Middle < matrixCode.WINDOW_HEIGHT)
                CharUpdateEvent?.Invoke(XPos, Middle, charGrid[XPos, Middle], matrixCode.CurrentColor.c3);
            if (End < 0)
                return;
            CharUpdateEvent?.Invoke(XPos, End, charGrid[XPos, End], -1);
        }

        /// <summary>
        /// Change one character in the line to a different character.
        /// </summary>
        /// <param name="y">Y position to change.</param>
        /// <param name="character">Character to change to.</param>
        /// <param name="colors">Current color set to use.</param>
        public void ChangeChar(int y, char character, ColorSet colors)
        {
            if (y >= this.Start || y <= this.End)
                return;

            int color = -1;
            if (y < Start && y >= Middle)
            {
                color = colors.c2;
            }
            else
                color = colors.c3;
            CharUpdateEvent?.Invoke(XPos, y, character, color);
        }

        /// <summary>
        /// Constructor for a Matrix Line.
        /// </summary>
        /// <param name="x">X position of the line.</param>
        /// <param name="matrixCode">Base MatrixCode object.</param>
        /// <param name="r">Random number generator.</param>
        public MatrixLine(int x, MatrixCode matrixCode, Random r)
        {
            this.XPos = x;
            Reset(r, matrixCode, matrixCode.WINDOW_HEIGHT);
        }

        /// <summary>
        /// Resets the position of the line.
        /// </summary>
        /// <param name="r">Random number generator.</param>
        /// <param name="matrixCode">Base MatrixCode object.</param>
        /// <param name="min">Lowest y coordinate that we can reset to.</param>
        private void Reset(Random r, MatrixCode matrixCode, int min = 0)
        {
            int length = r.Next(3, matrixCode.WINDOW_HEIGHT/2);
            this.Start = -r.Next(min);
            this.Middle = this.Start - length;
            this.End = this.Middle - length;
        }
    }
}
