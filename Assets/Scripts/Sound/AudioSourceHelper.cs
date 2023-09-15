using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Messy
{
    public class AudioSourceHelper
    {
        public static float audioFactor = 0.5f;

        public static AudioSource PlayClipAt(AudioClip clip, Vector3 pos, float volume)
        {
            GameObject tempGO = new GameObject("TempAudio"); // create the temp object
            tempGO.transform.SetParent(ObjectRetriever.GetTreeFolderSounds().transform);
            tempGO.transform.position = pos; // set its position
            AudioSource aSource = tempGO.AddComponent<AudioSource>(); // add an audio source
            aSource.clip = clip; // define the clip
            aSource.spatialBlend = 1;
            aSource.spread = 1;
            aSource.dopplerLevel = 0.5f;
            aSource.volume = volume * audioFactor;

            // set other aSource properties here, if desired
            aSource.Play(); // start the sound
            GameObject.Destroy(tempGO, clip.length); // destroy object after clip duration
            return aSource; // return the AudioSource reference
        }
    }
}