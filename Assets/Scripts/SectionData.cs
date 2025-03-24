using UnityEngine;

public class SectionData : MonoBehaviour
{
    public Section section;
    public HintData hintData;

    public static SectionData instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
}