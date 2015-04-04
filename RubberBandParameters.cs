using System;
using System.Text;
using System.Drawing;

namespace RubberBandNet
{
    public class RubberBandParameters
    {
        public string OutputFilePath;
        public string InputFilePath;
        public string Options;
        public string InputOptions;
        public string OutputOptions;
        public string VideoCodec;
        public string AudioCodec;
        public string Format;
        public bool DisableAudio;

        public string ComplexVideoFilterInputs;
        public string ComplexVideoFilterCommands;
        public string Preset;

        private StringBuilder m_assembledOptions;

        public RubberBandParameters()
        {
            m_assembledOptions = new StringBuilder();
        }

        protected void AddOption(string option)
        {
            if ((m_assembledOptions.Length > 0) && (m_assembledOptions.ToString().EndsWith(" ") == false))
            {
                m_assembledOptions.Append(" ");
            }

            m_assembledOptions.Append("-");
            m_assembledOptions.Append(option);
        }

        protected void AddParameter(string parameter)
        {
            m_assembledOptions.Append(parameter);
        }

        protected void AddOption(string option, string parameter)
        {
            AddOption(option);
            m_assembledOptions.Append(" ");
            AddParameter(parameter);
        }
        protected void AddComplexOption(string option, string inputs, string commands)
        {
            m_assembledOptions.Append(" ");
            AddParameter(inputs);
            AddOption(option);
            m_assembledOptions.Append(" \"");
            AddParameter(commands);
            m_assembledOptions.Append("\"");
        }

        protected void AddOption(string option, string parameter1, string separator, string parameter2)
        {
            AddOption(option);
            m_assembledOptions.Append(" ");
            AddParameter(parameter1);
            m_assembledOptions.Append(separator);
            AddParameter(parameter2);
        }

        protected void AddSeparator(string separator)
        {
            m_assembledOptions.Append(separator);
        }

        protected void AddRawOptions(string rawOptions)
        {
            m_assembledOptions.Append(rawOptions);
        }

        protected void AssembleGeneralOptions()
        {


            if (!String.IsNullOrWhiteSpace(Options))
            {
                AddSeparator(" ");
                AddRawOptions(Options);
            }

        }

        protected void AssembleInputOptions()
        {
            if (!String.IsNullOrWhiteSpace(InputOptions))
            {
                AddSeparator(" ");
                AddRawOptions(OutputOptions);
            }

        }

        protected void AssembleComplexOutputOptions()
        {
            if (!String.IsNullOrWhiteSpace(ComplexVideoFilterInputs))
            {
                AddComplexOption("filter_complex", ComplexVideoFilterInputs, ComplexVideoFilterCommands);
            }

        }

        protected void AssembleOutputOptions()
        {
            if (!String.IsNullOrWhiteSpace(VideoCodec))
            {
                AddOption("vcodec", VideoCodec);
            }

            if (!String.IsNullOrWhiteSpace(AudioCodec))
            {
                AddOption("acodec", AudioCodec);
            }

            if (!String.IsNullOrWhiteSpace(Format))
            {
                AddOption("f", Format);
            }


            if (!String.IsNullOrWhiteSpace(Preset))
            {
                AddOption("preset", Preset);
            }


            if (DisableAudio)
            {
                AddOption("an");
            }

            if (!String.IsNullOrWhiteSpace(OutputOptions))
            {
                AddSeparator(" ");
                AddRawOptions(OutputOptions);
            }

        }

        public override string ToString()
        {
            m_assembledOptions.Clear();
            AssembleGeneralOptions();
            AssembleInputOptions();
            if (!String.IsNullOrWhiteSpace(InputFilePath))
            {
                AddOption("i", String.Format("\"{0}\"", InputFilePath));
            }

            AssembleOutputOptions();

            AssembleComplexOutputOptions();

            if (String.IsNullOrWhiteSpace(OutputFilePath))
            {
                AddSeparator(" ");
                AddParameter("NUL");
            }
            else
            {
                AddSeparator(" ");
                AddParameter(String.Format("\"{0}\"", OutputFilePath));
            }

            return m_assembledOptions.ToString();
        }
    }

}
