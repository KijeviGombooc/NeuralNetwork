class NetworkTrainer
{
    public static void Train(Network network, List<TrainData> trainDatas, int epochCount, int printEvery, double learningRate)
    {
        double error = 0;
        int errorCount = 0;
        for (int e = 0; e < epochCount; e++)
        {
            trainDatas = trainDatas.OrderBy(c => Utils.Random.Next()).ToList();

            for (int i = 0; i < trainDatas.Count; i++)
            {
                network.Forward(trainDatas[i].inputData);
                var output = network.Output();
                for (int j = 0; j < output.Count; j++)
                {
                    error += Math.Pow(output[j] - trainDatas[i].expectedValue[j], 2);
                    errorCount++;
                }
                network.Backward(trainDatas[i].inputData, trainDatas[i].expectedValue, learningRate);
            }
            if((e + 1) % printEvery == 0)
            {
                System.Console.WriteLine($"Epoch: {e + 1}; Error: {error / errorCount}");
                errorCount = 0;
                error = 0;
            }
        }
    }
}