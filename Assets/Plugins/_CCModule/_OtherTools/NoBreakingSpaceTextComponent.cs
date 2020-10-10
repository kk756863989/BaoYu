using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class NoBreakingSpaceTextComponent : MonoBehaviour
{
    Text text;
    static readonly string no_breaking_space = "\u00A0";
    private void Awake()
    {
        text = GetComponent<Text>();
        text.RegisterDirtyVerticesCallback(OnTextChange);
    }

    private void OnTextChange()
    {
        if (text.text.Contains(" "))
            text.text = text.text.Replace(" ", no_breaking_space);
    }
}
