﻿using Melanchall.DryWetMidi.Core;
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
        bool wrapNotes;

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
            
            useArrayNotation = cbArrayNotation.Checked;
            wrapNotes = cbWrapNotes.Checked;

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
            int offset = Convert.ToInt32(Math.Round((offsetToNextNote) * speed));

            if (kalimbaMappedNote == 0)
            {
                unplayableCounter++;

                // We can never make unplayable notes work because of how a Kalimba is arranged
                // We still need to add a placeholder note to the output because otherwise the timing will get mesed up

                addNoteToOutput(0, offset);
                return;
            }

            if (kalimbaMappedNote + ((noteOctave - baseOctave) * 7) < 1)
            {
                tooLowCounter++;

                if (wrapNotes)
                {
                    Console.WriteLine("kalimbaMappedNote(" + kalimbaMappedNote + ") + ((noteOctave(" + noteOctave + ") - baseOctave(" + baseOctave + ")) * 7)(" + (kalimbaMappedNote + ((noteOctave - baseOctave) * 7)) + ") < 1");

                    // Adjust the octave of the note upwards until the note is in the playable range
                    while (kalimbaMappedNote + ((noteOctave - baseOctave) * 7) < 1)
                    {

                        noteOctave++;

                        Console.WriteLine("kalimbaMappedNote(" + kalimbaMappedNote + ") + ((noteOctave(" + noteOctave + ") - baseOctave(" + baseOctave + ")) * 7)(" + (kalimbaMappedNote + ((noteOctave - baseOctave) * 7)) + ") < 1");
                    }
                }
                else
                {
                    // If we dont wrap the notes around, we are done with this note
                    // We still need to add a placeholder note to the output because otherwise the timing will get mesed up
                    addNoteToOutput(0, offset);
                    return;
                }
            }
            else if (kalimbaMappedNote + ((noteOctave - baseOctave) * 7) > 17)
            {
                tooHighCounter++;

                if (wrapNotes)
                {
                    // Adjust the octave of the note downwards until the note is in the playable range
                    while (kalimbaMappedNote + ((noteOctave - baseOctave) * 7) > 17)
                    {
                        noteOctave--;
                    }
                }
                else
                {
                    // If we dont wrap the notes around, we are done with this note
                    // We still need to add a placeholder note to the output because otherwise the timing will get mesed up
                    addNoteToOutput(0, offset);
                    return;
                }
            }
            else
            {
                goodNotesCounter++;
            }

            // If we got here, we either had a good note, or we wrapped a note that was too low or too high

            int kalimbaNote = (kalimbaMappedNote + ((noteOctave - baseOctave) * 7));
            addNoteToOutput(kalimbaNote, offset);
        }

        public void addNoteToOutput(int note, int offset)
        {
            if (useArrayNotation)
            {
                // If the array notation is used, we arrange the note and the offset in a way that they can be used create valid c/c++ arrays
                if (notesArrayString == "")
                {
                    notesArrayString = note.ToString();
                    offsetArrayString = offset.ToString();
                }
                else
                {
                    notesArrayString += ", " + note.ToString();
                    offsetArrayString += ", " + offset.ToString();
                }
            }
            else
            {
                // If we dont use the array notation, the note and the offset is comma seperated in a way that we can send it via the serial interface to the arduino
                kalimbaString += note + "," + offset + ";";
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
