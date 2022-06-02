
class Program
{
    static void Main(string[] args)
    {
        int inputCount = 2;
        int outputCount = 1;
        int hiddenCount = 5;
        int epochCount = 1000000;
        double learningRate = 0.05;

        List<TrainData> trainDatas = new List<TrainData>()
        {
            new TrainData(new List<double>(){ 0, 0 }, new List<double>(){ 0 }),
            new TrainData(new List<double>(){ 0, 1 }, new List<double>(){ 1 }),
            new TrainData(new List<double>(){ 1, 1 }, new List<double>(){ 0 }),
            new TrainData(new List<double>(){ 1, 0 }, new List<double>(){ 1 })
        };
        Network network = new Network(inputCount, hiddenCount, outputCount);
        for (int e = 0; e < epochCount; e++)
        {
            trainDatas = trainDatas.OrderBy(c => Utils.Random.Next()).ToList();

            for (int i = 0; i < trainDatas.Count; i++)
            {
                network.Forward(trainDatas[i].inputData);
                network.Backward(trainDatas[i].inputData, trainDatas[i].expectedValue, learningRate);
            }
        }
        for (int i = 0; i < trainDatas.Count; i++)
        {
            network.Forward(trainDatas[i].inputData);
            System.Console.WriteLine($"Input: {{{trainDatas[i].inputData[0]}; {trainDatas[i].inputData[1]}}}; Expected output: {trainDatas[i].expectedValue[0]}; Trained output: {network.Output()[0]}");
        }
    }
}
