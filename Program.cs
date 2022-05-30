
// using Neuron = System.Collections.Generic.List<double>;
// using Layer = System.Collections.Generic.List<Neuron>;
using Inputs = System.Collections.Generic.List<double>;
using Outputs = System.Collections.Generic.List<double>;



class Program
{
    static void Main(string[] args)
    {
        Network network = new Network(
            new List<Layer>(){
                new Layer(new List<Neuron>{
                     new Neuron(new List<double>(){
                         0.13436424411240122, 0.8474337369372327
                     }, 0.763774618976614)
                }),
                new Layer(new List<Neuron>{
                     new Neuron(new List<double>(){
                         0.2550690257394217
                     }, 0.49543508709194095),
                     new Neuron(new List<double>(){
                         0.4494910647887381
                     }, 0.651592972722763)
                })
            }
        );

        foreach (var output in ForwardPropagate(network, new Inputs{1, 0, 0}))
        {
            System.Console.WriteLine(output);
        }
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
