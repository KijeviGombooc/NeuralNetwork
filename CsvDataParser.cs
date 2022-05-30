using System.Globalization;

class CsvDataParser
{
    public static List<TrainData> Parse(string path, float inputCount, float outputCount)
    {
        List<TrainData> trainDatas = new List<TrainData>();
        int lineCounter = 0;
        foreach (var line in File.ReadLines(path))
        {
            lineCounter++;
            string[] parts = line.Split(",");
            if(parts.Length != inputCount + outputCount)
            {
                throw new FileLoadException($"Number of datas doesn't match the sum given input count '{inputCount}' and given output count '{outputCount}' at line '{lineCounter}' of file '{path}'");
            }
            List<double> inputs = new List<double>();
            List<double> outputs = new List<double>();
            for (int i = 0; i < parts.Length - outputCount; i++)
            {
                inputs.Add(double.Parse(parts[i], CultureInfo.InvariantCulture));
            }
            outputs.Add(double.Parse(parts.Last()));
            trainDatas.Add(new TrainData(inputs, outputs));
        }
        return trainDatas;
    }
}