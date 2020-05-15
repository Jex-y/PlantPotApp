using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Xml.Serialization;

namespace PlantPotApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColourPage: ContentPage
    {
        private readonly Color defaultColour = Color.FromHex("#FF0080");
        private Color currentColour;
        private double brightness = 1;
        private Boolean on = true;
        public ColourPage()
        {
            InitializeComponent();
            currentColour = defaultColour;
            brightnessSlider.Value = 100;
            brightnessSliderLabel.Text = "Brightness: 100%";
            onSwitch.IsToggled = true;

            // Colour changed command

            // Released command 

            
            xfColourWheel.ReleasedCommand = new Command<string>((colour) => {ColourWheelReleased(colour);});
            xfColourWheel.ValueChangedCommand = new Command<string>((colour) => { ColourWheelValueChanged(colour); });

        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args) 
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = currentColour.ToSKColor(),
                StrokeWidth = 25
            };

            canvas.DrawCircle(info.Width / 2, info.Height / 2, 320, paint);

        }

        private void ColourWheelReleased(String colour) 
        { 
            currentColour = Color.FromHex(colour);
            colourExample.InvalidateSurface();
        }

        private void ColourWheelValueChanged(String colour) 
        {
            currentColour = Color.FromHex(colour);
            colourExample.InvalidateSurface();
        }


        private void ResetClicked(object sender, EventArgs args)
        {
            currentColour = defaultColour;
            brightness = 1;
            brightnessSlider.Value = 100;
            brightnessSliderLabel.Text = "Brightness: 100%";
            colourExample.InvalidateSurface();
            UpdateLights();
        }

        private void BrightnessChanged(object sender, EventArgs args)
        {
            double value = brightnessSlider.Value;
            brightness = value/100;
            brightnessSliderLabel.Text = String.Format("Brightness: {0}%", value);
        }

        private void OnSwitchToggeled(object sender, ToggledEventArgs args) 
        {
            on = args.Value;
        }


        private void UpdateLights()
        {
            double red, blue, green;
            red = currentColour.R;
            blue = currentColour.B;
            green = currentColour.G;
        }
    }
}