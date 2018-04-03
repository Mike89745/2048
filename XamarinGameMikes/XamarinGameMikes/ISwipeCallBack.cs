using System;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace XamarinGameMikes
{
    public interface ISwipeCallBack
    {

        void onLeftSwipe(View view);
        Task onRightSwipe(View view);
        void onTopSwipe(View view);
        void onBottomSwipe(View view);
        void onNothingSwiped(View view);
    }
}