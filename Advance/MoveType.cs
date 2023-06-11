using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents an instance of move with starting coordinations (startX, startY) and ending coordinations (endX, endY).
    /// A Base Class for different type of moves. For example: Move, Capture, Demolish, Shoot, Convince, Swap, Build.
    /// </summary>
    public abstract class MoveType
    {
        public int StartX { get; }
        public int StartY { get; }
        public int EndX { get; }
        public int EndY { get; }

        /// <summary>
        /// Getting the name of the move type.
        /// </summary>
        /// <returns>The movement's name</returns>
        public override string ToString()
        {
            return "MoveType";
        }
        /// <summary>
        /// Initializes a move with starting points and ending points. 
        /// </summary>
        /// <param name="StartX">x-coordination of starting point</param>
        /// <param name="StartY">y-coordination of starting point</param>
        /// <param name="EndX">x-coordination of ending point</param>
        /// <param name="EndY">y-coordination of ending point</param>
        public MoveType(int StartX, int StartY, int EndX, int EndY)
        {
            this.StartX = StartX;
            this.StartY = StartY;
            this.EndX = EndX;
            this.EndY = EndY;
        }
        /// <summary>
        /// Make move directly to the current board
        /// </summary>
        /// <param name="myboard">the current board</param>
        public virtual void MakeMove(ref Board myboard) { }
    }
    /// <summary>
    /// Represents an instance of Move with starting coordinations (startX, startY) and ending coordinations (endX, endY),
    /// a derived class from base class MoveType.
    /// </summary>
    class Move : MoveType
    {
        /// <summary>
        /// Initializes a move with starting points and ending points. 
        /// </summary>
        /// <param name="StartX">x-coordination of starting point</param>
        /// <param name="StartY">y-coordination of starting point</param>
        /// <param name="EndX">x-coordination of ending point</param>
        /// <param name="EndY">y-coordination of ending point</param>
        public Move(int StartX, int StartY, int EndX, int EndY) : base(StartX, StartY, EndX, EndY) { }
        /// <summary>
        /// Getting the name of the move type.
        /// </summary>
        /// <returns>The movement's name</returns>
        public override string ToString()
        {
            return "Move";
        }
        /// <summary>
        /// Make move directly to the current board
        /// </summary>
        /// <param name="myboard">the current board</param>
        public override void MakeMove(ref Board myboard)
        {
            // Remove piece at ending point for piece at starting point.
            // Empty the starting point since there is no piece.
            PieceType newPiece = myboard.Grid[StartX, StartY].Type;
            Color color = myboard.Grid[StartX, StartY].Color;
            myboard.Grid[StartX, StartY].Type = new None();
            myboard.Grid[EndX, EndY].Type = newPiece;
            myboard.Grid[EndX, EndY].Color = color;
        }
    }
    class Capture : MoveType
    {
        /// <summary>
        /// Initializes a move with starting points and ending points. 
        /// </summary>
        /// <param name="StartX">x-coordination of starting point</param>
        /// <param name="StartY">y-coordination of starting point</param>
        /// <param name="EndX">x-coordination of ending point</param>
        /// <param name="EndY">y-coordination of ending point</param>
        public Capture(int StartX, int StartY, int EndX, int EndY) : base(StartX, StartY, EndX, EndY) { }
        /// <summary>
        /// Getting the name of the move type.
        /// </summary>
        /// <returns>The movement's name</returns>
        public override string ToString()
        {
            return "Capture";
        }
        /// <summary>
        /// Make move directly to the current board
        /// </summary>
        /// <param name="myboard">the current board</param>
        public override void MakeMove(ref Board myboard)
        {
            // Remove piece at ending point for piece at starting point.
            // Empty the starting point since there is no piece.
            PieceType newPiece = myboard.Grid[StartX, StartY].Type;
            Color color = myboard.Grid[StartX, StartY].Color;
            myboard.Grid[StartX, StartY].Type = new None();
            myboard.Grid[EndX, EndY].Type = newPiece;
            myboard.Grid[EndX, EndY].Color = color;
        }
    }
    class Swap : MoveType
    {
        /// <summary>
        /// Initializes a move with starting points and ending points. 
        /// </summary>
        /// <param name="StartX">x-coordination of starting point</param>
        /// <param name="StartY">y-coordination of starting point</param>
        /// <param name="EndX">x-coordination of ending point</param>
        /// <param name="EndY">y-coordination of ending point</param>
        public Swap(int StartX, int StartY, int EndX, int EndY) : base(StartX, StartY, EndX, EndY) { }
        /// <summary>
        /// Getting the name of the move type.
        /// </summary>
        /// <returns>The movement's name</returns>
        public override string ToString()
        {
            return "Swap";
        }
        /// <summary>
        /// Make move directly to the current board
        /// </summary>
        /// <param name="myboard">the current board</param>
        public override void MakeMove(ref Board myboard)
        {
            // Swap two pieces at starting and ending points
            (myboard.Grid[StartX, StartY].Type, myboard.Grid[EndX, EndY].Type) = (myboard.Grid[EndX, EndY].Type, myboard.Grid[StartX, StartY].Type);
        }
    }
    class Convince : MoveType
    {
        /// <summary>
        /// Initializes a move with starting points and ending points. 
        /// </summary>
        /// <param name="StartX">x-coordination of starting point</param>
        /// <param name="StartY">y-coordination of starting point</param>
        /// <param name="EndX">x-coordination of ending point</param>
        /// <param name="EndY">y-coordination of ending point</param>
        public Convince(int StartX, int StartY, int EndX, int EndY) : base(StartX, StartY, EndX, EndY) { }
        /// <summary>
        /// Getting the name of the move type.
        /// </summary>
        /// <returns>The movement's name</returns>
        public override string ToString()
        {
            return "Convince";
        }
        /// <summary>
        /// Make move directly to the current board
        /// </summary>
        /// <param name="myboard">the current board</param>
        public override void MakeMove(ref Board myboard)
        {
            // Convince a piece by changing its color. 
            myboard.Grid[EndX, EndY].Color = myboard.Grid[StartX, StartY].Color;
        }
    }
    class Build : MoveType
    {
        /// <summary>
        /// Initializes a move with starting points and ending points. 
        /// </summary>
        /// <param name="StartX">x-coordination of starting point</param>
        /// <param name="StartY">y-coordination of starting point</param>
        /// <param name="EndX">x-coordination of ending point</param>
        /// <param name="EndY">y-coordination of ending point</param>
        public Build(int StartX, int StartY, int EndX, int EndY) : base(StartX, StartY, EndX, EndY) { }
        /// <summary>
        /// Getting the name of the move type.
        /// </summary>
        /// <returns>The movement's name</returns>
        public override string ToString()
        {
            return "Build";
        }
        /// <summary>
        /// Make move directly to the current board
        /// </summary>
        /// <param name="myboard">the current board</param>
        public override void MakeMove(ref Board myboard)
        {
            // Build a new Wall at the ending point.
            myboard.Grid[EndX, EndY].Type = new Wall();
        }
    }
    class Shoot : MoveType
    {
        /// <summary>
        /// Initializes a move with starting points and ending points. 
        /// </summary>
        /// <param name="StartX">x-coordination of starting point</param>
        /// <param name="StartY">y-coordination of starting point</param>
        /// <param name="EndX">x-coordination of ending point</param>
        /// <param name="EndY">y-coordination of ending point</param>
        public Shoot(int StartX, int StartY, int EndX, int EndY) : base(StartX, StartY, EndX, EndY) { }
        /// <summary>
        /// Getting the name of the move type.
        /// </summary>
        /// <returns>The movement's name</returns>
        public override string ToString()
        {
            return "Shoot";
        }
        /// <summary>
        /// Make move directly to the current board
        /// </summary>
        /// <param name="myboard">the current board</param>
        public override void MakeMove(ref Board myboard)
        {
            // Remove a piece at ending point without moving the piece.
            myboard.Grid[EndX, EndY].Type = new None();
        }
    }
    class Demolish : MoveType
    {
        /// <summary>
        /// Initializes a move with starting points and ending points. 
        /// </summary>
        /// <param name="StartX">x-coordination of starting point</param>
        /// <param name="StartY">y-coordination of starting point</param>
        /// <param name="EndX">x-coordination of ending point</param>
        /// <param name="EndY">y-coordination of ending point</param>
        public Demolish(int StartX, int StartY, int EndX, int EndY) : base(StartX, StartY, EndX, EndY) { }
        /// <summary>
        /// Getting the name of the move type.
        /// </summary>
        /// <returns>The movement's name</returns>
        public override string ToString()
        {
            return "Demolish";
        }
        /// <summary>
        /// Make move directly to the current board
        /// </summary>
        /// <param name="myboard">the current board</param>
        public override void MakeMove(ref Board myboard)
        {
            // Remove the wall and move the Miner piece to the ending point.
            // Empty the starting point since the cell is empty.
            myboard.Grid[EndX, EndY].Type = new Miner();
            myboard.Grid[EndX, EndY].Color = myboard.Grid[StartX, StartY].Color;
            myboard.Grid[StartX, StartY].Type = new None();
        }
    }

}
