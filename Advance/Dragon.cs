using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents an instance of a Dragon Piece, derived class from Base Class PieceType.
    /// The Dragon is a powerful piece that can move any number of squares in a straight line in any 
    /// of the 8 directions.It is most similar to a Queen in the game of chess, except for one
    /// downside: the Dragon cannot capture any piece it is immediately next to
    /// </summary>
    public class Dragon : PieceType
    {
        public override char Icon => 'd';
        public override int Value => 7;
        /// <summary>
        /// Getting the Piece's name.
        /// </summary>
        /// <returns>Return name of the Piece.</returns>
        public override string ToString()
        {
            return "Dragon";
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
        private void OptimizedFindMoves(int x, int y, Board myboard, int directionX, int directionY, ref List<MoveType> moves)
        {
            Color color = myboard.Grid[x, y].Color;
            for (int i = 1; i < 9; i++)
            {
                int newX = x + i * directionX;
                int newY = y + i * directionY;
                // Check if the piece still move inside the board.
                if (newX >= myboard.Size || newY >= myboard.Size || newX < 0 || newY < 0) break;
                // Dragon can move to a cell that is not being occupied.
                if (myboard.Grid[newX, newY].Type.ToString() == "None")
                {
                    MoveType current = new Move(x, y, newX, newY);
                    moves.Add(current);
                }
                else
                {
                    // Dragon can move and capture an enemy piece in its moving path if the enemy piece can be legally captured.
                    // Dragon can not demolish a Wall.
                    if (myboard.Grid[newX, newY].Color != color && myboard.Grid[newX, newY].Type.ToString() != "Wall" &&
                        !myboard.Grid[newX, newY].Type.isProtected(newX, newY, myboard) && i != 1)
                    {
                        MoveType current = new Capture(x, y, newX, newY);
                        moves.Add(current);
                    }
                    // Dragon can not move forward if it encounters a piece. Therefore, we break the loop.
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
            // Move in 8 directions: Vertically, Horizontally and Diagonally. 
            int[] directionX = { -1, 0, 1 };
            int[] directionY = { -1, 0, 1 };

            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    // Skip since the piece does not move.
                    if (directionX[i] == 0 && directionY[j] == 0) continue;
                    OptimizedFindMoves(x, y, myboard, directionX[i], directionY[j], ref moves);
                }
            }
            return moves;
        }
        
    }
}
