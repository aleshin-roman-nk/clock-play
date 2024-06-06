using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomGameObjectPlayer : MonoBehaviour
{
	public GameObject[] objects;

	private GameObject[] _objectsPlayList;
	private int _index = 0;

	public Transform wherePoint;

	// Start is called before the first frame update
	void Start()
	{
		_objectsPlayList = generatePlaylist(objects, 1, 15);
	}

	public void Play()
	{
		if (_objectsPlayList.Length == 0) return;
		if(_index >= _objectsPlayList.Length) _index = 0;
		Instantiate(_objectsPlayList[_index], wherePoint.position, Quaternion.identity);
		_index++;
	}

	private T[] generatePlaylist<T>(T[] originalArray, int maxRepetition, int playListLength)
	{
		if (maxRepetition < 1)
		{
			Debug.LogError("maxRepetition must be at least 1.");
		}

		T[] newArray = new T[originalArray.Length * playListLength];

		if (newArray.Length == 0) return newArray;

		Queue<T> lastElements = new Queue<T>(maxRepetition);

		for (int i = 0; i < newArray.Length; i++)
		{
			T element;

			// Randomly select an element from the original array
			do
			{
				element = originalArray[Random.Range(0, originalArray.Length)];
			}
			// Ensure that the same element is not repeated more than maxRepetition times in a row
			while (lastElements.Contains(element) && lastElements.Count >= maxRepetition);

			newArray[i] = element;

			// Update the queue with the latest element
			if (lastElements.Count == maxRepetition)
			{
				lastElements.Dequeue();
			}
			lastElements.Enqueue(element);
		}

		return newArray;
	}
}
