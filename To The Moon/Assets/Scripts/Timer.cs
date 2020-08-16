using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

    [SerializeField]
    private int timeLeft = 30; //Seconds Overall

    [SerializeField]
    private Text countdown; //UI Text Object

    private bool isRunning = true;

    public int TimeLeft { get => timeLeft; set => timeLeft = value; }

    public delegate void TimeEnded();
    public static event TimeEnded OnTimeEnded;

    // Start is called before the first frame update
    IEnumerator Start() {
        yield return StartCoroutine(LoseTime());
    }

    void Update() {
        countdown.text = ("" + TimeLeft); //Showing the Score on the Canvas
    }

    //Simple Coroutine
    IEnumerator LoseTime() {
        while(isRunning) {
            yield return new WaitForSeconds(1);
            TimeLeft--;
            if(TimeLeft <= 0) {
                isRunning = false;
                OnTimeEnded();
                //Initiate.Fade("GameOver", Color.black, 1f);
            }
        }
    }
}
