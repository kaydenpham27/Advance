using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents an instance of a Jester Piece, derived class from Base Class PieceType.
    /// The Jester is the only piece that cannot capture other pieces. It can move to any of the 8 
    /// adjoining squares, and it has two special abilities that make it a versatile, useful piece.
    /// The first ability is that the Jester is nimble and can exchange places with a friendly piece, if it 
    /// is on one of the adjoining squares.The only limitation here is that the Jester cannot exchange places with another Jester (as this would result in no change to the board state.)
    /// The other ability a Jester has is to convince enemy pieces to change to your side. This ability can be used on an enemy piece(other than the enemy General) in one of the 8 adjoining squares and changes that piece into one of your pieces.The Jester performs this action without moving
    /// </summary>
    public class Jester : PieceType
    {
        public override char Icon => 'j';
        public override int Value => 3;
        /// <summary>
        /// Getting the Piece's name.
        /// </summary>
        /// <returns>Return name of the Piece.</returns>
        public override string ToString()
        {
            return "Jester";
        }
        /// <summary>
        /// Find all the possible moves the piece can make in a specific Board state.
        /// </summary>
        /// <param name="x">x-coordinate of the piece in the board</param>
        /// <param name="y">y-coordinate of the piece in the board</param>
        /// <param name="board">the current board</param>
        /// <returns>A list of MoveType derived class elements, containing possible moves for a specific piece</returns>
        public override List<MoveType> FindMoves (int x, int y, Board myboard)
        {
            List<MoveType> moves = new List<MoveType>();
            Color color = myboard.Grid[x, y].Color;
            // Jester can move to cells, exchange and convince pieces in 8 adjacent cells.
            int[] moveX = { -1, 0, 1 };
            int[] moveY = { -1, 0, 1 };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // Skip because the piece does not move
                    if (moveX[i] == 0 && moveY[j] == 0) continue;
                    int newX = x + moveX[i];
                    int newY = y + moveY[j];
                    if (newX >= 0 && newY >= 0 && newX < 9 && newY < 9)
                    {
                        // Move to adjoining squares if it is not being occupied.
                        if (myboard.Grid[newX, newY].Type.ToString() == "None")
                        {
                            MoveType current = new Move(x, y, newX, newY);
                            moves.Add(current);
                        }
                        // Swap two ally pieces 
                        else if (myboard.Grid[newX, newY].Type.ToString() != "Wall" &&
                                 myboard.Grid[newX, newY].Color == color &&
                                 myboard.Grid[newX, newY].Type.ToString() != this.ToString())
                        {
                            MoveType current = new Swap(x, y, newX, newY);
                            moves.Add(current);
                        }

                        // Convince an enemy piece
                        else if (myboard.Grid[newX, newY].Type.ToString() != "Wall" &&
                                myboard.Grid[newX, newY].Color != color)
                        {
                            MoveType current = new Convince(x, y, newX, newY);
                            moves.Add(current);
                        }
                    }
                }
            }
            return moves;
        }
       
    }
}
