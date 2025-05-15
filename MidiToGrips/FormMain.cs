using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MidiToGrips
{
    public partial class FormMain : Form
    {
        const bool addChunkEndNote = true;

        bool useArrayNotation;
        bool wrapNotes;

        int goodNotesCounter;
        int tooHighCounter;
        int tooLowCounter;

        String gripsString;
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
            var readingSettings = new ReadingSettings
            {
                EndOfTrackStoringPolicy = EndOfTrackStoringPolicy.Store
            };

            try
            {
                midiFile = MidiFile.Read(tbFilePath.Text, readingSettings); //readingSettings
            }
            catch (Exception exception)
            {
                MessageBox.Show("Reading the midi file failed: " + exception.Message);
                return;
            }

            baseOctave = (int)nudBaseOctave.Value;
            speed = (float)nudSpeed.Value;
            var tempoMap = midiFile.GetTempoMap();

            goodNotesCounter = 0;
            tooHighCounter = 0;
            tooLowCounter = 0;

            gripsString = "";
            notesArrayString = "";
            offsetArrayString = "";

            useArrayNotation = cbArrayNotation.Checked;
            wrapNotes = cbWrapNotes.Checked;

            

            foreach (var trackChunk in midiFile.GetTrackChunks())
            {
                Melanchall.DryWetMidi.Interaction.Note[] notesArray = trackChunk.ManageNotes().Objects.ToArray();

                if (notesArray.Length == 0)
                {
                    continue;
                }

                long previousTime = 0;

                for (int i = 0; i < notesArray.Length; i++)
                {
                    MetricTimeSpan metricTime = TimeConverter.ConvertTo<MetricTimeSpan>(
                                          notesArray[i].Time,
                                          tempoMap);

                    long time = metricTime.Minutes * 60 * 1000 + metricTime.Seconds * 1000 + metricTime.Milliseconds;
                    processNote(notesArray[i].NoteName, notesArray[i].Octave, time - previousTime);
                    previousTime = time;
                }

                if (addChunkEndNote)
                {
                    // Add final statement here to show pause after last note to allow loops to work
                    var timeDivision = midiFile.TimeDivision;

                    short ticksPerQuarterNote = 0;
                    if (timeDivision is TicksPerQuarterNoteTimeDivision tpq)
                    {
                        ticksPerQuarterNote = tpq.TicksPerQuarterNote;
                    }

                    float bpm = (float)tempoMap.GetTempoChanges().First().Value.BeatsPerMinute;
                    float secondsPerTick = 60f / (bpm * ticksPerQuarterNote);
                    float msPerTick = secondsPerTick * 1000f;

                    long t = notesArray.Last().Time;
                    float noteTimeSec = t * secondsPerTick;
                    float noteTimeMs = t * msPerTick;
                    long ticksPerBar = ticksPerQuarterNote * 4;
                    long loopTick = ((t + ticksPerBar - 1) / ticksPerBar) * ticksPerBar;

                    float barEndTimeSec = loopTick * secondsPerTick;
                    float barEndTimeMs = loopTick * msPerTick;

                    addNoteToOutput(0, (int)(Math.Round(barEndTimeMs, 0) - previousTime));
                    goodNotesCounter++;
                }

                if (useArrayNotation)
                {
                    tbConvertedMidi.Text = "int notes[" + goodNotesCounter + "] = {" + notesArrayString + "};" + Environment.NewLine +
                        "int offsets[" + goodNotesCounter + "] = {" + offsetArrayString + "};";
                }
                else
                {
                    tbConvertedMidi.Text = gripsString;
                }

                lblGoodNotes.Text = "Good Notes: " + goodNotesCounter;
                lblTooLowNotes.Text = "Too Low Notes: " + tooLowCounter;
                lblTooHighNotes.Text = "Too High Notes: " + tooHighCounter;
            }
        }

        private void processNote(NoteName noteName, int noteOctave, long offsetToNextNote)
        {
            int gripsMappedNote = getGripsNote(noteName);
            int offset = Convert.ToInt32(Math.Round((offsetToNextNote) * speed));

            if (gripsMappedNote + ((noteOctave - baseOctave) * 7) < 1)
            {
                tooLowCounter++;

                if (wrapNotes)
                {
                    Console.WriteLine("gripsMappedNote(" + gripsMappedNote + ") + ((noteOctave(" + noteOctave + ") - baseOctave(" + baseOctave + ")) * 7)(" + (gripsMappedNote + ((noteOctave - baseOctave) * 7)) + ") < 1");

                    // Adjust the octave of the note upwards until the note is in the playable range
                    while (gripsMappedNote + ((noteOctave - baseOctave) * 7) < 1)
                    {

                        noteOctave++;

                        Console.WriteLine("gripsMappedNote(" + gripsMappedNote + ") + ((noteOctave(" + noteOctave + ") - baseOctave(" + baseOctave + ")) * 7)(" + (gripsMappedNote + ((noteOctave - baseOctave) * 7)) + ") < 1");
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
            else if (gripsMappedNote + ((noteOctave - baseOctave) * 7) > 17)
            {
                tooHighCounter++;

                if (wrapNotes)
                {
                    // Adjust the octave of the note downwards until the note is in the playable range
                    while (gripsMappedNote + ((noteOctave - baseOctave) * 7) > 17)
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

            int gripsNote = (gripsMappedNote + ((noteOctave - baseOctave) * 7));
            addNoteToOutput(gripsNote, offset);
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
                gripsString += note + "," + offset + ";";
            }
        }

        private int getGripsNote(NoteName noteName)
        {
            switch (noteName)
            {
                case NoteName.C: return 1;
                case NoteName.CSharp: return 2;
                case NoteName.D: return 3;
                case NoteName.DSharp: return 4;
                case NoteName.E: return 5;
                case NoteName.F: return 5;
                case NoteName.FSharp: return 7;
                case NoteName.G: return 8;
                case NoteName.GSharp: return 9;
                case NoteName.A: return 10;
                case NoteName.ASharp: return 11;
                case NoteName.B: return 12;
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

        private void nudSpeed_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
