using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents an instance of a Miner Piece, derived class from Base Class PieceType.
    /// The Miner moves like a Rook does in chess, it can move any number of squares in one of the 4 cardinal
    /// directions(up, down, left and right) and can capture at any of those positions as well.
    /// Miner is able to demolish a wall, it captures walls in the same way it captures pieces, and it is subject to the same limitations:  it can only 
    /// capture one piece(or wall) in a move and there must be an unobstructed path to that piece
    /// in one of the cardinal directions
    /// </summary>
    public class Miner : PieceType
    {
        public override char Icon => 'm';
        public override int Value => 4;
        /// <summary>
        /// Getting the Piece's name.
        /// </summary>
        /// <returns>Return name of the Piece.</returns>
        public override string ToString()
        {
            return "Miner";
        }
        /// <summary>
        /// Avoid repeating myself by creating a function to find moves in different directions based on parsed in parameters
        /// </summary>
        /// <param name="x">x-coordinate of the piece in the board</param>
        /// <param name="y">y-coordinate of the piece in the board</param>
        /// <param name="myboard">the current board</param>
        /// <param name="directionX">direction in x-axis</param>
        /// <param name="directionY">direction in y-axis</param>
        /// <param name="moves">List of possible moves of the piece</param>
        private void OptimizedFindMoves (int x, int y, Board myboard, int directionX, int directionY, ref List<MoveType> moves)
        {
            Color color = myboard.Grid[x, y].Color;

            for (int i = 1; i < 9; i++)
            {
                int newX = x + i * directionX;
                int newY = y + i * directionY;
                // Break the loop if the piece moves outside the board.
                if (newX >= myboard.Size || newY >= myboard.Size || newX < 0 || newY < 0) break;
                // Miner can move to a location that is not being occupied.
                if (myboard.Grid[newX, newY].Type.ToString() == "None")
                {
                    MoveType current = new Move(x, y, newX, newY);
                    moves.Add(current);
                }
                else
                {
                    // Miner can move to a location that is occupied by a wall and demolish it.
                    if (myboard.Grid[newX, newY].Type.ToString() == "Wall")
                    {
                        MoveType current = new Demolish(x, y, newX, newY);
                        moves.Add(current);
                    }
                    // Miner can move to a location that is occupied by an enemy piece and capture that piece if it can be legally captured.
                    else if (myboard.Grid[newX, newY].Color != color && !myboard.Grid[newX, newY].Type.isProtected(newX, newY, myboard))
                    {
                        MoveType current = new Capture(x, y, newX, newY);
                        moves.Add(current);
                    }
                    // If a Miner encounters another piece, it can not move forward. Therefore, we break the loop.
                    break;
                }
            }
        }
        /// <summary>
        /// Find all the possible moves the piece can make in a specific Board state.
        /// </summary>
        /// <param name="x">x-coordinate of the piece in the board</param>
        /// <param name="y">y-coordinate of the piece in the board</param>
        /// <param name="board">the current board</param>
        /// <returns>A list of MoveType derived class elements, containing possible moves for a specific piece</returns>
        public override List<MoveType> FindMoves(int x, int y, Board myboard)
        {
            List<MoveType> moves = new List<MoveType>();
            // Directions to move vertically and horizontally. 
            int[] directionX = { 1, -1, 0, 0 };
            int[] directionY = { 0, 0, -1, 1 };
            for(int i = 0; i < 4; i++)
            {
                OptimizedFindMoves(x, y, myboard, directionX[i], directionY[i], ref moves);
            }
            return moves;
        }
        
    }
}
