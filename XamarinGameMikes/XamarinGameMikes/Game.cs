
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using System.Threading.Tasks;
namespace XamarinGameMikes
{
    public class Game
    {
        public List<List<Tile>> Tiles = new List<List<Tile>>();
        public List<List<Label>> GameTiles = new List<List<Label>>();
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
                    OldTilesList.Add(new Tile() {
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
                GameTiles.Add(new List<Label>());
            }
            foreach (var LabelList in GameTiles)
            {
                for (int i = 0; i < 4; i++)
                {
                    LabelList.Add(new Label()
                    {
                        BackgroundColor = Color.LightGray,
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment = TextAlignment.Center,
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
        public void RenderGame()
        {
            x = 0;
            y = 0;
            foreach (List<Tile> Tiles in Tiles)
            {
                foreach (Tile tile in Tiles)
                {
                    if (tile.size != 0)
                    {
                        GameTiles[y][x].Text = tile.size.ToString();
                        GameTiles[y][x].Opacity = 100;
                    }
                    else
                    {
                        GameTiles[y][x].Opacity = 0;
                        GameTiles[y][x].Text = "";
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
        public void SpawnRandomTile()
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

                        FreeTiles.Add(new TilePos() {
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
            }

            RenderGame();
        }
        public async Task RightAsync()
        {
          //  Debug.WriteLine("??!");
            OldScore = score;
            CreateOldTileList();
            bool CanSpawnNext = false;
            List<Task> Animations = new List<Task>();
            x = 0;
            y = 0;
            int counter = 0;
            while (x < 4)
            {
                while (y < 3)
                {
                    for (int i = 0 + y; i < Tiles[x].Count; i++)
                    {
                        if (y < 3)
                        {
                            if (Tiles[y + 1][x].size != 0)
                            {
                                if (Tiles[y][x].size == Tiles[y + 1][x].size)
                                {
                                    Animations.Add(TileMerge(GameTiles[y][x].X, GameTiles[y][x].Y,Tiles[y][x].size,81,0));
                                    Tiles[y + 1][x].size *= 2;
                                    score += Tiles[y + 1][x].size;
                                    Tiles[y][x].size = 0;
                                    CanSpawnNext = true;
                                    y++;
                                    y++;
                                }

                                else
                                {
                                    y++;
                                }
                            }
                            else
                            {
                                counter = 1;
                                while (Tiles[y + 1][x].size == 0 && y < 3)
                                {
                                    Debug.WriteLine("TileMove");
                                    Tiles[y + 1][x].size = Tiles[y][x].size;
                                    CanSpawnNext = true;
                                    Tiles[y][x].size = 0;
                                    y++;
                                    counter++;
                                }
                                Animations.Add(TileMove(GameTiles[y][x].X, GameTiles[y][x].Y, Tiles[y][x].size, 81*counter, 0));
                            }
                        }
                    }
                    y = 0;
                    for (int i = 0 + y; i < Tiles[x].Count; i++)
                    {
                        if (y != 3)
                        {
                            if (Tiles[y + 1][x].size == 0)
                            {
                                counter = 1;
                                while (Tiles[y + 1][x].size == 0 && y < 3)
                                {
                                    Debug.WriteLine("TileMove1");
                                    Tiles[y + 1][x].size = Tiles[y][x].size;
                                    CanSpawnNext = true;
                                    Tiles[y][x].size = 0;
                                    counter++;
                                    y++;
                                }
                                Animations.Add(TileMove(GameTiles[y][x].X, GameTiles[y][x].Y, Tiles[y][x].size, 81 * counter, 0));
                            }
                            else
                            {
                                y++;
                            }
                        }
                    }
                }
                y = 0;
                x++;
            }

            Debug.WriteLine("??!");
            await Task.WhenAll(
                Animations
            );
            
            if (CanSpawnNext)
            {
                SpawnRandomTile();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                Debug.WriteLine("umrels");
            }
            RenderGame();
        }
        public void Left()
        {
            OldScore = score;
            CreateOldTileList();
            x = 3;
            y = 3;
            bool CanSpawnNext = false;
            while (x >= 0)
            {
                while (y > 0)
                {
                    for (int i = 0 + y; i >= 0; i--)
                    {

                        if (y >= 1)
                        {
                            if (Tiles[y - 1][x].size != 0)
                            {
                                if (Tiles[y][x].size == Tiles[y - 1][x].size)
                                {
                                    Tiles[y - 1][x].size *= 2;
                                    CanSpawnNext = true;
                                    score += Tiles[y - 1][x].size;
                                    Tiles[y][x].size = 0;

                                    y--;
                                    y--;
                                }

                                else
                                {
                                    y--;
                                }
                            }
                            else
                            {
                                Tiles[y - 1][x].size = Tiles[y][x].size;
                                CanSpawnNext = true;
                                Tiles[y][x].size = 0;
                                y--;
                            }
                        }
                    }
                    y = 3;
                    for (int i = 0 + y; i >= 0; i--)
                    {
                        if (y >= 1)
                        {
                            if (Tiles[y - 1][x].size == 0)
                            {
                                Tiles[y - 1][x].size = Tiles[y][x].size;
                                CanSpawnNext = true;
                                Tiles[y][x].size = 0;
                                y--;
                            }
                            else
                            {
                                y--;
                            }
                        }
                    }
                }
                y = 3;
                x--;
            }
            if (CanSpawnNext)
            {
                SpawnRandomTile();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                Debug.WriteLine("umrels");
            }
            RenderGame();
        }
        public void Up()
        {
            OldScore = score;
            CreateOldTileList();
            x = 3;
            y = 3;
            bool CanSpawnNext = false;
            while (y >= 0)
            {
                while (x > 0)
                {
                    for (int i = 0 + x; i >= 0; i--)
                    {
                        if (x >= 1)
                        {
                            if (Tiles[y][x - 1].size != 0)
                            {
                                if (Tiles[y][x].size == Tiles[y][x - 1].size)
                                {
                                    Tiles[y][x - 1].size *= 2;
                                    CanSpawnNext = true;
                                    score += Tiles[y][x - 1].size;
                                    Tiles[y][x].size = 0;
                                    x--;
                                    x--;
                                }

                                else
                                {
                                    x--;
                                }
                            }
                            else
                            {
                                Tiles[y][x - 1].size = Tiles[y][x].size;
                                CanSpawnNext = true;
                                Tiles[y][x].size = 0;
                                x--;
                            }
                        }
                    }
                    x = 3;
                    for (int i = 0 + x; i >= 0; i--)
                    {
                        if (x >= 1)
                        {
                            if (Tiles[y][x - 1].size == 0)
                            {
                                Tiles[y][x - 1].size = Tiles[y][x].size;
                                CanSpawnNext = true;
                                Tiles[y][x].size = 0;
                                x--;
                            }
                            else
                            {
                                x--;
                            }
                        }
                    }
                }
                x = 3;
                y--;
            }
            if (CanSpawnNext)
            {
                SpawnRandomTile();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                Debug.WriteLine("umrels");
            }
            RenderGame();
        }
        public void Down()
        {
            OldScore = score;
            CreateOldTileList();
            x = 0;
            y = 0;
            bool CanSpawnNext = false;
            while (y < 4)
            {
                while (x < 3)
                {
                    for (int i = 0 + x; i < Tiles[y].Count; i++)
                    {
                        if (x < 3)
                        {
                            if (Tiles[y][x + 1].size != 0)
                            {
                                if (Tiles[y][x].size == Tiles[y][x + 1].size)
                                {
                                    Tiles[y][x + 1].size *= 2;
                                    score += Tiles[y][x + 1].size;
                                    CanSpawnNext = true;
                                    Tiles[y][x].size = 0;
                                    x++;
                                    x++;
                                }

                                else
                                {
                                    x++;
                                }
                            }
                            else
                            {
                                Tiles[y][x + 1].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                CanSpawnNext = true;
                                x++;
                            }
                        }
                    }
                    x = 0;
                    for (int i = 0 + x; i < Tiles[y].Count; i++)
                    {
                        if (x != 3)
                        {
                            if (Tiles[y][x + 1].size == 0)
                            {
                                Tiles[y][x + 1].size = Tiles[y][x].size;
                                Tiles[y][x].size = 0;
                                CanSpawnNext = true;
                                x++;
                            }
                            else
                            {
                                x++;
                            }
                        }
                    }
                }
                x = 0;
                y++;
            }
            if (CanSpawnNext)
            {
                SpawnRandomTile();
            }
            else if (!CanMove.CanMove(Tiles))
            {
                Debug.WriteLine("umrels");
            }
            RenderGame();
        }

        public void GoBack()
        {
            score = OldScore;
            Tiles = OldTiles;
            RenderGame();
        }

        public async Task TileMerge(double X, double Y, int size, int NewX, int NewY)
        {
            Debug.WriteLine("TileMerge");
            Label AnimLabel = new Label()
            {
                Text = size.ToString(),
                TranslationX = X,
                TranslationY = Y,
            };
            await AnimLabel.TranslateTo(81, 0, 300);
            await AnimLabel.FadeTo(0, 300);
        }
        public async Task TileMove(double X, double Y,int size,int NewX,int NewY)
        {
            Debug.WriteLine("TileMove");
            Label AnimLabel = new Label()
            {
                Text = size.ToString(),
                TranslationX = X,
                TranslationY = Y,
            };

            await AnimLabel.TranslateTo(NewX, NewY, 300);
        }
    }
    
}
