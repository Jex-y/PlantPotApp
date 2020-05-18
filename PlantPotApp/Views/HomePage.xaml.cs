using PlantPotApp.Services;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlantPotApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        BLEConnection server;
        ICharacteristic moistureCharactersitic;
        int moisureValue = 0;
        public HomePage(BLEConnection server)
        {
            InitializeComponent();
            this.server = server;
            keepMoistureUpdated();
        }

        private async Task getCharacteristic()
        {
            IService moistureService = await server.device.GetServiceAsync(Guid.Parse("dbf40f61-dac7-43b1-b99c-55463c7e8742"));
            moistureCharactersitic = await moistureService.GetCharacteristicAsync(Guid.Parse("2dd541bd-2d18-447d-aa51-625787d8b3df"));
        }

        private async Task updateMoisture() 
        {
            byte[] bytes = await moistureCharactersitic.ReadAsync();
            string text = Encoding.ASCII.GetString(bytes);
            float value = float.Parse(text);
            moisureValue = (int)(value * 100);
        }

        private async Task keepMoistureUpdated()
        {
            while (true) {
                await updateMoisture();
                moistureLabel.Text = string.Format("The moisture is {0}",moisureValue);
            }
        }
    }
}