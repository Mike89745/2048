using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Reflection;
namespace XamarinGameMikes
{
    public class Game
    {
        public List<List<Tile>> Tiles = new List<List<Tile>>();
        public List<List<Image>> GameTiles = new List<List<Image>>();
        private Grid GameGrid;
        public List<List<Tile>> OldTiles = new List<List<Tile>>();
        private CanMakeMove CanMove = new CanMakeMove();

        public Label ScoreLabel;
        public Label HighScoreLabel;

        public int OldScore;
        public int score;
        private int HighScore = 50;


        int x = 0;
        int y = 0;

        public Game(Grid grid, Label Labelscore, Label LabelHighScore)
        {
            GameGrid = grid;
            ScoreLabel = Labelscore;
            HighScoreLabel = LabelHighScore;
            HighScoreLabel.Text = "HighScore" + HighScore.ToString();
            CreateGameField();
            CreateTileList();
            for (int i = 0; i < 4; i++)
            {
                Tiles[0][i].size = 2;
            }
            RenderGame();

        }
        public void CreateTileList()
        {
            for (int i = 0; i < 4; i++)
            {
                Tiles.Add(new List<Tile>());
            }
            foreach (var TileList in Tiles)
            {
                for (int i = 0; i < 4; i++)
                {
                    TileList.Add(new Tile());
                }
            }
        }
        private void CreateOldTileList()
        {
            OldTiles = new List<List<Tile>>();
            int counter = 0;
            for (int i = 0; i < 4; i++)
            {
                OldTiles.Add(new List<Tile>());
            }
            foreach (var OldTilesList in OldTiles)
            {
                for (int i = 0; i < 4; i++)
                {
                    OldTilesList.Add(new Tile()
                    {
                        size = Tiles[counter][i].size,
                    });
                }
                counter++;
            }
        }
        public void CreateGameField()
        {
            for (int i = 0; i < 4; i++)
            {
                GameTiles.Add(new List<Image>());
            }
            foreach (var LabelList in GameTiles)
            {
                for (int i = 0; i < 4; i++)
                {
                    LabelList.Add(new Image()
                    {
                        BackgroundColor = Color.LightGray,
                    });
                }
            }

            int c = 0;
            int b = 0;
            foreach (var LabelList in GameTiles)
            {
                foreach (var Label in LabelList)
                {
                    GameGrid.Children.Add(Label, b, c);
                    c++;
                }
                c = 0;
                b++;
            }
        }
        public Grid GetGameGrid()
        {
            return GameGrid;
        }
        public async Task RenderGame()
        {
            x = 0;
            y = 0;
            foreach (List<Tile> Tiles in Tiles)
            {
                foreach (Tile tile in Tiles)
                {
                    if (tile.size != 0)
                    {
                        GameTiles[y][x].Opacity = 100;
                        GameTiles[y][x].Source = "Tile_" + tile.size.ToString() + ".jpg";
                    }
                    else
                    {
                        GameTiles[y][x].Opacity = 0;
                    }
                    x++;
                }
                x = 0;
                y++;
            }
            ScoreLabel.Text = "Current score" + score.ToString();
            if (score > HighScore)
            {
                HighScoreLabel.Text = "HighScore" + score.ToString();
            }
        }
        public async Task SpawnRandomTileAsync()
        {
            Random RandTile = new Random();
            List<TilePos> FreeTiles = new List<TilePos>();
            int TileSize = 2;
            x = 0;
            y = 0;

            foreach (List<Tile> TileList in Tiles)
            {
                x = 0;
                foreach (Tile Tile in TileList)
                {
                    if (Tile.size == 0)
                    {

                        FreeTiles.Add(new TilePos()
                        {
                            PosX = x,
                            PosY = y,
                        });
                    }
                    x++;
                }
                y++;
            }
            if (RandTile.Next(0, 4) >= 3)
            {
                TileSize = 4;
            }
            if (FreeTiles.Count != 0)
            {
                int rand = RandTile.Next(0, FreeTiles.Count - 1);
                Tiles[FreeTiles[rand].PosY][FreeTiles[rand].PosX].size = TileSize;
                score += TileSize;
                await SpawnAnim(GameTiles[FreeTiles[rand].PosY][FreeTiles[rand].PosX]);
            }
            await RenderGame();
        }
        public async Task Right()
        {
            OldScore = score;
            CreateOldTileList();

            x = 0;
            y = 2;

            int counter = 0;

            int StartY = 0;
            int StartSize = 0;
            bool Moved = false;

            while (x < 4)
            {
                while (y >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        while (y < 3)
                        {
                            if (Tiles[y + 1][x].size == 0)
                            {
                                Tiles[y + 1][x].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                counter++;
                                y++;
                                Moved = true;
                            }
                            else
                            {
                                y--;
                                break;
                            }
                        }
                        y -= counter;
                        if (counter > 0)
                        {
                            // await TileMove(GameTiles[StartY][x].X, GameTiles[StartY][x].Y, StartSize, 81 * counter, (int)GameTiles[StartY][x].Y, false);
                            counter = 0;
                        }
                    }
                    else
                    {
                        y--;
                    }
                }

                counter = 0;
                y = 2;

                while (y >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        if (Tiles[y][x].size == Tiles[y + 1][x].size)
                        {
                            Tiles[y + 1][x].size *= 2;
                            Tiles[y][x].size = 0;
                            score += Tiles[y + 1][x].size;
                            counter++;
                            y--;
                            y--;
                            // await TileMove(GameTiles[StartY][x].X, GameTiles[StartY][x].Y, StartSize, 81 * counter, (int)GameTiles[StartY][x].Y, true);
                            counter = 0;
                            Moved = true;
                        }
                        else
                        {
                            y--;
                        }
                    }
                    else
                    {
                        y--;
                    }
                }

