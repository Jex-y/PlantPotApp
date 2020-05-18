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
using System.Diagnostics;
using PlantPotApp.Services;
using Plugin.BLE.Abstractions.Contracts;

namespace PlantPotApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColourPage: ContentPage
    {
        private readonly Color defaultColour = Color.FromHex("#FF0080");
        private Color currentColour;
        private double brightness = 1;
        private Boolean on = true;
        BLEConnection server;
        IService lightsOnService;
        ICharacteristic lightsOnCharacteristic;
        IService colourService;
        ICharacteristic redCharacteristic;
        ICharacteristic greenCharacteristic;
        ICharacteristic blueCharacteristic;

        public ColourPage(BLEConnection server)
        {
            InitializeComponent();
            this.server = server;
            GetServices();
            currentColour = defaultColour;
            brightnessSlider.Value = 100;
            brightnessSliderLabel.Text = "Brightness: 100%";
            onSwitch.IsToggled = true;

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
            UpdateLightsColour();
        }

        private void ColourWheelValueChanged(String colour) 
        {
            currentColour = Color.FromHex(colour);
            colourExample.InvalidateSurface();
            UpdateLightsColour();
        }


        private void ResetClicked(object sender, EventArgs args)
        {
            currentColour = defaultColour;
            brightness = 1;
            brightnessSlider.Value = 100;
            brightnessSliderLabel.Text = "Brightness: 100%";
            colourExample.InvalidateSurface();
            onSwitch.IsToggled = true;
            UpdateLightsOn(true);
            UpdateLightsColour();
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
            UpdateLightsOn(on);
        }


        private async Task UpdateLightsColour()
        {
            byte[] buffer = { (byte)(currentColour.R * 255 * brightness) };
            await redCharacteristic.WriteAsync(buffer);
            buffer[0] = (byte)(currentColour.G * 255 * brightness);
            await redCharacteristic.WriteAsync(buffer);
            buffer[0] = (byte)(currentColour.B * 255 * brightness);
            await redCharacteristic.WriteAsync(buffer);
        }

        private async Task UpdateLightsOn(bool on) 
        {
            String value;
            if (on)
            {
                value = "true";
            }
            else 
            {
                value = "false";
            }
            await lightsOnCharacteristic.WriteAsync(Encoding.ASCII.GetBytes(value));
        }

        public async Task GetServices() 
        {
            lightsOnService =           await server.device.GetServiceAsync(Guid.Parse("b11c29d7-941e-4fb4-9e7f-c66b61342727"));
            lightsOnCharacteristic =    await lightsOnService.GetCharacteristicAsync(Guid.Parse("fdda478f-7123-44fb-b74e-1198ca501108"));

            colourService =             await server.device.GetServiceAsync(Guid.Parse("0be15d7f-17a0-4864-b797-4b80ab199eb6"));
            redCharacteristic =         await lightsOnService.GetCharacteristicAsync(Guid.Parse("02f3a14a-ff6f-4188-a086-34c0c4817944"));
            greenCharacteristic =       await lightsOnService.GetCharacteristicAsync(Guid.Parse("95a49efa-4ad8-43da-af58-b81b5d5f635e"));
            blueCharacteristic =        await lightsOnService.GetCharacteristicAsync(Guid.Parse("31e2031c-6256-4335-8f1a-012c4c1abd5c"));
        }
    }
}