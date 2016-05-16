namespace Services.Math
{
    /// <summary>
    /// Module responsible for generating random numbers
    /// </summary>
    public interface IGenerateRandomNumbers
    {
        double GetRandomDouble();

        int GetRandomInt(int inclusiveMin, int inclusiveMax);
    }
}
