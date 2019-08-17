using SharpAdbClient;
using System;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Collections.Generic;

namespace ADB
{
    public class Device
    {
        DeviceData device;
        private Point screen;

        public Point Screen
        {
            get
            {
                if (screen == null || screen.X == 0 || screen.Y == 0)
                {
                    var image = Screenshot();
                    screen = new Point(image.Width, image.Height);
                }
                return screen;
            }
        }

        public static Device Create(string identifier)
        {
            var device = AdbClient.Instance.GetDevices()
                .Where(d => d.Serial == identifier || d.Name == identifier)
                .FirstOrDefault();

            return new Device(device);
        }

        public static List<string> GetDevices()
        {
            return AdbClient.Instance.GetDevices().Select(d => d.Serial)
                .Concat(AdbClient.Instance.GetDevices().Select(d => d.Name))
                .Distinct()
                .Where(d => !string.IsNullOrEmpty(d))
                .ToList();
        }

        private Device(DeviceData device)
        {
            this.device = device;
        }

        public void Tap(int x, int y)
        {
            var receiver = new ConsoleOutputReceiver();
            AdbClient.Instance.ExecuteRemoteCommand($"input tap {x} {y}", device, receiver);
        }

        public void Tap(TapPosition position)
        {
            switch (position)
            {
                case TapPosition.Center:
                    Tap(Percentage(Screen.X,50), Percentage(Screen.Y, 45));
                    break;
                case TapPosition.Character:
                    Tap(20, 20);
                    break;
            }
        }

        public int Percentage(int value, int percentage)
        {
            return (value * percentage) / 100;
        }

        public Image Screenshot()
        {
            return AdbClient.Instance.GetFrameBufferAsync(device, CancellationToken.None).Result;
        }

        public enum TapPosition
        {
            Center,
            Character
        }
    }
}
