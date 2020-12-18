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

        private WriteableBitmap _outputBitmap = null;
        private async void Part1_Clicked(object sender, RoutedEventArgs e)
        {
            // Disable buttons.
            Part1Button.IsEnabled = false;
            Part2Button.IsEnabled = false;

            await Simulate(1);

            Part1Button.IsEnabled = true;
            Part2Button.IsEnabled = true;
        }

        private async void Part2_Clicked(object sender, RoutedEventArgs e)
        {
            // Disable buttons.
            Part1Button.IsEnabled = false;
            Part2Button.IsEnabled = false;

            await Simulate(2);

            Part1Button.IsEnabled = true;
            Part2Button.IsEnabled = true;
        }

        private async Task Simulate(int part)
        {
            // Initialise our stuff.
            Cell[,] current = await LoadMapData("input.txt");

            Cell[,] next = (Cell[,]) current.Clone();

            _outputBitmap = new WriteableBitmap(current.GetLength(0), current.GetLength(1), 96, 96, PixelFormats.Bgra32,
                null);

            RenderMap(current);

            FerryImage.Source = _outputBitmap;

            bool stable = false;
            int stepCount = 0;
            int occupiedCount = 0;
            while (!stable)
            {
                await Task.Delay(100);
                stable = Step(current, next, part);
                stepCount++;
                RenderMap(next);
                (next, current) = (current, next);
                occupiedCount = CountCells(current, Cell.OccupiedChair);
                StatusText.Text = $"step: {stepCount}, occupied: {occupiedCount}";
            }

            StatusText.Text = $"DONE! {stepCount} steps, total occupied = {occupiedCount}";
        }

        private Cell GetCell(Cell[,] map, int x, int y)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            if (x < 0 || x >= width) return Cell.Floor;
            if (y < 0 || y >= height) return Cell.Floor;
            return map[x, y];
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

        private int CountAdjOccupied(Cell[,] map, int x, int y)
        {
            int emptyCount = 0;
            
            // Top row
            if (GetCell(map, x - 1, y - 1) == Cell.OccupiedChair) emptyCount++;
            if (GetCell(map, x , y - 1) == Cell.OccupiedChair) emptyCount++;
            if (GetCell(map, x + 1, y - 1) == Cell.OccupiedChair) emptyCount++;

            // Middle row
            if (GetCell(map, x - 1, y ) == Cell.OccupiedChair) emptyCount++;
            if (GetCell(map, x + 1, y ) == Cell.OccupiedChair) emptyCount++;

            // Bottom row
            if (GetCell(map, x - 1, y + 1) == Cell.OccupiedChair) emptyCount++;
            if (GetCell(map, x, y + 1) == Cell.OccupiedChair) emptyCount++;
            if (GetCell(map, x + 1, y + 1) == Cell.OccupiedChair) emptyCount++;

            return emptyCount;
        }

        private static readonly (int x, int y)[] Directions =
        {
            (-1, -1), (0, -1), (1, -1),
            (-1, 0),           (1, 0),
            (-1, 1),  (0, 1),  (1, 1),
        };

        private int CountVisibleOccupied(Cell[,] map, int x, int y) => 
            Directions.Count(dir => OccupiedSeenFrom(map, x, y, dir.x, dir.y));

        private bool OccupiedSeenFrom(Cell[,] map, int x, int y, int dx, int dy)
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
                        1 => (CountAdjOccupied(current, x, y), 4),
                        2 => (CountVisibleOccupied(current, x, y), 5),
                        _ => throw new InvalidOperationException("There are ony 2 parts!"),
                    };

                    var newState = current[x, y] switch
                    {
                        Cell.EmptyChair when occupiedAround == 0 => Cell.OccupiedChair,
                        Cell.OccupiedChair when occupiedAround >= maxOccupied => Cell.EmptyChair,
                        Cell anyOther => anyOther,
                    };

                    if (newState != current[x, y]) stable = false;
                    next[x, y] = newState;
                }
            }

            return stable;
        }

        private void RenderMap(Cell[,] map)
        {
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
        
    }
}
