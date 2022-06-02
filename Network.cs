using System.Collections;

class Network : IEnumerable<Layer>
{
    public List<Layer> layers;

    public Network(List<Layer> layers)
    {
        this.layers = layers;
    }

    public Network(int nInputs, int nHiddenNeuron, int nOutputs, int nHiddenLayer = 1)
    {
        layers = new List<Layer>();
        Layer hiddenLayer = new Layer(Utils.GenerateList(nHiddenNeuron, (_) => {
            return new Neuron(Utils.GenerateList(nInputs, (_) => {
                return Utils.Random.NextDouble();
            }), Utils.Random.NextDouble());
        }));
        layers.Add(hiddenLayer);
        for (int i = 0; i < nHiddenLayer - 1; i++)
        {
            Layer hiddenLayerI = new Layer(Utils.GenerateList(nHiddenNeuron, (_) => {
                return new Neuron(Utils.GenerateList(nHiddenNeuron, (_) => {
                    return Utils.Random.NextDouble();
                }), Utils.Random.NextDouble());
            }));
            layers.Add(hiddenLayerI);
        }
        Layer outputLayer = new Layer(Utils.GenerateList(nOutputs, (_) => {
            return new Neuron(Utils.GenerateList(nHiddenNeuron, (_) => {
                return Utils.Random.NextDouble();
            }), Utils.Random.NextDouble());
        }));
        layers.Add(outputLayer);
    }

    public void Forward(List<double> input)
    {
        layers[0].ActivateAndTransferNeurons(input);
        for (int i = 1; i < layers.Count; i++)
        {
            Layer previousLayer = layers[i - 1];
            Layer currentLayer = layers[i];
            currentLayer.ActivateAndTransferNeurons(previousLayer.NeuronOutputs());
        }
    }

    public void Backward(List<double> input, List<double> expectedOutput, double learningRate)
    {
        // first layer is a little different than the rest, it has its own separate code:
        {
            // variables for ease of read
            Layer lastLayer = layers[layers.Count() - 1];
            Layer previousLayer = layers[layers.Count() - 2];
            int lastLayerNeuronCount = lastLayer.Count();

            // go through each neuron in last layer
            for (int i = 0; i < lastLayerNeuronCount; i++)
            {
                // variables for ease of read
                Neuron currentNeuron = lastLayer[i];
                double output = currentNeuron.output;
                double neuronWeightCount = currentNeuron.Count();
                double target = expectedOutput[i];

                // calculate partial derivatives for this neuron (caching them for later use)
                currentNeuron.dEtotal_dOutput = -(target - output);
                currentNeuron.dOutput_dNet  = output * (1 - output);

                // update bias
                currentNeuron.cachedBias -= learningRate * currentNeuron.dEtotal_dOutput * currentNeuron.dOutput_dNet;

                // go through each weight of the neuron
                for (int j = 0; j < neuronWeightCount; j++)
                {
                    // variables for ease of read
                    double previousOutput = previousLayer[j].output;
                    double currentWeight = currentNeuron[j];

                    // calculate partial derivative of pure neuron output according to current weight
                    double dNet_dWeight  = previousOutput;

                    // calculate total error effect on current weight
                    double dEtotal_dWeight = currentNeuron.dEtotal_dOutput * currentNeuron.dOutput_dNet * dNet_dWeight;

                    // calculate new weight and cache it
                    currentNeuron.SetCachedWeight(j, currentWeight - learningRate * dEtotal_dWeight);
                }
            }
        }

        // the other layers are calculated the same way:
        // go through each layer
        for (int layerIndex = layers.Count - 2; layerIndex >= 0 ; layerIndex--)
        {
            // variables for ease of read
            Layer currentLayer = layers[layerIndex];
            Layer nextLayer = layers[layerIndex + 1];
            Layer? previousLayer = layerIndex == 0 ? null : layers[layerIndex - 1];
            int currentLayerNeuronCount = currentLayer.Count();
            int nextLayerNeuronCount = nextLayer.Count();

            for (int i = 0; i < currentLayerNeuronCount; i++)
            {
                // variables for ease of read
                Neuron currentNeuron = currentLayer[i];
                double output = currentNeuron.output;
                double neuronWeightCount = currentNeuron.Count();
                
                // calculate partial derivatives for this neuron (caching them for later use)
                currentNeuron.dEtotal_dOutput = 0;
                // go through each neuron in next layer and calculate partial derivative of next layer neuron error according to current output
                for (int j = 0; j < nextLayerNeuronCount; j++)
                {
                    // variable for ease of read
                    Neuron nextLayerCurrentNeuron = nextLayer[j];
                    double dNet_dOutput = nextLayerCurrentNeuron[i];

                    // calculate one part of derivative and add it to sum
                    currentNeuron.dEtotal_dOutput += nextLayerCurrentNeuron.dEtotal_dOutput * nextLayerCurrentNeuron.dOutput_dNet * dNet_dOutput;
                }
                currentNeuron.dOutput_dNet  = output * (1 - output);

                // update bias
                currentNeuron.cachedBias -= learningRate * currentNeuron.dEtotal_dOutput * currentNeuron.dOutput_dNet;

                // go through each weight of the neuron
                for (int j = 0; j < neuronWeightCount; j++)
                {
                    // variables for ease of read
                    double previousOutput = previousLayer == null ? input[j] : previousLayer[j].output;
                    double currentWeight = currentNeuron[j];

                    // calculate partial derivative of pure neuron output according to current weight
                    double dNet_dWeight  = previousOutput;

                    // calculate total error effect on current weight
                    double dEtotal_dWeight = currentNeuron.dEtotal_dOutput * currentNeuron.dOutput_dNet * dNet_dWeight;

                    // calculate new weight and cache it
                    currentNeuron.SetCachedWeight(j, currentWeight - learningRate * dEtotal_dWeight);
                }
            }
        }

        foreach (var layer in layers)
        {
            foreach (var neuron in layer)
            {
                neuron.UpdateWeights();
            }
        }
    }

    public List<double> Output()
    {
        return layers.Last().NeuronOutputs();
    }

    public IEnumerator<Layer> GetEnumerator()
    {
        return new NetworkEnumerator(this);
    }

    public Layer this[int index]
    {
        get => layers[index];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new NetworkEnumerator(this);
    }

    private class NetworkEnumerator : IEnumerator<Layer>
    {
        private int position = -1;
        private Network network;
        public NetworkEnumerator(Network network)
        {
            this.network = network;
        }

        public Layer Current => network.layers[position];

        object IEnumerator.Current => network.layers[position];

        public bool MoveNext()
        {
            position++;
            return position < network.layers.Count;
        }

        public void Reset()
        {
            position = -1;
        }

        public void Dispose()
        {
        }
    }
}