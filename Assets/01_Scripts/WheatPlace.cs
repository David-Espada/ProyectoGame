using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WheatPlace : MonoBehaviour
{
    [Header("Stats")]
    [Range(0, 100)]
    public float maxWheat;
    [Range(0, 100)]
    public float minWheat;
    public float wheatMultier;
    public float wheatGenerated;
    public float actualWheat;
    public bool IsEnable;
    public bool IsOcuped;
    public bool IsDay;
    public float regenerationRate;
    public Sheep sheep;
    void Start()
    {
        float random = Random.Range(minWheat, maxWheat) * wheatMultier;
        wheatGenerated = random;
        actualWheat = random;
        IsEnable = true;
        IsOcuped = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (sheep != null)
        {
            float distance = Vector3.Distance(transform.position, sheep.transform.position);
            if (distance <= 10)
            {
                FeedSheep();
            }
        }
        if (!IsDay)
        {
            RegenerateWheat();
        }
    }
    public void FeedSheep()
    {
        float amount = Time.deltaTime;
        actualWheat -= amount;
        if (actualWheat <= 0)
        {
            IsEnable = false;
            sheep.ChangeState(SheepState.Resting);
            IsOcuped = false;
            sheep = null;
        }
        else
        {
            transform.localScale = Vector3.one * (actualWheat/wheatGenerated);
            sheep.Eat(amount);
        }
        
    }
    public void RegenerateWheat()
    {
        actualWheat += Time.deltaTime * regenerationRate;
        if (actualWheat >= wheatGenerated)
        {
            actualWheat = wheatGenerated;
        }
        transform.localScale = Vector3.one * (actualWheat / wheatGenerated);
    }

}
