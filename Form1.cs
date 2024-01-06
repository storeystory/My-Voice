using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace MyButtons
{

    public partial class frmMain : Form
    {
        //put your keys in environment variables with these names
        public static string speechKey = Environment.GetEnvironmentVariable("SPEECH_KEY");
        public static string speechRegion = Environment.GetEnvironmentVariable("SPEECH_REGION");

        //this code was in the quickstart i don't completely understand it
        static void OutputSpeechSynthesisResult(SpeechSynthesisResult speechSynthesisResult, string text)
        {
            switch (speechSynthesisResult.Reason)
            {
                case ResultReason.SynthesizingAudioCompleted:
                    Console.WriteLine($"Speech synthesized for text: [{text}]");
                    break;
                case ResultReason.Canceled:
                    var cancellation = SpeechSynthesisCancellationDetails.FromResult(speechSynthesisResult);
                    Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                    if (cancellation.Reason == CancellationReason.Error)
                    {
                        Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                        Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                        Console.WriteLine($"CANCELED: Did you set the speech resource key and region values?");
                    }
                    break;
                default:
                    break;
            }
        }

        public frmMain()
        {
            InitializeComponent();
        }

        //hooray for visual studio events, every button just calls this
        private async void btnAny_Click(object sender, EventArgs e)
        {
            if (chkSayNow.Checked)
            {
                var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
                string text = (sender as Button).Text;

                // The language of the voice that speaks.
                speechConfig.SpeechSynthesisVoiceName = "en-US-NancyNeural";
                using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
                {
                    var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
                    OutputSpeechSynthesisResult(speechSynthesisResult, text);
                }
            }
            else
            {
                boxWords.Text += (sender as Button).Text + " ";
            }
        }

        //says everything in boxWords
        private async void btnSpeak_Click(object sender, EventArgs e)
        {
            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            string text = boxWords.Text;

            // The language of the voice that speaks.
            speechConfig.SpeechSynthesisVoiceName = "en-US-NancyNeural";
            using (var speechSynthesizer = new SpeechSynthesizer(speechConfig))
            {
                var speechSynthesisResult = await speechSynthesizer.SpeakTextAsync(text);
                OutputSpeechSynthesisResult(speechSynthesisResult, text);
            }
        }

        //plays caws
        private void btnCaw_Click(object sender, EventArgs e)
        {
            SoundPlayer snd = new SoundPlayer(Properties.Resources.crow_39377);
            snd.Play();
        }

        //for numbers   
        private void btnAddNum_Click(object sender, EventArgs e)
        {
            boxWords.Text = boxWords.Text + nudNums.Value + " ";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            boxWords.Text = string.Empty;
        }
    }
}
