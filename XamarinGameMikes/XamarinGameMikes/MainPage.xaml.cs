using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinGameMikes
{
	public partial class MainPage : ContentPage
	{
        public Game gameManager;

        public MainPage()
        {
            InitializeComponent();
            gameManager = new Game(GameGrid,Score,HighScore);
            
            gameManager.SpawnRandomTile();

            Left.Command = new Command(() => gameManager.Left());
            Right.Command = new Command(() => gameManager.RightAsync());
            Up.Command = new Command(() => gameManager.Up());
            Down.Command = new Command(() => gameManager.Down());
        }
	}
}
