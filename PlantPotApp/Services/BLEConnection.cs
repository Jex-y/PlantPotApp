using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PlantPotApp.Services
{
    public class BLEConnection
    {
        private IBluetoothLE ble;
        private IAdapter adapter;
        private BluetoothState state;
        public IDevice device { get; private set; }
        public bool connected { get; private set; }

        public BLEConnection() {
            ble = CrossBluetoothLE.Current;
            adapter = CrossBluetoothLE.Current.Adapter;
            state = ble.State;
            connected = false;
        }

        public bool bluetoothIsOn() {
            return ble.State == BluetoothState.On;
        }

        public async Task connect() {
            List<IDevice> devices = new List<IDevice>();
            adapter.DeviceDiscovered += (s, a) => devices.Add(a.Device);
            await adapter.StartScanningForDevicesAsync();
            int index = 0;
            for (int i = 0; i < devices.Count; i++) {
                if (devices[i].Name == "SmartPotv2") {
                    index = 0;
                    Debug.Write("Device GUID is: ");
                    Debug.WriteLine(devices[i].Id);
                }
            }
            try
            {
                device = devices[index];
                await adapter.ConnectToDeviceAsync(devices[index]);
                connected = true;
            }
            catch (Plugin.BLE.Abstractions.Exceptions.DeviceConnectionException e)
            {
                // ... could not connect to device
                Debug.WriteLine("Could not connect to device");
            }
            return;
        }



    }
}
