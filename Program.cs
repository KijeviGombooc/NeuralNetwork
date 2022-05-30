
using Neuron = System.Collections.Generic.List<double>;
using Layer = System.Collections.Generic.List<System.Collections.Generic.List<double>>;
using Network = System.Collections.Generic.List<System.Collections.Generic.List<System.Collections.Generic.List<double>>>;
using Inputs = System.Collections.Generic.List<double>;
using Outputs = System.Collections.Generic.List<double>;

class Program
{
    static Random random = new Random(1);

    static void Main(string[] args)
    {
        Network network = InitializeNetwork(2, 1, 2);
        foreach (var layer in network)
        {
            foreach (var neuron in layer)
            {
                foreach (var weight in neuron)
                {
                    System.Console.Write(weight + "; ");
                }
                System.Console.WriteLine();
            }
        }
        System.Console.WriteLine("Output:");
        foreach (var output in ForwardPropagate(network, new Inputs{1.0, 0.0, 0.0}))
        {
            System.Console.Write(output + "; ");
        }
        System.Console.WriteLine();
    }

    static Outputs ForwardPropagate(Network network, Inputs lastOutput)
    {
        Inputs inputs = lastOutput;
        foreach (var layer in network)
        {
            Inputs newInputs = new Inputs();
            foreach (var neuron in layer)
            {
                double activation = Activate(neuron, inputs);
                double neuronOutput = Transfer(activation);
                newInputs.Add(neuronOutput);
            }
            inputs = newInputs;
        }
        return inputs;
    }

    static double Transfer(double activation)
    {
        return 1.0 / (1.0 + Math.Exp(-activation));
    }

    static double Activate(Neuron neuronWeights, Inputs inputs)
    {
        double activation = neuronWeights.Last();
        for (int i = 0; i < neuronWeights.Count - 1; i++)
        {
            activation += neuronWeights[i] * inputs[i];
        }
        return activation;
    }

    static Network InitializeNetwork(int nInputs, int nHidden, int nOutputs)
    {
        List<Layer> layers = new List<Layer>();
        Layer hiddenLayer = GenerateList(nHidden, (_) => {
            return new Neuron(GenerateList(nInputs + 1, (_) => {
                return random.NextDouble();
            }));
        });
        layers.Add(hiddenLayer);
        Layer outputLayer = GenerateList(nOutputs, (_) => {
            return new Neuron(GenerateList(nHidden + 1, (_) => {
                return random.NextDouble();
            }));
        });
        layers.Add(outputLayer);
        return layers;
    }

    static List<T> GenerateList<T>(int count, Func<int, T> func)
    {
        List<T> list = new List<T>(count);
        for (int i = 0; i < count; i++)
            list.Add(func(i));
        return list;
    }
}
