using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSCore.SoundOut;
using CSCore;
using CSCore.Streams;
using System.Threading;
using System.Windows.Input;
namespace synth_test
{
    internal class Program
    {
        const double default_freq = 440;
        static double freq = 10;
        static int note = 0;
        static int octave = 0;

        static SawGenerator wave = new SawGenerator(freq, 1.0);

        //static void key_is_pressed()
        //{
        //    if(Console.KeyAvailable)
        //        synth_out.Play();
        //    else synth_out.Pause();
        //}
        static ConsoleKey Input_listener()
        {
            while (true)
            {
                Input_handler(Console.ReadKey().Key);
                frequency_calc();

            }

        }
        static void Input_handler(ConsoleKey key)
        {
            double freq = 440;
            Dictionary<ConsoleKey, int> notes = new Dictionary<ConsoleKey, int>()
            { {ConsoleKey.Z,-9}, {ConsoleKey.S,-8},{ConsoleKey.X,-7},{ConsoleKey.D,-6},{ConsoleKey.C,-5},{ConsoleKey.V,-4},{ConsoleKey.G,-3},{ConsoleKey.B,-2},{ConsoleKey.H,-1},{ConsoleKey.N,0},{ConsoleKey.J, 1},{ConsoleKey.M, 2}
            };
            Dictionary<ConsoleKey, int> octaves = new Dictionary<ConsoleKey, int>()
            { {ConsoleKey.NumPad0,-4}, {ConsoleKey.NumPad1,-3},  {ConsoleKey.NumPad2,-2},  {ConsoleKey.NumPad3,-1}, {ConsoleKey.NumPad4,0}, {ConsoleKey.NumPad5,1}, {ConsoleKey.NumPad6,2}, {ConsoleKey.NumPad7,3}, {ConsoleKey.NumPad8,4}, {ConsoleKey.NumPad9,5}
            };
            if (notes.ContainsKey(key))
            {
                note = notes[key];

            }
            else if (octaves.ContainsKey(key))
            {
                octave = octaves[key];
            }



        }
        static void frequency_calc()
        {
            freq = default_freq * Math.Pow(2, octave) * Math.Pow(Math.Pow(2.0, 1 / 12.0), note);

            wave.Frequency = freq;
        }

        static void Main(string[] args)
        {


            Task.Run(Input_listener);
            //Task.Run(key_is_pressed);
            SineGenerator wave2 = new SineGenerator(220, 1.0, 0);
            IWaveSource s = FluentExtensions.ToWaveSource(wave);
            IWaveSource s1 = FluentExtensions.ToWaveSource(wave2);
            WasapiOut synth_out = new WasapiOut();
            WasapiOut synth_out2 = new WasapiOut();
            synth_out2.Initialize(s1);
            synth_out.Initialize(s);
            synth_out.Play();
            //synth_out2.Play();
        }
    }
}
