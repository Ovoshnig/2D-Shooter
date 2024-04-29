using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text textScore;

    static private int playerScore;
    static private int playerComputerScore;

    public static int PlayerScore { get => playerScore; set => playerScore = value; }
    public static int PlayerComputerScore { get => playerComputerScore; set => playerComputerScore = value; }

    private void Start()
    {
        playerScore = 0;
        playerComputerScore = 0;
    }

    private void Update()
    {
        textScore.text = $"<color=#04B400>{playerScore}</color> : <color=#B40E00>{playerComputerScore}</color>";
    }
}
