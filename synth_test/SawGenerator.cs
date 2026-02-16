using CSCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace synth_test
{
    internal class SawGenerator : ISampleSource
    {
        private double _phase;
        private double _phaseIncrement;

        private double _frequency;
        private double _amplitude = 1.0;

        public SawGenerator(double frequency, double amplitude = 1.0)
        {
            Frequency = frequency;
            Amplitude = amplitude;
        }

        public double Frequency
        {
            get => _frequency;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Frequency must be non-negative.");
                _frequency = value;
                UpdatePhaseIncrement();
            }
        }

        public double Amplitude
        {
            get => _amplitude;
            set
            {
                if (value < 0 || value > 1) throw new ArgumentOutOfRangeException(nameof(value), "Amplitude should be between 0 and 1.");
                _amplitude = value;
            }
        }

        private void UpdatePhaseIncrement()
        {
            _phaseIncrement = _frequency / (double)WaveFormat.SampleRate;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            for (int i = 0; i < count; i++)
            {
                double pos = _phase % 1.0;

                float sample = (float)(2.0 * pos - 1.0);

                buffer[offset + i] = sample * (float)_amplitude;

                _phase += _phaseIncrement;
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
            get => _phase % 1.0;
            set => _phase = value % 1.0;
        }

        public bool CanSeek => false;
    }
}
