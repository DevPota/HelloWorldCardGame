using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System.Linq;

public class LogoManager : MonoBehaviour
{
    PlayableDirector PD;

    int markerIndex = 0;

    private void Start()
    {
        PD = GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0) == true)
        {
            var timelineAsset = PD.playableAsset as TimelineAsset;
            var markers = timelineAsset.markerTrack.GetMarkers().ToArray();
            if(markers.Length > markerIndex)
            {
                PD.Play();
                PD.time = markers[markerIndex].time;
            }
        }
    }

   public void AddMarkerIndex()
   {
        markerIndex++;
   }

    public void ToMenuScene()
    {
        Core.I.LoadScene(Define.SCENE_MENU_STR);
    }
}
