using System.IO;

public class RandomWeight
{
    public static int Random(int[] weights)
    {
        int sumOfWeights = 0;
        foreach (int i in weights)
        {
            sumOfWeights += i;
        }
        int randomNumber = UnityEngine.Random.Range(1, sumOfWeights + 1);
        int currentWeight = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            currentWeight += weights[i];
            if (currentWeight >= randomNumber)
            {
                return i;
            }
        }
        throw new InvalidDataException("Yo bro what did you do");
    }
}