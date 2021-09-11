using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class NicknameSelection : MonoBehaviour
{
    [SerializeField] TMP_InputField nicknameInputField;
    [SerializeField] TMP_Text errorText;

    public void SelectNickname()
    {
        if (string.IsNullOrEmpty(nicknameInputField.text))
            return;

        if (nicknameInputField.text.Length > 17)
        {
            errorText.gameObject.SetActive(true);
            return;
        }
        if (nicknameInputField.text == "")
            return;
        
        PhotonNetwork.LocalPlayer.NickName = nicknameInputField.text;

        using (StreamWriter writer = new StreamWriter("./pseudo.tpas"))
        {
            writer.Write(nicknameInputField.text);
        }
        
        MenuManager.Instance.OpenMenu("main");
    }
}
