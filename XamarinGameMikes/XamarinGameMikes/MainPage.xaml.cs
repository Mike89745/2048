using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinGameMikes
{
	public partial class MainPage : ContentPage, ISwipeCallBack
	{
        public Game gameManager;

        public MainPage()
        {
            InitializeComponent();
            gameManager = new Game(GameGrid,Score,HighScore);
            SwipeListener swipeListener = new SwipeListener(GameGrid, this);
            gameManager.SpawnRandomTile();

            Left.Command = new Command(() => gameManager.Left());
            Right.Command = new Command(() => gameManager.RightAsync());
            Up.Command = new Command(() => gameManager.Up());
            Down.Command = new Command(() => gameManager.Down());
            Goback.Command = new Command(() => gameManager.GoBack());
        }

        public void onBottomSwipe(View view)
        {
            //Debug.WriteLine("BottomSwipe");
            gameManager.Down();
        }

        public void onLeftSwipe(View view)
        {
           // Debug.WriteLine("LeftSwipe");
            gameManager.Left();
        }

        public void onNothingSwiped(View view)
        {
            //Debug.WriteLine("NothingSwiped");
        }

        public async Task onRightSwipe(View view)
        {
          //  Debug.WriteLine("RightSwipe");
            await gameManager.RightAsync();
        }

        public void onTopSwipe(View view)
        {
           // Debug.WriteLine("TopSwipe");
            gameManager.Up();
        }
    }
}
