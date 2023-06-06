using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent (typeof (AudioSource))]
public class AudioPeer : MonoBehaviour
{

    AudioSource _audioSource;
    public static float[] _samples = new float[512];
    public static float[] _freqBand = new float[8];
    public static float[] _bandBuffer = new float[8];
    float[] _bufferDecrease = new float[8];


    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
    }

    // GetSpectrumData

    void GetSpectrumAudioSource() {
        _audioSource.GetSpectrumData(_samples,0,FFTWindow.Blackman);
    }

    void BandBuffer() {
        for(int i = 0; i < 8; i++) {
            if (_freqBand[i] > _bandBuffer[i]) {
                _bandBuffer[i] = _freqBand[i];
                _bufferDecrease[i] = 0.005f;
            }
            if(_freqBand[i] < _bandBuffer[i]) {
                _bandBuffer[i] -= _bufferDecrease[i];
                _bufferDecrease[i] *= 1.02f;
            }
        }
    }

    void MakeFrequencyBands() {
        /* 
        44100hz track over 2 channels
        ~22050hz loading in

        22050/512 = ~43hz per sample
        Frequency Bands
        20-60
        60-250
        250-500
        500-2000
        2000-4000
        4000-6000
        6000-20000

        we set up our frequency bands in powers of 2 based on our samples to get close to the table above
        band 0 = 2 samples (0-86hz)
        band 1 = 4 samples (87-258hz)
        band 2 = 8 samples (259-602hz)
        band 3 = 16 samples (603-1290hz)
        band 4 = 32 samples (1291-2666hz)
        band 5 = 64 samples (2667-5418hz)
        band 6 = 128 samples (5419-10922hz)
        band 7 = 256 samples + 2 remaining = 258 samples (10923-22050hz)
        */
        //we create a nested for loop
        //first for loop to put numbers into our frequency bands
        //second for loop to get the samples for our computations

        int count = 0; //counts our current position on the samples
        for(int i = 0; i<8; i++){
            int sampleCount = (int) Mathf.Pow(2, i + 1); // based on our band calculations it is 2^(band number + 1)
            float average = 0; //average of data which gets stored in our frequency bands
            if(i==7){
                sampleCount += 2;
            }
            for(int j = 0; j<sampleCount; j++){
                average += _samples [count]  * (count + 1);
                count++;
            }

            average /= count;

            _freqBand[i] = average * 10;
        }
    }
}
