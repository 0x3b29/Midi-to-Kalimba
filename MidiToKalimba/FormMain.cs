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

            int goodNotesCounter = 0;
            int unplayableCounter = 0;
            int tooHighCounter = 0;
            int tooLowCounter = 0;

            String kalimbaString = "";
            String notesArrayString = "";
            String offsetArrayString = "";

            int kalimbaMappedNote;

            bool useArrayNotation = cbArrayNotation.Checked;

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
                            goodNotesCounter++;

                            int kalimbaNote = (kalimbaMappedNote + ((previousNoteOctave - baseOctave) * 7));
                            int offset = Convert.ToInt32(Math.Round((note.Time - previousNoteTime) * speed));

                            if (useArrayNotation)
                            {
                                if (notesArrayString == "")
                                {
                                    notesArrayString = kalimbaNote.ToString();
                                    offsetArrayString = offset.ToString();
                                }
                                else
                                {
                                    notesArrayString += ", " + kalimbaNote.ToString();
                                    offsetArrayString += ", " + offset.ToString();
                                }
                            }
                            else
                            {
                                kalimbaString += kalimbaNote + "," + offset + ";";
                            }
                        }
                    }

                    previousNoteTime = note.Time;
                    previousNoteName = note.NoteName;
                    previousNoteOctave = note.Octave;
                }
            }

            kalimbaMappedNote = getKalimbaNote(previousNoteName);
            int lastKalimbaNote = (kalimbaMappedNote + ((previousNoteOctave - baseOctave) * 7));

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
                goodNotesCounter++;

                if (useArrayNotation)
                {
                    if (notesArrayString == "")
                    {
                        notesArrayString = lastKalimbaNote.ToString();
                        offsetArrayString = "0";
                    }
                    else
                    {
                        notesArrayString += ", " + lastKalimbaNote.ToString();
                        offsetArrayString += ", " + 0;
                    }
                }
                else
                {
                    kalimbaString += lastKalimbaNote + "," + 0 + ";";
                }
            }

            if (useArrayNotation)
            {
                tbConvertedMidi.Text = "unsigned char notes[" + goodNotesCounter + "] = {" + notesArrayString + "};" + Environment.NewLine +
                    "unsigned short int offsets[" + goodNotesCounter + "] = {" + offsetArrayString + "};";
            }
            else
            {
                tbConvertedMidi.Text = kalimbaString;
            }

            lblGoodNotes.Text = "Good Notes: " + goodNotesCounter;
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
