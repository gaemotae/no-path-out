using UnityEngine;
using UnityEngine.SceneManagement;

public class UIStateControl : MonoBehaviour
{
    public enum UIState
    {
        Start,
        Playing,
        GameOver,
        Clear
    }

    [Header("UI Objects (Text)")]
    public GameObject m_StartText;
    public GameObject m_GameOverText;
    public GameObject m_ClearText;

    [Header("State")]
    public UIState m_State = UIState.Start;

    public bool IsPlaying => m_State == UIState.Playing;

    private void Start()
    {
        SetState(UIState.Start);
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        if (m_State == UIState.Start)
        {
            SetState(UIState.Playing);
        }
        else if (m_State == UIState.GameOver || m_State == UIState.Clear)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public bool ShowGameOver()
    {
        return TrySetResult(UIState.GameOver);
    }

    public bool ShowClear()
    {
        return TrySetResult(UIState.Clear);
    }

    private bool TrySetResult(UIState result)
    {
        // Only the first result during Playing can finish this run.
        if (!IsPlaying) return false;

        SetState(result);
        return true;
    }

    private void SetState(UIState newState)
    {
        m_State = newState;

        if (m_StartText) m_StartText.SetActive(m_State == UIState.Start);
        if (m_GameOverText) m_GameOverText.SetActive(m_State == UIState.GameOver);
        if (m_ClearText) m_ClearText.SetActive(m_State == UIState.Clear);

        bool pause = (m_State != UIState.Playing);
        Time.timeScale = pause ? 0f : 1f;

        Cursor.lockState = pause ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = pause;
    }
}
