using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TargetIndicatorInCavas : MonoBehaviour, IPooledObject
{
    //Saves a reference to the script that instantiated it
    //[HideInInspector] public AddTargetInticatorCanvas objectWorld;
    //Guarda uma referência do transform que instanciou
    public Transform Target;
    //Vai recer o modo atual de Render
    [HideInInspector] public RenderMode renderMode = RenderMode.ScreenSpaceOverlay;
    //Transform do objeto dentro do Canvas
    [HideInInspector] public RectTransform thisObject;
    //Guarda uma referência para o canvas da cena
    //[HideInInspector] public RectTransform mainCanvas;


    //Receives the value for internal space
    [HideInInspector] public float padding;
    Image myImage;
    //private IEnumerator Start()
    //{
    //    myImage = GetComponent<Image>();
    //    yield return null;
    //    float minX = (Screen.width / 2) - padding;
    //    float maxY = (Screen.height / 2) - padding;
    //    Debug.Log("Max X: " + minX);
    //    Debug.Log("Max Y : " + maxY);
    //}
    void Update()
    {
        UpdateInfoIndicatorCanvas();
    }

    void UpdateInfoIndicatorCanvas()
    {
        UpdateIndicatorPosition();
        //Other actions ...
        float minX = (Screen.width / 2) - padding;
        float maxY = (Screen.height / 2) - padding;
        if (!Target || !Target.gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            return;
        }

        if (thisObject.anchoredPosition.x <= -minX || thisObject.anchoredPosition.x >= minX || thisObject.anchoredPosition.y <= -maxY || thisObject.anchoredPosition.y >= maxY)
            Dbug(true);
        else
            Dbug(false);
    }
    void Dbug(bool state)
    {
        if (state)
        {
            if (myImage.color.a == 0)
                myImage.DOFade(1, 0);
        }
        else
        {
            if (myImage.color.a == 1)
                myImage.DOFade(0, 0);
        }
    }
    private void UpdateIndicatorPosition()
    {
        switch (renderMode)
        {

            case RenderMode.ScreenSpaceOverlay:
                {
                    Vector3 convertedPosition = Camera.main.WorldToViewportPoint(Target.position);

                    if (convertedPosition.z < 0)
                    {

                        convertedPosition = Vector3Invert(convertedPosition);
                        convertedPosition = Vector3FixEdge(convertedPosition);
                    }
                    convertedPosition = Camera.main.ViewportToScreenPoint(convertedPosition);
                    KeepCameraInside(convertedPosition);
                }
                break;
            case RenderMode.ScreenSpaceCamera:
                {

                    Debug.LogWarning("Render Mode - Screen Space Camera não suportado!");
                }
                break;
            case RenderMode.WorldSpace:
                {

                    Debug.LogWarning("Render Mode - Worl Space não suportado!");
                }
                break;
        }
    }
    private void KeepCameraInside(Vector2 reference)
    {
        //Keeping the object inside the Screen adding to the padding
        reference.x = Mathf.Clamp(reference.x, padding, Screen.width - padding);
        reference.y = Mathf.Clamp(reference.y, padding, Screen.height - padding);
        thisObject.transform.position = reference;

    }


    Vector3 Vector3Invert(Vector3 viewport_position)
    {
        Vector3 invertedVector = viewport_position;
        //Inverts position based on screen dimension
        invertedVector.x = 1f - invertedVector.x;
        invertedVector.y = 1f - invertedVector.y;
        invertedVector.z = 0;
        return invertedVector;
    }
    public Vector3 Vector3FixEdge(Vector3 vector)
    {
        Vector3 vectorFixed = vector;

        //Getting higher value of the Vector, higher value always greater than 0
        float highestValue = Vector3Max(vectorFixed);

        //Getting lower value of the Vector, lower value always less than 0
        float lowerValue = Vector3Min(vectorFixed);

        //Obento the side closest to the target for the indicator to be[0.0] or[1.1]


        float highestValueBetween = DirectionPreference(lowerValue, highestValue);

        Debug.Log("Vetor:" + vector + " Maior : " + highestValue + " Menor : " + lowerValue + " Melhor:" + highestValueBetween);


        /**Pinning vector to the edge [1,1]**/

        //If highest is the best to look at
        if (highestValueBetween == highestValue)
        {

            vectorFixed.x = vectorFixed.x == highestValue ? 1 : vectorFixed.x;
            vectorFixed.y = vectorFixed.y == highestValue ? 1 : vectorFixed.y;

            //If lower value is the best look
        }

        /*Fixing vector in the border [0,0]*/

        //If lowerValue is the best to look at
        if (highestValueBetween == lowerValue)
        {

            vectorFixed.x = Mathf.Abs(vectorFixed.x) == lowerValue ? 0 : Mathf.Abs(vectorFixed.x);
            vectorFixed.y = Mathf.Abs(vectorFixed.y) == lowerValue ? 0 : Mathf.Abs(vectorFixed.y);
        }
        Debug.Log("Vetor:" + vectorFixed);
        return vectorFixed;
    }
    float Vector3Max(Vector3 vector)
    {

        float highestValue = Mathf.Max(vector.x, vector.y);
        return highestValue;
    }
    float Vector3Min(Vector3 vector)
    {
        float lowerValue = 0f;
        lowerValue = vector.x <= lowerValue ? vector.x : lowerValue;
        lowerValue = vector.y <= lowerValue ? vector.y : lowerValue;

        return lowerValue;
    }

    float DirectionPreference(float lowerValue, float highestValue)
    {

        //Converting values ​​to positive to see which is greater
        lowerValue = Mathf.Abs(lowerValue);
        highestValue = Mathf.Abs(highestValue);


        //Obtaining greater value between the two
        float highestValueBetween = Mathf.Max(lowerValue, highestValue);

        return highestValueBetween;
    }

    public void OnObjectSpawn()
    {
        //throw new System.NotImplementedException();
    }

    public void OnObjectActive()
    {
        if (!myImage)
        {
            myImage = GetComponent<Image>();
            myImage.DOFade(0, 0);
        }
    }
}