                counter = 0;
                y = 2;

                while (y >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        while (y < 3)
                        {
                            if (Tiles[y + 1][x].size == 0)
                            {
                                Tiles[y + 1][x].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                counter++;
                                y++;
                                Moved = true;
                            }
                            else
                            {
                                y--;
                                break;
                            }
                        }
                        y -= counter;
                        if (counter > 0)
                        {
                            // await TileMove(GameTiles[StartY][x].X, GameTiles[StartY][x].Y, StartSize, 81 * counter, (int)GameTiles[StartY][x].Y, false);
                            counter = 0;
                        }
                    }
                    else
                    {
                        y--;
                    }
                }

                counter = 0;
                y = 2;
                x++;
            }
            if (Moved)
            {
                await SpawnRandomTileAsync();
                CreateOldTileList();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                Debug.WriteLine("umrels");
            }
           
            await RenderGame();
        }
        public async Task Left()
        {
            OldScore = score;
            CreateOldTileList();

            x = 0;
            y = 3;

            int counter = 0;

            int StartY = 0;
            int StartSize = 0;

            bool Moved = false;

            while (x < 4)
            {
                while (y > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        while (y > 0)
                        {
                            if (Tiles[y - 1][x].size == 0)
                            {
                                Tiles[y - 1][x].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                counter++;
                                y--;
                                Moved = true;
                            }
                            else
                            {
                                y--;
                                break;
                            }
                        }
                        //await TileMove(GameTiles[StartY][x].X, GameTiles[StartY][x].Y, StartSize, -81 * counter, (int)GameTiles[StartY][x].Y, false);
                        counter = 0;
                    }
                    else
                    {
                        y--;
                    }
                }

                counter = 0;
                y = 3;

                while (y > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        if (Tiles[y][x].size == Tiles[y - 1][x].size)
                        {
                            Tiles[y - 1][x].size *= 2;
                            Tiles[y][x].size = 0;
                            score += Tiles[y - 1][x].size;
                            counter++;
                            y--;
                            y--;
                            // await TileMove(GameTiles[StartY][x].X, GameTiles[StartY][x].Y, StartSize, -81 * counter, (int)GameTiles[StartY][x].Y, true);
                            counter = 0;
                            Moved = true;
                        }
                        else
                        {
                            y--;
                        }
                    }
                    else
                    {
                        y--;
                    }
                }

                counter = 0;
                y = 3;

                while (y > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartY = y;
                        StartSize = Tiles[y][x].size;
                        while (y > 0)
                        {
                            if (Tiles[y - 1][x].size == 0)
                            {
                                Tiles[y - 1][x].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                counter++;
                                y--;
                                Moved = true;
                            }
                            else
                            {
                                y--;
                                break;
                            }
                        }
                        //await TileMove(GameTiles[StartY][x].X, GameTiles[StartY][x].Y, StartSize, -81 * counter, (int)GameTiles[StartY][x].Y, false);
                        counter = 0;
                    }
                    else
                    {
                        y--;
                    }
                }
                counter = 0;
                y = 3;
                x++;
            }
            if (Moved)
            {
                await SpawnRandomTileAsync();
                CreateOldTileList();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                Debug.WriteLine("umrels");
            }
            
            await RenderGame();
        }
        public async Task Up()
        {
            OldScore = score;
            CreateOldTileList();

            y = 0;
            x = 3;
            
            bool Moved = false;
            int counter = 0;

            int StartX = 0;
            int StartSize = 0;

            while (y < 4)
            {
                while (x > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        while (x > 0)
                        {
                            if (Tiles[y][x - 1].size == 0)
                            {
                                Tiles[y][x - 1].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                Moved = true;
                                counter++;
                                x--;
                            }
                            else
                            {
                                x--;
                                break;
                            }
                        }
                        x -= counter;
                        if (counter > 0)
                        {
                            //await TileMove(GameTiles[y][StartX].X, GameTiles[y][StartX].Y, StartSize, (int)GameTiles[y][StartX].X,81 * counter, false);
                            counter = 0;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }

                counter = 0;
                x = 3;

                while (x > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        if (Tiles[y][x].size == Tiles[y][x - 1].size)
                        {
                            Tiles[y][x - 1].size *= 2;
                            Tiles[y][x].size = 0;
                            score += Tiles[y][x - 1].size;
                            counter++;
                            x--;
                            x--;
                            //await TileMove(GameTiles[y][StartX].X, GameTiles[y][StartX].Y, StartSize, (int)GameTiles[y][StartX].X, 81 * counter, true);
                            counter = 0;
                            Moved = true;
                        }
                        else
                        {
                            x--;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }

                counter = 0;
                x = 3;

                while (x > 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        while (x > 0)
                        {
                            if (Tiles[y][x - 1].size == 0)
                            {
                                Tiles[y][x - 1].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                counter++;
                                x--;
                                Moved = true;
                            }
                            else
                            {
                                x--;
                                break;
                            }
                        }
                        x -= counter;
                        if (counter > 0)
                        {
                            //await TileMove(GameTiles[y][StartX].X, GameTiles[y][StartX].Y, StartSize, (int)GameTiles[y][StartX].X, 81 * counter, false);
                            counter = 0;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }

                counter = 0;
                x = 3;
                y++;
            }
            if (Moved)
            {
                await SpawnRandomTileAsync();
                CreateOldTileList();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                Debug.WriteLine("umrels");
            }

            await RenderGame();
        }
        public async Task Down()
        {
            OldScore = score;
            CreateOldTileList();

            y = 0;
            x = 2;

            int counter = 0;

            int StartX = 0;
            int StartSize = 0;
            bool Moved = false;

            while (y < 4)
            {
                while (x >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        while (x < 3)
                        {
                            if (Tiles[y][x + 1].size == 0)
                            {
                                Tiles[y][x + 1].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                counter++;
                                x++;
                                Moved = true;

                            }
                            else
                            {
                                x--;
                                break;
                            }
                        }
                        x -= counter;
                        if (counter > 0)
                        {
                            //await TileMove(GameTiles[y][StartX].X, GameTiles[y][StartX].Y, StartSize, (int)GameTiles[y][StartX].X, 81 * counter, false);
                            counter = 0;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }

                counter = 0;
                x = 2;

                while (x >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        if (Tiles[y][x].size == Tiles[y][x + 1].size)
                        {
                            Tiles[y][x + 1].size *= 2;
                            Tiles[y][x].size = 0;
                            score += Tiles[y][x + 1].size;
                            counter++;
                            x--;
                            x--;
                            //await TileMove(GameTiles[y][StartX].X, GameTiles[y][StartX].Y, StartSize, (int)GameTiles[y][StartX].X, 81 * counter, true);
                            counter = 0;
                            Moved = true;

                        }
                        else
                        {
                            x--;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }

                counter = 0;
                x = 2;

                while (x >= 0)
                {
                    if (Tiles[y][x].size != 0)
                    {
                        StartX = x;
                        StartSize = Tiles[y][x].size;
                        while (x < 3)
                        {
                            if (Tiles[y][x + 1].size == 0)
                            {
                                Tiles[y][x + 1].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                counter++;
                                x++;
                                Moved = true;

                            }
                            else
                            {
                                x--;
                                break;
                            }
                        }
                        x -= counter;
                        if (counter > 0)
                        {
                          //  await TileMove(GameTiles[y][StartX].X, GameTiles[y][StartX].Y, StartSize, (int)GameTiles[y][StartX].X, 81 * counter, false);
                            counter = 0;
                        }
                    }
                    else
                    {
                        x--;
                    }
                }

                counter = 0;
                x = 2;
                y++;
            }
            if (Moved)
            {
                await SpawnRandomTileAsync();
                CreateOldTileList();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                Debug.WriteLine("umrels");
            }

            await RenderGame();
        }

        public void GoBack()
        {
            score = OldScore;
            Tiles = OldTiles;
            RenderGame();
        }
        public async Task TileMove(double X, double Y, int size, int NewX, int NewY, bool merge)
        {
            if (size != 0)
            {
                Image AnimLabel = new Image()
                {
                    TranslationX = X,
                    TranslationY = Y,
                    BackgroundColor = Color.LightGray,
                    Source = "Tile_" + size.ToString() + ".jpg",
                };
                GameGrid.Children.Add(AnimLabel);
                if (merge)
                {
                    await AnimLabel.TranslateTo(NewX, NewY, 150);
                    await AnimLabel.ScaleTo(1.25, 100);
                    await AnimLabel.ScaleTo(1, 50);
                    await AnimLabel.FadeTo(0, 50);
                }
                else
                {
                    await AnimLabel.TranslateTo(NewX, NewY, 150);
                }
                GameGrid.Children.Remove(AnimLabel);
            }
        }
        public async Task SpawnAnim(Image label)
        {
            await label.ScaleTo(1.25, 100);
            await label.ScaleTo(1, 50);
        }
    }

}
