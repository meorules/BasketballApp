using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SkiaSharp;
using SkiaSharp.Views.Forms;
using TouchTracking.Forms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

using BasketballApp.ViewModels;
using BasketballApp.Models;
using Xamarin.Forms.Internals;
using System.Numerics;
using System.Security.Cryptography;

namespace BasketballApp.Views
{
  [XamlCompilation(XamlCompilationOptions.Compile)]
  public partial class DataCollectionPage : ContentPage
  {

    SKPaint greenLineCircle = new SKPaint
    {
      Style = SKPaintStyle.Stroke,
      Color = SKColors.Green,
      StrokeWidth = 2
    };

    SKPaint redCross = new SKPaint
    {
      Style = SKPaintStyle.Stroke,
      Color = SKColors.Red,
      StrokeWidth = 2
    };

    SKPaint whiteStrokePaint = new SKPaint
    {
      Style = SKPaintStyle.Stroke,
      Color = SKColors.White,
      StrokeWidth = 1,
      StrokeCap = SKStrokeCap.Butt,
      IsAntialias = true
    };

    SKPaint halfcourtBackgroundColor = new SKPaint
    {
      Style = SKPaintStyle.StrokeAndFill,
      Color = SKColors.White,
      ColorF = SKColors.Navy,
      StrokeCap = SKStrokeCap.Round,
      IsAntialias = true
    };

    SKPath basketballHalfCourt = new SKPath();
    SKPath redCrossPath = new SKPath();

    TimeSpan clock;
    TimeSpan shotClock;

    bool timePlay = false;

    bool blocked = false;

    bool reDraw = false;

    


      public DataCollectionPage()
    {
      this.BindingContext = new GameObjectViewModel();
      var viewModel = (GameObjectViewModel)BindingContext;

      clock = viewModel.getGameClock();
      shotClock = viewModel.getGameShotClock();

      InitializeComponent();


      //Title = "Data Collection Page"; 

      Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
      {
        if (reDraw)
        {
          canvasView.InvalidateSurface();
          reDraw = false;
        }
        clockButton.Text = clock.ToString(@"mm\:ss\.f");
        shotClockButton.Text = shotClock.ToString(@"ss\.f");
        if (timePlay)
        {
          //Call Add minutes to each player
          viewModel.AddMinutes(TimeSpan.FromMilliseconds(100));
          clock = clock-TimeSpan.FromMilliseconds(100);
          shotClock = shotClock - TimeSpan.FromMilliseconds(100);

          if (shotClock == TimeSpan.Zero)
          {
            shotClock = new TimeSpan(0, 0, 24);
            timePlay = false;
          }
        }
        
        return true;
      });

      redCrossPath.MoveTo(0, 0);
      redCrossPath.LineTo(10, 10);
      redCrossPath.MoveTo(10, 0);
      redCrossPath.LineTo(0, 10);
      redCrossPath.Close();
      

      basketballHalfCourt.MoveTo(20, 0);
      basketballHalfCourt.LineTo(20, 30);
      basketballHalfCourt.CubicTo(20, 125, 195, 125, 195, 30);
      basketballHalfCourt.LineTo(195, 0);
      basketballHalfCourt.MoveTo(135, 65);
      basketballHalfCourt.LineTo(80, 65);
      basketballHalfCourt.LineTo(80, 0);
      basketballHalfCourt.LineTo(135, 0);
      basketballHalfCourt.LineTo(135, 65);
      basketballHalfCourt.AddCircle(107.5f, 65, 22); 
      basketballHalfCourt.MoveTo(0, 0);
      basketballHalfCourt.LineTo(0, 130);
      basketballHalfCourt.LineTo(210, 130);
      basketballHalfCourt.LineTo(210, 0);
      basketballHalfCourt.LineTo(0, 0);
      basketballHalfCourt.Close();

    }

    protected override async void OnAppearing()
    {
      base.OnAppearing();
      var viewModel = (GameObjectViewModel)BindingContext;
      viewModel.initaliseData();

      clock = viewModel.getGameClock();
      shotClock = viewModel.getGameShotClock();

    }

