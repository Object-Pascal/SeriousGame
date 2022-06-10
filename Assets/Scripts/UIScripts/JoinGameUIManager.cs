using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static TMPro.TMP_Dropdown;

public class JoinGameUIManager : MonoBehaviour
{
    public TextMeshProUGUI joinGame;
    public TextMeshProUGUI searchGamePlaceholder;
    public TextMeshProUGUI searchGameInput;

    public TMP_Dropdown gameListDropdown;

    private string searchGameValue;

    private List<string> gameList;
    private List<string> currentGameList;
    private string selectedGame;

    private void Start()
    {
        gameList = new();
        currentGameList = new();

        string json = "{\"strings\":[\"string1\",\"string2\",\"string3\",\"string4\"]}";
        SetGameList(json);
    }

    public void ChangeJoinGameText(string text)
    {
        joinGame.text = text;
    }

    public void ChangeSearchGamePlaceholderText(string text)
    {
        searchGamePlaceholder.text = text;
    }

    public void GetSearchGameInput()
    {
        searchGameValue = searchGameInput.text;
    }

    public void SetGameList(List<string> games)
    {
        currentGameList.Clear();
        gameList.AddRange(games);
        currentGameList.AddRange(games);
        gameListDropdown.AddOptions(StringToOptionData(gameList));
    }

    public void SetGameList(string gamesJson)
    {
        currentGameList.Clear();
        JsonStringList list = JsonUtility.FromJson<JsonStringList>(gamesJson);
        gameList.AddRange(list.strings);
        currentGameList.AddRange(list.strings);
        gameListDropdown.AddOptions(StringToOptionData(gameList));
    }

    public void FiltrerGameList()
    {
        string searchText = searchGameInput.text.Substring(0, searchGameInput.text.Length - 1);
        currentGameList.Clear();

        if (searchText == null)
            currentGameList.AddRange(gameList);
        else
            foreach (string s in gameList)
                if (s.Contains(searchText))
                    currentGameList.Add(s);

        gameListDropdown.ClearOptions();
        gameListDropdown.AddOptions(StringToOptionData(currentGameList));
    }

    public void SetSelectedGame()
    {
        selectedGame = currentGameList[gameListDropdown.value];
        Debug.Log(selectedGame);
    }

    public void SendGameToServer()
    {
        Debug.Log(selectedGame);
    }

    private List<OptionData> StringToOptionData(List<string> strings)
    {
        List<OptionData> options = new();
        foreach (string s in strings)
        {
            options.Add(new(s));
        }
        return options;
    }

    private class JsonStringList
    {
        public List<string> strings;
    }

}
