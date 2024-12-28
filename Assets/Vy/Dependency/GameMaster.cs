using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    [SerializeField] private UserProfile userProfile;
    public static GameMaster Instance { get; private set; }
    public UserProfile GetUserProfile() => userProfile;
    public int DisplayLevel => userProfile.CurrentLevel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }
}