    void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
    {
      var viewModel = (GameObjectViewModel)BindingContext;

      SKSurface surface = args.Surface;
      SKCanvas canvas = surface.Canvas;
      canvas.Clear(SKColors.AliceBlue);

      int width = args.Info.Width;
      int height = args.Info.Height;
      int middleX = width / 2;
      int middleY = height / 2;

      /*      canvas.DrawRoundRect(1430, 260, 1500, 1000,10,10, halfcourtBackgroundColor);
       *      
      */
      canvas.Save();
      canvas.Translate(width, height - 5);
      canvas.RotateDegrees(180);
      canvas.DrawRect(0, 0, 1400, 960, halfcourtBackgroundColor);
      canvas.Restore();

      //Drawing the HalfCourt
      canvas.Save();
      canvas.Translate(width-10, height-20);
      canvas.Scale(6.5f, 7);
      canvas.RotateDegrees(180);
      canvas.DrawPath(basketballHalfCourt, whiteStrokePaint);
      canvas.Restore();


      //Green Circle Drawing, this can be called to add where makes are
        List<GameLogActivity> makes = viewModel.returnShots(1);
        foreach (GameLogActivity make in makes)
        {
          if (make != null)
          {
            canvas.Save();
          //canvas.Translate(width/2, height/2);
          float x = (float)(3.525 *  make.positionX);
          float y = (float)(3.546 *  make.positionY);

            canvas.Translate(x, y);
            canvas.Scale(5, 5);
            canvas.DrawCircle(0, 0, 5, greenLineCircle);
            canvas.Restore();
          }

        }


        //Drawing Red Crosses for misses
        List<GameLogActivity> misses = viewModel.returnShots(2);
        foreach (GameLogActivity miss in misses)
        {
          if (miss != null)
          {
            canvas.Save();
          //canvas.Translate(width / 2, height / 2);
          float x = (float)(3.525 * miss.positionX);
          float y = (float)(3.546 * miss.positionY);

          canvas.Translate(x,y);
            canvas.Scale(5, 5);
            canvas.DrawPath(redCrossPath, redCross);
            canvas.Restore();
          }
        }

    }
    private async void TouchEffect_TouchAction(object sender, TouchTracking.TouchActionEventArgs args)
    {
      if (blocked == false)
      {
        var x = args.Location.X;
        var y = args.Location.Y;
        if (x >= 405 && y >= 80)
        {
          timePlay = false;
          blocked=true;
          string assistedPlayer = "No One";

          string playerName = await Shell.Current.DisplayActionSheet("Pick a Player", "Cancel", null, Player1Name.Text, Player2Name.Text, Player3Name.Text, Player4Name.Text, Player5Name.Text);
          if (playerName != "Cancel" && playerName != null)
          {
            string makeOrMiss = await Shell.Current.DisplayActionSheet("Made or Missed?", "Cancel", null, "Made", "Missed");
            if (makeOrMiss != "Cancel" && makeOrMiss != null)
            {
              bool makeBool = false;
              if (makeOrMiss == "Made")
              {
                assistedPlayer = await Shell.Current.DisplayActionSheet("Who assisted the play?", "Cancel", null,"No One",Player1Name.Text, Player2Name.Text, Player3Name.Text, Player4Name.Text, Player5Name.Text);
                makeBool = true;
              }
              string pointWorth = await Shell.Current.DisplayActionSheet("2PT or 3PT?", "Cancel", null, "2PT", "3PT");

              if (pointWorth != "Cancel" && pointWorth != null)
              {
                int pointsWorth = 2;
                if (pointWorth == "3PT")
                {
                  pointsWorth = 3;
                }
                //ADD SHOT TO SHOT MAP
                var viewModel = (GameObjectViewModel)BindingContext;

                // fire the command
                viewModel.registerShot(x, y, playerName, makeBool, pointsWorth, clock, shotClock,assistedPlayer);
                shotClock = new TimeSpan(0, 0, 24);
                timePlay = true;
                blocked = false;
                reDraw = true;

              }
              else
              {
                blocked = false;
              }
            }
            else
            {
              blocked = false;
            }
          }
          else
          {
            blocked = false;
          }



        }
      }
    }

    void TimerStart(object sender, EventArgs args) {
      if (timePlay)
      {
        timePlay = false;
        TimerButtonPlay.IsVisible = true;
        TimerButtonPause.IsVisible = false;
      }
      else
      {
        timePlay = true;
        TimerButtonPlay.IsVisible = false;
        TimerButtonPause.IsVisible = true;
      }
    }

    private void CloseBoxScore(object sender, EventArgs e)
    {
      BoxScoreOpen.IsVisible = true;
      BoxScoreClose.IsVisible = false;
      BoxScoreClose.IsEnabled = false;
      BoxScoreBackground.IsVisible = false;

      BoxScoreStack.IsVisible = false;
      BoxScoreStack.Children.Clear();
    }

