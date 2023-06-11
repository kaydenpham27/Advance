using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represent a Board instance that consists of Cells according to the Size of the board.
    /// </summary>
    public class Board
    {
        public Cell[,] Grid { get; set; }
        public int Size { get; }
        /// <summary>
        /// Initializes a new Board based on the size parameter. 
        /// </summary>
        /// <param name="size">The size of the board</param>
        public Board(int size) 
        {
            Size = size;
            Grid = new Cell[Size, Size];
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j] = new Cell(i, j, new None(), Color.Black);
                }
            }
        }
        /// <summary>
        /// Produces a Copy of a specific board.
        /// </summary>
        /// <returns>Return a copy of the original board</returns>
        public Board Copy()
        {
            Board copy = new Board(Size);

            for(int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    PieceType Type = Grid[i, j].Type;
                    Color Color = Grid[i, j].Color;
                    copy.Grid[i, j] = new Cell(i, j, Type, Color);
                }
            }
            return copy;
        }
        /// <summary>
        /// Evaluate a board based on the color of player.
        /// </summary>
        /// <param name="color">The color of player to evaluate</param>
        /// <returns>Return the score of the evaluated board</returns>
        public int Evaluate(Color color)
        {
            int Score = 0;
            for(int i = 0; i < Size; i++)
            {
                for(int j = 0; j < Size; j++)
                {
                    int PieceValue = Grid[i, j].Type.Value;
                    if (Grid[i, j].Color == color) Score += PieceValue;
                    else Score -= PieceValue;
                }
            }
            return Score;
        }
    }
}
