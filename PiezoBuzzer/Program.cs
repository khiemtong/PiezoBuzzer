using System;
using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
//using PWM = SecretLabs.NETMF.Hardware.PWM;

namespace PiezoBuzzer
{
    public class Program
    {
        private static InterruptPort _onboardSwitch;
        private static bool _ledOn;
        private static OutputPort _led;
        private static PWM _buzzer;

        public static void Main()
        {
            Thread.Sleep(1000);

            _ledOn = false;

            _led = new OutputPort(Pins.ONBOARD_LED, _ledOn);
            _buzzer = new PWM(PWMChannels.PWM_PIN_D5, 4000, 0.5, false);
            
            _onboardSwitch = new InterruptPort(Pins.ONBOARD_SW1, false, Port.ResistorMode.Disabled,
                                           Port.InterruptMode.InterruptEdgeLow);

            _onboardSwitch.OnInterrupt += InterruptHandler;

            while (true)
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }


        public static void InterruptHandler(UInt32 data1, UInt32 data2, DateTime time)
        {
            // you don't want another interrupt while you're handling the current one
            _onboardSwitch.DisableInterrupt();

            _ledOn = !_ledOn;
            _led.Write(_ledOn);

            if (_ledOn)
            {
                
                _buzzer.Start();

            } else
            {
                _buzzer.Stop();
            }

            _onboardSwitch.EnableInterrupt();
        }

    }
}
