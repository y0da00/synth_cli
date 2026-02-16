using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synth_test
{
    internal class SquareGenerator : ISampleSource
    {
        private double phase;
        private double phaseIncrement;
        private double frequency;
        private double amplitude = 1.0;
        private double dutyCycle = 0.5;

        public double Frequency
        {
            get => frequency;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Frequency must be non-negative.");
                }
                frequency = value;
                UpdatePhaseIncrement();
            }
        }
        public double Amplitude
        {
            get => amplitude;
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Amplitude should be between 0 and 1.");
                }
                amplitude = value;
            }
        }
        public double DutyCycle
        {
            get => dutyCycle;
            set
            {
                if (value <= 0 || value >= 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Duty cycle must be strictly between 0 and 1.");
                }
                dutyCycle = value;
            }
        }

        public SquareGenerator(double frequency, double amplitude, double dutyCycle)
        {
            this.frequency = frequency;
            this.amplitude = amplitude;
            this.dutyCycle = dutyCycle;
            //Console.WriteLine($"Sample rate {WaveFormat.SampleRate}");

            WaveFormat = new WaveFormat(44100, 32, 1, AudioEncoding.IeeeFloat);

            UpdatePhaseIncrement();
        }

        private void UpdatePhaseIncrement()
        {
            //const int sampleRate = 44100;
            phaseIncrement = frequency / (double)WaveFormat.SampleRate;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                double pos = Phase;

                float sample = (pos < dutyCycle) ? 1f : -1f;

                buffer[offset + i] = (float)(sample * amplitude);

                phase += phaseIncrement;
            }

            return count;
        }

        public WaveFormat WaveFormat { get; } = new WaveFormat(44100, 32, 1, AudioEncoding.IeeeFloat);

        public long Length => long.MaxValue;

        public long Position
        {
            get => 0;
            set => throw new NotSupportedException("Seeking not supported in infinite generators.");
        }

        public void Dispose()
        {
        }

        public double Phase
        {
            get => phase % 1.0;
            set => phase = value % 1.0;
        }

        public bool CanSeek => false;
    }
}
