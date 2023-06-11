using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represent an instance of a Catapult Piece, derived class from Base Class PieceType.
    /// The Catapult can only move 1 square at a time, and only in the 4 cardinal directions. Also, it 
    /// can only move to those squares – it cannot capture enemy pieces on those squares.
    /// However, the Catapult is able to capture pieces that are either 3 squares away in a cardinal 
    /// direction or 2 squares away in two perpendicular cardinal directions. The Catapult’s attack is special, in that it can remove an enemy piece on one of its capture squares, but it does not itself move when doing so.It also does not matter if there are other 
    /// pieces in the way, as the Catapult’s projectile flies over the battlefield.
    /// </summary>
    public class Catapult : PieceType
    {
        public override char Icon => 'c';
        public override int Value => 6;
        /// <summary>
        /// Getting the Piece's name.
        /// </summary>
        /// <returns>Return name of the Piece.</returns>
        public override string ToString()
        {
            return "Catapult";
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
            // Normal Move: only move 1 square at a time, in 4 cardinal directions.
            int[] moveX = { 1, -1, 0, 0 };
            int[] moveY = { 0, 0, 1, -1 };
            for (int i = 0; i < 4; i++)
            {
                int newX = x + moveX[i];
                int newY = y + moveY[i];
                // Check if the piece still move inside the board and move to a cell that is not being occupied.
                if (newX >= 0 && newY >= 0 && newX < myboard.Size && newY < myboard.Size && myboard.Grid[newX, newY].Type.ToString() == "None")
                {
                    MoveType current = new Move(x, y, newX, newY);
                    moves.Add(current);
                }
            }

            // Shoots pieces 3 square aways in a cardinal direction or 2 squares away in two perpendicular cardinal directions.
            int[] fightX = { -3, 3, 0, 0, -2, 2, -2, 2 };
            int[] fightY = { 0, 0, 3, -3, 2, 2, -2, -2 };
            for (int i = 0; i < 8; i++)
            {
                int newX = x + fightX[i];
                int newY = y + fightY[i];
                // Check if the piece still move inside the board.
                if (newX >= 0 && newY >= 0 && newX < myboard.Size && newY < myboard.Size)
                {
                    // Catapult can shoot a piece that can be legally captured without moving.
                    // Catapult can not shoot a Wall.
                    if (myboard.Grid[newX, newY].Type.ToString() != "None" &&
                        myboard.Grid[newX, newY].Type.ToString() != "Wall" &&
                        myboard.Grid[newX, newY].Color != color &&
                        !myboard.Grid[newX, newY].Type.isProtected(newX, newY, myboard))
                    {
                        MoveType current = new Shoot(x, y, newX, newY);
                        moves.Add(current);
                    }
                }
            }

            return moves;
        }
    }
}
