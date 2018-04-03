
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

        private CanMakeMove CanMove = new CanMakeMove();

        public Label ScoreLabel;
        public Label HighScoreLabel;

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
                        BackgroundColor = Color.Gray,
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
            bool CanSpawnNext = false;
            List<Task> Animations = new List<Task>();
            x = 0;
            y = 0;
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
                                    await Task.WhenAll(
                                          GameTiles[y][x].TranslateTo(81, 0, 300),
                                          GameTiles[y][x].FadeTo(0, 300)
                                    );
                                    Tiles[y + 1][x].size *= 2;
                                    score += Tiles[y + 1][x].size;
                                    Tiles[y][x].size = 0;
                                    CanSpawnNext = true;
                                    //  await GameTiles[y][x].TranslateTo(-81, 0, 1);
                                    // await GameTiles[y][x].FadeTo(100, 1);
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
                                // GameTiles[y][x].TranslateTo(75, 0, 300);
                                Tiles[y + 1][x].size = Tiles[y][x].size;
                                CanSpawnNext = true;
                                Tiles[y][x].size = 0;
                                y++;
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
                                //GameTiles[y][x].TranslateTo(75, 0, 300);
                                Tiles[y + 1][x].size = Tiles[y][x].size;
                                CanSpawnNext = true;
                                Tiles[y][x].size = 0;
                                y++;
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

        public void TileMerge(Tile tile)
        {
            GameTiles[y][x].TranslateTo(81, 0, 300);
            GameTiles[y][x].FadeTo(0, 300);
        }
    }
    
}
