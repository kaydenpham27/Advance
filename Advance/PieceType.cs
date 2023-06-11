using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents an instance of PieceType with Piece's Icon and Piece's Value. A Base class for specific Pieces. 
    /// </summary>
    public abstract class PieceType
    {
        public abstract char Icon { get; }
        public abstract int Value { get; }

        /// <summary>
        /// Getting the Piece's name.
        /// </summary>
        /// <returns>Return name of the Piece.</returns>
        public override string ToString() {
            return "Piece";
        }
        /// <summary>
        /// Find all the possible moves a specific piece can make in a specific Board state.
        /// </summary>
        /// <param name="x">x-coordinate of the piece in the board</param>
        /// <param name="y">y-coordinate of the piece in the board</param>
        /// <param name="board">the current board</param>
        /// <returns>A list of MoveType derived class elements, containing possible moves for a specific piece</returns>
        public virtual List<MoveType> FindMoves(int x, int y, Board board)
        {
            List<MoveType> moves = new List<MoveType>();
            return moves;
        }
        /// <summary>
        /// Check if a specific piece is protected by an ally Sentinel.
        /// </summary>
        /// <param name="x">x-coordinate of the piece in the board</param>
        /// <param name="y">y-coordinate of the piece in the board</param>
        /// <param name="myboard">the current board</param>
        /// <returns>True if the piece is protected, else false</returns>
        public virtual bool isProtected(int x, int y, Board myboard)
        {
            Color color = myboard.Grid[x, y].Color;
            int[] moveX = { 1, -1, 0, 0 };
            int[] moveY = { 0, 0, 1, -1 };
            for (int i = 0; i < 4; i++)
            {
                int newX = x + moveX[i];
                int newY = y + moveY[i];
                if (newX >= 0 && newY >= 0 && newX < 9 && newY < 9)
                {
                    if (myboard.Grid[newX, newY].Type.ToString() == "Sentinel" &&
                        myboard.Grid[newX, newY].Color == color) return true;
                }
            }
            return false;
        }
        
    }
    
    /// <summary>
    /// Represents an instance of a None piece, derived class from Base Class PieceType.
    /// </summary>
    public class None : PieceType
    {
        public override string ToString()
        {
            return "None";
        }
        public override char Icon => '.';
        public override int Value => 0;
    }

    /// <summary>
    /// Represents an instance of a Wall piece, derived class from Base Class PieceType.
    /// </summary>
    public class Wall : PieceType
    {
        public override string ToString()
        {
            return "Wall";
        }
        public override char Icon => '#';
        public override int Value => 0;
    }

}
