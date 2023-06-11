using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents a Game instance with current player color and a game board. 
    /// </summary>
    internal class Game
    {
        private Color PlayerColor { get; }
        private Board Board { get; set; }
        /// <summary>
        /// Initializes a new Game with specific player color.
        /// </summary>
        /// <param name="PlayerColor">The color of the player</param>
        public Game(Color PlayerColor)
        {
            this.PlayerColor = PlayerColor;
            Board = new Board(9);
        }
        /// <summary>
        /// Read the contents from input file into the game board.
        /// </summary>
        /// <param name="reader">The TextReader used for reading the input file</param>
        /// <exception cref="Exception">Exception used for handling insufficient data from the input file</exception>
        public void Read(TextReader? reader)
        {
            for(int row = 0; row < Board.Size; row++)
            {
                string? currentRow = reader.ReadLine();
                if(currentRow == null) throw new Exception("Ran out of data before reading full board");
                if (currentRow.Length != Board.Size) throw new Exception($"Row {row} is not the right length");
                
                for(int col = 0; col < Board.Size; col++)
                {
                    char icon = currentRow[col];
                    Board.Grid[row, col].Color = Char.IsUpper(icon) ? Color.White : Color.Black;
                    Board.Grid[row, col].AddPiece(icon);
                }
            }
        }
        /// <summary>
        /// Write the game Board into the output file
        /// </summary>
        /// <param name="writer">The TextWriter used for writing game board into the output file</param>
        public void Write(TextWriter? writer)
        {
            for (int row = 0; row < Board.Size; row++)
            {
                for(int col = 0; col < Board.Size; col++)
                {
                    char Icon = Board.Grid[row, col].Type.Icon;
                    Color color = Board.Grid[row, col].Color;
                    if(color == Color.White) Icon = char.ToUpper(Icon);
                    writer.Write(Icon);
                }
                writer.WriteLine();
            }
        }
        /// <summary>
        /// Check if the current player is being checked (ally General is being targeted) in the current game board.
        /// </summary>
        /// <param name="color">The color of player to check for checkmate</param>
        /// <param name="myboard">The current game board</param>
        /// <returns>Return true if specified player is in checkmate, else return false. </returns>
        private bool isCheckMate(Color color, Board myboard)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (myboard.Grid[i, j].Color != color)
                    {
                        List<MoveType> current = myboard.Grid[i, j].Type.FindMoves(i, j, myboard);
                        for (int k = 0; k < current.Count; k++)
                        {
                            if (current[k].ToString() == "Capture" || current[k].ToString() == "Shoot")
                            {
                                if (myboard.Grid[current[k].EndX, current[k].EndY].Type.ToString() == "General" &&
                                    myboard.Grid[current[k].EndX, current[k].EndY].Color == color)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Find all moves a player can make using every pieces in a specific Board state. Then filter those moves using function 
        /// isCheckMate to get legal moves that the ally General is not in targeted. 
        /// </summary>
        /// <param name="myboard">The current game board</param>
        /// <param name="color">The color of player to find legal moves</param>
        /// <returns>Return a list of MoveType derived class elements. Containing legal moves for the specified player. </returns>
        private List<MoveType> FindAllMoves(Board myboard, Color color)
        {
            List<MoveType> moves = new List<MoveType>();

            for (int row = 0; row < Board.Size; row++)
            {
                for(int col = 0; col < Board.Size; col++)
                {
                    if (myboard.Grid[row, col].Color == color)
                    {
                        List<MoveType> move = myboard.Grid[row, col].Type.FindMoves(row, col, myboard);
                        for (int k = 0; k < move.Count; k++)
                        {
                            Board copy = myboard.Copy();
                            move[k].MakeMove(ref copy);
                            if (!isCheckMate(color, copy)) moves.Add(move[k]);
                        }
                    }
                }
            }
            return moves;
        }
        /// <summary>
        /// Find all the moves a player can make to maximize the advantage in material (Functionality 6) in current game board.
        /// Prioritise winning moves (opponent can not make any moves after those moves).  
        /// </summary>
        /// <param name="myboard">The current game board</param>
        /// <param name="color">The color of player to maximize advantage</param>
        /// <returns>Return a tuple of bool value (true when we find winning moves, else false) and
        /// a list of MoveType derived class elements containing moves that maximize material advantage for the specific player</returns>
        private (bool, List<MoveType>) MaximizeAdvantage(Board myboard, Color color)
        {
            List<MoveType> legalmoves = FindAllMoves(myboard, color);
            List<MoveType> moves = new List<MoveType>();
            int bestscore = int.MinValue;
            bool canWin = false;
            foreach (MoveType move in legalmoves)
            {
                Board copy = myboard.Copy();
                move.MakeMove(ref copy);
                int x = copy.Evaluate(color);
                /// Check if we can win (opponent doesn't have any move to play)
                List<MoveType> opponentMoves = FindAllMoves(copy, (color == Color.Black) ? Color.White: Color.Black);
                if(opponentMoves.Count == 0)
                {
                    canWin = true;
                    moves.Clear();
                    moves.Add(move);
                    break;
                }
                if (x > bestscore)
                {
                    bestscore = x;
                    moves.Clear();
                    moves.Add(move);
                }
                else if (x == bestscore)
                {
                    moves.Add(move);
                }
            }
            return (canWin, moves);
        }
        /// <summary>
        /// Works as functionality 7. This is a recursive function that acts as a small game that each player takes turn to play based on functionality 6, 
        /// this function will look into 3 turns deep to decide which move produces the best material advantage.
        /// </summary>
        /// <param name="myboard">The current game board</param>
        /// <param name="color">The color of player to maximize advantage</param>
        /// <param name="turn">The depth of prediction</param>
        /// <param name="alpha">Parameter used for alpha-beta pruning, reduce the number of explored nodes in the search tree</param>
        /// <param name="beta">Parameter used for alpha-beta pruning, reduce the number of explored nodes in the search tree</param>
        /// <returns></returns>
        private (int, List<MoveType>?) Predict(Board myboard, Color color, int turn, int alpha, int beta)
        {
            // Base:
            if (turn == 0) return (myboard.Evaluate(PlayerColor), new List<MoveType>());
            
            (bool canWin, List<MoveType>moves) = MaximizeAdvantage(myboard, color);

            (int, List<MoveType>?) bestT = (color == PlayerColor) ? (int.MinValue, new List<MoveType>()) : (int.MaxValue, new List<MoveType>());

            if (canWin)
            {
                bestT.Item2.Add(moves[0]);
                return (color == PlayerColor) ? (int.MaxValue, bestT.Item2) : (int.MinValue, bestT.Item2);
            }

            // Recursion:

            //(int, MoveType?) bestT = (color == PlayerColor) ? (int.MinValue, null) : (int.MaxValue, null);

            foreach (MoveType move in moves)
            {
                Board copy = myboard.Copy();
                move.MakeMove(ref copy);
                Color opponentColor = (color == Color.White) ? Color.Black : Color.White;
               
                (int, List<MoveType>?) cur = Predict(copy, opponentColor, turn - 1, alpha, beta);
               
                if (color == PlayerColor)
                {
                    if (cur.Item1 > bestT.Item1)
                    {
                        bestT.Item1 = cur.Item1;
                        bestT.Item2.Clear();
                        bestT.Item2.Add(move);
                    }
                    else if(cur.Item1 == bestT.Item1)
                    {
                        bestT.Item2.Add(move);
                    }
                    alpha = Math.Max(alpha, bestT.Item1);
                }
                else
                {
                    if (cur.Item1 < bestT.Item1)
                    {
                        bestT.Item1 = cur.Item1;
                        bestT.Item2.Clear();
                        bestT.Item2.Add(move);
                    }
                    else if (cur.Item1 == bestT.Item1)
                    {
                        bestT.Item2.Add(move);
                    }
                    beta = Math.Min(beta, bestT.Item1);
                }
                if (beta <= alpha) break;
            }
            return bestT;
        }  
        /// <summary>
        /// Play one turn using Predict function to produce the bestmove then make move to the current game board.
        /// </summary>
        public void PlayOneTurn()
        {

            (int bestScore, List<MoveType> moves) = Predict(Board, PlayerColor, 3, int.MinValue, int.MaxValue);
            if(moves.Count == 0)
            {
                Console.WriteLine("Impossible to make any legal moves");
                Environment.Exit(1);
            }

            /*List<MoveType> bestMoves = moves;

            foreach( MoveType move in moves)
            {
                Board copy = Board.Copy();
                move.MakeMove(ref copy);
                (int currentScore, List<MoveType> currentMoves) = Predict(Board, PlayerColor, 4, int.MinValue, int.MaxValue);
                if(currentScore > bestScore)
                {
                    bestScore = currentScore;
                    bestMoves = currentMoves;
                }
                //Console.WriteLine($"{move}, [{move.StartX}, {move.StartY}] -> [{move.EndX}, {move.EndY}]");
            }

            Random random = new Random();
            int randomIndex = random.Next(bestMoves.Count);
            MoveType bestMove = bestMoves[randomIndex];*/

            MoveType bestMove = moves[0];

            Board newBoard = Board.Copy();
            bestMove.MakeMove(ref newBoard);
            Board = newBoard.Copy();

            Console.WriteLine($"{bestMove}, [{bestMove.StartX}, {bestMove.StartY}] -> [{bestMove.EndX}, {bestMove.EndY}]");
           
        }

    }
}
