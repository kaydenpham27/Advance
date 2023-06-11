using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents an instance of a General Piece, derived class from Base Class PieceType.
    /// The General functions almost identically to the King in chess. It can move and capture on any of the 8 adjoining squares, like the Builder.
    /// The General is not allowed to be captured. Moving the General to a square where an enemy piece could capture it(moving it into danger) is an illegal move
    /// If your opponent makes a move that places your General in the attacking range of one of their pieces(putting your General in danger), 
    /// you must get your General out of danger on the next move, any move that does not achieve this is an illegal move.
    /// </summary>
    public class General : PieceType
    {
        public override char Icon => 'g';
        public override int Value => 999;
        /// <summary>
        /// Getting the Piece's name.
        /// </summary>
        /// <returns>Return name of the Piece.</returns>
        public override string ToString()
        {
            return "General";
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
            // General can move and capture pieces in 8 adjacent cells.
            int[] moveX = { -1, 0, 1 };
            int[] moveY = { -1, 0, 1 };
            Color color = myboard.Grid[x, y].Color;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Skip because the piece does not move.
                    if (moveX[i] == 0 && moveY[j] == 0) continue;
                    int newX = x + moveX[i];
                    int newY = y + moveY[j];
                    // Check if the piece still move inside the board.
                    if (newX >= 0 && newY >= 0 && newX < myboard.Size && newY < myboard.Size)
                    {
                        // General can move to cell that it is not being occupied.
                        if (myboard.Grid[newX, newY].Type.ToString() == "None")
                        {
                            MoveType current = new Move(x, y, newX, newY);
                            moves.Add(current);
                        }
                        // General can capture enemy pieces if they can be legally captured.
                        // General can not demolish a Wall.
                        else if (myboard.Grid[newX, newY].Type.ToString() != "Wall" &&
                                myboard.Grid[newX, newY].Color != color &&
                                !myboard.Grid[newX, newY].Type.isProtected(newX, newY, myboard))
                        {
                            MoveType current = new Capture(x, y, newX, newY);
                            moves.Add(current);
                        }
                    }
                }
            }
            return moves;
        }
    }
}
