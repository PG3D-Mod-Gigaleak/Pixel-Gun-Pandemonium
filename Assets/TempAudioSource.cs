using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAudioSource
{
	public TempAudioSource(Vector3 position, AudioClip clip)
	{
		AudioSource source = new GameObject(clip.name).AddComponent<AudioSource>();
		
		source.transform.position = position;

		source.clip = clip;
		source.Play();

		Object.Destroy(source.gameObject, clip.length);
	}
}
