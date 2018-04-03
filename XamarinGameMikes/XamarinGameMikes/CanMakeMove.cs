using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinGameMikes
{
    class CanMakeMove
    {
        int x;
        int y;
        public List<List<Tile>> Tiles = new List<List<Tile>>();
        public bool CanMove(List<List<Tile>> NewTiles)
        {
            Tiles = NewTiles;
            if (!Right())
            {
                if (!Left())
                {
                    if (!Up())
                    {
                        if (!Down())
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        
        public bool Right()
        {
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
                                    return true;
                                }

                                else
                                {
                                    y++;
                                }
                            }
                            else
                            {
                                return true;
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
                                return true;
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
            return false;
        }
        public bool Left()
        {
            x = 3;
            y = 3;
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
                                    return true;
                                }

                                else
                                {
                                    y--;
                                }
                            }
                            else
                            {
                                return true;
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
                                return true;
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
            return false;
        }
        public bool Up()
        {
            x = 3;
            y = 3;
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
                                    return true;
                                }

                                else
                                {
                                    x--;
                                }
                            }
                            else
                            {
                                return true;
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
                                return true;
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
            return false;
        }
        public bool Down()
        {
            x = 0;
            y = 0;
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
                                    return true;

                                }

                                else
                                {
                                    x++;
                                }
                            }
                            else
                            {
                                return true;

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
                                return true;

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
            return false;
        }
    }
}