    private void ViewBoxScore(object sender, EventArgs e)
    {
      var viewModel = (GameObjectViewModel)BindingContext;

      BoxScoreOpen.IsVisible = false;
      BoxScoreClose.IsVisible = true;
      BoxScoreClose.IsEnabled = true;

      BoxScoreBackground.IsVisible = true;

      StackLayout textStack = new StackLayout
      {
        Padding = new Thickness(5),
        Spacing = 10
      };
      Label titleLabel = new Label();

      titleLabel.Text = "Player Name  |  Minutes  | Points   |   Rebs    |   Assist   |   Steals   |   Blocks   |   FG   |   FT   |  3PTs  |  Turnovers  |   PF   |   +/-  ";
      titleLabel.TextColor = Color.Navy;
      titleLabel.FontAttributes = FontAttributes.Bold;


      textStack.Children.Add(titleLabel);

      List<BoxScore> boxes = viewModel.currentBoxScores;
      for(int i=0;i<boxes.Count; i++)
      {
        if (boxes[i].minutesOnCourt != TimeSpan.Zero)
        {
          string boxScoreText = boxes[i].ToString();
          Label newLabel = new Label();
          newLabel.Text = boxScoreText;
          newLabel.FontAttributes = FontAttributes.Bold;
          newLabel.FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label));
          newLabel.FontFamily = "Eigrantch";
          //newLabel.BackgroundColor = Color.White;
          newLabel.TextColor = Color.Black;
          textStack.Children.Add(newLabel);
        }
      }

      ScrollView scrollView = new ScrollView
      {
        Content = textStack,
        VerticalOptions = LayoutOptions.FillAndExpand,
        Padding = new Thickness(5, 0),
      };

      

      BoxScoreStack.Children.Add(scrollView);

      BoxScoreStack.InputTransparent = false;

      BoxScoreStack.IsVisible = true;

    }
    //statType Cases
    //0 = STL, 1 = TO, 2 = FOUL, 3 = OREB, 4 = DREB,5 = FT
    private void AddSTEAL(object sender, EventArgs e)
    {
      var viewModel = (GameObjectViewModel)BindingContext;
      viewModel.addStat(0, clock, shotClock);
    }
    private void AddTO(object sender, EventArgs e)
    {
      var viewModel = (GameObjectViewModel)BindingContext;
      viewModel.addStat(1, clock, shotClock);
    }
    private void AddFOUL(object sender, EventArgs e)
    {
      var viewModel = (GameObjectViewModel)BindingContext;
      viewModel.addStat(2, clock, shotClock);
    }

    private void AddOREB(object sender, EventArgs e)
    {
      var viewModel = (GameObjectViewModel)BindingContext;
      viewModel.addStat(3, clock, shotClock);
    }

    private void AddDREB(object sender, EventArgs e)
    {
      var viewModel = (GameObjectViewModel)BindingContext;
      viewModel.addStat(4, clock, shotClock);
    }

    private void AddFT(object sender, EventArgs e)
    {
      var viewModel = (GameObjectViewModel)BindingContext;
      viewModel.addStat(5, clock, shotClock);
    }

    private void AddBLCK(object sender, EventArgs e)
    {
      var viewModel = (GameObjectViewModel)BindingContext;
      viewModel.addStat(6, clock, shotClock);
    }

    private void AddASST(object sender, EventArgs e)
    {
      var viewModel = (GameObjectViewModel)BindingContext;
      viewModel.addStat(7, clock, shotClock);
    }

    private void StopGame(object sender, EventArgs e)
    {
      var viewModel = (GameObjectViewModel)BindingContext;
      viewModel.endGame(clock,shotClock);
    }

    public async void updateGameTime(object sender, EventArgs e)
    {
      int maxMinutes = 12;
      string[] minuteArray = getNumberStringArray(0, maxMinutes);
      string minute = await Shell.Current.DisplayActionSheet("Change Time, Minutes", "Cancel", null, minuteArray);
      if (minute != "Cancel" && minute != null)
      {
        string[] secondArray = getNumberStringArray(0, 60);
        string second = await Shell.Current.DisplayActionSheet("Change Time, Seconds", "Cancel", null, secondArray);
        if (second != "Cancel" && second != null)
        {
          string[] milliSecondArray = getNumberStringArray(0, 10);
          string millisecond = await Shell.Current.DisplayActionSheet("Change Time, Milliseconds", "Cancel", null, milliSecondArray);
          if (millisecond != "Cancel" && millisecond != null)
          {
            clock = TimeSpan.FromSeconds(Int16.Parse(second)).Add(TimeSpan.FromMilliseconds(100*Int16.Parse(millisecond)).Add(TimeSpan.FromMinutes(Int16.Parse(minute))));
          }
        }

      }
    }

    public async void updateShotClock(object sender, EventArgs e)
    {
      string option = await Shell.Current.DisplayActionSheet("Change Shot Clock", "Cancel", null, "Reset Clock", "Change Manually");
      if (option != "Cancel" && option != null)
      {
        if (option == "Reset Clock")
        {
          shotClock = new TimeSpan(0, 0, 24);
        }
        else if (option == "Change Manually")
        {
          string[] secondArray = getNumberStringArray(0, 24);
          string second = await Shell.Current.DisplayActionSheet("Change Time, Seconds", "Cancel", null, secondArray);
          if (second != "Cancel" && second != null)
          {
            string[] milliSecondArray = getNumberStringArray(0, 10);
            string millisecond = await Shell.Current.DisplayActionSheet("Change Time, Milliseconds", "Cancel", null, milliSecondArray);
            if (millisecond != "Cancel" && millisecond != null)
            {
              shotClock = TimeSpan.FromSeconds(Int16.Parse(second)).Add(TimeSpan.FromMilliseconds(100 * Int16.Parse(millisecond)));
            }
          }
        }
      }
    }

    private string[] getNumberStringArray(int min, int max)
    {
      string[] numberArray = new string[max+1];
      for (int i = min; i < max + 1; i++)
      {
        numberArray[i] = (i - min).ToString();
      }
      return numberArray;
    }

  }


}