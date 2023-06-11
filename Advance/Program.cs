using System.Xml.Linq;

namespace Advance
{
    /// <summary>
    /// Main class 
    /// </summary>
    class Program
    {
        /// <summary>
        /// Starting point of the software. Load, Play and Save game based on the Player Color, path to input and output
        /// files from passed arguments. 
        /// </summary>
        /// <param name="args">Passed arguments contain information about Player Color, path to input and output files.</param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Hello, Advance World");
            /// Use ParseArguments function to ensure the inputs are correct and sufficient. 
            if (ParseArguments(args, out Color color, out string infile, out string outfile))
            {
                /// Using constructor to create game based on player color.  
                Game game = new Game(color);

                Load(game, infile);
                Play(game);
                Save(game, outfile);
            }
            else
            {
                Console.WriteLine("Passed Arguments are not correct!");
            }
        }
        /// <summary>
        /// Check if input arguments are correct and sufficient. Modify and return Player Color, path to input and output files.  
        /// </summary>
        /// <param name="args">Input arguments</param>
        /// <param name="color">Player Color</param>
        /// <param name="infile">Path to input file</param>
        /// <param name="outfile">Path to output file</param>
        /// <returns>Return true if input arguments are adequate, else return false.</returns>
        private static bool ParseArguments(string[] args, out Color color, out string infile, out string outfile)
        {
            // Bot name
            if (args[0] == "name")
            {
                Console.Write("Kien Pham");
                Environment.Exit(0);
            }
            infile = string.Empty;
            outfile = string.Empty;
            color = Color.White;
            if (args.Length >= 3)
            {
                infile = args[1];
                outfile = args[2];
                // Return true if successfully parse the color.
                return Enum.TryParse<Color>(args[0], ignoreCase: true, out color);
            }
            else
            {
                Console.WriteLine("Insufficient arguments passed to the program!");
                Environment.Exit(0);
                return false;
            }
        }
        /// <summary>
        /// Use information from input path to initialize the Board state in current Game. 
        /// </summary>
        /// <param name="game">Current Game</param>
        /// <param name="infile">Path to input file</param>
        private static void Load(Game game, string infile)
        {
            try
            {
                using StreamReader reader = new StreamReader(infile);
                game.Read(reader);
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error saving game to {infile}: {ex.Message}");
                Environment.Exit(1);
            }
        }
        /// <summary>
        /// Convert current Game information into output file.
        /// </summary>
        /// <param name="game">Current Game</param>
        /// <param name="outfile">Path to output file</param>
        private static void Save(Game game, string outfile)
        {
            try
            {
                using StreamWriter writer = new StreamWriter(outfile);
                game.Write(writer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game to '{0}': {1}", outfile, ex.Message);
            }
        }
        /// <summary>
        /// Play one turn of current Game.
        /// </summary>
        /// <param name="game">Current Game</param>
        private static void Play(Game game)
        {
            /// Use PlayOneTurn in Class Game to play.
            game.PlayOneTurn();
        }
    }
}
