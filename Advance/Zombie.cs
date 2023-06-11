using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents an instance of a Zombie Piece, derived class from Base Class PieceType.
    /// Zombies can move to and capture pieces on any of the three adjoining squares in front of the Zombie
    /// If there is an enemy piece two squares away in any of those three directions and 
    /// the intermediate square is empty, a Zombie can perform a leaping attack, capturing the
    /// piece on that square. However, it can only perform this move if there is an enemy piece there that can be legally 
    /// captured. As the Zombie can only advance, once it reaches the back row (the top row for white, the 
    /// bottom row for black) it will no longer be able to make any moves.
    /// </summary>
    public class Zombie : PieceType
    {
        public override char Icon => 'z';
        public override int Value => 1;
        /// <summary>
        /// Getting the Piece's name.
        /// </summary>
        /// <returns>Return name of the Piece.</returns>
        public override string ToString()
        {
            return "Zombie";
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
            // Normal Move
            int[] moveX = new int[3];
            int[] moveY = new int[3];
            if (color == Color.White)
            {
                // White pieces move upward
                moveX[0] = -1; moveX[1] = -1; moveX[2] = -1;
                moveY[0] = 0; moveY[1] = -1; moveY[2] = 1;
            }
            else
            {
                // Black pieces move downward
                moveX[0] = 1; moveX[1] = 1; moveX[2] = 1;
                moveY[0] = 0; moveY[1] = -1; moveY[2] = 1;
            }
            for (int i = 0; i < 3; i++)
            {
                int newX = x + moveX[i];
                int newY = y + moveY[i];
                // Check if Zombie still moves inside the board.
                if (newX >= 0 && newY >= 0 && newX < myboard.Size && newY < myboard.Size)
                {
                    if (myboard.Grid[newX, newY].Type.ToString() == "None")
                    {
                        // Zombie can move to a new location that is not occupied by a piece.
                        MoveType current = new Move(x, y, newX, newY);
                        moves.Add(current);

                        // Leap attack
                        int newX2 = newX + moveX[i];
                        int newY2 = newY + moveY[i];
                        // Check if Zombie still moves inside the board.
                        if (newX2 >= 0 && newY2 >= 0 && newX2 < myboard.Size && newY2 < myboard.Size)
                        {
                            // Leap attack can only be performed if the intermediate cell is empty and the enemy piece can be legally captured.
                            // Zombie can not demolish a Wall.
                            if (myboard.Grid[newX2, newY2].Type.ToString() != "Wall" &&
                                myboard.Grid[newX2, newY2].Type.ToString() != "None" &&
                                myboard.Grid[newX2, newY2].Color != color &&
                                !myboard.Grid[newX2, newY2].Type.isProtected(newX2, newY2, myboard))
                            {
                                current = new Capture(x, y, newX2, newY2);
                                moves.Add(current);
                            }
                        }
                    }
                    // Zombie can perform normal attack in its moving direction if there is an enemy piece that can be legally captured.
                    // Zombie can not demolish a Wall.
                    if (myboard.Grid[newX, newY].Type.ToString() != "Wall" &&
                       myboard.Grid[newX, newY].Type.ToString() != "None" &&
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
