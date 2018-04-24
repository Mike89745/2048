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
            gameManager = new Game(GameGrid, Score, HighScore);
            SwipeListener swipeListener = new SwipeListener(GameGrid, this);

            Left.Command = new Command(async () => await gameManager.Left());
            Right.Command = new Command(async () => await gameManager.Right());
            Up.Command = new Command(async () => await gameManager.Up());
            Down.Command = new Command(async () => await gameManager.Down());
            Goback.Command = new Command(() => gameManager.GoBack());
        }

        public async Task onBottomSwipeAsync(View view)
        {
            await gameManager.Down();
        }
        public async Task onLeftSwipeAsync(View view)
        {
            await gameManager.Left();
        }

        public void onNothingSwiped(View view)
        {
            //Debug.WriteLine("NothingSwiped");
        }

        public async Task onRightSwipeAsync(View view)
        {
            await gameManager.Right();
        }

        public async Task onTopSwipeAsync(View view)
        {
            await gameManager.Up();
        }
    }
}
