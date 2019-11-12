using System.Collections.Generic;
using CustomInput;
using Songs.Model;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace Songs.Gameplay {
	public class SongSetup : MonoBehaviour {

		const string basePath = "Assets/Resources/";

		public GameObject lanePrefab;

		public Song readSong(string file) {
			string path = "Songs/" + file + "/";
			List<SongNote> notes = MidiParser.readMidi(basePath + path + "notes.mid");
			AudioClip songBackground = Resources.Load<AudioClip>(path + "background");
			AudioClip hitNoise = Resources.Load<AudioClip>(path + "instrument");
			//Song test = Song(notes, songBackground, hitNoise);
			return new Song(notes, songBackground, hitNoise);
		}
		/*public float getDifficulty(songObj){
			
		}*/
		public List<Lane> setupLanes() {
			List<Lane> lanes = new List<Lane>();
			
			//coooooordinate transforms. but undisciplined.
			float cameraHeight = 10f;
			float lanePrefabWidth = lanePrefab.GetComponent<SpriteRenderer>().size.x;

			float screenToWorld = cameraHeight / Screen.height;
			Vector2 startingPoint = new Vector2(-1 * Screen.width * screenToWorld, cameraHeight) * 0.5f;
		
			InputSettings.setToDefaultMidi();
			
			float laneWidth = Screen.width * 1f / InputSettings.keys.Length * screenToWorld;
			for (int i = 0; i < InputSettings.keys.Length; i++) {
				Vector2 horizontalOffset = new Vector2(i * laneWidth + (laneWidth * 0.5f), 0);
				GameObject lane = Instantiate(lanePrefab, startingPoint + horizontalOffset, lanePrefab.transform.rotation);
				lane.GetComponent<SpriteRenderer>().size = new Vector2(laneWidth, 10);
				lanes.Add(lane.GetComponent<Lane>());
			}
			
			string[] notes = {"C","C#","D","D#","E","F","F#","G","G#","A","A#","B"};
			int index  = 0;
			for (int j=InputSettings.middleC; j<InputSettings.keys.Length;j++){
				Lane selectedLane = lanes[j];
				Text noteText = selectedLane.GetComponentInChildren<Text>();
				Debug.Log(index);
				if (index>=notes.Length){
					Debug.Log(index%notes.Length);
					noteText.text=notes[index%notes.Length];
				}
				else{
					noteText.text = notes[index];
				}
				index+=1;
			}
			/* 	try{
					noteText.text = notes[index];
				}
				catch (ArgumentOutOfRangeException){
					Debug.Log(index);
				} */
				//noteText.text = notes[index];
				//index+=1;
			
			if (InputSettings.middleC>0){
				int count = 0;
				for (int k=InputSettings.middleC-1;k>=0;k--){
					Lane newLane = lanes[k];
					Text noteText2 = newLane.GetComponentInChildren<Text>();
					if ((notes.Length-1)-count<0){
						noteText2.text = notes[Math.Abs((notes.Length-1)-count%notes.Length)];
					
					}
					else{
						noteText2.text = notes[(notes.Length-1)-count];
					}
					//noteText2.text = notes[(notes.Length-1)-count];
					count+=1;
				}	
			}		
	
				return lanes;
			}
			/* Lane firstLane = lanes[0];
			Text firstText = firstLane.GetComponentInChildren<Text>();
			firstText.text = "test";
		 */
			
		}
	}
