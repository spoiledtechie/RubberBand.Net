using RubberBandNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubberBand.Net.Audio
{
    public class AudioFile
    {
        public string FilePath { get; set; }

        public AudioFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new Exception("Could not find the location of the video file");
            }

            if (!File.Exists(filePath))
            {
                throw new Exception(String.Format("The video file {0} does not exist.", FilePath));
            }

            FilePath = filePath;
        }

        public string ChangePitch(int pitch, AudioFormat type)
        {
            string tempFile = Path.ChangeExtension(Path.GetTempFileName(), type.ToString());

            RubberBandParameters parameters = new RubberBandParameters()
            {
                InputFilePath = FilePath,
                OutputFilePath = tempFile,
                Pitch = pitch.ToString()
            };

            string output = RubberBandService.Execute(parameters);

            if (!File.Exists(tempFile))
            {
                throw new Exception("Could not change pitch in audio");
            }

            return tempFile;
        }

    }
}
