using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Advance
{
    /// <summary>
    /// Represents a Cell instance with row and col number, type of the Piece occupied this Cell and its color.
    /// </summary>
    public class Cell
    {
        public int RowNumber { get; }
        public int ColumnNumber { get; }
        public PieceType Type { get; set; }
        public Color Color { get; set; }
        
        /// <summary>
        /// Initializes a new Cell with column, row number, PieceType of the current Piece occupied this cell and its color.
        /// </summary>
        /// <param name="x">x-coordinate of the cell</param>
        /// <param name="y">y-coordinate of the cell</param>
        /// <param name="type">PieceType of current Piece occupied this cell</param>
        /// <param name="color">The color of this Piece</param>
        public Cell(int x, int y, PieceType type, Color color)
        {
            RowNumber = x;
            ColumnNumber = y;
            Type = type;
            Color = color;
        }

        /// <summary>
        /// Convert characters from the input file into PieceType variables.
        /// </summary>
        /// <param name="icon">The character that needs to be converted</param>
        /// <exception cref="ArgumentException">Exception used for handling the unrecognised character from the input file</exception>
        public void AddPiece(char icon)
        {
            // change icon to lowercase since piece's icon is defined in lowercase letter.
            icon = char.ToLower(icon);
            PieceType pieceType;
            switch (icon)
            {
                case 'z':
                    pieceType = new Zombie(); break;
                case 'b':
                    pieceType = new Builder(); break;
                case 'm':
                    pieceType = new Miner(); break;
                case 'j':
                    pieceType = new Jester(); break;
                case 's':
                    pieceType = new Sentinel(); break;
                case 'c':
                    pieceType = new Catapult(); break;
                case 'd':
                    pieceType = new Dragon(); break;
                case 'g':
                    pieceType = new General(); break;
                case '.':
                    pieceType = new None(); break;
                case '#':
                    pieceType = new Wall(); break;
                default:
                    throw new ArgumentException("Unrecognised icon");
            }
            Type = pieceType;
        }

    }
}
