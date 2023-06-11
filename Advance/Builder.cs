using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represent an instance of a Buider Piece, derived class from Base Class PieceType.
    /// The Builder can move and capture on any of the 8 adjoining squares
    /// Builders can also build walls on any of the 8 adjoining squares as well, 
    /// as long as there is  nothing occupying that square.The builder does this without moving. 
    /// Walls are special pieces. They do not belong to either player and they do not move. 
    /// However, they do obstruct other pieces as neither player can move a piece into a square
    /// that is being occupied by a wall.The only way to get rid of a wall is to capture it with a
    /// Miner.
    /// </summary>
    public class Builder : PieceType
    {
        public override char Icon => 'b';
        public override int Value => 2;
        /// <summary>
        /// Getting the Piece's name.
        /// </summary>
        /// <returns>Return name of the Piece.</returns>
        public override string ToString()
        {
            return "Builder";
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
            // Builder can move in 8 adjacent cells.
            int[] moveX = { -1, 0, 1 };
            int[] moveY = { -1, 0, 1 };
            List<MoveType> move = new List<MoveType>();
            Color color = myboard.Grid[x, y].Color;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Skip since the piece does not move.
                    if (moveX[i] == 0 && moveY[j] == 0) continue;
                    int newX = x + moveX[i];
                    int newY = y + moveY[j];
                    // Check if the piece still move inside the board.
                    if (newX >= 0 && newX < myboard.Size && newY >= 0 && newY < myboard.Size)
                    {
                        // Builder can move to a cell that is not being occupied.
                        if (myboard.Grid[newX, newY].Type.ToString() == "None")
                        {
                            MoveType current = new Move(x, y, newX, newY);
                            move.Add(current);
                            // Builder can build a wall at a cell that is not being occupied without moving.
                            current = new Build(x, y, newX, newY);
                            move.Add(current);
                        }
                        // Builder can capture an enemy piece that can be legally captured.
                        // Builder can not demolish a Wall.
                        else if (myboard.Grid[newX, newY].Color != color &&
                                myboard.Grid[newX, newY].Type.ToString() != "Wall" &&
                                !myboard.Grid[newX, newY].Type.isProtected(newX, newY, myboard))
                        {
                            MoveType current = new Capture(x, y, newX, newY);
                            move.Add(current);
                        }
                    }
                }
            }
            return move;
        }
       
    }
}
