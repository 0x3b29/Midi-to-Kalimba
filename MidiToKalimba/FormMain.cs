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
        bool useArrayNotation;

        int goodNotesCounter;
        int unplayableCounter;
        int tooHighCounter;
        int tooLowCounter;

        String kalimbaString;
        String notesArrayString;
        String offsetArrayString;

        int baseOctave;
        float speed;

        public FormMain()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            MidiFile midiFile = null;

            try
            {
                midiFile = MidiFile.Read(tbFilePath.Text);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Reading the midi file failed: " + exception.Message);
                return;
            }

            baseOctave = (int)nudBaseOctave.Value;
            speed = (float)nudSpeed.Value;

            goodNotesCounter = 0;
            unplayableCounter = 0;
            tooHighCounter = 0;
            tooLowCounter = 0;

            kalimbaString = "";
            notesArrayString = "";
            offsetArrayString = "";
            
            bool useArrayNotation = cbArrayNotation.Checked;

            foreach (var trackChunk in midiFile.GetTrackChunks())
            {
                Melanchall.DryWetMidi.Interaction.Note[] notesArray = trackChunk.ManageNotes().Notes.ToArray();

                for (int i = 0; i < notesArray.Length; i++)
                {
                    if (i < notesArray.Length -1)
                    {
                        processNote(notesArray[i].NoteName, notesArray[i].Octave, notesArray[i + 1].Time - notesArray[i].Time);
                    }
                    else
                    {
                        processNote(notesArray[i].NoteName, notesArray[i].Octave, 0);
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
        }

        private void processNote(NoteName noteName, int noteOctave, long offsetToNextNote)
        {
            int kalimbaMappedNote = getKalimbaNote(noteName);

            if (kalimbaMappedNote == 0)
            {
                unplayableCounter++;
            }
            else if (kalimbaMappedNote + ((noteOctave - baseOctave) * 7) < 1)
            {
                tooLowCounter++;
            }
            else if (kalimbaMappedNote + ((noteOctave - baseOctave) * 7) > 17)
            {
                tooHighCounter++;
            }
            else
            {
                goodNotesCounter++;

                int kalimbaNote = (kalimbaMappedNote + ((noteOctave - baseOctave) * 7));
                int offset = Convert.ToInt32(Math.Round((offsetToNextNote) * speed));

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
