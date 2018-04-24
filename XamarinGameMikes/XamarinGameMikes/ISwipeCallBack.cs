using System;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace XamarinGameMikes
{
    public interface ISwipeCallBack
    {

        Task onLeftSwipeAsync(View view);
        Task onRightSwipeAsync(View view);
        Task onTopSwipeAsync(View view);
        Task onBottomSwipeAsync(View view);
        void onNothingSwiped(View view);
    }
}