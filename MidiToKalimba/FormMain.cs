using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Devices;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidiToKalimba
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            MidiFile midiFile = null;

            try
            {
                midiFile =  MidiFile.Read(tbFilePath.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Reading the midi file failed: " + exception.Message);
                return;
            }

            long previousNoteTime = 0;
            int previousNoteOctave = 0;

            NoteName previousNoteName = 0;
            bool first = true;

            int baseOctave = (int)nudBaseOctave.Value;
            float speed = (float)nudSpeed.Value;

            int unplayableCounter = 0;
            int tooHighCounter = 0;
            int tooLowCounter = 0;

            String kalimbaString = "";
            int kalimbaMappedNote;

            foreach (var trackChunk in midiFile.GetTrackChunks())
            {
                foreach (var note in trackChunk.ManageNotes().Notes)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        kalimbaMappedNote = getKalimbaNote(previousNoteName);

                        if (kalimbaMappedNote == 0)
                        {
                            unplayableCounter++;
                        }
                        else if (kalimbaMappedNote + ((previousNoteOctave - baseOctave) * 7) < 1)
                        {
                            tooLowCounter++;
                        }
                        else if (kalimbaMappedNote + ((previousNoteOctave - baseOctave) * 7) > 17)
                        {
                            tooHighCounter++;
                        }
                        else
                        {
                            kalimbaString += kalimbaMappedNote + ((previousNoteOctave - baseOctave) * 7) + "," + ((note.Time - previousNoteTime) * speed)  + ";";
                        }
                    }

                    previousNoteTime = note.Time;
                    previousNoteName = note.NoteName;
                    previousNoteOctave = note.Octave;
                }
            }

            kalimbaMappedNote = getKalimbaNote(previousNoteName);

            if (kalimbaMappedNote == 0)
            {
                unplayableCounter++;
            }
            else if (kalimbaMappedNote + ((previousNoteOctave - baseOctave) * 7) < 1)
            {
                tooLowCounter++;
            }
            else if (kalimbaMappedNote + ((previousNoteOctave - baseOctave) * 7) > 17)
            {
                tooHighCounter++;
            }
            else
            {
                kalimbaString += kalimbaMappedNote + ((previousNoteOctave - baseOctave) * 7) + "," + 0 + ";";
            }

            tbConvertedMidi.Text = kalimbaString;
            lblUnplayableCounter.Text = "Unplayable Notes: " + unplayableCounter;
            lblTooLowNotes.Text = "Too Low Notes: " + tooLowCounter;
            lblTooHighNotes.Text = "Too High Notes: " + tooHighCounter;
        }

        private int getKalimbaNote(NoteName noteName)
        {
            switch (noteName)
            {
                case NoteName.C: return 1;
                case NoteName.D: return 2;
                case NoteName.E: return 3;
                case NoteName.F: return 4;
                case NoteName.G: return 5;
                case NoteName.A: return 6;
                case NoteName.B: return 7;

                default: return 0;
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbFilePath.Text = openFileDialog1.FileName;
            }
        }
    }
}
