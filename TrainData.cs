class TrainData
{
    public List<double> inputData;
    public List<double> expectedValue;
    public TrainData(List<double> inputData, List<double> expectedValue)
    {
        this.inputData = inputData;
        this.expectedValue = expectedValue;
    }
}