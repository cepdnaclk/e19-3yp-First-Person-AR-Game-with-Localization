using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoginController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField userNameInputField;

    [SerializeField]
    private TMP_InputField passwordInputField;

    public void onSubmitLogin()
    {
        string userName = userNameInputField.text;
        string passWord = passwordInputField.text;
        Debug.Log(userName);
    }

}
