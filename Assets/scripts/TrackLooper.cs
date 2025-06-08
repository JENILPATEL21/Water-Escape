using UnityEngine;

public class TrackLooper : MonoBehaviour
{
    public Transform[] tracks;           // Two track pieces
    public float trackHeight = 18f;      // Height of the track (visual size, not scale)
    public float scrollSpeed = 5f;       // Speed of movement
    public bool isRunning = false;

    void Update()
    {
        if (!isRunning) return;

        foreach (Transform track in tracks)
        {
            track.position += Vector3.down * scrollSpeed * Time.deltaTime;

            // Snap reposition when track is just off the bottom
            if (track.position.y <= -trackHeight)
            {
                Transform highestTrack = GetHighestTrack();
                track.position = new Vector3(0, highestTrack.position.y + trackHeight, 0);
            }
        }
    }

    Transform GetHighestTrack()
    {
        Transform highest = tracks[0];
        foreach (Transform t in tracks)
        {
            if (t.position.y > highest.position.y)
                highest = t;
        }
        return highest;
    }

    public void StartRunning()
    {
        isRunning = true;
    }
    public void StopRunning()
    {
        isRunning = false;
    }
}
