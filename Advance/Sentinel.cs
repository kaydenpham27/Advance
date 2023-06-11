using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents an instance of a Sentinel Piece, derived class from Base Class PieceType.
    ///  It moves and captures similarly to a Knight in the game of chess, moving two squares in one cardinal direction and then one square in a perpendicular direction, jumping over any intervening pieces(or walls).
    ///  A Sentinel will protect any piece of the same colour on those 4 squares, including other Sentinels.
    ///  Enemy pieces will not be able to capture a piece protected by a Sentinel. This includes the Catapult’s ranged attack.However, it does not include the Jester’s conversion move, which can be used even if the piece is protected by a Sentinel
    /// </summary>
    public class Sentinel : PieceType
    {
        public override char Icon => 's';
        public override int Value => 5;
        /// <summary>
        /// Getting the Piece's name.
        /// </summary>
        /// <returns>Return name of the Piece.</returns>
        public override string ToString()
        {
            return "Sentinel";
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
            Color color = myboard.Grid[x, y].Color;
            // Normal move, Sentinel moves like a Knight.
            int[] moveX = { -2, -1, 1, 2, -2, -1, 1, 2 };
            int[] moveY = { 1, 2, 2, 1, -1, -2, -2, -1 };
            for (int i = 0; i < 8; i++)
            {
                int newX = x + moveX[i];
                int newY = y + moveY[i];
                // Check if the piece still move inside the board.
                if (newX >= 0 && newY >= 0 && newX < myboard.Size && newY < myboard.Size)
                {
                    // Sentinel can move to a location that is not being occupied.
                    if (myboard.Grid[newX, newY].Type.ToString() == "None")
                    {
                        MoveType current = new Move(x, y, newX, newY);
                        moves.Add(current);
                    }
                    // Sentinel can capture an enemy piece in its moving path when the enemy piece can be legally captured.
                    // Sentinel can not demolish a Wall.
                    if (myboard.Grid[newX, newY].Type.ToString() != "None" &&
                        myboard.Grid[newX, newY].Type.ToString() != "Wall" &&
                        myboard.Grid[newX, newY].Color != color &&
                        !myboard.Grid[newX, newY].Type.isProtected(newX, newY, myboard))
                    {
                        MoveType current = new Capture(x, y, newX, newY);
                        moves.Add(current);
                    }
                }
            }
            return moves;
        }
      
    }
}
