using System.Collections.Generic;
using CustomInput;
using Songs.Model;
using UnityEngine;
using Melanchall.DryWetMidi.MusicTheory;
using Songs.Gameplay;
using FluidSynth;

namespace Songs.Gameplay {
	public class SongManager : MonoBehaviour {

		public Song song;
		public SongSetup songSetup;
		public AudioSource backgroundMusic;
		public List<Lane> lanes;
		public float songStartTime;
		public InputManager input;

		private MiddlewareAPI midiSys;

		[HideInInspector]
		public int numPerfect;
		[HideInInspector]
		public int numGood;
		[HideInInspector]
		public int numMiss;
		[HideInInspector]
		public int score = 0;
		[HideInInspector]
		public int numCorrect = 0;
		[HideInInspector]
		public int highestCombo = 0;
		[HideInInspector]
		public int combo = 0;

		public string bandName = "Test";
		public string songName = "HotCrossBunsLow";

		void Awake() {
			midiSys = new Middleware(hotplugMIDI: false);
			midiSys.SetGain(1f);
			var sfont = midiSys.LoadSoundFont("soundfonts/*.sf2");
			midiSys.SetChannelInstrument(0, sfont, 0, 0);
		}

		void Start() {
			if (!InputSettings.initalized) {
				InputSettings.setToDefault();
			}
			song = songSetup.readSong(bandName, songName);

			backgroundMusic.clip = song.backgroundTrack;
			backgroundMusic.Play();

			lanes = songSetup.setupLanes(song);
			songStartTime = Time.time;
		}

		void Update() {
			createNewNotes();

			//Detect notes being pressed
			senseKeyPresses();
		}

		//Summon new notes as necessary
		private void createNewNotes() {
			List<SongNote> currentNotes = song.getNotesAtTime(Time.time - songStartTime);
			foreach (SongNote note in currentNotes) {
				int offsetFromC = note.toIndex() - new SongNote(NoteName.C, 4).toIndex();
				int noteIndex = offsetFromC + InputSettings.middleC;
				if (noteIndex > 0 && noteIndex < InputSettings.keys.Length - 1) {
					lanes[noteIndex].createNote(note);
				} else {
					Debug.Log("Note " + note + " outside of playable range");
				}
			}
		}

		private void senseKeyPresses() {
			//sense which k_eys are pressed
			foreach (int laneIdx in input.keysIndiciesPressedButton) {
				Lane lane = lanes[laneIdx];
				lane.makePressVFx();
				LaneNote lowestNote = lane.getLowestNote();
				midiSys.PlayNote(0, lane.myPitch.toIndex(), 127);

				if (lowestNote != null) {
					if (lowestNote.isHoldNote) {
						lowestNote.isBeingPressed = true;
					} else {
						numCorrect = calculateScore(numCorrect, lowestNote.bottomCircle, lane);
						lane.destroyLowestNote();
					}
				}
			}

			foreach (int laneIdx in input.keysIndiciesReleasedButton) {
				Lane lane = lanes[laneIdx];
				lane.makePressVFx();
				LaneNote lowestNote = lane.getLowestNote();
				midiSys.StopNote(0, lane.myPitch.toIndex());

				if (lowestNote != null) {
					if (lowestNote.isHoldNote && lowestNote.isBeingPressed) {
						numCorrect = calculateScore(numCorrect, lowestNote.topCircle, lane);
						lane.destroyLowestNote();
					} else {
					}
				}
			}
		}

		//returns new num correct
		private int calculateScore(int numCorrect, GameObject note, Lane lane) {
			int scoreMult = Multiplier(numCorrect);
			float distance = (note.transform.position - lane.noteTarget.transform.position).magnitude;
			if (distance < 0.5) {
				lane.noteTapVFx(PressAccuracy.Perfect);
				score += 100 * scoreMult;
				numPerfect++;
				combo++;
				if (combo > highestCombo) {
					highestCombo = combo;
				}
				return numCorrect + 1;
			} else if (distance < 1) {
				lane.noteTapVFx(PressAccuracy.Good);
				score += 75 * scoreMult;
				numGood++;
				combo++;
				if (combo > highestCombo) {
					highestCombo = combo;
				}
				return numCorrect + 1;
			} else {
				lane.noteTapVFx(PressAccuracy.Miss);
				scoreMult = 0;
				numMiss++;
				if (combo > highestCombo) {
					highestCombo = combo;
				}
				combo = 0;
				return 0;
			}
		}

		public static int Multiplier(int numCorrect) {
			if (numCorrect > 200) {
				return 8;
			} else if (numCorrect > 100) {
				return 4;
			} else if (numCorrect > 50) {
				return 2;
			} else {
				return 1;
			}
		}
	}
}