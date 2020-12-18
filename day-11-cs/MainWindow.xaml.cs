using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace day_11_cs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private WriteableBitmap _outputBitmap;

        private void DisableButtons()
        {
            Part1Button.IsEnabled = false;
            Part2Button.IsEnabled = false;
        }

        private void EnableButtons()
        {
            Part1Button.IsEnabled = true;
            Part2Button.IsEnabled = true;
        }

        private async void Part1_Clicked(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            await Simulate(1);
            EnableButtons();
        }

        private async void Part2_Clicked(object sender, RoutedEventArgs e)
        {
            DisableButtons();
            await Simulate(2);
            EnableButtons();
        }

        private async Task Simulate(int part)
        {
            // Initialise our stuff.
            Cell[,] current = await LoadMapData("input.txt");

            Cell[,] next = (Cell[,]) current.Clone();

            RenderMap(current);

            bool stable = false;
            int stepCount = 0;
            int occupiedCount = 0;
            while (!stable)
            {
                await Task.Delay(50);
                stable = Step(current, next, part);
                stepCount++;
                RenderMap(next);
                (next, current) = (current, next);
                occupiedCount = CountCells(current, Cell.OccupiedChair);
                StatusText.Text = $"step: {stepCount}, occupied: {occupiedCount}";
            }

            StatusText.Text = $"DONE! {stepCount} steps, total occupied = {occupiedCount}";
        }

        private int CountCells(Cell[,] map, Cell cell)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            int count = 0;
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                if (map[x, y] == cell)
                    count++;
            return count;
        }

        private static readonly (int x, int y)[] Directions =
        {
            (-1, -1), (0, -1), (1, -1),
            (-1, 0),           (1, 0),
            (-1, 1),  (0, 1),  (1, 1),
        };

        private int CountOccupied(Cell[,] map, int x, int y, bool onlyAdjacent) => 
            Directions.Count(dir => CheckOccupiedInDir(map, x, y, dir.x, dir.y, onlyAdjacent));

        private bool CheckOccupiedInDir(Cell[,] map, int x, int y, int dx, int dy, bool onlyAdjacent)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);
            x += dx;
            y += dy;
            while (x >= 0 && x < width && y >= 0 && y < height)
            {
                if (map[x, y] == Cell.OccupiedChair) return true;
                if (map[x, y] == Cell.EmptyChair) return false;
                x += dx;
                y += dy;
                if (onlyAdjacent) break;
            }
            return false;
        }

        private bool Step(Cell[,] current, Cell[,] next, int part)
        {
            int width = current.GetLength(0);
            int height = current.GetLength(1);
            bool stable = true;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var (occupiedAround, maxOccupied) = part switch
                    {
                        1 => (CountOccupied(current, x, y, true), 4),
                        2 => (CountOccupied(current, x, y, false), 5),
                        _ => throw new InvalidOperationException("There are ony 2 parts!"),
                    };

                    var newState = current[x, y] switch
                    {
                        Cell.EmptyChair when occupiedAround == 0 => Cell.OccupiedChair,
                        Cell.OccupiedChair when occupiedAround >= maxOccupied => Cell.EmptyChair,
                        { } anyOther => anyOther,
                    };

                    if (newState != current[x, y]) stable = false;
                    next[x, y] = newState;
                }
            }

            return stable;
        }

        private void RenderMap(Cell[,] map)
        {
            _outputBitmap ??= new WriteableBitmap(map.GetLength(0), map.GetLength(1), 96, 96, PixelFormats.Bgra32,
                null);

            FerryImage.Source ??= _outputBitmap;

            _outputBitmap.Lock();
            try
            {
                unsafe
                {
                    var pixels = (byte*)_outputBitmap.BackBuffer.ToPointer();
                    for (int y = 0; y < _outputBitmap.PixelHeight; y++)
                    for (int x = 0; x < _outputBitmap.PixelWidth; x++)
                    {
                        var pixel = pixels + 4 * x + y * _outputBitmap.BackBufferStride;

                        var col = map[x, y] switch
                        {
                            Cell.Floor => Color.FromRgb(50,50,50),
                            Cell.EmptyChair => Color.FromRgb(50, 150, 50),
                            Cell.OccupiedChair => Color.FromRgb(150,50,50),
                        };

                        pixel[0] = col.B;
                        pixel[1] = col.G;
                        pixel[2] = col.R;
                        pixel[3] = col.A;
                    }
                }
                _outputBitmap.AddDirtyRect(new Int32Rect(0,0,_outputBitmap.PixelWidth, _outputBitmap.PixelHeight));
            }
            finally
            {
                _outputBitmap.Unlock();
            }
        }

        private enum Cell
        {
            Floor,
            EmptyChair,
            OccupiedChair,
        }

        private static Cell CellFromChar(char c) => c switch
        {
            '.' => Cell.Floor,
            'L' => Cell.EmptyChair,
            '#' => Cell.OccupiedChair,
            _ => throw new ArgumentException("Invalid character"),
        };

        private async Task<Cell[,]> LoadMapData(string filePath)
        {
            var lines = await File.ReadAllLinesAsync(filePath);

            var mapWidth = lines[0].Length;
            var mapHeight = lines.Length;
            var mapData = new Cell[mapWidth, mapHeight];
            for (int y = 0; y < mapHeight; y++)
            for (int x = 0; x < mapWidth; x++)
                mapData[x, y] = CellFromChar(lines[y][x]);
            return mapData;
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Display initial map.
            var initialMap = await LoadMapData("input.txt");
            RenderMap(initialMap);
        }
    }
}
