using System.Collections;

class Layer : IEnumerable<Neuron>
{
    private List<Neuron> neurons = new List<Neuron>();

    public Layer(List<Neuron> neurons)
    {
        this.neurons = neurons;
    }

    public void ActivateAndTransferNeurons(List<double> inputs)
    {
        foreach (var neuron in neurons)
        {
            neuron.Activate(inputs);
            neuron.Transfer(); // TODO: extract to parameter or smth
        }
    }

    public List<double> NeuronOutputs()
    {
        return Utils.GenerateList(neurons.Count, (i) => neurons[i].output);
    }

    public IEnumerator<Neuron> GetEnumerator()
    {
        return new LayerEnumerator(this);
    }

    public Neuron this[int index]
    {
        get => neurons[index];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new LayerEnumerator(this);
    }

    private class LayerEnumerator : IEnumerator<Neuron>
    {
        private int position = -1;
        private Layer layer;
        public LayerEnumerator(Layer layer)
        {
            this.layer = layer;
        }

        public Neuron Current => layer.neurons[position];

        object IEnumerator.Current => layer.neurons[position];

        public bool MoveNext()
        {
            position++;
            return position < layer.neurons.Count;
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