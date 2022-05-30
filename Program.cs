
// using Neuron = System.Collections.Generic.List<double>;
// using Layer = System.Collections.Generic.List<Neuron>;
using Inputs = System.Collections.Generic.List<double>;
using Outputs = System.Collections.Generic.List<double>;



class Program
{
    static void Main(string[] args)
    {
        Network network = new Network(2, 1, 2);
        foreach (var layer in network)
        {
            foreach (var neuron in layer)
            {
                foreach (var weight in neuron)
                {
                    System.Console.Write(weight + "; ");
                }
                System.Console.WriteLine("bias: " + neuron.bias);
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
                neuron.output = Transfer(activation);
                newInputs.Add(neuron.output);
            }
            inputs = newInputs;
        }
        return inputs;
    }

    static double Transfer(double activation)
    {
        return 1.0 / (1.0 + Math.Exp(-activation));
    }

    static double Activate(Neuron neuron, Inputs inputs)
    {
        double activation = neuron.bias;
        int i = 0;
        foreach (var weight in neuron)
            activation += weight * inputs[i++];
        return activation;
    }
}
