using System;
using System.Collections.Generic;
using NAudio.Wave;
using prosy;

namespace grabwave
{
    public class PronyWaveProvider32: WaveProvider32
    {
        int sample;
        /// <summary>
        /// Sum(Ak*exp(LAMBDAk*dt*n), 0..P-1) ; P - порядок модели
        /// </summary>
        /// <param name="SampleRate">Число отсчетов в секунду</param>
        /// <param name="Channels">Число каналов</param>
        /// <param name="ampl">Комплексные амплитуды модели</param>
        /// <param name="pol">полюса модели</param>
        public PronyWaveProvider32(int SampleRate, int Channels, Complex[] ampl, Complex[] pol)
        {
            SetWaveFormat(SampleRate, Channels);
        }

        public float Frequency { get; set; }
        public float Amplitude { get; set; }

        public override int Read(float[] buffer, int offset, int sampleCount)
        {
            int sampleRate = WaveFormat.SampleRate;
            for (int n = 0; n < sampleCount; n++)
            {
                buffer[n + offset] = (float)(Amplitude * Math.Sin((2 * Math.PI * sample * Frequency) / sampleRate));

                if (sample++ >= sampleRate)
                    sample = 0;
            }
            return sampleCount;
        }
    }
}
